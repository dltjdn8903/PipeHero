using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eStateType
{
    STATE_TYPE_NONE,
    STATE_TYPE_LOGO,
    STATE_TYPE_LOBBY,
    STATE_TYPE_STAGE,

    STATE_TYPE_COUNT,
}

public enum eTriggerInvokeType
{
    INVOKE_IMMEDIATE,
    INVOKE_CALL,
    INVOKE_COLLISION,
}

public enum eTriggerResultType
{
    TRIGGER_RESULT_WAIT,
    TRIGGER_RESULT_ING,
    TRIGGER_RESULT_END,
}

public enum eUIType
{
    Pf_Ui_Loading,
    Pf_Ui_Popup,
    Pf_Ui_BoardManager,

    Pf_UI_Stage,
    Pf_UI_Popup_StageMenu,
    Pf_UI_Popup_Shop,
    Pf_UI_SoldierInfo,
    Pf_UI_SoldierShop,
    Pf_UI_Soldier,
    Pf_UI_Shop,
    Pf_UI_TownMenu,
    Pf_UI_Popup_TownMenu,
    Pf_UI_Popup_Status,
    Pf_UI_Popup_Rank,
    Pf_UI_Popup_NPC,

    Pf_UI_Dungeon,
    Pf_UI_DungeonMenu,
    Pf_UI_Result_Fail,

    Pf_Status_Label
}

public enum eGameCondition
{
    // Clear
    CLEAR_START,

    CLEAR_TRIGGER,
    MONSTER_CLEAR,

    CLEAR_END,

    // Fail
    FAIL_START = 10000,

    SELF_CHARACTER_DIE,

    FAIL_END,
}

public enum eSkillAttackRangeType
{
    RANGE_BOX,
    RANGE_SPHERE,
    RANGE_NONE,
}

public enum eSkillTargettingType
{
    TARGETTINGTYPE_NONE,
    TARGETTINGTYPE_TARGET,
    TARGETTINGTYPE_NONETARGET,
    TARGETTINGTYPE_DONTSELECTED,

}

public enum eConditionType
{
    CONDITION_NONE,
    CONDITION_STERN                      ,//1
    CONDITION_KNOCKBACK              ,//2
    CONDITION_AGRRO                     ,//3
    CONDITION_IMMUNEDAMAGE        ,//4
    CONDITION_HIDE                        ,//5
    CONDITION_CREATESKILL              ,//6
}

public enum eSkillTemplateType
{
    ATTACK_TARGET,
    ATTACK_MISSILE,
    SKILL_AGRRO,
    SKILL_WHIRLWIND,
    SKILL_IMMUNEDAMAGE,
    SKILL_PIERCING,
    SKILL_RAMPAGE,
    SKILL_HIDE,
    SKILL_HEAL,
    SKILL_FIREBALL,
    SKILL_BUFF,
    SKILL_FIRENOVA,
    SKILL_SUMMON,
    SKILL_METEOR,
    SKILL_METEORBULLET,
    SKILL_FIRENOVABULLET,
    BASIC_HEAL,
}

public enum eTeamType
{
    TEAM_NONE,
    TEAM_PLAYER,
    TEAM_ENEMY,
    TEAM_NPC,
}

public enum eAreaType
{
    AREA_TOWN,
    AREA_DUNGEON,
    AREA_LASTBOSS,
}

public delegate IEnumerator SkillEvent(BaseObject _caster, BaseObject _target);

public class GlobalValue : BaseManager<GlobalValue>
{
    int m_PotionCount = 0;

    GameObject Mercenary;

    public const float magicnumber = 1.1958131745004019414600484002536f;
    //magicnumber^9=5, log magicnumber 5 = 9
    public float MAGICNUMBER
    {
        get { return magicnumber; }
    }

    public int MAX_LEVEL
    {
        get { return 10; }
    }
            

    List<string> m_SkillList = new List<string>();

    public int m_Level      = 0;
    public int m_Money      = -1;
    public int m_ExpPoint   = -1;
    public int m_ItemCount  = -1;
    public int m_LeaderShip = 0;
    public int m_GetPoint   = -1;
    public int m_GetMoney   = -1;
    public float m_FlowTime = -1;


    public int m_NeedExp    = 0;
    public int m_AttackDmg  = 0;
    public double m_MaxHP      = 0;
   

    // 캐릭터가 용병을 몇명 갖고 있는지 카운트
    public int Have_KnightCount = 0;
    public int Have_SaintCount = 0;
    public int Have_MagicianCount = 0;
    public int Have_MagicianIceCount = 0;

