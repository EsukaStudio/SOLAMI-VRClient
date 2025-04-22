using Pipelines.Sockets.Unofficial.Arenas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionRecorder : MonoBehaviour
{
    public string rootSuffix = "";
    public string testFlag = "";
    private List<Transform> hipsAndDescendants = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        Transform rootTransform = FindRootTransform(transform, rootSuffix);
        if(rootTransform != null)
        {
            StoreRootAndDescendants(rootTransform);
        }
    }

    public List<Transform> GetHipsAndDescendants() { return hipsAndDescendants; }

    public Transform FindRootTransform(Transform parent, string RootSuffix)
    {
        foreach (Transform child in parent)
        {
            if(child.name.Contains(RootSuffix))
            {
                return child;
            }
            Transform result = FindRootTransform(child, RootSuffix);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void StoreRootAndDescendants(Transform parent)
    {
        hipsAndDescendants.Add(parent);
        foreach (Transform child in parent)
        {
            StoreRootAndDescendants(child);
        }

    }

    // Update is called once per frame
    void Update()
    {
       if(testFlag == "PrintRotation")
        {
            foreach (Transform bone in hipsAndDescendants)
            {
                Debug.Log($"{bone.name} Rotation: {bone.localRotation}");
            }
        }
        if (testFlag == "PrintRoot")
        {
            Debug.Log($"Root Suffix: {rootSuffix}");
        }
    }
}
