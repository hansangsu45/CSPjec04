using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour  //유저의 아이템 리스트에서 아이템 UI 하나마다 붙는 스크립트
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
