using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class AudioReceiver : MonoBehaviour
{
    public GameObject ThinkingLabel;
    public RedisConnection redisConnection;
    private AudioSource audioSource;
    private WSCommu wsCommu;
    private bool audioReady = false;
    private string audioFilePath;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        wsCommu = FindObjectOfType<WSCommu>();
    }

    void Update()
    {
        byte[] audioData = wsCommu.GetAudioData();
        if (audioData != null)
        {
            audioFilePath = Path.Combine(Application.persistentDataPath, "receivedAudio.wav");
            File.WriteAllBytes(audioFilePath, audioData);
            audioReady = true;
            //redisConnection.FetchChangeId();
            StartCoroutine(LoadAndPlayAudioWithDelay(audioFilePath, 0f));
        }
    }

    public void PlayReceivedAudio()
    {
        audioSource.Play();
    }
    //public void PlayReceivedAudio(string filePath)
    //{
    //    StartCoroutine(LoadAndPlayAudioWithDelay(filePath, 0f));
    //}

    private IEnumerator LoadAndPlayAudioWithDelay(string filePath, float delay)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Failed to load audio file: {www.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                ThinkingLabel.SetActive(false);

                yield return new WaitForSeconds(delay);
                audioSource.Play();

                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
        }
    }
}
