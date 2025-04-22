using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerEvent : MonoBehaviour
{

    public GameObject SensingLabel;
    public GameObject ThinkingLabel;
    public AudioRecorder audioRecorder;
    public RedisConnection redisConnection;
    private bool isUIPanelActive;
    // Start is called before the first frame update
    void Start()
    {
        SensingLabel.SetActive(false);
        ThinkingLabel.SetActive(false);
        isUIPanelActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            ToggleUIPanel();
        }
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            ToggleUIPanel();
        }
    }
    void ToggleUIPanel()
    {
        // ÇÐ»»UIÃæ°åµÄÏÔÊ¾×´Ì¬
        isUIPanelActive = !isUIPanelActive;
        SensingLabel.SetActive(isUIPanelActive);
        if(isUIPanelActive == true)
        {
            ThinkingLabel.SetActive(false);
            audioRecorder.StartRecording();
            redisConnection.StartSendingUserStream();
        }
        else
        {
            ThinkingLabel.SetActive(true);
            redisConnection.StopSendingUserStream();
            audioRecorder.StopRecordingAndSend();
        }
    }
}
