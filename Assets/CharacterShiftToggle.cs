using Meta.WitAi.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShiftToggle : MonoBehaviour
{
    public Toggle[] toggles;
    public CharacterManager characterManager;
    public GameObject FloatingUI;
    public MotionRecorder motionRecorder;
    public RedisConnection redisConnection;

    private GameObject currentActiveCharacter;
    private GameObject currentActiveHead;
    private GameObject currentActiveRoot;
    private SkinnedMeshRenderer currentActiveFace;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) => { if (isOn) ShowOnlyOne(index); });
        }
        SetActiveCharacter(0);
    }

    void ShowOnlyOne(int index)
    {
        // ��������������
        foreach (var characterInfo in characterManager.characters)
        {
            characterInfo.character.SetActive(false);
        }

        // ��ʾѡ�е������壬������FloatingUI��λ��
        if (index >= 0 && index < characterManager.characters.Length)
        {
            SetActiveCharacter(index);
        }
    }

    private void SetActiveCharacter(int index)
    {
        currentActiveCharacter = characterManager.characters[index].character;
        currentActiveCharacter.SetActive(true);
        currentActiveHead = characterManager.characters[index].head;
        currentActiveRoot = characterManager.characters[index].root;
        currentActiveFace = characterManager.characters[index].face; // ��ȡ��ǰ��ɫ��face
        offset = characterManager.characters[index].labelOffsets;

        FloatingUI.transform.position = currentActiveHead.transform.position + offset;

        if (motionRecorder != null)
        {
            motionRecorder.rootSuffix = currentActiveRoot.name;
            Transform rootTransform = motionRecorder.FindRootTransform(currentActiveCharacter.transform, motionRecorder.rootSuffix);
            if (rootTransform != null)
            {
                motionRecorder.GetHipsAndDescendants().Clear(); // ���֮ǰ�洢�Ĺ�����Ϣ
                motionRecorder.StoreRootAndDescendants(rootTransform);
            }
            if (redisConnection != null)
            {
                redisConnection.UpdateAIBones(motionRecorder.GetHipsAndDescendants());
            }
        }

        if (redisConnection != null)
        {
            string charId = characterManager.characters[index].characterName;
            redisConnection.SendCharacterId(charId);
            redisConnection.UpdateFaceRenderer(currentActiveFace); // ����faceRenderer
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentActiveHead != null)
        {
            FloatingUI.transform.position = currentActiveHead.transform.position + offset;
        }
    }
}