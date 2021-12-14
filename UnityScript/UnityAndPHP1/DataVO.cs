using System;

[Serializable]
public class LoginData  //�α����ϸ� �� Ŭ������ �����͸� �޴´�
{
    public int money;
    public int id;
    public ItemType[] item_type;
}

[Serializable]
public class ItemType  //�������� Ÿ�� �ޱ����� Ŭ���� (���� �� �迭�� ����)
{
    public int id;
    public string type_name;  //������ ���� �̸� (��: ����, ��)
}

[Serializable]
public class UserData  //������ ������
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
public class StoreData  //�α��� �Ŀ� ���� ������ �����ϸ� �ش� ������ �����۵��� ��� �ޱ����� Ŭ����. left join�� ���ؼ� type_name���� ������
{   //�� Ŭ������ �ڽ��� ������ ��� ������ ���� ����
    public int id;
    public string name;
    public int type;
    public int price;
    public string type_name;
}

[Serializable]
public class StoreInfo //���� �����۵��� ������ �� �迭�� �޴´�
{
    public StoreData[] store_data;
}
