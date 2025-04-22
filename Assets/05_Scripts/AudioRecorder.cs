using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Android;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private bool isRecording = false;
    private WSCommu wsCommu;

    void Start()
    {
        wsCommu = FindObjectOfType<WSCommu>();

        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    public void StartRecording()
    {
        StartCoroutine(DelayedStartRecording(1f));
    }

    private IEnumerator DelayedStartRecording(float delay)
    {
        yield return new WaitForSeconds(delay); // µÈ´ý1Ãë
        isRecording = true;
        audioClip = Microphone.Start(null, false, 10, 44100);
    }

    public void StopRecordingAndSend()
    {
        if (isRecording)
        {
            isRecording = false;
            Microphone.End(null);
            string filePath = Application.persistentDataPath + "/recordedAudio.wav";
            SaveAudioClipToWav(audioClip, filePath);
            SendAudioFile(filePath);
        }
    }

    private void SaveAudioClipToWav(AudioClip clip, string filePath)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);
        var bytes = WavUtility.FromAudioClip(clip);
        File.WriteAllBytes(filePath, bytes);
    }

    private void SendAudioFile(string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        if (wsCommu != null)
        {
            wsCommu.SendAudioData(bytes);
        }
    }
}
