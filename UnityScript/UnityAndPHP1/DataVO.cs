using System;

[Serializable]
public class LoginData  //로그인하면 이 클래스로 데이터를 받는다
{
    public int money;
    public int id;
    public ItemType[] item_type;
}

[Serializable]
public class ItemType  //아이템의 타입 받기위한 클래스 (받을 땐 배열로 받음)
{
    public int id;
    public string type_name;  //아이템 종류 이름 (예: 무기, 방어구)
}

[Serializable]
public class UserData  //유저의 데이터
{
    public int id;
    public string loginId;
    public string pw;
    public int money;

    public UserData() { }

    public UserData(int id, string li, string pw, int m)
    {
        this.id = id;
        loginId = li;
        this.pw = pw;
        money = m;
    }
}

[Serializable]
public class StoreData  //로그인 후에 상점 종류를 선택하면 해당 종류의 아이템들을 모두 받기위한 클래스. left join을 통해서 type_name까지 가져옴
{   //이 클래스는 자신의 아이템 목록 가져올 때도 쓴다
    public int id;
    public string name;
    public int type;
    public int price;
    public string type_name;
}

[Serializable]
public class StoreInfo //상점 아이템들을 가져올 때 배열로 받는다
{
    public StoreData[] store_data;
}
