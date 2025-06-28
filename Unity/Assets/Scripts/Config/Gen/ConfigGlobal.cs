using System.IO;
using cfg;
using SimpleJSON;

public static class Config
{
    private static Tables _tables;
    private static string configPath = "Assets/Scripts/Config/Gen/Luban";

    public static Tables Tables
    {
        get
        {
            if (_tables == null)
            {
                _tables = new Tables(file => JSON.Parse(File.ReadAllText($"{configPath}/{file}.json")));
            };
            return _tables;
        }
    }
}
