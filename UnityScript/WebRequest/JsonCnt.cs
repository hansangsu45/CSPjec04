using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;

[Serializable]
public class JsonInfo
{
    //public int cnt;
    //public string user_id;
    public string login_id;
    public string password;
}

[Serializable]
public class Data
{
    public bool result;
    public JsonInfo[] jsonInfo;
}

public class JsonCnt : MonoBehaviour
{
    public Text[] idTxts;
    public Text[] pwTxts;

    public InputField idInput, pwInput;

    private int cnt = 0;
    [SerializeField] private string userId;

    private void Start()
    {
        StartCoroutine(SetWWWURL());
    }

    public void SetData()
    {
        StartCoroutine(WWWURLCo());
    }

    private IEnumerator WWWURLCo()
    {
        WWWForm form = new WWWForm();
        //form.AddField("id", userId +"_"+0);
        //form.AddField("id", "0_0");
        form.AddField("id", "0_0");

        string url = "http://127.0.0.1/project_02/login_process.php";

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("error_1:" + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("error_2" + www.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("" + www.downloadHandler.text);

                /*JsonInfo jsonInfo = JsonUtility.FromJson<JsonInfo>(www.downloadHandler.text);
                cnt = jsonInfo.cnt;
                userId = jsonInfo.user_id;
                Debug.Log(userId);
                Debug.Log(cnt);*/

                Data d = new Data();
                d = JsonUtility.FromJson<Data>(www.downloadHandler.text);

                Debug.Log(d.jsonInfo[0].login_id + "   " + d.jsonInfo[0].password);

                for (int i = 0; i < 1; i++)
                {
                    idTxts[i].text = d.jsonInfo[i].login_id;
                    pwTxts[i].text = d.jsonInfo[i].password;
                }

                break;

            default:
                break;
        }
    }

    IEnumerator SetWWWURL()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.wikipedia.org/");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

}
