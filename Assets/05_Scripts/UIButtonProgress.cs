using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class UIButtonProgress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public Image buttonImage;
    public Image ProgressBar;
    public TMP_Text SpeechText;
    public Color defaultButtonColor;
    public Color pressedButtonColor;
    public AudioRecorder audioRecorder;
    public RedisConnection redisConnection;

    private bool isButtonPressed = false;
    private float progressTime = 0f;
    private float duration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage.color = defaultButtonColor;
        SpeechText.text = "长按右侧按钮输入语音";
        ProgressBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isButtonPressed)
        {
            progressTime += Time.deltaTime;
            ProgressBar.fillAmount = Mathf.Clamp01(progressTime / duration);

            if (ProgressBar.fillAmount >= 1f)
            {
                Debug.Log("UI调试成功");
                ResetButtonState();
                audioRecorder.StopRecordingAndSend();
                redisConnection.StopSendingUserStream();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        buttonImage.color = pressedButtonColor;
        SpeechText.text = string.Empty;
        audioRecorder.StartRecording();
        redisConnection.StartSendingUserStream();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetButtonState();
        audioRecorder.StopRecordingAndSend();
        redisConnection.StopSendingUserStream();
    }

    private void ResetButtonState()
    {
        isButtonPressed = false;
        progressTime = 0f;
        ProgressBar.fillAmount = 0f;
        buttonImage.color = defaultButtonColor;
    }
}
