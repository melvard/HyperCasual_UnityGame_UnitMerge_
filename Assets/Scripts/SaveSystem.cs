using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    //private static string fileName = "PlayerData.json";
    private static string path = Application.persistentDataPath+ "/PlayerData.json";
    //private static string fullPath = Path.Combine(saveDataDirectory, fileName);

    public static void SaveData(PlayerData pd)
    {
        Debug.Log("Player data is saved");
        string text = JsonUtility.ToJson(pd);
        File.WriteAllText(path, text);

    }

    public static PlayerData LoadData()
    {
        Debug.Log($"Player data is loaded");

        PlayerData pd = null;
        if (!File.Exists(path))
        {
            //pd = new PlayerData();
        }
        else
        {
            string text = File.ReadAllText(path);
            pd = JsonUtility.FromJson<PlayerData>(text);
        }

        return pd;
    }


    public static void SaveDataWithNewtonSoft(PlayerData pd)
    {
    }
}
