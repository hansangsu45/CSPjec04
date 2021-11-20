using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class Test1 : MonoBehaviour
{
    public Camera cam;
    private bool takeScreenShot;
    public AudioSource _audio;

    private IEnumerator Start()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + Application.streamingAssetsPath + "/Audio/bgm1.wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                _audio.clip = clip;
                _audio.Play();
            }
        }
    }

    private void OnPostRender()
    {
        if (takeScreenShot)
        {
            takeScreenShot = false;
            RenderTexture renderTexture = cam.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            //File.WriteAllBytes(Application.dataPath + $"/Screenshot/Screenshot {UnityEngine.Random.Range(0,100000)}.png", byteArray);
            File.WriteAllBytes(Application.streamingAssetsPath + $"/Screenshot/Screenshot {UnityEngine.Random.Range(0, 100000)}.png", byteArray);
            Debug.Log("Saved Screenshot");

            RenderTexture.ReleaseTemporary(renderTexture);
            cam.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        cam.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenShot = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot(1920, 1080);
        }

    }
}
