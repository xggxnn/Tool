using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;

public class fguicode
{
    public static void codeInit(string outExplortPath, string copyToPath)
    {
        DirectoryInfo direction = new DirectoryInfo(outExplortPath + "/code");
        DirectoryInfo[] dirs = direction.GetDirectories();
        List<string> packNames = new List<string>();
        List<string> bindPath = new List<string>();
        List<string> bindNames = new List<string>();
        for (int j = 0; j < dirs.Length; j++)
        {
            packNames.Add(dirs[j].Name);
            console.log(dirs[j].Name);
            string windPath = copyToPath + "/src/gamemodule/Windows/" + dirs[j].Name + "Win.ts";

            string dirPath = copyToPath + "/src/fgui/Generates/" + dirs[j].Name;
            string dirPathExtends = copyToPath + "/src/fgui/Extend/" + dirs[j].Name;


            FileInfo[] files = dirs[j].GetFiles("*", SearchOption.TopDirectoryOnly);
            List<string> subName = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.Contains("fui_"))
                {
                    string result2 = files[i].Name.Remove(0, 4);
                    // 类名称
                    string result = result2.Remove(result2.LastIndexOf("."));
                    string txt = File.ReadAllText(files[i].FullName);
                    // 复制fgui生成的code到laya文件夹
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    files[i].CopyTo(dirPath + "/" + files[i].Name, true);
                    if (!Directory.Exists(dirPathExtends))
                    {
                        Directory.CreateDirectory(dirPathExtends);
                    }
                    // 继承文件是否存在存在
                    if (!File.Exists(dirPathExtends + "/" + "UI_" + result + ".ts"))
                    {
                        string importName = files[i].Name.Remove(files[i].Name.LastIndexOf("."));
                        string exts = extendsTs(importName, dirs[j].Name, "UI_" + result);
                        StringWriter idSW2 = new StringWriter();
                        idSW2.WriteLine(exts);
                        File.WriteAllText(dirPathExtends + "/" + "UI_" + result + ".ts", idSW2.ToString());
                    }
                    subName.Add("UI_" + result);
                }
            }
            // window文件是否存在
            if (!File.Exists(windPath))
            {
                if (subName.Count > 0)
                {
                    string winFileTxt = createWinTxt(dirs[j].Name + "Win", subName, dirs[j].Name);
                    StringWriter idSW2 = new StringWriter();
                    idSW2.WriteLine(winFileTxt);
                    File.WriteAllText(windPath, idSW2.ToString());
                }
            }
        }
        string con = packNamesTs(packNames);
        StringWriter idSW3 = new StringWriter();
        idSW3.WriteLine(con);
        string resPackPath = copyToPath + "/src/fgui/GuiPackageNames.ts";
        File.WriteAllText(resPackPath, idSW3.ToString());
    }

    static string extendsTs(string importName, string dirName, string clasName)
    {
        string winName = dirName + "Win";
        string note = "import " + importName + " from \"../../Generates/" + dirName + "/" + importName + "\";\n";
        note += "import " + winName + " from \"../../../gamemodule/Windows/" + winName + "\";\n\n";
        note += "/** 此文件自动生成，可以直接修改，后续不会覆盖 **/\n";
        note += "export default class " + clasName + " extends " + importName + " {\n";
        note += "\n	moduleWindow: " + winName + ";\n\n";
        note += "	public static DependPackages: string[] = [\"" + dirName + "\"];\n";
        note += "\n";
        note += "	public static createInstance(): " + clasName + " { \n";
        note += "		return <" + clasName + ">(" + importName + ".createInstance());\n";
        note += @"	}
";
        note += "	public static bind(): void {\n";
        note += "		fairygui.UIObjectFactory.setPackageItemExtension(" + clasName + ".URL, " + clasName + ");\n";
        note += @"	}
";
        note += @"
	protected constructFromXML(xml: any): void {
		super.constructFromXML(xml);
        // 此处可以引入初始化信息，比如初始化按钮点击，相当于awake()
        // ToDo

	}

	// 关闭ui
	closeUI(): void {
		this.moduleWindow.menuClose();
	}
	// 返回上一级ui
	backUI(): void {
		this.moduleWindow.menuBack();
	}
	// 显示，相当于enable
	onWindowShow(): void {

    }
    // 关闭时调用，相当于disable
    onWindowHide(): void {

    }


}
";
        note += clasName + ".bind();";
        return note;
    }

    static string createWinTxt(string names, List<string> className, string dirName)
    {
        string note = "import FWindow from \"../FWindow\";\n";
        string contentName = className[0];
        console.log(dirName + "Main");
        for (int i = 0; i < className.Count; i++)
        {
            note += "import " + className[i] + " from \"../../fgui/Extend/" + dirName + "/" + className[i] + "\";\n";
            if (className[i].Remove(0,3).Equals(dirName + "Main"))
            {
                contentName = className[i];
            }
        }
        note += "/** 此文件自动生成，可以直接修改，后续不会覆盖 **/\n";
        note += "export default class " + names + " extends FWindow {\n";
        note += "	content: " + contentName + ";\n\n";
        for (int i = 0; i < className.Count; i++)
        {
            if (!className[i].Equals(contentName))
            {
                string temstr = className[i].Remove(0, 3);
                note += "	" + temstr + ": " + className[i] + ";\n";
            }
        }
        note += @"	constructor() {
		super();
";
        for (int i = 0; i < className.Count; i++)
        {
            note += "		this.addAssetForFguiComponent(" + className[i] + ");\n";
        }
        note += @"	}
	protected onMenuCreate(): void {";
        note += "\n		this.content = " + contentName + ".createInstance();\n";
        note += @"		this.contentPane = this.content;
		super.onMenuCreate();
	}
}";
        return note;
    }

    static string packNamesTs(List<string> packNames)
    {
        string note = @"/**
 * fgui 包名称
 */
export default class GuiPackageNames {
";
        for (int i = 0; i < packNames.Count; i++)
        {
            note += "	public static " + packNames[i] + ": string = \"" + packNames[i] + "\"; \n";
        }
        note += "}";
        return note;
    }







}
