using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour  //상점에서 종류 선택하고 나오는 상점 아이템 목록 하나마다 붙는 스크립트
{
    public StoreData sd;
    public Text nameTxt, goldText;
    public Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            GameManager.instance.OnClickPurchase(sd);  //해당 아이템 구매 시도
        });
    }

    public void SetInit(StoreData sd)
    {
        this.sd = sd;

        nameTxt.text = sd.name;
        goldText.text = sd.price.ToString() + "g";
    }
}
