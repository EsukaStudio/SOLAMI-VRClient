using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using UnityEngine;
using UnityEngine.InputSystem;

public class RedisConnection : MonoBehaviour
{
    public MotionRecorder USER;
    public MotionRecorder AI;
    public string EditorAddress;
    public string QuestAddress;
    public AudioReceiver audioReceiver;
    private List<Transform> USER_Bones;
    private List<Transform> AI_Bones;
    private ConnectionMultiplexer redis;
    private IDatabase db;

    private const string userStreamKey = "userStream";
    private const string aiStreamKey = "aiStream";
    private const string aiFBStreamKey = "aifbStream";
    private string lastAiStreamId = "0-0";
    private string changeId = null;

    private bool isPlaying = false;
    private int minFramesBeforePlay = 10;  // 减少初始缓冲区帧数
    private float deleteInterval = 30f;  // 5分钟删除一次
    private float lastDeleteTime;
    private bool isSending = false;
    private string charID = "Vrmgirl";

    private SkinnedMeshRenderer faceRenderer;
    private HashSet<string> validBlendShapeKeys = new HashSet<string>();

    void Start()
    {
#if UNITY_EDITOR
        Application.runInBackground = true;
        redis = ConnectionMultiplexer.Connect(EditorAddress);
        Debug.Log("Redis Server:" + EditorAddress + "连接成功！");
#else
        redis = ConnectionMultiplexer.Connect(QuestAddress);
        Debug.Log("Redis Server:"+ QuestAddress +"连接成功！");
#endif
        db = redis.GetDatabase();
        ClearRedisDatabase();
        USER_Bones = USER.GetHipsAndDescendants();
        AI_Bones = AI.GetHipsAndDescendants();


        lastDeleteTime = Time.time;

        StartCoroutine(SendDataCoroutine());
        StartCoroutine(ReceiveDataCoroutine());
        StartCoroutine(DeleteOldEntriesCoroutine());
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            FetchChangeId();
        }
    }
    void ClearRedisDatabase()
    {
        db.Execute("FLUSHDB");
        Debug.Log("Redis database cleared.");
    }

    public string FetchChangeId()
    {
        if (db == null)
        {
            Debug.LogError("Redis database is not connected.");
            return null;
        }

        if (db.KeyExists("change_id"))
        {
            // 强制转换为字符串
            changeId = (string)db.StringGet("change_id");
            Debug.Log($"Fetched change_id: {changeId}");
        }
        else
        {
            Debug.LogWarning("Key 'change_id' does not exist in Redis.");
            changeId = null;
        }

        return changeId;
    }


    IEnumerator SendDataCoroutine()
    {
        while (isSending)
        {
            if (redis != null && isSending) // Check isSending before sending data
            {
                Task sendTask = SendDataAsync(USER_Bones, userStreamKey);
                yield return new WaitUntil(() => sendTask.IsCompleted); // Wait for the task to complete
                if (sendTask.IsFaulted)
                {
                    Debug.LogError(sendTask.Exception);
                }
            }

            if (!isSending)
            {
                yield break; // Exit the coroutine if isSending is set to false
            }

            yield return new WaitForSeconds(1f / 30f); // Wait approximately 33.33 milliseconds
        }
    }

    private void PlayAudio()
    {
        if (audioReceiver != null)
        {
            audioReceiver.PlayReceivedAudio();
        }
    }

    IEnumerator ReceiveDataCoroutine()
    {
        while (true)
        {
            if (redis != null)
            {
                Task receiveTask = ReceiveAndUpdateDataAsync(aiStreamKey);
                yield return new WaitUntil(() => receiveTask.IsCompleted); // Wait for the task to complete
                //if (receiveTask.IsFaulted)
                //{
                //    Debug.LogError(receiveTask.Exception);
                //}
                //Debug.Log($"changeId: {changeId}, StreamID: {lastAiStreamId}");
                //if (changeId != null && lastAiStreamId == changeId)
                //{
                //    PlayAudio();
                //    changeId = null;
                //}
            }
            yield return new WaitForSeconds(1f / 30f);
        }
    }

    IEnumerator DeleteOldEntriesCoroutine()
    {
        while (true)
        {
            if (Time.time - lastDeleteTime >= deleteInterval)
            {
                Task deleteTask = DeleteOldEntriesAsync(aiStreamKey);
                yield return new WaitUntil(() => deleteTask.IsCompleted); // Wait for the task to complete
                if (deleteTask.IsFaulted)
                {
                    Debug.LogError(deleteTask.Exception);
                }
                lastDeleteTime = Time.time; // Reset the timer
            }
            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    async Task SendDataAsync(List<Transform> boneList, string streamKey)
    {
        List<NameValueEntry> entries = new List<NameValueEntry>();
        foreach (Transform bone in boneList)
        {
            if (bone.name == USER.rootSuffix || bone.name == AI.rootSuffix)
                entries.Add(new NameValueEntry(bone.name + "_position", $"{bone.position.x},{bone.position.y},{bone.position.z}"));
            entries.Add(new NameValueEntry(bone.name + "_rotation", $"{bone.localRotation.x},{bone.localRotation.y},{bone.localRotation.z},{bone.localRotation.w}"));
        }
        string entryId = await db.StreamAddAsync(streamKey, entries.ToArray());
        //Debug.Log($"Stream entry added with ID: {entryId}");
    }

    async Task ReceiveAndUpdateDataAsync(string streamKey)
    {
        var entries = await db.StreamReadAsync(streamKey, lastAiStreamId, 10);
        foreach (var entry in entries)
        {
            UpdateTransformFromEntry(entry);

            // Update lastAiStreamId immediately after processing the entry
            lastAiStreamId = entry.Id;
        }
    }

    async Task DeleteOldEntriesAsync(string streamKey)
    {
        var entries = await db.StreamReadAsync(streamKey, "0", count: 1000);
        List<RedisValue> idsToDelete = new List<RedisValue>();

        foreach (var entry in entries)
        {
            if (entry.Id.CompareTo(lastAiStreamId) < 0)  // Only delete entries older than the last processed ID
            {
                idsToDelete.Add(entry.Id);
            }
        }

        if (idsToDelete.Count > 0)
        {
            // Batch delete the processed entries
            await db.StreamDeleteAsync(streamKey, idsToDelete.ToArray());
            Debug.Log($"Deleted {idsToDelete.Count} old entries from the stream.");
        }
    }

    private void UpdateTransformFromEntry(StreamEntry entry)
    {
        if (AI_Bones == null || AI_Bones.Count == 0)
        {
            Debug.LogWarning("AI_Bones is not initialized yet.");
            return;
        }
        Dictionary<string, Transform> boneDictionary = AI_Bones.ToDictionary(bone => bone.name, bone => bone);

        foreach (var value in entry.Values)
        {
            string key = value.Name.ToString();
            if (key.EndsWith("_position"))
            {
                string boneName = key.Replace("_position", "");
                //Debug.Log($"AI bones check: {boneName}");
                if (boneDictionary.TryGetValue(boneName, out Transform bone))
                {
                    var posParts = value.Value.ToString().Split(',');
                    Vector3 position = new Vector3(float.Parse(posParts[0]), float.Parse(posParts[1]), float.Parse(posParts[2]));
                    bone.localPosition = position;
                    //bone.position = position;
                }
            }
            else if (key.EndsWith("_rotation"))
            {
                string boneName = key.Replace("_rotation", "");
                if (boneDictionary.TryGetValue(boneName, out Transform bone))
                {
                    var rotParts = value.Value.ToString().Split(',');
                    Quaternion rotation = new Quaternion(float.Parse(rotParts[0]), float.Parse(rotParts[1]), float.Parse(rotParts[2]), float.Parse(rotParts[3]));
                    bone.localRotation = rotation;
                    //bone.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                    //if (boneName == "Upperarm.l") { bone.rotation = new Quaternion(0.707f, 0.0f, 0.0f, 0.707f); }
                    //if (boneName == "Upperarm.l") { bone.localRotation =rotation; }
                    //bone.rotation = rotation;
                }
            }
            else
            {
                if (faceRenderer != null && validBlendShapeKeys.Contains(key))
                {
                    //Debug.Log($"valid face key:");
                    //foreach (var validkey in validBlendShapeKeys)
                    //{
                    //    Debug.Log($"BlendShape Key: {key}");
                    //}
                    int blendShapeIndex = faceRenderer.sharedMesh.GetBlendShapeIndex(key);
                    //Debug.Log($"find face index: {blendShapeIndex}");
                    if (blendShapeIndex >= 0)
                    {
                        if (float.TryParse(value.Value.ToString(), out float blendShapeWeight))
                        {
                            if (charID == "Vrmgirl")
                                faceRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight*100);
                            else
                                faceRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight * 300);
                        }
                    }
                }
            }
        }
    }

    public void UpdateFaceRenderer(SkinnedMeshRenderer newFaceRenderer)
    {
        faceRenderer = newFaceRenderer;
        validBlendShapeKeys.Clear();
        if (faceRenderer != null && faceRenderer.sharedMesh != null)
        {
            for (int i = 0; i < faceRenderer.sharedMesh.blendShapeCount; i++)
            {
                validBlendShapeKeys.Add(faceRenderer.sharedMesh.GetBlendShapeName(i));
                string blendShapeName = faceRenderer.sharedMesh.GetBlendShapeName(i);
                //Debug.Log($"Update BlendShape {i}: {blendShapeName}");
            }
        }
    }

    public void UpdateAIBones(List<Transform> newAIBones)
    {
        AI_Bones = newAIBones;
        //Debug.Log("AI_Bones has been updated with the following bones:");
        //foreach (var bone in AI_Bones)
        //{
        //    Debug.Log(bone.name);
        //}
    }

    public void SendCharacterId(string charId)
    {
        if (db!=null)
        {
            db.StringSet("char_id", charId);
            charID = charId;
            //Debug.Log("Sent char_id to Redis" + charId);
        }
    }

    public void StartSendingUserStream()
    {
        if (!isSending)
        {
            isSending = true;
            Debug.Log("change to sending redis motion data!");

            Task sendAITask = SendDataAsync(AI_Bones, aiFBStreamKey);
            sendAITask.ContinueWith(task =>
            {
                if(task.IsFaulted)
                {
                    Debug.LogError("Error sending AI Bones data:" + task.Exception);
                }
            });
            StartCoroutine(SendDataCoroutine());
        }
    }

    public void StopSendingUserStream()
    {
        isSending = false;
        Debug.Log("change to stop redis motion data!");
        StopCoroutine(SendDataCoroutine());
    }

    void OnDestroy()
    {
        if (redis != null)
        {
            redis.Close();
            redis.Dispose();
        }
    }
}
