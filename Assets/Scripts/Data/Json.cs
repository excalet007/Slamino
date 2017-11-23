using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;
using System.IO;


/// <summary>
/// this class writes and reads data( Highest score, Played time, Game Option)  
/// refer : http://geekcoders.tistory.com/entry/Unity-JsonFX-%EC%82%AC%EC%9A%A9%ED%95%98%EA%B8%B0
/// </summary>
public class Json {
    public static string Write(Dictionary<string, object> dic)
    {
        return JsonWriter.Serialize(dic);
    }

    public static Dictionary<string,object> Read(string json)
    {
        return JsonReader.Deserialize<Dictionary<string, object>>(json);
    }

    /// <summary>
    /// data, score
    /// </summary>
    /// <param name="strJson"></param>
    /// <param name="fileName"></param>
    public static void Save(string fileName, string strJson)
    {
        File.WriteAllText(Application.persistentDataPath +"/" + fileName + ".json", strJson);
    }

    public static string Load(string fileName)
    {
        return File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json");
    }

    public static bool Check_Exsits(string fileName)
    {
        return File.Exists(Application.persistentDataPath + "/" + fileName + ".json");
    }
}
