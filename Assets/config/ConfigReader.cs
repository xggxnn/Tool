using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigReader<T> : AbstractParseCsv, IConfigReader
{

    public char delimiter = '\t';

    public Dictionary<int, T> configs = new Dictionary<int, T>();
    protected ConfigCsvAttribute arrtr;

    virtual public void Load()
    {
        Type t = this.GetType();
        arrtr = t.GetCustomAttributes(typeof(ConfigCsvAttribute), false)[0] as ConfigCsvAttribute;
        ConfigSetting.Load(arrtr.assetName, ParseAsset);
    }

    virtual public void ParseAsset(string path, string txt)
    {
        if (txt == null)
        {
            Debug.LogWarningFormat("{0}: txt={1}, path={2}", this, txt, path);
            return;
        }
        StringReader sr = new StringReader(txt);

        string line;
        string[] csv;

        if (arrtr.hasHeadType)
        {
            line = sr.ReadLine();
            csv = line.Split(delimiter);
            ParseHeadTypes(csv);
        }

        line = sr.ReadLine();
        csv = line.Split(delimiter);
        ParseHeadKeyCN(csv);

        line = sr.ReadLine();
        csv = line.Split(delimiter);
        ParseHeadKeyEN(csv);

        if (arrtr.hasHeadPropId)
        {
            line = sr.ReadLine();
            csv = line.Split(delimiter);
            ParseHeadPropId(csv);
        }

        while (true)
        {
            line = sr.ReadLine();
            if (line == null)
            {
                break;
            }

            csv = line.Split(delimiter);
            if (csv.Length != 0 && !string.IsNullOrEmpty(csv[0]))
            {
                ParseCsv(csv);
            }
        }

        sr.Dispose();
    }

    virtual public void Reload()
    {
        configs.Clear();
        Load();
    }

    virtual public void OnGameConfigLoaded()
    {
    }

    public T GetConfig(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        Debug.LogWarningFormat("没有找到配置{0}, id={1}", typeof(T), id);

        return default(T);
    }

}