using System.IO;
using SimpleJSON;
using UnityEngine;

public class ConfigGlobal : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        string gameConfDir = "Assets/Scripts/Config/Gen/Luban";
        var tables = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
    }

    
}
