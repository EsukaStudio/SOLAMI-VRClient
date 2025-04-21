using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class test_rotation : MonoBehaviour
{
    // �� Inspector ������Ŀ������ JSON �ļ�·��
    public GameObject targetObject;
    public string jsonFilePath = "Assets/LocalRotations.json"; // ������޸����·��

    void Start()
    {
        if (targetObject != null && !string.IsNullOrEmpty(jsonFilePath))
        {
            // ����һ���ֵ����洢�������ת��Ϣ
            Dictionary<string, Dictionary<string, float>> localRotations = new Dictionary<string, Dictionary<string, float>>();

            // ��ȡĿ�����弰�����������ת��Ϣ
            GetLocalRotations(targetObject.transform, localRotations);

            // ���ֵ�ת��Ϊ JSON �ַ���
            string json = JsonConvert.SerializeObject(localRotations, Formatting.Indented);

            // �� JSON д���ļ�
            File.WriteAllText(jsonFilePath, json);

            Debug.Log("Local rotations saved to: " + jsonFilePath);
        }
    }

    // �ݹ麯������ȡ���弰��������ľֲ���ת���洢���ֵ���
    void GetLocalRotations(Transform objTransform, Dictionary<string, Dictionary<string, float>> rotationData)
    {
        // ��ȡ��ǰ����ľֲ���ת
        Vector3 localRotation = objTransform.localEulerAngles;

        // ʹ�������������Ϊ�ֵ�ļ����洢��ֲ���ת (x, y, z)
        rotationData[objTransform.name] = new Dictionary<string, float>
        {
            { "x", localRotation.x },
            { "y", localRotation.y },
            { "z", localRotation.z }
        };

        // �ݹ鴦������������
        foreach (Transform child in objTransform)
        {
            GetLocalRotations(child, rotationData);
        }
    }
}
