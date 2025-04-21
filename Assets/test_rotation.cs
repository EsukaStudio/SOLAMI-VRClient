using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class test_rotation : MonoBehaviour
{
    // 在 Inspector 中设置目标对象和 JSON 文件路径
    public GameObject targetObject;
    public string jsonFilePath = "Assets/LocalRotations.json"; // 你可以修改这个路径

    void Start()
    {
        if (targetObject != null && !string.IsNullOrEmpty(jsonFilePath))
        {
            // 创建一个字典来存储物体的旋转信息
            Dictionary<string, Dictionary<string, float>> localRotations = new Dictionary<string, Dictionary<string, float>>();

            // 获取目标物体及其子物体的旋转信息
            GetLocalRotations(targetObject.transform, localRotations);

            // 将字典转换为 JSON 字符串
            string json = JsonConvert.SerializeObject(localRotations, Formatting.Indented);

            // 将 JSON 写入文件
            File.WriteAllText(jsonFilePath, json);

            Debug.Log("Local rotations saved to: " + jsonFilePath);
        }
    }

    // 递归函数：获取物体及其子物体的局部旋转并存储到字典中
    void GetLocalRotations(Transform objTransform, Dictionary<string, Dictionary<string, float>> rotationData)
    {
        // 获取当前物体的局部旋转
        Vector3 localRotation = objTransform.localEulerAngles;

        // 使用物体的名称作为字典的键，存储其局部旋转 (x, y, z)
        rotationData[objTransform.name] = new Dictionary<string, float>
        {
            { "x", localRotation.x },
            { "y", localRotation.y },
            { "z", localRotation.z }
        };

        // 递归处理所有子物体
        foreach (Transform child in objTransform)
        {
            GetLocalRotations(child, rotationData);
        }
    }
}
