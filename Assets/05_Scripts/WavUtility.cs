using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        int headerSize = 44;
        int fileSize = clip.samples * 2 + headerSize - 8;
        int dataSize = clip.samples * 2;
        int sampleRate = clip.frequency;
        int channels = clip.channels;

        writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
        writer.Write(fileSize);
        writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
        writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((ushort)1);
        writer.Write((ushort)channels);
        writer.Write(sampleRate);
        writer.Write(sampleRate * channels * 2);
        writer.Write((ushort)(channels * 2));
        writer.Write((ushort)16);
        writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
        writer.Write(dataSize);

        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        foreach (var sample in samples)
        {
            writer.Write((short)(sample * 32767));
        }

        writer.Close();
        return stream.ToArray();
    }
}
