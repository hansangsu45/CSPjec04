
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SaveEnemy : MonoBehaviour, ISerializable  //Test Code
{
    public int hp = 1000;
    public string _name = "enemy1";

    private void Start()
    {
        FindObjectOfType<SaveManager>().objToSaveList.Add(this);
    }

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject returnVal = JObject.Parse(jsonString);
        return returnVal;
    }

    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }

    public string GetJsonKey()
    {
        return this.gameObject.name;
    }
}
