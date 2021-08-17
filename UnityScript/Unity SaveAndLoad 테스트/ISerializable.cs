
using UnityEngine;
using Newtonsoft.Json.Linq;

public interface ISerializable  //Test Code
{
    JObject Serialize();
    void Deserialize(string jsonString);
    string GetJsonKey();
}
