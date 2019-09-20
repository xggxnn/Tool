using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjToDic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

//    public get LoadFailed(): string { return "网络情况不佳！请点击确认按钮，调整手机网络通畅以后重开打开游戏。"; }
//public get BuyHaveMax(): string { return "购买次数已达上限，请明日再来或者手动刷新！"; }
//    public get KingHave(): string { return "你已经获得过此奖励，无法重复获取"; }
//    public get KingLevelLimit(): string { return "等级不足，无法获取"; }
//    public get KingVIPLimit(): string { return "暂未开放"; }
//    public get SignHave(): string { return "已签到"; }
//    public get GetNewHero(): string { return "恭喜你获得新英雄 {0}"; }
//    public get DebrisCollection(): string { return "碎片收集中，剩余时间 {0}"; }
//    public get CollectingRewards(): string { return "获取关卡的探索奖励"; }
//    public get NoGainTip(): string { return "暂无收益!"; }
//    public get Conquest(): string { return "已征服{0}个关卡\n最近{1}产出{2}金币、{3}钻石"; }
//    public get MaxLevel(): string { return "已满级!"; }
//    public get MaxStar(): string { return "已满级!"; }
//    public get GoldNoEnough(): string { return "金币不足\n试炼可获取大量金币"; }
//    public get JadeiteNoEnough(): string { return "翡翠不足\n闯关可获得大量翡翠"; }
//    public get LevelNoEnough(): string { return "等级不够，无法升星，请先升级"; }
//    public get UpLevel(): string { return "请提升金乌等级!"; }
//    public get UpStar(): string { return "请提升玉蟾等级!"; }
//    public get ExitWaveTip(): string { return "是否确定退出当前战斗？退出后无法获取任何收益!"; }
//    public get AddSpeedTip(): string { return "通过第二关解锁加速功能！"; }
//    public get FightFailReqTip(): string { return "大王，我们的英雄阵容还不够强大，还修炼修炼再战不迟！是否继续挑战？"; }
//    public get FightSkipTip(): string { return "战斗达标，是否看视频直接跳过战斗？"; }
//    public get SkillCD(): string { return "技能cd中，请稍候！"; }

//    // 英雄升品质
//    public get QualityName(): string { return "普通"; };
//    public get AttributeName(): string { return "攻击力"; };
//    public get UpHeroQuality(): string { return "将英雄“{0}”从{1}提升至{2}，将消耗{3}个英雄“{0}”碎片，是否确定提升？\n\nPs:当前拥有英雄“{0}”碎片{4}个"; }
//    public get UpHeroAttribute(): string { return "重置英雄“{0}”的{1}属性，将消耗{2}个英雄“{0}”碎片，是否确定重置？\n\nPs:当前拥有英雄“{0}”碎片{3}个"; }
//    public get MaxQualityTip(): string { return "该英雄已经提升到最高品质，无需再次提升！"; }
//    public get ReqMaxQuality(): string { return "该英雄品质未达到最高，无法洗属性~！"; }
//    public get LevelNoEnoughToResetAtt(): string { return "金乌等级达到50级开启洗属性功能~！"; }
//    public get saveAttTip(): string { return "英雄“{0}”的{1}发生变化：\n{2}->{3}(max:{4})\n是否替换？"; }
//    public get NoReqClips(): string { return "英雄碎片数量不足，当前需要碎片{0}，当前拥有碎片数量{1}"; }

//    // 引导提示
//    public get battleGuid(): string { return "快帮我把师傅救出来~"; };
//    public get battleXzf(): string { return "大王，这些都是猴子叫来的救兵！快叫小的们出动！"; }
//    public get wukongTip1(): string { return "师傅我来救你!   "; }
//    public get wukongTip2(): string { return "师傅莫急我去搬救兵!   "; }
//    public get wukongTip3(): string { return "妖精快还我师傅!   "; }
//    public get Guid2(): string { return "点击英雄标签，切换到英雄列表!"; }
//    public get Guid3(): string { return "搜集英雄碎片可以合成获取新英雄哦！"; }
//    public get Guid4(): string { return "点击按钮，合成一个英雄！"; }
//    public get battleDrag(): string { return "拖动英雄上阵！"; }
//    public get battleFight(): string { return "准备好了，击退敌人！"; }
//    public get battleSkill(): string { return "大王，快施法鼓舞英雄，增加攻击力！"; }
//    public get battleSynth(): string { return "快去合成新的英雄！"; }
//    public get synthetise(): string { return "恭喜大王获得了新英雄，快去布阵！"; }
//    public get fettersTip(): string { return "哇！阵容中有3个五行属火的英雄，激活了攻击力提升10%的羁绊！"; }
//    public get continueFightTip(): string { return "变更强大了，继续去闯关！"; }
//    public get fiveEnterMenus(): string { return "准备闯关！"; }
//    public get fiveSelectWave(): string { return "选择关卡！"; }
//    public get fiveFight(): string { return "准备挑战！"; }
//    public get fiveStartFive(): string { return "开始战斗！"; }
//    public get clickUpLevelBtn(): string { return "恭喜大王获得了足够的金币，快去升级！"; }
//    public get fiveUpLevel(): string { return "给所有英雄升级！"; }
//    public get fiveUpLevelOver(): string { return "所有英雄都变更强大了，再接再厉！"; }
//    public get clickGoldBtn(): string { return "已完成的关卡可再次挑战试炼，获取大量金币！大王，我们要更多金币提升更高的等级！"; }

//    // 帮助提示
//    public get ArrangeTip(): string { return "拖动英雄即可调换位置，将英雄拖动到英雄列表即可下阵\n所有英雄同时升级，花费金币(已完成的关卡可再次挑战试炼，获取大量金币)\n所有英雄同时升星，花费翡翠(闯关可获取大量翡翠)\n羁绊，上阵英雄同五行/门派达到一定数量，可激活该五行/门派的羁绊效果。还有一部分英雄有特殊的私人关系，能激活特殊的羁绊哦！"; }
//    public get HeroInfTip(): string { return "合成，获取英雄碎片，达到一定数量，可以合成该英雄\n升级品质，已经拥有的英雄，花费一定数量的碎片，可以提升英雄品质（普通—>稀有—>史诗—>传说—>神话）\n重置属性，神话品质的英雄，花费一定数量的碎片，可以重置1个属性，结果随机（欧皇认定！）"; }
//    public get TrialTip(): string { return "闯关，过关可以获得大量的金币翡翠，更有专属的英雄碎片和碎片卡包！\n试炼，成功闯过的关卡可以反复挑战，难度越来越大，是获取金币的主要途径！\n探索，试炼进度完成100% 之后，关卡会随时间产出英雄碎片，挂机也能得英雄哦！"; }
//    public get PassWaveTip(): string { return "悟空，快去多搬些救兵！"; }
}
