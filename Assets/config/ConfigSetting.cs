using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ConfigSetting
{


    public static void Load(string path, Action<string, string> call)
    {
        if (call != null)
        {
            call(path, LoadConfig(path));
        }
    }

    public static string LoadConfig(string filename)
    {
        filename = EditorGetConfigPath(filename);
        return File.ReadAllText(filename);
    }

    public static string EditorGetConfigPath(string path)
    {

        //path = path.Replace("Config/", EditorRoot.Config + "/");
        string result = path + ".csv";
        if (File.Exists(result))
            return result;


        result = path + ".json";
        if (File.Exists(result))
            return result;

        result = path + ".txt";
        if (File.Exists(result))
            return result;

        return path;
    }
}