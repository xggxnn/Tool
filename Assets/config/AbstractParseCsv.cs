using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractParseCsv : IParseCsv
{
    public Dictionary<int, string> headTypes = new Dictionary<int, string>();
    public Dictionary<string, int> headKeyEns = new Dictionary<string, int>();
    public Dictionary<int, string> headKeyFields = new Dictionary<int, string>();
    public Dictionary<int, string> headKeyCns = new Dictionary<int, string>();
    public Dictionary<int, int> headPropIds = new Dictionary<int, int>();

    virtual public void ParseHeadTypes(string[] csv)
    {
        string key;
        for (int i = 0; i < csv.Length; i++)
        {
            key = csv[i];
            if (!string.IsNullOrEmpty(key))
            {
                key = key.Trim();
                headTypes.Add(i, key);
            }
        }
    }

    virtual public void ParseHeadKeyCN(string[] csv)
    {
        string key;
        for (int i = 0; i < csv.Length; i++)
        {
            key = csv[i];
            if (!string.IsNullOrEmpty(key))
            {
                key = key.Trim();
                headKeyCns.Add(i, key);
            }
        }
    }

    virtual public void ParseHeadKeyEN(string[] csv)
    {
        string key;
        for (int i = 0; i < csv.Length; i++)
        {
            key = csv[i];
            if (!string.IsNullOrEmpty(key))
            {
                key = key.Trim();

                if (headKeyEns.ContainsKey(key))
                {
                    Debug.LogWarningFormat("{0}: ParseHeadKeyEN 已经存在key= {1},  i = {2} ", this, key, i);
                }
                headKeyEns.Add(key, i);
                headKeyFields.Add(i, key);
            }
        }
    }

    virtual public void ParseHeadPropId(string[] csv)
    {
        for (int i = 0; i < csv.Length; i++)
        {
            if (string.IsNullOrEmpty(csv[i]))
                continue;

            headPropIds.Add(i, csv.GetInt32(i));
        }
    }

    virtual public void ParseCsv(string[] csv)
    {
    }

    virtual public string GetHeadField(int index)
    {
        return headKeyFields[index];
    }

    virtual public int GetHeadIndex(string enName)
    {
        if (headKeyEns.ContainsKey(enName))
        {
            return headKeyEns[enName];
        }

        Debug.LogWarningFormat("{0}: headKeyEns[{1}] = -1", this, enName);
        return -1;
    }

    virtual public int GetHeadPropId(int index)
    {
        if (headPropIds.ContainsKey(index))
        {
            return headPropIds[index];
        }
        return -1;
    }
}