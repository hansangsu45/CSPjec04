using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour  //�������� ���� �����ϰ� ������ ���� ������ ��� �ϳ����� �ٴ� ��ũ��Ʈ
{
    public StoreData sd;
    public Text nameTxt, goldText;
    public Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            GameManager.instance.OnClickPurchase(sd);  //�ش� ������ ���� �õ�
        });
    }

    public void SetInit(StoreData sd)
    {
        this.sd = sd;

        nameTxt.text = sd.name;
        goldText.text = sd.price.ToString() + "g";
    }
}
