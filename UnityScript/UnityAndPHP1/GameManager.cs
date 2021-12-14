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
        if(Input.GetKeyDown(KeyCode.Escape))  //�ǵ��ư��� & ���� ������ ������
        {
            EscapeUI();
        }
    }

    public void EscapeUI()
    {
        if (panelStack.Count > 1)  //���� �ֱ��� UI�� �ݾ��ְ� ���ÿ��� ���� �� �ܰ� ������ UI�� ���ش�
        {
            panelStack.Pop().SetActive(false);
            panelStack.Peek().SetActive(true);

            if(panelStack.Count==1) //�α��� �гθ� ���� �� ��� ���� �гε� �ݾ��ش�. GamePanel�� �α����ϰ��� �� ���Ǳ��� ���������� ��� �����ִ� UI
            {
                GamePanel.SetActive(false);
            }
        }
    }

    private void SetUI()  //÷�� �α����г� ���ְ� ������ �ݾ���
    {
        systemMsgText.gameObject.SetActive(false);
        storePanel.SetActive(false);
        storeItemListPanel.SetActive(false);
        userItemListPanel.SetActive(false);
        GamePanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void OnClickLoginBtn()  //�α��� ��ư Ŭ��
    {
        StartCoroutine(LoginCo());
    }

    private IEnumerator LoginCo()  //�α���
    {
        //���̵�, ����� ������
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
                
                if (ld.money>=0) //�α��� ����
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
                else  //�α��� ����
                {
                    SystemMsg("���̵� ��й�ȣ�� �ٽ� Ȯ�����ּ���."); //���̵� ��� �߸� �Է������� �޽��� ���
                }
                break;
        }
    }

    private IEnumerator StoreCo(int type) //���� ������ �´� �����۵� ������
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type.ToString());  //������ Ÿ���� ������ (int��)

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

                for(int i=0; i<storeItemCellParent.childCount; i++) //���� ������ ����� ������
                {
                    Destroy(storeItemCellParent.GetChild(i).gameObject);
                }

                foreach(StoreData sd in storeInfo.store_data) //���� ������ ��� ����
                {
                    StoreItem si = Instantiate(storeItemCellPref, storeItemCellParent).GetComponent<StoreItem>();
                    si.SetInit(sd);
                }

                break;
        }
    }

    private IEnumerator PurchaseCo(StoreData sd)  //���� ������ ���� 
    {
        userData.money -= sd.price;  //���� ������
        goldText.text = userData.money.ToString() + "Gold";

        //���� ���̵�, ���� ��, ������ ������ ���̵� ������
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

                for (int i = 0; i < userItemCellParent.childCount; i++)  //���� ������ ����� ������
                {
                    Destroy(userItemCellParent.GetChild(i).gameObject);
                }
                
                foreach (StoreData item in userItemInfo.store_data)  //���� ������ ��� ����
                {
                    UserItem uit = Instantiate(userItemCellPref, userItemCellParent).GetComponent<UserItem>();
                    uit.Init(item);
                }

                break;
        }
    }

    public void OnClickStoreBtn(int type)  //���� ���� ��ư Ŭ��
    {
        //Debug.Log(type);
        StartCoroutine(StoreCo(type));
    }

    public void OnClickPurchase(StoreData sd)  //���� ������ ���� ��ư Ŭ��
    {
        if(userData.money<sd.price)  //��尡 ���ڸ��� �޽��� ���
        {
            SystemMsg("��尡 �����մϴ�.");
            return;
        }

        StartCoroutine(PurchaseCo(sd));
    }

    public void InsertStack(GameObject panel)  //���ο� �г� ų �� ȣ��
    {
        panel.SetActive(true);
        panelStack.Push(panel);
    }

    public void SystemMsg(string msg) // �ý��� �޽���
    {
        CancelInvoke("InactiveSystemTxt");
        systemMsgText.text = msg;
        systemMsgText.gameObject.SetActive(true);
        Invoke("InactiveSystemTxt", 2);  //2�� �Ŀ� �ݾ��ش�
    }

    public void InactiveSystemTxt()
    {
        systemMsgText.gameObject.SetActive(false);
    }
}
