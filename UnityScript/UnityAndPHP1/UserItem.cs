using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour  //������ ������ ����Ʈ���� ������ UI �ϳ����� �ٴ� ��ũ��Ʈ
{
    public Text itemTypeText, itemNameText;
    public StoreData sd;

    public void Init(StoreData sd)
    {
        this.sd = sd;

        itemTypeText.text = sd.type_name;
        itemNameText.text = sd.name;
    }
}
