using UnityEngine;
using UnityEngine.UI;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = -90f;  // Ðý×ªËÙ¶È

    private RectTransform rectTransform;
    private bool isRotating = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isRotating)
        {
            rectTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        isRotating = true;
    }

    void OnDisable()
    {
        isRotating = false;
    }
}
