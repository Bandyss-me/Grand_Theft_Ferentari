using UnityEngine;
using System;
using System.IO;
using TMPro;

public class data_saving : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI money_text;
    
    player_data data;
    string path;
    
    void Start()
    {
        data = new player_data();
        path = Path.Combine(Application.persistentDataPath, "player_data.json");
        Load();
        GetComponent<player_raycast>().data = data;
    }

    void LateUpdate()
    {
        money_text.text = "RON: " + data.money.ToString();
    }

    public void Reset()
    {
        data.money = 0f;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<player_data>(json);
        }
    }
}
