using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class FguiToLaya : EditorWindow
{
    [MenuItem("LYJ/Toos2")]
    public static void OpenEditor()
    {
        editor = GetWindow<FguiToLaya>("FguiToLaya");
        editor.minSize = new Vector2(900, 500);
        editor.Show();
    }
    static FguiToLaya editor;

    public static string outExplortPath = "D:/Tower2Fgui/outExplort";
    public static string copyToPath = "D:/Tower2/Tower2";
    public static string soundPath = "D:/Tower2Fgui/assets/Sound";
    public static string fSpritePath = "D:/Tower2Fgui/assets/fSprite";

    void OnGUI()
    {
        EditorGUILayout.LabelField("fgui导出文件所在的绝对路径：");
        outExplortPath = EditorGUILayout.TextField(outExplortPath);
        EditorGUILayout.LabelField("fgui声音文件所在的绝对路径：");
        soundPath = EditorGUILayout.TextField(soundPath);
        EditorGUILayout.LabelField("fgui图片资源文件所在的绝对路径：");
        fSpritePath = EditorGUILayout.TextField(fSpritePath);
        EditorGUILayout.LabelField("laya项目目录文件所在的绝对路径：");
        copyToPath = EditorGUILayout.TextField(copyToPath);

        if (GUILayout.Button("复制fgui资源", GUILayout.Height(30)))
        {
            outExplortPath = outExplortPath.Replace('\\', '/');
            copyToPath = copyToPath.Replace('\\', '/');
            startCope();
        }
        if (GUILayout.Button("复制fguiCode文件", GUILayout.Height(30)))
        {
            outExplortPath = outExplortPath.Replace('\\', '/');
            copyToPath = copyToPath.Replace('\\', '/');
            //startCope2();
            fguicode.codeInit(outExplortPath, copyToPath);
        }
        if (GUILayout.Button("复制fgui声音文件", GUILayout.Height(30)))
        {
            soundPath = soundPath.Replace('\\', '/');
            copyToPath = copyToPath.Replace('\\', '/');
            startCopeSound();
        }
        if (GUILayout.Button("复制fgui图片文件", GUILayout.Height(30)))
        {
            fSpritePath = fSpritePath.Replace('\\', '/');
            copyToPath = copyToPath.Replace('\\', '/');
            fguiSprite.createSprite(fSpritePath, copyToPath);
        }
        if (GUILayout.Button("生成可加载文件列表", GUILayout.Height(30)))
        {
            copyToPath = copyToPath.Replace('\\', '/');
            startCopereResInf();
        }
        if (GUILayout.Button("生成配置文件", GUILayout.Height(30)))
        {
            copyToPath = copyToPath.Replace('\\', '/');
            startCopeConfigs();
        }
        if (GUILayout.Button("生成Proto协议及event文件", GUILayout.Height(30)))
        {
            copyToPath = copyToPath.Replace('\\', '/');
            startProtoResInf();
        }
        if (GUILayout.Button("生成csv文件", GUILayout.Height(30)))
        {
            copyToPath = copyToPath.Replace('\\', '/');
            ConfigCSV.csvInit(copyToPath);
        }
        if (GUILayout.Button("random", GUILayout.Height(30)))
        {
            randddd();
        }

    }

    void randddd()
    {
        int num = 0;
        while (num < 5)
        {
            Random.InitState(2382);
            console.log("::" + Random.Range(0, 10.0f));
            num++;
        }
    }

    void startCope()
    {
        DirectoryInfo direction = new DirectoryInfo(outExplortPath);
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        string picPath = copyToPath + "/bin/res/fgui";
        if (!Directory.Exists(picPath))
        {
            Directory.CreateDirectory(picPath);
        }
        string resPackPath = copyToPath + "/src/fgui/FGUIResPackageConfig.ts";
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        for (int i = 0; i < files.Length; i++)
        {
            console.log(files[i].FullName + "  " + files[i].Name);
            string filName = files[i].Name.Remove(files[i].Name.LastIndexOf("."));
            string keys = filName;
            if (filName.Contains("_"))
            {
                string[] str = filName.Split('_');
                keys = str[0];
                if (!dic.ContainsKey(keys))
                {
                    dic.Add(keys, new List<string>());
                }
                List<string> dicList = dic[keys];
                dicList.Add(files[i].Name);
                dic[keys] = dicList;
            }
            files[i].CopyTo(picPath + "/" + files[i].Name, true);
        }
        string con = packConfigTs(dic);
        StringWriter idSW = new StringWriter();
        idSW.WriteLine(con);
        File.WriteAllText(resPackPath, idSW.ToString());
        console.log("复制图片完成");
    }

    void startCope2()
    {

        DirectoryInfo direction = new DirectoryInfo(outExplortPath + "/code");
        DirectoryInfo[] dirs = direction.GetDirectories();
        List<string> packNames = new List<string>();
        List<string> bindPath = new List<string>();
        List<string> bindNames = new List<string>();
        for (int j = 0; j < dirs.Length; j++)
        {
            packNames.Add(dirs[j].Name);
            string bindNa = dirs[j].Name + "Binder";
            bindPath.Add("import " + bindNa + " from " + "\"./code/" + dirs[j].Name + "/" + bindNa + "\";\n");
            bindNames.Add(bindNa);
            string dirPath = copyToPath + "/src/fgui/code/" + dirs[j].Name;
            string dirPathExtends = copyToPath + "/src/fgui/Extends/" + dirs[j].Name;
            string windPath = copyToPath + "/src/gamemodule/Windows/" + dirs[j].Name + "Window.ts";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!Directory.Exists(dirPathExtends))
            {
                Directory.CreateDirectory(dirPathExtends);
            }
            FileInfo[] files = dirs[j].GetFiles("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.Contains("_"))
                {
                    string[] str = files[i].Name.Split('_');
                    string txt = File.ReadAllText(files[i].FullName);
                    string result = files[i].Name.Remove(files[i].Name.LastIndexOf("."));
                    string result2 = str[1].Remove(str[1].LastIndexOf("."));
                    txt = txt.Replace("default class " + result, "default class fgui_" + result2);
                    txt = txt.Replace(" from \"./", " from \"../../Extends/" + dirs[j].Name + "/");
                    string addList = "\n	public static DependPackages:string[] = [\"" + dirs[j].Name + "\"];\n\n";
                    addList += "	public constructor() {";
                    txt = txt.Replace("	public constructor() {", addList);
                    string strTop = "import " + result + " from \"../../Extends/" + dirs[j].Name + "/" + result + "\";\n";
                    StringWriter idSW = new StringWriter();
                    idSW.WriteLine(strTop + txt);
                    File.WriteAllText(dirPath + "/fgui_" + str[1], idSW.ToString());
                    // 继承文件是否存在存在
                    if (!File.Exists(dirPathExtends + "/" + files[i].Name))
                    {
                        string imports = "fgui_" + str[1];
                        string importName = imports.Remove(imports.LastIndexOf("."));
                        string exts = extendsTs(importName, dirs[j].Name, result);
                        StringWriter idSW2 = new StringWriter();
                        idSW2.WriteLine(exts);
                        File.WriteAllText(dirPathExtends + "/" + files[i].Name, idSW2.ToString());
                    }
                    // window文件是否存在
                    if (!File.Exists(windPath))
                    {
                        string winFileTxt = createWinTxt(dirs[j].Name + "Window", result, dirs[j].Name);
                        StringWriter idSW2 = new StringWriter();
                        idSW2.WriteLine(winFileTxt);
                        File.WriteAllText(windPath, idSW2.ToString());
                    }
                }
                else
                {
                    string txt = File.ReadAllText(files[i].FullName);
                    txt = txt.Replace("./UI_", "../../Extends/" + dirs[j].Name + "/UI_");
                    StringWriter idSW = new StringWriter();
                    idSW.WriteLine(txt);
                    File.WriteAllText(dirPath + "/" + files[i].Name, idSW.ToString());
                }
            }
            console.log("复制文件完成:" + dirs[j].Name);
        }
        string con = packNamesTs(packNames);
        StringWriter idSW3 = new StringWriter();
        idSW3.WriteLine(con);
        string resPackPath = copyToPath + "/src/fgui/GuiPackageNames.ts";
        File.WriteAllText(resPackPath, idSW3.ToString());
        bindAllTs(bindNames, bindPath);
    }

    void bindAllTs(List<string> packNames, List<string> importPath)
    {
        string note = "";
        for (int i = 0; i < importPath.Count; i++)
        {
            note += importPath[i];
        }
        note += @"
// 会自动覆盖
export default class FguiBinderAll {
	static bindAll(): void {
";
        note += "\n";
        for (int i = 0; i < packNames.Count; i++)
        {
            note += "		" + packNames[i] + ".bindAll();\n";
        }
        note += @"
	}
}";
        StringWriter idSW3 = new StringWriter();
        idSW3.WriteLine(note);
        string resPackPath = copyToPath + "/src/fgui/FguiBinderAll.ts";
        File.WriteAllText(resPackPath, idSW3.ToString());
    }

    string packNamesTs(List<string> packNames)
    {
        string note = @"/**
 * fgui 包名称
 */
export class GuiPackageNames {
";
        for (int i = 0; i < packNames.Count; i++)
        {
            note += "	public static " + packNames[i] + ": string = \"" + packNames[i] + "\"; \n";
        }
        note += @"
}";
        return note;
    }

    string extendsTs(string importName, string dirName, string clasName)
    {
        string winName = dirName + "Window";
        string note = "import " + importName + " from \"../../code/" + dirName + "/" + importName + "\";\n";
        note += "import Game from \"../../../Game\";\n";
        note += "import " + winName + " from \"../../../gamemodule/Windows/" + winName + "\";\n\n";
        note += "/** 此文件自动生成，可以直接修改，后续不会覆盖 **/\n";
        note += "export default class " + clasName + " extends " + importName + " {\n";
        note += "\n	moduleWindow: " + winName + ";\n";
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


}";
        return note;
    }

    string createWinTxt(string names, string className, string dirName)
    {
        string note = "import FWindow from \"../FWindow\";\n";
        note += "import " + className + " from \"../../fgui/Extends/" + dirName + "/" + className + "\";\n";
        note += "/** 此文件自动生成，可以直接修改，后续不会覆盖 **/\n";
        note += "export default class " + names + " extends FWindow {\n";
        note += "	content: " + className + ";\n";
        note += @"	constructor() {
		super();";
        note += "\n		this.addAssetForFguiComponent(" + className + ");\n";
        note += @"	}
	protected onMenuCreate(): void {";
        note += "\n		this.content = " + className + ".createInstance();\n";
        note += @"		this.contentPane = this.content;
		super.onMenuCreate();
	}
}";
        return note;
    }

    string packConfigTs(Dictionary<string, List<string>> dic)
    {
        string note = "import ResPackageConfig from \"./ResPackageConfig\";\n";
        note += "import Dictionary from \"../Tool/Dictionary\";\n";
        note += @"
// 此文件不要修改，会被覆盖
export default class FGUIResPackageConfig {

    static dict: Dictionary<string, ResPackageConfig> = new Dictionary<string, ResPackageConfig>();
    // 添加配置
    static addconfig(config: ResPackageConfig) {
        this.dict.add(config.packageName, config);
    }

    // 获取配置
    static getconfig(packageName: string) {
        return this.dict.getValue(packageName);
    }

    static install() {

        let config: ResPackageConfig;
";

        foreach (var item in dic)
        {
            note += "\n\n\n";
            note += "        config = new ResPackageConfig();\n";
            note += "        config.packageName = \"" + item.Key + "\";\n";
            note += "        config.resDir = \"" + "fgui" + "\";\n";
            note += "        config.resBin = \"" + item.Key + ".bin\";\n";
            List<string> lis = item.Value;
            for (int i = 0; i < lis.Count; i++)
            {
                if (item.Key == "Sound")
                {
                    note += "        config.resSounds.push(\"" + lis[i] + "\");\n";
                }
                else
                {
                    note += "        config.resAtlas.push(\"" + lis[i] + "\");\n";
                }
            }
            note += "        this.addconfig(config);";
        }

        note += @"
    }
}";
        return note;
    }


    void startCopeConfigs()
    {
        DirectoryInfo direction = new DirectoryInfo(copyToPath + "/bin/res_code/csv/");
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);


        for (int i = 0; i < files.Length; i++)
        {
            console.log("dirs[j].Name:" + files[i].Name);
            string note = "import CSV from \"./CSV\";\n";
            string infName = files[i].Name.Remove(files[i].Name.LastIndexOf(".")) + "Info";
            note += "export default class " + infName + " { \n";
            string note2 = @"    private static _hash: Object = {};
    private static _list: Array<string> = [];

    /**
     * 安装csv文件
     */
    public static installCSV(csv: CSV): void {
        this._hash = {};
        var data = csv.getAllData();
        for (var id in data) {
            this._hash[id] = new aabbccddeeff(data[id]);
            this._list.push(id);
        }
    }
    /**
     * 获取数据数量
     */
    public static getCount(): number {
        return this._list.length;
    }
    /**
     * 通过id获取aabbccddeeff
     */
    public static getInfo(id: any): aabbccddeeff {
        id = String(id);
        if (this._hash.hasOwnProperty(id)) {
            return this._hash[id];
        }
        return null;
    }
    /**
     * 通过index获取aabbccddeeff
     */
    public static getInfoFromIndex(index: number): aabbccddeeff {
        try {
            let id = this._list[index];
            return this.getInfo(id);
        } catch (error) {
            return null;
        }
    }
    //构造函数
    private constructor(obj: Object) {
        //录入数据
";
            note += note2.Replace("aabbccddeeff", infName);
            string txt = File.ReadAllText(files[i].FullName);
            StringReader sr = new StringReader(txt);
            string lines = sr.ReadLine();
            string[] st = lines.Split(',');
            for (int j = 0; j < st.Length; j++)
            {
                note += "        this._" + st[j] + " = obj[\"" + st[j] + "\"];\n";
            }
            note += "    }\n";
            for (int j = 0; j < st.Length; j++)
            {
                string note3 = @"
    private _aabbccddeeff: string;
    public get aabbccddeeff(): string {
        return this._aabbccddeeff;
    }
    public set aabbccddeeff(v: string)
    {
        this._aabbccddeeff = v;
    }
";
                note += note3.Replace("aabbccddeeff", st[j]);
            }
            note += @"
}";

            StringWriter idSW = new StringWriter();
            idSW.WriteLine(note);
            File.WriteAllText(copyToPath + "/src/data/" + infName + ".ts", idSW.ToString());

        }
    }

    void startCopereResInf()
    {
        DirectoryInfo direction = new DirectoryInfo(copyToPath + "/bin/res/");
        DirectoryInfo[] dirs = direction.GetDirectories();
        string note = @"export default class LoadFilesList {
";

        List<string> funList = new List<string>();

        for (int i = 0; i < dirs.Length; i++)
        {
            if (dirs[i].Name != "fgui")
            {

                FileInfo[] files = dirs[i].GetFiles("*", SearchOption.TopDirectoryOnly);

                Dictionary<string, List<string>> filedict = new Dictionary<string, List<string>>();

                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j].Extension.Equals(".sk"))
                    {
                        string tt = "res/" + dirs[i].Name + "/" + files[j].Name;
                        string ss = "        _list.push(\"" + tt + "\");\n";

                        string result2 = files[j].Name.Remove(files[j].Name.LastIndexOf("_"));
                        List<string> _list = new List<string>();
                        if (filedict.ContainsKey(result2))
                        {
                            _list = filedict[result2];
                        }
                        _list.Add(ss);
                        filedict[result2] = _list;
                    }
                }

                foreach (var item in filedict)
                {
                    note += "\n    // " + dirs[i].Name + "文件夹内的" + item.Key + "类别的加载列表 \n";
                    string nae = dirs[i].Name + "_" + item.Key + "_ResList";
                    funList.Add(nae);
                    note += "    static get " + nae + "() {\n";
                    note += "        let _list: Array<string> = [];\n";
                    List<string> _list = item.Value;
                    for (int s = 0; s < _list.Count; s++)
                    {
                        note += _list[s];
                    }
                    note += @"        return _list;
    }
