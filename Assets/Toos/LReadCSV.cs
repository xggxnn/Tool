using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public partial class LReadCSV : EditorWindow
{
    [MenuItem("LYJ/Toos")]
    public static void OpenEditor()
    {
        editor = GetWindow<LReadCSV>("Toos");
        editor.minSize = new Vector2(900, 500);
        editor.Show();
    }
    static LReadCSV editor;
    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("请输入文件名：");
        oldFile = EditorGUILayout.TextField(oldFile);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        //GUI.color = Color.red;
        if (GUILayout.Button("生成ts文件", GUILayout.Height(30)))
        {
            Generate();
        }
        if (GUILayout.Button("生成PROTO文件", GUILayout.Height(30)))
        {
            proto();
        }
        if (GUILayout.Button("生成protoevent文件", GUILayout.Height(30)))
        {
            protoEvent();
        }
        low = EditorGUILayout.TextField(low);
        if (low.Length > 0)
        {
            big = low.ToUpper();
        }
        GUILayout.TextField(big);
        if (GUILayout.Button("选择文件或文件夹", GUILayout.Height(30)))
        {
            Debug.Log("openPath "+openPath);
            select();
        }
        //Debug.Log("openPath " + openPath);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("生成php文件", GUILayout.Height(30)))
        {
            php();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ts文件所在的绝对路径：");
        clearDir = EditorGUILayout.TextField(clearDir);
        if (GUILayout.Button("清理无效的import引用", GUILayout.Height(30)))
        {
            clearDir = replaceX(clearDir);
            importClear();
        }



    }
    private string replaceX(string path)
    {
        return path.Replace('\\','/');
    }
    public string clearDir = "D:/tt/importclear";
    private void importClear()
    {
        clearReWrite(clearDir);
        //DirectoryInfo direction = new DirectoryInfo(clearDir);
        //FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        //for (int i = 0; i < files.Length; i++)
        //{
        //    if (files[i].Name.EndsWith(".ts") || files[i].Name.EndsWith(".TS") || files[i].Name.EndsWith(".Ts") || files[i].Name.EndsWith(".tS"))
        //    {
        //        clearReWrite(files[i].FullName);
        //    }
        //}
        //clearReWrite("D:/tt/importclear/ssdd/RecommendFriend.ts");
    }
    private void clearReWrite(string path) { 
        string txt = File.ReadAllText(path);
        List<string> tslines = ParseTsAsset(txt);

        StringWriter idSW = new StringWriter();
        string note = "";
        clearWithImportEmpty(ref tslines);
        for (int i = 0; i < tslines.Count; i++)
        {
            note += tslines[i];
            if (i < tslines.Count - 1)
            {
                note += "\n";
            }
        }
        idSW.WriteLine(note);

        string outPath = path;

        File.WriteAllText(outPath, idSW.ToString());
        Debug.Log(outPath + "创建成功");
    }
    /** 解析 */
    public List<string> ParseTsAsset(string txt)
    {
        StringReader sr = new StringReader(txt);
        List<string> tslines = new List<string>();
        string line;
        while (true)
        {
            line = sr.ReadLine();
            if (line == null)
            {
                break;
            }
            tslines.Add(line);
        }
        sr.Dispose();
        return tslines;
    }
    public void clearWithImportEmpty(ref List<string> list)
    {
        List<string> emptyClass = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            string[] st = list[i].Split(' ');
            //var str = "//s+";
            //string[] st = System.Text.RegularExpressions.Regex.Split(list[i], @"\s{1,}");

            //string[] st = System.Text.RegularExpressions.Regex.Split(list[i], @"\s{1,}");
            //string[] st = list[i].Split(str, @"\s{2,}");
            if (st.Length > 1 && st[0] == "import")
            {
                if (emptyClass.IndexOf(st[1]) == -1)
                {
                    emptyClass.Add(st[1]);
                }
            }
        }
        Dictionary<int, int> doubleNum = new Dictionary<int, int>();
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < emptyClass.Count; j++)
            {
                if (list[i].IndexOf(emptyClass[j]) >= 0)
                {
                    if (!doubleNum.ContainsKey(j))
                    {
                        doubleNum.Add(j, 0);
                    }
                    int num = doubleNum[j];
                    doubleNum[j] = num + 1;
                }
            }
        }
        foreach (var item in doubleNum)
        {
            if (item.Value == 1)
            {
                list.RemoveAt(item.Key);
            }
        }
    }

    private void php()
    {
        getss();
        for (int i = 1001; i <= 1054; i++)
        {
            StringWriter idSW = new StringWriter();
            string not = "<?php\n";
            not += "require 'errcode.php';\n";
            not += "\n";
            not += "// " + dicKey[i - 1001] + "\n";
            not += @"function read($req)
{
    $redis = new Redis();\n";
            not += "    if (!$redis->connect(\"" + "127.0.0.1" + "\", \"6379\")) {\n";
            not += @"        $json['error'] = 'redis is not connected!';
        return array('errcode' => 4003);
    } else {
        //$redis->auth(REDIS_KEY);
    }

    $json = json_decode($req, true);

    $openid = $json['openid'];
    $psw = $json['password'];

    $key = getOpenidKey($openid);\n";
            not += "\n";
            not += "    $userData['password'] = $redis->hGet($key, \"password\");\n";
            not += "\n";
            not += "   if ($userData[\"password\"] != $psw) {\n";
            not += @"        return $retData['errcode'] = ERR_CODE_4009;
    }";
            not += "\n\n";
            not += "//TODO";
            not += "\n\n";
            not += @"
}";

            idSW.WriteLine(not);
            string outPath = "D:/tt/Ts/php/proto" + i + ".php";

            File.WriteAllText(outPath, idSW.ToString());
            Debug.Log(outPath + "创建成功");
        }
    }

    private void select()
    {
        //openPath = EditorUtility.OpenFilePanel("titles","e:","");
        openPath = EditorUtility.OpenFolderPanel("选择文件夹", "e:","");
    }
    public string openPath = "";


    public List<string> dicKey = new List<string>();
    public List<string> dicVal = new List<string>();
    public void getss()
    {
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("登录", "login");
        //dic.Add("玩家完成新手引导后调用(一个账号只会调用一次)", "initPlay");
        //dic.Add("通关-----", "wavePass");
        //dic.Add("召唤祭坛抽卡", "card");
        //dic.Add("英雄升级", "heroUpgrade");
        //dic.Add("英雄升段", "heroUpSegment");
        //dic.Add("布阵", "setSeat");
        //dic.Add("选关", "selectWave");
        //dic.Add("领取每日登陆奖励", "loginReward");
        //dic.Add("领取资源补给箱", "box");
        //dic.Add("领取邀请奖励", "invite");
        //dic.Add("限时活动购买英雄", "active");
        //dic.Add("领取成就奖励", "achieve");
        //dic.Add("领取成就勋章奖励", "achieveMedal");
        //dic.Add("领取每日任务奖励", "task");
        //dic.Add("领取每日任务大奖", "taskBig");
        //dic.Add("领取离线挂机奖励", "offLine");
        //dic.Add("领取国王之路奖励", "king");
        //dic.Add("免费领奖", "free");
        //dic.Add("无尽时空挑战", "endless");
        //dic.Add("无尽时空过关", "endlessPass");
        //dic.Add("无尽时空领奖", "endlessReward");
        //dic.Add("图鉴技能升级", "mapSkill");
        //dic.Add("领取累充奖励", "total");
        //dic.Add("领取首充奖励", "firstPay");
        //dic.Add("领取每日充值奖励", "dayPay");
        //dic.Add("领取每周充值奖励", "weekPay");
        //dic.Add("充值", "pay");
        //dic.Add("口令兑换", "cdKey");
        //dic.Add("获取玩家资源数据", "playInf");
        //dic.Add("获取玩家其他数据", "playOther");
        //dic.Add("获取选关界面数据", "waveRefrush");
        //dic.Add("获取每日登陆数据", "everyRefrush");
        //dic.Add("获取邀请奖励数据", "inviteRefrush");
        //dic.Add("获取限时活动数据", "activeRefrush");
        //dic.Add("获取成就数据", "achieveRefrush");
        //dic.Add("获取每日任务数据", "taskRefrush");
        //dic.Add("获取资源补给箱数据", "boxRefrush");
        //dic.Add("获取离线挂机数据", "offLineRefrush");
        //dic.Add("获取国王之路数据", "kingRefrush");
        //dic.Add("获取免费领奖数据", "freeRefrush");
        //dic.Add("获取图鉴技能数据", "mapRefrush");
        //dic.Add("获取图鉴英雄数据", "heroInfRefrush");
        //dic.Add("获取图鉴怪物数据", "mapMonsterRefrush");
        //dic.Add("获取首充奖励数据", "firstRefrush");
        //dic.Add("获取累计充值数据", "totalRefrush");
        //dic.Add("获取每日充值数据", "dayRefrush");
        //dic.Add("获取每周充值数据", "weekRefrush");
        //dic.Add("获取无尽时空数据", "endlessRefrush");
        //dic.Add("获取布阵数据", "seatRefrush");
        //dic.Add("获取英雄详情数据", "heroInf");
        //dic.Add("加速", "addSpeed");
        //dic.Add("哥布林", "goblin");
        //dic.Add("商人", "npc");
        //return dic;
        dicKey = new List<string>();
        dicVal = new List<string>();
        dicKey.Add("登录"); dicVal.Add("login");
        dicKey.Add("玩家完成新手引导后调用(一个账号只会调用一次)"); dicVal.Add("initPlay");
        dicKey.Add("通关-----"); dicVal.Add("wavePass");
        dicKey.Add("召唤祭坛抽卡"); dicVal.Add("card");
        dicKey.Add("英雄升级"); dicVal.Add("heroUpgrade");
        dicKey.Add("英雄升段"); dicVal.Add("heroUpSegment");
        dicKey.Add("布阵"); dicVal.Add("setSeat");
        dicKey.Add("选关"); dicVal.Add("selectWave");
        dicKey.Add("领取每日登陆奖励"); dicVal.Add("loginReward");
        dicKey.Add("领取资源补给箱"); dicVal.Add("box");
        dicKey.Add("领取邀请奖励"); dicVal.Add("invite");
        dicKey.Add("限时活动购买英雄"); dicVal.Add("active");
        dicKey.Add("领取成就奖励"); dicVal.Add("achieve");
        dicKey.Add("领取成就勋章奖励"); dicVal.Add("achieveMedal");
        dicKey.Add("领取每日任务奖励"); dicVal.Add("task");
        dicKey.Add("领取每日任务大奖"); dicVal.Add("taskBig");
        dicKey.Add("领取离线挂机奖励"); dicVal.Add("offLine");
        dicKey.Add("领取国王之路奖励"); dicVal.Add("king");
        dicKey.Add("免费领奖"); dicVal.Add("free");
        dicKey.Add("无尽时空挑战"); dicVal.Add("endless");
        dicKey.Add("无尽时空过关"); dicVal.Add("endlessPass");
        dicKey.Add("无尽时空领奖"); dicVal.Add("endlessReward");
        dicKey.Add("图鉴技能升级"); dicVal.Add("mapSkill");
        dicKey.Add("领取累充奖励"); dicVal.Add("total");
        dicKey.Add("领取首充奖励"); dicVal.Add("firstPay");
        dicKey.Add("领取每日充值奖励"); dicVal.Add("dayPay");
        dicKey.Add("领取每周充值奖励"); dicVal.Add("weekPay");
        dicKey.Add("充值"); dicVal.Add("pay");
        dicKey.Add("口令兑换"); dicVal.Add("cdKey");
        dicKey.Add("获取玩家资源数据"); dicVal.Add("playInf");
        dicKey.Add("获取玩家其他数据"); dicVal.Add("playOther");
        dicKey.Add("获取选关界面数据"); dicVal.Add("waveRefrush");
        dicKey.Add("获取每日登陆数据"); dicVal.Add("everyRefrush");
        dicKey.Add("获取邀请奖励数据"); dicVal.Add("inviteRefrush");
        dicKey.Add("获取限时活动数据"); dicVal.Add("activeRefrush");
        dicKey.Add("获取成就数据"); dicVal.Add("achieveRefrush");
        dicKey.Add("获取每日任务数据"); dicVal.Add("taskRefrush");
        dicKey.Add("获取资源补给箱数据"); dicVal.Add("boxRefrush");
        dicKey.Add("获取离线挂机数据"); dicVal.Add("offLineRefrush");
        dicKey.Add("获取国王之路数据"); dicVal.Add("kingRefrush");
        dicKey.Add("获取免费领奖数据"); dicVal.Add("freeRefrush");
        dicKey.Add("获取图鉴技能数据"); dicVal.Add("mapRefrush");
        dicKey.Add("获取图鉴英雄数据"); dicVal.Add("heroInfRefrush");
        dicKey.Add("获取图鉴怪物数据"); dicVal.Add("mapMonsterRefrush");
        dicKey.Add("获取首充奖励数据"); dicVal.Add("firstRefrush");
        dicKey.Add("获取累计充值数据"); dicVal.Add("totalRefrush");
        dicKey.Add("获取每日充值数据"); dicVal.Add("dayRefrush");
        dicKey.Add("获取每周充值数据"); dicVal.Add("weekRefrush");
        dicKey.Add("获取无尽时空数据"); dicVal.Add("endlessRefrush");
        dicKey.Add("获取布阵数据"); dicVal.Add("seatRefrush");
        dicKey.Add("获取英雄详情数据"); dicVal.Add("heroInf");
        dicKey.Add("加速"); dicVal.Add("addSpeed");
        dicKey.Add("哥布林"); dicVal.Add("goblin");
        dicKey.Add("商人"); dicVal.Add("npc");
    }



    public string low = "";
    public string big = "";

    public void protoEvent()
    {
        StringWriter idSW = new StringWriter();
        string not = "import Proto from \"./Proto\";\n";
        for (int i = 1001; i < 1054; i++)
        {
            not += "import Proto" + i + " from \"./Proto" + i + "\";\n";
        }
        not += "\n\n";
        not += "\n\n";
        for (int i = 1001; i < 1054; i++)
        {
            not += "\"" + i + "\": new Proto" + i + "(),\n";
        }
        not += "\n\n";
        not += "\n\n";

        for (int i = 1001; i < 1054; i++)
        {
            not += "    public static get " + i + " _CALL_BACK(): string {\n";
            not += "        return this.getProtoEvent(" + i + ");\n";
            not += "    }\n";
        }

        not += "\n\n";
        not += "\n\n";
        getss();
        //Debug.Log("dicKey.Count:" + dicKey.Count);
        for (int i = 1001; i <= 1054; i++)
        {
            if (dicKey.Count >= i - 1014)
            {
                not += "    /**\n";
                not += "     * " + dicKey[i - 1001] + "\n";
                not += "     * @param data\n";
                not += "     */\n";
                not += "    public static " + dicVal[i - 1001] + "(data: Object): void {\n";
                not += "        let pro: Proto = this.getProto(" + i + ");\n";
                not += "        pro.send(data);\n";
                not += "    }\n";
            }
        }


        not += "\n\n";
        not += "\n\n";
        for (int i = 1001; i <= 1054; i++)
        {
            not += "    public static get " + dicVal[i - 1001].ToUpper() + "_CALL_BACK(): string {\n";
            not += "        return this.getProtoEvent(" + i + ");\n";
            not += "    }\n";
        }

        not += "\n\n";
        not += "\n\n";
        for (int i = 1001; i <= 1054; i++)
        {
            not += "协议号： " + i + " 对应中文解释：" + dicKey[i - 1001] + " \n";
        }


        idSW.WriteLine(not);
        string outPath = "D:/tt/Ts/ProtomangerHead.ts";

        File.WriteAllText(outPath, idSW.ToString());
        Debug.Log(outPath + "创建成功");
    }

    //public void getkey()
    //{
    //    Dictionary<string, string> dic = getss();
    //    foreach (var ky in dic)
    //    {
    //        dicKey.Add(ky.Key);
    //        dicVal.Add(ky.Value);
    //    }
    //}

    public string oldFile;
    public string newFilePath;

    public GameObject got;
    public string selectPaht()
    {


        got = EditorGUILayout.ObjectField(got, typeof(GameObject), false) as GameObject;
        if (got != null)
        {
            string result = AssetDatabase.GetAssetPath(got);
            Debug.Log(result);
        }
        return "";
    }

    public void proto()
    {
        getss(); 
        for (int i = 1001; i <= 1054; i++)
        {
            StringWriter idSW = new StringWriter();
            string not = "import Proto from \"./Proto\";\n";
            not += "import UserData from \"../data/UserData\";";
            not += "\n\n";
            not += "export default class Proto" + i + " extends Proto {";
            not += "\n";
            not += "    protected protoid: number = " + i + ";";
            not += "\n";
            not += @"
    public send(data: Object = {}): void {
";
            not += "        data[\"openid\"] = UserData.openid;\n";
            not += "        data[\"password\"] = UserData.password;";
            not += @"
        let sendData = {
            protoid: this.protoid,
            data: data,
        }
        this.request(sendData);
    }
    /**
     * ";
            not += "\n";
            not += "     * " + dicKey[i - 1001] + "\n";
            not += @"     * @param json 
     */
    protected read(json: any): void {

    }
}";
            idSW.WriteLine(not);
            string outPath = "D:/tt/Ts/Proto" + i + ".ts";

            File.WriteAllText(outPath, idSW.ToString());
            Debug.Log(outPath + "创建成功");
        }
    }

    public Dictionary<string, string> pbs = new Dictionary<string, string>();
    public StringWriter sw = new StringWriter();
    public string toInt = "parseInt";
    public void Generate()
    {
        Debug.Log(oldFile);
        return;
        string csvPath = "D:/tt/csv/achievement.csv";


        CsvData csv = new CsvData().ReadFile(csvPath);
        int count = csv.LineCount;

        Dictionary<string, int> heads = new Dictionary<string, int>();
        string[] line = csv.GetLine(0);
        for (int i = 0; i < line.Length; i++)
        {
            heads.Add(line[i], i);
        }

        StringWriter idSW = new StringWriter();

        string note = @"// ======================================
// 该文件自动生成,，不要修改，否则会替换
// --------------------------------------			
";

        idSW.WriteLine(note);
        //idSW.WriteLine("\n\n");

        idSW.WriteLine(@"
export default class " + @"achievement" + @"
{
");
        idSW.WriteLine(@"    private static _hash: Object = {};
    private static _list: Array<string> = [];

    /**
     * 安装csv文件
     */
    public static installCSV(csv: CSV): void {
        this._hash = {};
        var data = csv.getAllData();
        for (var id in data) {
            this._hash[id] = new AchievementInfo(data[id]);
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
     * 通过id获取achievementInfo
     */
    public static getInfo(id: any): AchievementInfo {
        id = String(id);
        if (this._hash.hasOwnProperty(id)) {
            return this._hash[id];
        }
        return null;
    }
    /**
     * 通过index获取AchievementInfo
     */
    public static getInfoFromIndex(index: number): AchievementInfo {
        try {
            let id = this._list[index];
            return this.getInfo(id);
        } catch (error) {
            return null;
        }
    }
");
        idSW.WriteLine(@"    private constructor(obj: Object) {");
        foreach (var item in heads)
        {
            idSW.WriteLine(string.Format("        this._{0} = {1}(obj[{0}]);", item.Key, toInt));
        }
        idSW.WriteLine("    }");
        idSW.WriteLine("\n");
        foreach (var item in heads)
        {
            string keys = item.Key;
            idSW.WriteLine("    private _" + keys + ": number;");
            idSW.WriteLine("    public get " + keys +"(): number {");
            idSW.WriteLine("        return this._" + keys + ";");
            idSW.WriteLine("    }");
            idSW.WriteLine("    public set " + keys + "(v: number) {");
            idSW.WriteLine("        this._" + keys + " = v;");
            idSW.WriteLine("    }");
            idSW.WriteLine("\n");
        }



        idSW.WriteLine(@"

}
");



        string outPath = "D:/tt/Ts/achievement.ts";

        File.WriteAllText(outPath, idSW.ToString());
        Debug.Log(outPath + "创建成功");


    }

    public void AddProtoFile(string filename)
    {
        if (!pbs.ContainsKey(filename))
        {
            pbs.Add(filename, filename);
        }
    }
}
