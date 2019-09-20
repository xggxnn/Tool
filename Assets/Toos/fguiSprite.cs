using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;

public class fguiSprite
{
    public static void createSprite(string path,string copyToPath)
    {
        string xmPath = path + "/package.xml";
        string txt = File.ReadAllText(xmPath);
        console.log(txt);
        var doc = new XmlDocument();
        doc.LoadXml(txt);
        var rowNoteList = doc.SelectNodes("/packageDescription/resources");
        List<string> nameList = new List<string>();
        List<string> idList = new List<string>();
        List<string> lastList = new List<string>();
        List<string> topList = new List<string>();
        if (rowNoteList != null)
        {
            foreach (XmlNode rowNode in rowNoteList)
            {
                var fieldNodeList = rowNode.ChildNodes;
                foreach (XmlNode courseNode in fieldNodeList)
                {
                    if (courseNode.Attributes != null)
                    {
                        string nam = courseNode.Attributes["name"].Value;
                        nameList.Add(nam);
                        idList.Add(courseNode.Attributes["id"].Value);
                        string[] result2 = nam.Split('.');
                        lastList.Add("." + result2[result2.Length - 1]);
                        topList.Add(nam.Remove(nam.LastIndexOf(".")));
                    }
                }
            }
        }
        var rowNoteId = doc.SelectSingleNode("/packageDescription");
        console.log(rowNoteId.Attributes["id"].Value);

        string note = "import Fun from \"../tool/Fun\";\n";
        note += "import Dictionary from \"../tool/Dictionary\";\n";
        note += @"
export default class SpriteKey {
    private static _idDict: Dictionary<string, string>;
    static get idDict(): Dictionary<string, string> {
        if (!SpriteKey._idDict) {
            SpriteKey.init();
        }
        return SpriteKey._idDict;
    }


    private static _extDict: Dictionary<string, string>;
    static get extDict(): Dictionary<string, string> {
        if (!SpriteKey._extDict) {
            SpriteKey.init();
        }
        return SpriteKey._extDict;
    }

    public static getId(key: string): string {
        if (!SpriteKey.idDict.hasKey(key)) {
            console.error('SpriteKey 不存在 key=' + key);
            return '';
        }
        return SpriteKey.idDict.getValue(key);
    }

    public static getUrl(key: string): string {
        return `ui://${SpriteKey.SoundPackageId}${SpriteKey.getId(key)}`;
    }


    public static getPath(key: string): string {
        return Fun.getResPath(`${SpriteKey.SoundPackageName}_${SpriteKey.getId(key)}${SpriteKey.extDict.getValue(key)}`, 'fgui');
    }


    private static init() {
        let dict = SpriteKey._idDict = new Dictionary<string, string>();
";
        for (int i = 0; i < nameList.Count; i++)
        {
            note += "        dict.add(\"" + nameList[i] + "\", \"" + idList[i] + "\");\n";
        }
        note += "\n        let exts = SpriteKey._extDict = new Dictionary<string, string>();\n";
        for (int i = 0; i < nameList.Count; i++)
        {
            note += "        exts.add(\"" + nameList[i] + "\", \"" + lastList[i] + "\");\n";
        }
        note += @"
    }

";
        var rowNoteId2 = doc.SelectSingleNode("/packageDescription/publish");
        note += "	static SoundPackageName = \"" + rowNoteId2.Attributes["name"].Value + "\";\n";
        note += "	static SoundPackageId = \"" + rowNoteId.Attributes["id"].Value + "\";\n\n";

        for (int i = 0; i < topList.Count; i++)
        {
            note += "	public static " + topList[i] + " = \"" + nameList[i] + "\";\n";
        }

        note += @"
    
}";




        string resPackPath = copyToPath + "/src/fgui/SpriteKey.ts";
        StringWriter idSW = new StringWriter();
        idSW.WriteLine(note);
        File.WriteAllText(resPackPath, idSW.ToString());
        console.log("图片文件配置完毕");
    }
}
