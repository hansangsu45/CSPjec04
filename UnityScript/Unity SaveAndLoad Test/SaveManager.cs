using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

public class UICvsPopbar : MonoBehaviour  //Test Code
{
    private string saveDataPath;
    [SerializeField] DataContainer data;

    

    public List<ISerializable> objToSaveList;

    private void Awake()
    {
        saveDataPath = GetFilePath("SaveFile");
        data = new DataContainer();

        //GetEnemyObjs();
        objToSaveList = new List<ISerializable>();
    }

   
    [System.Serializable]
    private class DataContainer
    {
        public string _name;
        public int _level;
        
        public DataContainer(string name, int level)
        {
            _name = name;
            _level = level;
        }
        public DataContainer()
        {

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            

            JObject jSaveGame = new JObject();
            for (int i = 0; i < objToSaveList.Count; i++)
            {
                jSaveGame.Add(objToSaveList[i].GetJsonKey(), objToSaveList[i].Serialize());
            }


           /*StreamWriter sw = new StreamWriter(GetFilePath("SaveFile01"));
            sw.WriteLine(jSaveGame.ToString());
            sw.Close();*/

            byte[] encryptedSavegame = Encrypt(jSaveGame.ToString());
            File.WriteAllBytes(GetFilePath("SaveFile01"), encryptedSavegame);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            string fileStr = GetFilePath("SaveFile01");
            if(File.Exists(fileStr))
            {
                byte[] decryptedSavegame = File.ReadAllBytes(GetFilePath("SaveFile01"));
                string jString = Decrypt(decryptedSavegame);

                /*StreamReader sr = new StreamReader(fileStr);
                string jString = sr.ReadToEnd();
                sr.Close();*/

                JObject jSaveGame = JObject.Parse(jString);
                for(int i=0; i<objToSaveList.Count; i++)
                {
                    string objJsonString = jSaveGame[objToSaveList[i].GetJsonKey()].ToString();
                    objToSaveList[i].Deserialize(objJsonString);
                }
            }
            else
            {
                print("Savefile is null");
                //새로운 파일을 생성
            }
        }

        #region SaveAndLoad 1
        /*if(Input.GetKeyDown(KeyCode.S))
        {
            //DataContainer dc = new DataContainer("name", 1);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(saveDataPath, FileMode.OpenOrCreate);

            bf.Serialize(fs, data);
            fs.Close();

            
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(saveDataPath, FileMode.Open);

            //DataContainer dc = bf.Deserialize(fs) as DataContainer;
            data = bf.Deserialize(fs) as DataContainer;
        }*/
        #endregion
    }

    string GetFilePath(string path)
    {
        return string.Concat(Application.persistentDataPath, "/", path);
    }


    //암호화
    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
    byte[] _initVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    byte[] Encrypt(string msg)
    {
        //여기부터
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _initVector);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(msg);

        cryptoStream.Close();
        streamWriter.Close();
        memoryStream.Close();

        //여기까지 중 어딘가에 오류

        return memoryStream.ToArray();
    }

    string Decrypt(byte[] msg)  //여기서도 오류 있음
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decryptor = aes.CreateEncryptor(_key, _initVector);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMsg = streamReader.ReadToEnd();

        cryptoStream.Close();
        streamReader.Close();
        memoryStream.Close();

        return decryptedMsg;
    }
}
