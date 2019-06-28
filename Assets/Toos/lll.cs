using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lll : MonoBehaviour
{
    //public Dictionary<string, string> dic = new Dictionary<string, string>();
    public Dictionary<string, string> getss()
    {
        List<string> dicKey = new List<string>();
        List<string> dicVal = new List<string>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dicKey.Add("登录");dicVal.Add("login");
        dicKey.Add("玩家完成新手引导后调用(一个账号只会调用一次)");dicVal.Add("initPlay");
        dicKey.Add("通关-----");dicVal.Add("wavePass");
        dicKey.Add("召唤祭坛抽卡");dicVal.Add("card");
        dicKey.Add("英雄升级");dicVal.Add("heroUpgrade");
        dicKey.Add("英雄升段");dicVal.Add("heroUpSegment");
        dicKey.Add("布阵");dicVal.Add("setSeat");
        dicKey.Add("选关");dicVal.Add("selectWave");
        dicKey.Add("领取每日登陆奖励");dicVal.Add("loginReward");
        dicKey.Add("领取资源补给箱");dicVal.Add("box");
        dicKey.Add("领取邀请奖励");dicVal.Add("invite");
        dicKey.Add("限时活动购买英雄");dicVal.Add("active");
        dicKey.Add("领取成就奖励");dicVal.Add("achieve");
        dicKey.Add("领取成就勋章奖励");dicVal.Add("achieveMedal");
        dicKey.Add("领取每日任务奖励");dicVal.Add("task");
        dicKey.Add("领取每日任务大奖");dicVal.Add("taskBig");
        dicKey.Add("领取离线挂机奖励");dicVal.Add("offLine");
        dicKey.Add("领取国王之路奖励");dicVal.Add("king");
        dicKey.Add("免费领奖");dicVal.Add("free");
        dicKey.Add("无尽时空挑战");dicVal.Add("endless");
        dicKey.Add("无尽时空过关");dicVal.Add("endlessPass");
        dicKey.Add("无尽时空领奖");dicVal.Add("endlessReward");
        dicKey.Add("图鉴技能升级");dicVal.Add("mapSkill");
        dicKey.Add("领取累充奖励");dicVal.Add("total");
        dicKey.Add("领取首充奖励");dicVal.Add("firstPay");
        dicKey.Add("领取每日充值奖励");dicVal.Add("dayPay");
        dicKey.Add("领取每周充值奖励");dicVal.Add("weekPay");
        dicKey.Add("充值");dicVal.Add("pay");
        dicKey.Add("口令兑换");dicVal.Add("cdKey");
        dicKey.Add("获取玩家资源数据");dicVal.Add("playInf");
        dicKey.Add("获取玩家其他数据");dicVal.Add("playOther");
        dicKey.Add("获取选关界面数据");dicVal.Add("waveRefrush");
        dicKey.Add("获取每日登陆数据");dicVal.Add("everyRefrush");
        dicKey.Add("获取邀请奖励数据");dicVal.Add("inviteRefrush");
        dicKey.Add("获取限时活动数据");dicVal.Add("activeRefrush");
        dicKey.Add("获取成就数据");dicVal.Add("achieveRefrush");
        dicKey.Add("获取每日任务数据");dicVal.Add("taskRefrush");
        dicKey.Add("获取资源补给箱数据");dicVal.Add("boxRefrush");
        dicKey.Add("获取离线挂机数据");dicVal.Add("offLineRefrush");
        dicKey.Add("获取国王之路数据");dicVal.Add("kingRefrush");
        dicKey.Add("获取免费领奖数据");dicVal.Add("freeRefrush");
        dicKey.Add("获取图鉴技能数据");dicVal.Add("mapRefrush");
        dicKey.Add("获取图鉴英雄数据");dicVal.Add("heroInfRefrush");
        dicKey.Add("获取图鉴怪物数据");dicVal.Add("mapMonsterRefrush");
        dicKey.Add("获取首充奖励数据");dicVal.Add("firstRefrush");
        dicKey.Add("获取累计充值数据");dicVal.Add("totalRefrush");
        dicKey.Add("获取每日充值数据");dicVal.Add("dayRefrush");
        dicKey.Add("获取每周充值数据");dicVal.Add("weekRefrush");
        dicKey.Add("获取无尽时空数据");dicVal.Add("endlessRefrush");
        dicKey.Add("获取布阵数据");dicVal.Add("seatRefrush");
        dicKey.Add("获取英雄详情数据");dicVal.Add("heroInf");
        dicKey.Add("加速");dicVal.Add("addSpeed");
        dicKey.Add("哥布林");dicVal.Add("goblin");
        dicKey.Add("商人");dicVal.Add("npc");
        return dic;
    }

}
