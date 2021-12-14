using UnityEngine;
using UnityEngine.UI;

public class StoreBtn : MonoBehaviour  //���� ���� ���� ��ư�� �ٴ� ��ũ��Ʈ
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
