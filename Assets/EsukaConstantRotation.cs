using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsukaConstantRotation : MonoBehaviour
{
    public GameObject[] targetObjects;
    private Quaternion[] initialRotations;
    //private Quaternion targetRotation = Quaternion.Euler(0, 180, 180);

    private void Start()
    {
        initialRotations = new Quaternion[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                // 记录每个对象的初始旋转
                initialRotations[i] = targetObjects[i].transform.rotation;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.rotation = initialRotations[i];   
            }
        }
    }
}
