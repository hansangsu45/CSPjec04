using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Js_member : MonoBehaviour
{
    public InputField idInput, pwInput;

    public void Btn_reg()
    {
        StartCoroutine(ProgReg());
    }

    IEnumerator ProgReg()
    {
        WWWForm form = new WWWForm();
        form.AddField("login_id", idInput.text);
        form.AddField("login_pw", pwInput.text);

        string url = "http://127.0.0.1/member_1/reg_member.php";
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.LogError("ConnectionError " + www.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("DataProcessingError " + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("ProtocolError " + www.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(www.downloadHandler.text);
                break;
        }
    }
}