";
                }

            }
        }

        note += @"
    /**
     * 加载全部资源
     */
    static get allResList() {
        let _list: Array<string> = [];";
        note += "\n";
        for (int k = 0; k < funList.Count; k++)
        {
            note += "        _list = _list.concat(this." + funList[k] + "); \n";
        }
        note += @"        return _list;
    }
";

        note += @"
}";

        StringWriter idSW = new StringWriter();
        idSW.WriteLine(note);
        File.WriteAllText(copyToPath + "/src/Tool/LoadFilesList.ts", idSW.ToString());
        console.log("可加载列表生成完毕");
    }

    void startProtoResInf()
    {
        // 生成协议文件
        for (int i = 1001; i < 1021; i++)
        {
            string fileName = "Proto" + i;
            string windPath = copyToPath + "/src/protobuf/" + fileName + ".ts";
            if (!File.Exists(windPath))
            {
                string note2 = "import Proto from \"./Proto\";\n";
                note2 += "import Game from \"../Game\";\n\n";
                note2 += "export default class " + fileName + " extends Proto {\n";
                note2 += "    protected protoid: number = " + i + ";\n";
                note2 += @"
    public send<T>(data: T, encode: Function, decode: Function): void {
";
                note2 += "        data[\"protoid\"] = this.protoid;\n";
                note2 += "        data[\"openid\"] = Game.localStorage.openid;\n";
                note2 += "        data[\"password\"] = Game.localStorage.password;\n";
                note2 += @"        this.request(data, encode, decode);
    }
    // any == T
    protected read(json: any): void {
        
    }
}";

                StringWriter idSW2 = new StringWriter();
                idSW2.WriteLine(note2);
                File.WriteAllText(windPath, idSW2.ToString());
                console.log("协议列表生成完毕:" + windPath);
            }
        }


        DirectoryInfo direction = new DirectoryInfo(copyToPath + "/src/protobuf/");
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);

        List<string> nameList = new List<string>();
        List<string> numList = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            string result2 = files[i].Name.Remove(files[i].Name.LastIndexOf("."));
            if (result2.Length == 9 && result2 != "ProtoHash")
            {
                nameList.Add(result2);
                var ss = result2.Split('o');
                numList.Add(ss[2]);
            }
        }
        string note = "";
        for (int i = 0; i < nameList.Count; i++)
        {
            note += "import " + nameList[i] + " from \"./" + nameList[i] + "\";\n";
        }
        note += @"
