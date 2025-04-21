using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

public class WSCommu : MonoBehaviour
{
    private WebSocket ws;
    private Queue<byte[]> audioDataQueue = new Queue<byte[]>();
    private bool isConnected = false;
    public string EditorAddress = "ws://127.0.0.1:8099";
    public string QuestAddress = "ws://192.168.1.102:8099";

    private string serverAddress;

    void Start()
    {
#if UNITY_EDITOR
        Application.runInBackground = true;
        serverAddress = EditorAddress;
#else
        serverAddress = QuestAddress;
#endif
        ConnectToServer();
    }

    void ConnectToServer()
    {
        ws = new WebSocket(serverAddress);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected!");
            isConnected = true;
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received from server");
            if (e.RawData != null && e.RawData.Length > 0)
            {
                lock (audioDataQueue)
                {
                    audioDataQueue.Enqueue(e.RawData);
                }
            }
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket closed with code: " + e.Code + ", reason: " + e.Reason);
            isConnected = false;
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket error: " + e.Message);
        };

        ws.Connect();
    }

    void Update()
    {
        // 可以在这里处理音频数据队列中的数据
    }

    public void SendAudioData(byte[] audioData)
    {
        if (isConnected)
        {
            ws.Send(audioData);
            Debug.Log("Audio data sent");
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected, cannot send audio data");
        }
    }

    public byte[] GetAudioData()
    {
        lock (audioDataQueue)
        {
            if (audioDataQueue.Count > 0)
            {
                return audioDataQueue.Dequeue();
            }
        }
        return null;
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
            ws = null;
        }
    }
}