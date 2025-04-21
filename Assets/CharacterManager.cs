using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterInfo
    {
        public string characterName;
        public GameObject character;
        public GameObject head;
        public GameObject root;
        public SkinnedMeshRenderer face;
        public Vector3 labelOffsets;
    }

    public CharacterInfo[] characters;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