export default class ProtoHash {
    static protoHash: Object = {
";
        for (int i = 0; i < nameList.Count; i++)
        {
            note += "        \"" + numList[i] + "\": new " + nameList[i] + "(),\n";
        }
        note += @"    }
}";

        StringWriter idSW = new StringWriter();
        idSW.WriteLine(note);
        File.WriteAllText(copyToPath + "/src/protobuf/ProtoHash.ts", idSW.ToString());
        console.log("协议列表生成完毕");
    }


    void startCopeSound()
    {
        string xmPath = soundPath + "/package.xml";
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

        string note = "import Fun from \"../Tool/Fun\";\n";
        note += "import Dictionary from \"../Tool/Dictionary\";\n";
        note += @"
export default class SoundKey {
    private static _idDict: Dictionary<string, string>;
    static get idDict(): Dictionary<string, string> {
        if (!SoundKey._idDict) {
            SoundKey.init();
        }
        return SoundKey._idDict;
    }


    private static _extDict: Dictionary<string, string>;
    static get extDict(): Dictionary<string, string> {
        if (!SoundKey._extDict) {
            SoundKey.init();
        }
        return SoundKey._extDict;
    }

    public static getId(key: string): string {
        if (!SoundKey.idDict.hasKey(key)) {
            console.error('SoundKey 不存在 key=' + key);
            return '';
        }
        return SoundKey.idDict.getValue(key);
    }

    public static getUrl(key: string): string {
        return `ui://${SoundKey.SoundPackageId}${SoundKey.getId(key)}`;
    }


    public static getPath(key: string): string {
        return Fun.getResPath(`${SoundKey.SoundPackageName}_${SoundKey.getId(key)}${SoundKey.extDict.getValue(key)}`, 'fgui');
    }


    private static init() {
        let dict = SoundKey._idDict = new Dictionary<string, string>();
";
        for (int i = 0; i < nameList.Count; i++)
        {
            note += "        dict.add(\"" + nameList[i] + "\", \"" + idList[i] + "\");\n";
        }
        note += "\n        let exts = SoundKey._extDict = new Dictionary<string, string>();\n";
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




        string resPackPath = copyToPath + "/src/fgui/SoundKey.ts";
        StringWriter idSW = new StringWriter();
        idSW.WriteLine(note);
        File.WriteAllText(resPackPath, idSW.ToString());
        console.log("声音文件配置完毕");

    }











}

class console
{
    public static void log(string str, string str2 = "")
    {
        Debug.Log(str + "   " + str2);
    }
}