    // 캐릭터가 용병을 몇명 데려갈것인지 카운트
    public int m_KnightCount = 0;
    public int m_SaintCount = 0;
    public int m_MagicianCount = 0;
    public int m_MagicianIceCount = 0;

    GameCharacter m_playerCharacter = null;
    Observer_Component m_Player = null;

    public eAreaType m_AreaType = eAreaType.AREA_TOWN;
    public AudioSource m_AudioBGM;
    public AudioSource m_AudioSound;
    public AudioClip m_SoundCoin;

    void Awake()
    {
        m_AudioBGM = gameObject.AddComponent<AudioSource>();
        m_AudioSound = gameObject.AddComponent<AudioSource>();
        m_SoundCoin = Resources.Load<AudioClip>("Coin");

        /*/강제 초기화
        m_Level     = 1;
        m_Money     = 300;
        m_ExpPoint  = 0;
        m_ItemCount = 3;        
        m_LeaderShip = 3;
        m_GetPoint = 0;
        m_GetMoney = 0;
        m_FlowTime = 0;

        Have_KnightCount = 1;
        Have_SaintCount = 1;
        Have_MagicianCount = 1;
        Have_MagicianIceCount = 1;
        /*/


        m_Level = PlayerPrefs.GetInt("m_Level");     if (m_Level     ==  0) m_Level     = 1;
        m_Money     = PlayerPrefs.GetInt("m_Money");     if (m_Money     == -1) m_Money     = 300;
        m_ExpPoint  = PlayerPrefs.GetInt("m_ExpPoint");  if (m_ExpPoint  == -1) m_ExpPoint  = 0;
        m_ItemCount = PlayerPrefs.GetInt("m_ItemCount"); if (m_ItemCount == -1) m_ItemCount = 3;
        m_LeaderShip = PlayerPrefs.GetInt("m_LeaderShip"); if (m_LeaderShip == 0) m_LeaderShip = 3;
        m_GetPoint = PlayerPrefs.GetInt("m_GetPoint"); if (m_GetPoint == 0) m_GetPoint = 0;
        m_GetMoney = PlayerPrefs.GetInt("m_GetMoney"); if (m_GetMoney == -1) m_GetMoney = 0;
        m_FlowTime = PlayerPrefs.GetInt("m_FlowTime"); if (m_FlowTime == -1) m_FlowTime = 0;

        Have_KnightCount = PlayerPrefs.GetInt("Have_KnightCount"); if (Have_KnightCount == 0) Have_KnightCount = 1;
        Have_SaintCount = PlayerPrefs.GetInt("Have_SaintCount"); if (Have_SaintCount == 0) Have_SaintCount = 1;
        Have_MagicianCount = PlayerPrefs.GetInt("Have_MagicianCount"); if (Have_MagicianCount == 0) Have_MagicianCount = 1;
        Have_MagicianIceCount = PlayerPrefs.GetInt("Have_MagicianIceCount"); if (Have_MagicianIceCount == 0) Have_MagicianIceCount = 1;
        //*/

        if (m_Level > MAX_LEVEL) m_Level = MAX_LEVEL;

        //레벨값으로 후 계산하는 요소들
        m_NeedExp = (int)(Mathf.Pow(m_Level, 2) * 100);
        m_AttackDmg = (int)(Mathf.Pow(magicnumber, m_Level - 1) * 10 + 0.5f);
        m_MaxHP     = (double)(Mathf.Pow(magicnumber, m_Level - 1) * 100 + 0.5f);
        m_LeaderShip = 3 + (int)m_Level / 2;

    }

    void Update()
    {
        m_NeedExp = (int)(Mathf.Pow(m_Level, 2) * 100);
        m_AttackDmg = (int)(Mathf.Pow(magicnumber, m_Level - 1) * 10 + 0.5f);
        m_MaxHP = (double)(Mathf.Pow(magicnumber, m_Level - 1) * 100 + 0.5f);
        m_LeaderShip = 3 + (int)m_Level / 2;
    }

    void OnApplicationQuit()
//    public void SaveAndExit()
    {
        PlayerPrefs.SetInt("m_Level", m_Level);
        PlayerPrefs.SetInt("m_Money", m_Money);
        PlayerPrefs.SetInt("m_ExpPoint", m_ExpPoint);
        PlayerPrefs.SetInt("m_ItemCount", m_ItemCount);
        PlayerPrefs.SetInt("m_LeaderShip", m_LeaderShip);

        PlayerPrefs.SetInt("KnightCount", Have_KnightCount);
        PlayerPrefs.SetInt("SaintCount", Have_SaintCount);
        PlayerPrefs.SetInt("MagicianCount", Have_MagicianCount);
        PlayerPrefs.SetInt("MagicianIceCount", Have_MagicianIceCount);

        PlayerPrefs.Save();
    }

