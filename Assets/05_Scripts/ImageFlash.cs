using UnityEngine;
using UnityEngine.UI;

public class ImageFlash : MonoBehaviour
{
    private Image image;
    private bool isFlashing;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        StartFlashing();
    }

    void OnDisable()
    {
        StopFlashing();
    }

    void StartFlashing()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashImage());
        }
    }

    void StopFlashing()
    {
        isFlashing = false;
        StopCoroutine(FlashImage());
        image.enabled = true; // ȷ��Image�ڽ��ýű�ʱ�ǿɼ���
    }

    System.Collections.IEnumerator FlashImage()
    {
        while (isFlashing)
        {
            image.enabled = !image.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
