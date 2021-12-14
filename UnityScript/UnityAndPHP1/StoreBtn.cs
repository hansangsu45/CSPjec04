using UnityEngine;
using UnityEngine.UI;

public class StoreBtn : MonoBehaviour  //상점 종류 선택 버튼에 붙는 스크립트
{
    private int _type;
    private string _name;

    public void SetInit(int id, string _name)
    {
        _type = id;
        this._name = _name;

        transform.GetChild(0).GetComponent<Text>().text = this._name;
        GetComponent<Button>().onClick.AddListener(() => GameManager.instance.OnClickStoreBtn(_type));
    }
}
