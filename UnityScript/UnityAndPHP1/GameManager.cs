using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private readonly string loginUrl = "http://127.0.0.1/member_1/suhang.php";
    private readonly string storeUrl = "http://127.0.0.1/member_1/store.php";
    private readonly string purchaseUrl = "http://127.0.0.1/member_1/buy.php";

    public GameObject loginPanel, storePanel, storeItemListPanel, userItemListPanel;
    public GameObject GamePanel;
    public InputField idInput, pwInput;
    public Text nickText, goldText, systemMsgText;
    public StoreBtn[] storeBtns;

    private Stack<GameObject> panelStack = new Stack<GameObject>();

    public GameObject storeItemCellPref, userItemCellPref;
    public Transform storeItemCellParent, userItemCellParent;

    public UserData userData = new UserData();
    public LoginData ld = new LoginData();
    public StoreInfo storeInfo;
    public StoreInfo userItemInfo;

    private void Awake()
    {
        instance = this;
        SetUI();
        panelStack.Push(loginPanel);  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  //되돌아가기 & 상점 종류로 나가기
        {
            EscapeUI();
        }
    }

    public void EscapeUI()
    {
        if (panelStack.Count > 1)  //가장 최근의 UI를 닫아주고 스택에서 빼고 한 단계 이전의 UI를 켜준다
        {
            panelStack.Pop().SetActive(false);
            panelStack.Peek().SetActive(true);

            if(panelStack.Count==1) //로그인 패널만 남게 된 경우 게임 패널도 닫아준다. GamePanel은 로그인하고나면 이 조건까지 가기전까진 계속 남아있는 UI
            {
                GamePanel.SetActive(false);
            }
        }
    }

    private void SetUI()  //첨에 로그인패널 켜주고 나머지 닫아줌
    {
        systemMsgText.gameObject.SetActive(false);
        storePanel.SetActive(false);
        storeItemListPanel.SetActive(false);
        userItemListPanel.SetActive(false);
        GamePanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void OnClickLoginBtn()  //로그인 버튼 클릭
    {
        StartCoroutine(LoginCo());
    }

    private IEnumerator LoginCo()  //로그인
    {
        //아이디, 비번을 보내줌
        WWWForm form = new WWWForm();
        form.AddField("login_id", idInput.text);
        form.AddField("login_pw", pwInput.text);

        UnityWebRequest www = UnityWebRequest.Post(loginUrl, form);

        yield return www.SendWebRequest();

        switch(www.result)
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
                string s = www.downloadHandler.text;
                Debug.Log(s);
                ld = JsonUtility.FromJson<LoginData>(s);
                
                if (ld.money>=0) //로그인 성공
                {
                    userData = new UserData(ld.id, idInput.text, pwInput.text, ld.money);

                    nickText.text = userData.loginId;
                    goldText.text = string.Concat(userData.money, "Gold");

                    for(int i=0; i<storeBtns.Length; i++)
                    {
                        storeBtns[i].SetInit(ld.item_type[i].id, ld.item_type[i].type_name);
                    }

                    loginPanel.SetActive(false);
                    GamePanel.SetActive(true);
                    InsertStack(storePanel);

                    idInput.text = "";
                    pwInput.text = "";
                }
                else  //로그인 실패
                {
                    SystemMsg("아이디나 비밀번호를 다시 확인해주세요."); //아이디나 비번 잘못 입력했으니 메시지 띄움
                }
                break;
        }
    }

    private IEnumerator StoreCo(int type) //상점 종류에 맞는 아이템들 가져옴
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type.ToString());  //아이템 타입을 보내줌 (int형)

        UnityWebRequest www = UnityWebRequest.Post(storeUrl, form);

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
                string s = www.downloadHandler.text;
                Debug.Log(s);
                storeInfo = JsonUtility.FromJson<StoreInfo>(s);

                storePanel.SetActive(false);
                InsertStack(storeItemListPanel);

                for(int i=0; i<storeItemCellParent.childCount; i++) //상점 아이템 목록을 지워줌
                {
                    Destroy(storeItemCellParent.GetChild(i).gameObject);
                }

                foreach(StoreData sd in storeInfo.store_data) //상점 아이템 목록 갱신
                {
                    StoreItem si = Instantiate(storeItemCellPref, storeItemCellParent).GetComponent<StoreItem>();
                    si.SetInit(sd);
                }

                break;
        }
    }

    private IEnumerator PurchaseCo(StoreData sd)  //상점 아이템 구매 
    {
        userData.money -= sd.price;  //돈을 차감함
        goldText.text = userData.money.ToString() + "Gold";

        //유저 아이디, 유저 돈, 구매한 아이템 아이디를 보내줌
        WWWForm form = new WWWForm();
        form.AddField("uid", userData.id);
        form.AddField("money", userData.money);
        form.AddField("iid", sd.id);

        UnityWebRequest www = UnityWebRequest.Post(purchaseUrl, form);

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
                string s = www.downloadHandler.text;
                Debug.Log(s);

                userItemInfo = JsonUtility.FromJson<StoreInfo>(s);

                storeItemListPanel.SetActive(false);
                InsertStack(userItemListPanel);

                for (int i = 0; i < userItemCellParent.childCount; i++)  //유저 아이템 목록을 지워줌
                {
                    Destroy(userItemCellParent.GetChild(i).gameObject);
                }
                
                foreach (StoreData item in userItemInfo.store_data)  //유저 아이템 목록 갱신
                {
                    UserItem uit = Instantiate(userItemCellPref, userItemCellParent).GetComponent<UserItem>();
                    uit.Init(item);
                }

                break;
        }
    }

    public void OnClickStoreBtn(int type)  //상점 종류 버튼 클릭
    {
        //Debug.Log(type);
        StartCoroutine(StoreCo(type));
    }

    public void OnClickPurchase(StoreData sd)  //상점 아이템 구매 버튼 클릭
    {
        if(userData.money<sd.price)  //골드가 모자르면 메시지 띄움
        {
            SystemMsg("골드가 부족합니다.");
            return;
        }

        StartCoroutine(PurchaseCo(sd));
    }

    public void InsertStack(GameObject panel)  //새로운 패널 킬 때 호출
    {
        panel.SetActive(true);
        panelStack.Push(panel);
    }

    public void SystemMsg(string msg) // 시스템 메시지
    {
        CancelInvoke("InactiveSystemTxt");
        systemMsgText.text = msg;
        systemMsgText.gameObject.SetActive(true);
        Invoke("InactiveSystemTxt", 2);  //2초 후에 닫아준다
    }

    public void InactiveSystemTxt()
    {
        systemMsgText.gameObject.SetActive(false);
    }
}
