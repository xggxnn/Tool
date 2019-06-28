using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;

public class ConfigCSV
{
    public static void csvInit(string path)
    {
        DirectoryInfo direction = new DirectoryInfo("E:/netWork/csv");
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        int createNum = 0;
        int repeatNum = 0;
        List<string> importPath = new List<string>();
        List<string> packNames = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            string infName = files[i].Name.Remove(files[i].Name.LastIndexOf(".")) + "Info";
            infName = infName.Substring(0, 1).ToUpper() + infName.Substring(1);
            string windPath = path + "/src/dataInfo/" + infName + ".ts";
            packNames.Add(infName);
            importPath.Add("import " + infName + " from \"./" + infName + "\";\n");
            //if (File.Exists(windPath))
            //{
            //    repeatNum++;
            //    continue;
            //}
            createNum++;
            string note = "import Dictionary from \"../Tool/Dictionary\"; \n\n";
            note += "export default class " + infName + " {\n";
            note += @"
    private static infDic: Dictionary<string, Dictionary<string, any>> = new Dictionary<string, Dictionary<string, any>>();
    private curInf: Dictionary<string, any> = new Dictionary<string, any>();

";

            string txt = File.ReadAllText(files[i].FullName, Encoding.Default);
            StringReader sr = new StringReader(txt);
            string lines2 = sr.ReadLine();
            string lines = sr.ReadLine();
            string[] st2 = lines2.Split(',');
            string[] st = lines.Split(',');
            for (int j = 0; j < st.Length; j++)
            {
                note += "    /**\n";
                note += "     * " + st2[j] + "\n";
                note += "     */\n";
                note += "    public get " + st[j] + "(): any {\n";
                note += "        return this.curInf.getValue(\"" + st[j] + "\");\n";
                note += @"    }
";
            }

            note += "    constructor(id: string) {\n";
            note += "        this.curInf = " + infName + ".infDic.getValue(id);\n";
            note += "    }\n";

            note += @"
    /**
     * 依据id获得配置信息
     * @param id 配置id
     */
";
            note += "    public static getInfo(id: any): " + infName + " {\n";
            note += "        let ids: string = String(id);\n";
            note += "        if (this.infDic.hasKey(ids)) {\n";
            note += "            return new " + infName + "(ids);";
            note += @"
        }
        return null;
    }
";


            note += "    private static infList: " + infName + "[] = null;\n";
            note += "    public static getList(): Array<" + infName + "> {\n";
            note += @"        if (this.infList == null) {
            let list: string[] = this.infDic.getKeys();
            this.infList = [];
            for (let i = 0, len = list.length; i < len; i++) { 
";
            note += "                this.infList.push(new " + infName + "(list[i]));\n";
            note += @"            }
        }
        return this.infList;
    }";


            note += @"
    static init() {
        this.infDic = new Dictionary<string, Dictionary<string, any>>();
";
            while (true)
            {
                string linesss = sr.ReadLine();
                if (linesss == null)
                {
                    break;
                }
                else
                {
                    string[] csv = linesss.Split(',');
                    if (csv.Length > 0)
                    {
                        int id = int.Parse(csv[0]);
                        note += "        let dic" + id + " = new Dictionary<string, any>();\n";
                        for (int k = 0; k < csv.Length; k++)
                        {
                            note += "        dic" + id + ".add(\"" + st[k] + "\", \"" + csv[k] + "\");\n";
                        }
                        note += "        this.infDic.add(\"" + id + "\", dic" + id + ");\n";
                    }
                }
            }
            sr.Dispose();

            note += @"    }";
            note += @"
    
}";
            StringWriter idSW = new StringWriter();
            idSW.WriteLine(note);
            File.WriteAllText(windPath, idSW.ToString());
        }
        string allPath = path + "/src/dataInfo/CSVConfig.ts";
        initConfTs(packNames, importPath, allPath);
        console.log("配置文件生成完毕，重复数量：" + repeatNum, "新生成数量：" + createNum);
    }

    static void initConfTs(List<string> packNames, List<string> importPath, string path)
    {
        string note = "";
        for (int i = 0; i < importPath.Count; i++)
        {
            note += importPath[i];
        }
        note += @"
// 会自动覆盖
export default class CSVConfig {
	static InitAll(): void {
";
        note += "\n";
        for (int i = 0; i < packNames.Count; i++)
        {
            note += "		" + packNames[i] + ".init();\n";
        }
        note += @"
	}
}";
        StringWriter idSW3 = new StringWriter();
        idSW3.WriteLine(note);
        File.WriteAllText(path, idSW3.ToString());
    }

}