    public GameCharacter PlayerCharacter
    {
        get { return m_playerCharacter; }
    }

    public List<string> SkillList
    {
        get { return PlayerCharacter.CHARACTER_TEMPLATE.LIST_SKILL; }
    }

    public void SetMercenary()
    {
        Mercenary = GameObject.FindWithTag("Mercenary");
    }

    public Observer_Component Player
    {
        get { return m_Player; }
        set
        {
            m_Player = value;
            m_playerCharacter = (GameCharacter)m_Player.GetData("CHARACTER");
        }
    }

    public int PotionCount
    {
        get { return m_PotionCount; }
        set { m_PotionCount = value; }
    }

    public eSkillTargettingType GetSkillMouseTargetingType(eAIStateType _skillNumber)
    {
        int skillNumber = (int)_skillNumber - 3;

        string skillName = PlayerCharacter.CHARACTER_TEMPLATE.LIST_SKILL[skillNumber];

        SkillData skillData = SkillManager.Instance.GetSkillData(skillName);

        List<string> skillDataaa = skillData.LIST_COMBO_A;

        SkillTemplate playerSKillTemplate = SkillManager.Instance.GetSkillTemplate(skillDataaa[0]);

        return playerSKillTemplate.TARGETING_TYPE;
    }

    public void EnterArea( eAreaType _AreaType, AudioClip _bgm )
    {
        m_AreaType = _AreaType;
        PlayBGM(_bgm);

        if (m_AreaType == eAreaType.AREA_TOWN)
        {
            EnterTown();
        }
        else if (m_AreaType == eAreaType.AREA_DUNGEON)
        {
            EnterDungeon();
        }
    }

    public void EnterTown()
    {
        UI_Dungeon_EventHandler.Instance.m_Point.gameObject.SetActive(false);
        UI_Dungeon_EventHandler.Instance.m_ShowSoldierCount.gameObject.SetActive(false);
        m_GetPoint = 0;
        m_GetMoney = 0;
        m_FlowTime = 0;

        Mercenary.transform.DestroyChildren();
    }

    public void EnterDungeon()
    {
        UI_Dungeon_EventHandler.Instance.m_Point.gameObject.SetActive(true);
        UI_Dungeon_EventHandler.Instance.m_ShowSoldierCount.gameObject.SetActive(true);
        UI_Dungeon_EventHandler.Instance.b_Timer = true;

        GameObject pf_Knight = Resources.Load<GameObject>("Mercenary_Knight");
        GameObject pf_Priest = Resources.Load<GameObject>("Mercenary_Priest");
        GameObject pf_MagicianFire = Resources.Load<GameObject>("Mercenary_MagicianFire");
        GameObject pf_MagicianIce = Resources.Load<GameObject>("Mercenary_MagicianIce");
        Vector3 playerpos = TriggerManager.Instance.MAIN_OBJECT.transform.position;
        playerpos.z = playerpos.z-2;
        Quaternion playerrot = TriggerManager.Instance.MAIN_OBJECT.transform.rotation;

        int i;
        i = 0;
        while (i < m_KnightCount)
        {
            GameObject sol = GameObject.Instantiate(pf_Knight, playerpos, playerrot) as GameObject;
            sol.transform.SetParent(Mercenary.transform);
            ++i;
        }
        i = 0;
        while (i < m_SaintCount)
        {
            GameObject sol = GameObject.Instantiate(pf_Priest, playerpos, playerrot) as GameObject;
            sol.transform.SetParent(Mercenary.transform);
            ++i;
        }
        i = 0;
        while (i < m_MagicianCount)
        {
            GameObject sol = GameObject.Instantiate(pf_MagicianFire, playerpos, playerrot) as GameObject;
            sol.transform.SetParent(Mercenary.transform);
            ++i;
        }
        i = 0;
        while (i < m_MagicianIceCount)
        {
            GameObject sol = GameObject.Instantiate(pf_MagicianIce, playerpos, playerrot) as GameObject;
            sol.transform.SetParent(Mercenary.transform);
            ++i;
        }

    }

    public void PlayBGM( AudioClip _bgm )
    {
        m_AudioBGM.clip = _bgm;
        m_AudioBGM.loop = true;
        m_AudioBGM.volume = 0.1f;
        m_AudioBGM.Play();
    }

    public void PlaySound( AudioClip _AudioClip)
    {
        m_AudioSound.PlayOneShot(_AudioClip);
    }

    public void PlaySoundCoin()
    {
        PlaySound(m_SoundCoin);
    }
}
