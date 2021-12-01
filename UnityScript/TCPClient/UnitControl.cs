using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitControl : MonoBehaviour
{
    const float speed = 3.0f;
    const int MAX_HP = 100;
    const int DROP_HP = 4;

    public bool bMovable = false;
    GameManager gm;
    Vector3 targetPos;
    Vector3 orgPos;
    float timeToDest;
    float elapsed;
    bool bMoving;

    //public Image hpBar;
    public GameObject fillBar;
    //public Button sendBtn;
    float elapsedDrop;
    int currentHP;
    int maxHP;

    public ParticleSystem fxParticle;

    // Start is called before the first frame update
    void Start()
    {
        // sm = GameObject.Find("GameManager").GetComponent<SocketModule>();
        gm = GameManager.instance;
        orgPos = transform.position;
        targetPos = orgPos;
        timeToDest = 0;
        bMoving = false;
        maxHP = MAX_HP;
        currentHP = maxHP;
        //sendBtn = GameObject.Find("SendBtn").GetComponent<Button>();

        /*sendBtn.onClick.AddListener(() =>
        {
            gm.OnRevive();
        });*/
        StartCoroutine(Hp());
    }
    // Update is called once per frame
    void Update()
    {
        if (bMoving)
        {
            print(bMoving);
            elapsed += Time.deltaTime;
            if (elapsed >= timeToDest)
            {
                elapsed = timeToDest;
                transform.position = targetPos;
                bMoving = false;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp(orgPos, targetPos, elapsed / timeToDest);
                //print(bMoving.ToString() + newPos);
                transform.position = newPos;
            }
        }


    }
    public void Revive()
    {
        if (currentHP <= 0)
        {
            currentHP = maxHP;
        }
        //hpBar.fillAmount = Mathf.Clamp((float)currentHP / maxHP, 0, 1);
        SetHP(maxHP);
    }
    public void DropHP(int damage)
    {
        currentHP -= damage;
        SetHP(currentHP);
    }
    public void SetHP(int curHP)
    {
        currentHP = Mathf.Clamp(curHP, 0, maxHP);
        fillBar.transform.localScale = new Vector3(Mathf.Clamp((float)currentHP / maxHP, 0, 1), 1, 1);
    }
    public int GetHP()
    {
        return currentHP;
    }
    public void StartFx()
    {
        if (currentHP > 0)
        {
            fxParticle.Play();
        }
    }

    IEnumerator Hp()
    {
        while (true)
        {
            currentHP = Mathf.Clamp(currentHP - DROP_HP, 0, maxHP);

            fillBar.transform.localScale = new Vector3(Mathf.Clamp((float)currentHP / maxHP, 0, 1), 1, 1);
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetTargetPos(Vector3 pos)
    {
        orgPos = transform.position;
        targetPos = pos;
        targetPos.z = orgPos.z;
        timeToDest = Vector3.Distance(orgPos, targetPos) / speed;
        elapsed = 0;
        bMoving = true;
    }
    public void SetColor(Color col)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color = col;
        }
    }
}