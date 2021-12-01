using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SocketModule tcp;
    [SerializeField]
    private InputField nickname;

    const char CHAR_TERMINATOR = ';';
    const char CHAR_COMMA = ',';
    const int DAMAGE_ATTACK = 30;

    string myID;
    public GameObject prefabUnit;
    public GameObject mainChar;
    public UnitControl mainControl;
    Dictionary<string, UnitControl> remoteUnits;
    Queue<string> commandQueue;


    private void Awake()
    {
        if(instance==null)
           instance = this;
    }

    void Start()
    {
        tcp = GetComponent<SocketModule>();
        remoteUnits = new Dictionary<string, UnitControl>();
        commandQueue = new Queue<string>();
        mainControl = mainChar.GetComponent<UnitControl>();
    }
    // Update is called once per frame
    void Update()
    {
        ProcessQueue();
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 targetPos;
                //orgPos = transform.position;
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //targetPos.z = orgPos.z;
                mainControl.SetTargetPos(targetPos);
                string data = "#Move#" + targetPos.x + "," + targetPos.y;
                SendCommand(data);

                //SocketModule.GetInstance().SendData(data);
                //Debug.Log("sent: " + data);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            //mainChar
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (mainControl.GetHP() > 0)
                {
                    string data = $"#Attack#";
                    SendCommand(data);
                    mainControl.StartFx();
                }

            }
        }
    }
    void ProcessQueue()
    {
        while (commandQueue.Count > 0)
        {
            string nextCommand = commandQueue.Dequeue();
            ProcessCommand(nextCommand);
        }
    }
    public void OnLogin()
    {
        string id = nickname.text;
        myID = id;
        if (id.Length > 0)
        {
            tcp.Login(id);
            mainChar.transform.position = Vector3.zero;
        }
    }
    public void OnLogOut()
    {
        tcp.Logout();
        // delete all remote chars
        foreach (var remotePair in remoteUnits)
        {
            Destroy(remotePair.Value.gameObject);
        }
        remoteUnits.Clear();
    }
    public UnitControl AddUnit(string id)
    {
        UnitControl uc = null;
        if (!remoteUnits.ContainsKey(id))
        {

            GameObject newUnit = Instantiate(prefabUnit);
            uc = newUnit.GetComponent<UnitControl>();
            remoteUnits.Add(id, uc);
        }
        return uc;
    }
    public void LeaveUnit(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            GameObject unit = uc.gameObject;
            remoteUnits.Remove(id);
            Destroy(unit);
        }
    }
    public void SetMove(string id, string coordinates)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            var strs = coordinates.Split(',');
            Vector3 pos = new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), 0);
            uc.SetTargetPos(pos);
        }
    }
    public void UserLeft(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            Destroy(uc.gameObject);
            remoteUnits.Remove(id);
        }
    }
    private void LoadHistory(string history)
    {
        var strs = history.Split(',');
        int max = strs.Length;
        for (int i = 0; i + 2 < max; i += 3)

        {
            string id = strs[i];
            if (myID.CompareTo(id) != 0)
            {
                UnitControl uc = AddUnit(id);
                if (uc != null)
                {
                    float x = float.Parse(strs[i + 1]);
                    float y = float.Parse(strs[i + 2]);
                    //uc.SetTargetPos();
                    uc.transform.position = new Vector3(x, y, 0);
                }
            }
        }

        Debug.Log("LoadHistory");
    }
    public void SendCommand(string cmd)
    {
        SocketModule.Instance.SendData(cmd);
        Debug.Log("cmd sent: " + cmd);
    }
    private void TakeDamage(string remain)
    {
        Debug.Log("TakeDamage = " + remain);
        var str = remain.Split(CHAR_COMMA);
        for (int i = 0; i < str.Length; i++)
        {
            if (remoteUnits.ContainsKey(str[i]))
            {
                UnitControl uc = remoteUnits[str[i]];
                if (uc != null)
                {
                    uc.DropHP(DAMAGE_ATTACK);
                }
               
            }
            else
            {
                if (myID.CompareTo(str[i]) == 0)
                {
                    mainControl.DropHP(DAMAGE_ATTACK);
                }
            }
        }
    }
    public string GetID(string cmd)
    {
        int idx = cmd.IndexOf("$");
        string id = "";
        if (idx > 0)
        {
            id = cmd.Substring(0, idx);
        }
        return id;
    }
    public void QueueCommand(string cmd)
    {
        commandQueue.Enqueue(cmd);
    }
    public void ProcessCommand(string cmd) // 제일복잡함. RPG같은거나 시뮬레이션 만들려면 여기를잘해야함(처리잘하기)
    {
        // cmd = id$#Command#argument
        bool bMore = true;
        while (bMore)
        {
            Debug.Log("process cmd = " + cmd);
            int idx = cmd.IndexOf("$"); // $가 어디있는지
            string id = "";
            if (idx >= 0) // 있으면
            {
                id = cmd.Substring(0, idx); // 0~$전까지 (id값) 빼오기
            }
            int idx2 = cmd.IndexOf("#"); // #있으면
            if (idx2 > idx) // $보다 #가 뒤에있으면(정상적)
            {
                // command is there
                int idx3 = cmd.IndexOf("#", idx2 + 1); // #이 또있으면(정상, #명령# 이니까 두번쨰#)
                if (idx3 > idx2) // 첫# < 뒤# (정상)
                {
                    string command = cmd.Substring(idx2 + 1, idx3 - idx2 - 1);
                    string remain; //= cmd.Substring(idx3 + 1);
                    string nextCommand;
                    int idx4 = cmd.IndexOf(CHAR_TERMINATOR, idx3 + 1);

                    if (idx4 > idx3)
                    {
                        remain = cmd.Substring(idx3 + 1, idx4 - idx3 - 1);
                        nextCommand = cmd.Substring(idx4 + 1);
                    }
                    else
                    {
                        remain = cmd.Substring(idx3 + 1, cmd.Length - idx3 - 1);
                        nextCommand = cmd.Substring(idx3 + 1);
                    }



                    Debug.Log($"command= {command} id={id} remain={remain} nextCommand = {nextCommand}");

                    Debug.Log(myID);
                    if (myID.CompareTo(id) != 0) // 내 아이디가 아니면(내꺼는 프로세스할필요가없어 클라입장에서)
                    {
                        Debug.Log("check");
                        switch (command)
                        {
                            case "Enter":
                                AddUnit(id); // BroadCast 됨(누가들어옴)
                                break;
                            case "Move":
                                SetMove(id, remain);
                                break;
                            case "Left":
                                UserLeft(id);
                                break;
                            case "History":
                                LoadHistory(remain); // BroadCast 안됨(최초입장시 다른사람들 위치불러오기)
                                break;
                            case "Heal":
                                UserHeal(id);
                                break;
                            case "Attack":
                                UserAttack(id);
                                break;
                            case "Damage":
                                TakeDamage(remain);
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("ignore remote command");
                    }
                    cmd = nextCommand; // 들어오는시간 거의비슷해서 aaa$#Enter# bbb$#Enter 일때 bbb$#Enter# 뺴는용도
                    if (cmd.Length <= 0)
                    {
                        // No more data to process
                        bMore = false;
                    }
                }
                else
                {
                    // parsing error, 에러찾기
                    bMore = false;
                }
            }
            else
            {
                // parsing error
                bMore = false;
            }
        }
    }
    public void OnRevive()
    {
        mainControl.Revive();

        string data = "#Heal#";
        SendCommand(data);
    }
    private void UserAttack(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            uc.StartFx();
        }
    }

    private void UserHeal(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            uc.Revive();
        }
    }
}