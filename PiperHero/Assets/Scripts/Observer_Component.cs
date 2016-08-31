using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Observer_Component : BaseObject
{
    [SerializeField]
    eAIType m_MakeAIType = eAIType.MINION_AI;

    [SerializeField]
    bool m_bEnableBoard = true;

    [SerializeField]
    eTeamType m_TeamType;
     
    [SerializeField]
    string m_TemplateKey = string.Empty;

    bool m_IsCasting = false;

    GameCharacter m_SelfCharacter = null;
    BaseObject m_TargetObject = null;

    Vector3 m_Destination = Vector3.zero;

    BaseAI m_AI = null;
    public eTeamType TEAM_TYPE { get { return m_TeamType; } }

    public BaseAI AI
    {
        get { return m_AI; }
    }

    public bool ISCASTING
    {
        get { return m_IsCasting; }
        set { m_IsCasting = value; }
    }

    void Awake()
    {
        switch (m_MakeAIType)
        {
            case eAIType.MINION_AI:
                {
                    GameObject aiObject = new GameObject();
                    aiObject.name = m_MakeAIType.ToString("F");
                    m_AI = aiObject.AddComponent<MinionAI>();

                    aiObject.transform.SetParent(SelfTransform);
                }
                break;

            case eAIType.CHAMPION_AI:
                {
                    GameObject aiObject = new GameObject();
                    aiObject.name = m_MakeAIType.ToString("F");
                    m_AI = aiObject.AddComponent<ChampionAI>();

                    aiObject.transform.SetParent(SelfTransform);
                }
                break;
        }

        m_AI.OBSERVER_COMPONENT = this;

        GameCharacter gameCharacter = CharacterManager.Instance.AddCharacter(m_TemplateKey);
        if (m_TemplateKey == "PLAYER_GUNNER" || m_TemplateKey == "PLAYER_SWORDNSHILD" || m_TemplateKey == "PLAYER_STAFF")
        {
            CharacterTemplateData templateData = CharacterManager.Instance.GetTemplate(m_TemplateKey);
            templateData.FACTOR_TABLE.SetData(eFactorData.MAX_HP, GlobalValue.Instance.m_MaxHP);
            //+ GlobalValue 의 Attack에 캐릭터의 Attack값 세팅 
            templateData.FACTOR_TABLE.SetData(eFactorData.ATTACK, GlobalValue.Instance.m_AttackDmg);
            gameCharacter.SetTemplate(templateData);

            gameCharacter.SetTemplate(templateData);
        }

        gameCharacter.OBSERVER_COMPONENT = this;
        m_SelfCharacter = gameCharacter;
        
        for (int i = 0; i < gameCharacter.CHARACTER_TEMPLATE.LIST_NORMAL_ATTACK.Count; ++i)
        {
            SkillData skillData = SkillManager.Instance.GetSkillData(gameCharacter.CHARACTER_TEMPLATE.LIST_NORMAL_ATTACK[i]);
            gameCharacter.AddAttack(skillData);
        }

        for (int i = 0; i < gameCharacter.CHARACTER_TEMPLATE.LIST_SKILL.Count; ++i)
        {
            SkillData skillData = SkillManager.Instance.GetSkillData(gameCharacter.CHARACTER_TEMPLATE.LIST_SKILL[i]);
            gameCharacter.AddSkill(skillData);
        }

        if (m_bEnableBoard == true )
        {
            BaseBoard board = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_HP);
            if (m_MakeAIType == eAIType.CHAMPION_AI && m_TeamType == eTeamType.TEAM_PLAYER)
            {         
                board.SetData("HP", GetFactorData(eFactorData.MAX_HP), gameCharacter.CURRENT_HP);
            }
            else
            {
                board.SetData("HP", GetFactorData(eFactorData.MAX_HP), gameCharacter.CURRENT_HP);
            }
        }

        //+ 보드 HpBar 추가
        if (m_MakeAIType == eAIType.CHAMPION_AI && m_TeamType == eTeamType.TEAM_PLAYER)
        {
            UI_Dungeon_EventHandler.Instance.setHpData("HP_BAR", GetFactorData(eFactorData.MAX_HP), m_SelfCharacter.CURRENT_HP);
        }

        //+ 보드 ExpBar 추가
        if (m_MakeAIType == eAIType.CHAMPION_AI && m_TeamType == eTeamType.TEAM_PLAYER)
        {
            UI_Dungeon_EventHandler.Instance.setExpData("EXP_BAR", GlobalValue.Instance.m_ExpPoint);
        }

        ObserverManager.Instance.AddObserver(this);
    }

    void OnEnable()
    {
        BoardManager.Instance.ShowBoard(this, true);
    }

    void OnDisable()
    {
        BoardManager.Instance.ShowBoard(this, false);
    }

    public double GetFactorData( eFactorData factorData )
    {
        return m_SelfCharacter.CHARACTER_FACTOR.GetFactorData(factorData);
    }

    void OnDestroy()
    {
        //if (BoardManager.Instance != null)
        //    BoardManager.Instance.ClearBoard(this);

        if( ObserverManager.Instance != null)
            ObserverManager.Instance.RemoveObserver(this);
    }

    //int m_PrevLevel = 0;
    void Update()
    {
        m_AI.UpdateAI();
        if( m_AI.END )
        {
            Destroy(SelfObject);

            //+ 죽은 몬스터가 Destroy 되면, 그 후 팀을 파악해서 돈, 경험치 상승                      
            if (m_MakeAIType != eAIType.CHAMPION_AI && m_TeamType != eTeamType.TEAM_PLAYER)
            {
                GlobalValue.Instance.m_Money += 100;
                GlobalValue.Instance.m_GetMoney += 100;
                UI_Dungeon_EventHandler.Instance.m_Money.text = string.Format("{0}", GlobalValue.Instance.m_Money);
                GlobalValue.Instance.PlaySoundCoin();

                if (GlobalValue.Instance.m_Level < GlobalValue.Instance.MAX_LEVEL)
                {
                    GlobalValue.Instance.m_ExpPoint += 150;
                }
                GlobalValue.Instance.m_GetPoint += 150;

                //+ EXP보드 세팅
                BaseBoard expBoard = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_EXP);
                if (expBoard != null)
                {
                    expBoard.SetData("EXP_BOARD", 150);
                }

                //+ MONEY보드 세팅
                BaseBoard MoneyBoard = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_MONEY);
                if (MoneyBoard != null)
                {
                    MoneyBoard.SetData("MONEY_BOARD", 100);
                }
            }
        }

        // 몬스터가 죽은 후 레벨업처리
        GameCharacter gameCharacter = CharacterManager.Instance.AddCharacter(m_TemplateKey);
        if (m_TemplateKey == "PLAYER_GUNNER" || m_TemplateKey == "PLAYER_SWORDNSHILD" || m_TemplateKey == "PLAYER_STAFF")
        {
            if (GlobalValue.Instance.m_ExpPoint >= GlobalValue.Instance.m_NeedExp)
            {
                //+ LevelUp보드 세팅
                BaseBoard LevelUpBoard = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_LEVELUP);

                //EXP바 세팅
                UI_Dungeon_EventHandler.Instance.setExpData("EXP_BAR", GlobalValue.Instance.m_ExpPoint);
                CharacterTemplateData templateData = CharacterManager.Instance.GetTemplate(m_TemplateKey);

                //MAX_HP의 수치를 GlobalValue에 있는 MAX_HP로 값 다시 세팅
                GlobalValue.Instance.m_MaxHP = (double)(Mathf.Pow(GlobalValue.Instance.MAGICNUMBER, GlobalValue.Instance.m_Level - 1) * 100 + 0.5f);
                templateData.FACTOR_TABLE.SetData(eFactorData.MAX_HP, GlobalValue.Instance.m_MaxHP);

                //+ GlobalValue 의 Attack에 캐릭터의 Attack값 세팅 
                templateData.FACTOR_TABLE.SetData(eFactorData.ATTACK, GlobalValue.Instance.m_AttackDmg);

                m_SelfCharacter.SetTemplate(templateData);

                //레벨업 된 후 보드에 MAX_HP, CURRENT_HP 값 다시 세팅
                BaseBoard hpBoard = BoardManager.Instance.GetBoardData(this, eBoardType.BOARD_HP);
                if (hpBoard != null)
                {
                    hpBoard.SetData("HP", GetFactorData(eFactorData.MAX_HP), m_SelfCharacter.CURRENT_HP);
                    UI_Dungeon_EventHandler.Instance.setHpData("HP_BAR", GetFactorData(eFactorData.MAX_HP), m_SelfCharacter.CURRENT_HP);
                }
            }
            else
            {
                UI_Dungeon_EventHandler.Instance.setExpData("EXP_BAR", GlobalValue.Instance.m_ExpPoint);
            }
        }


    }

    public override object GetData(string keyData, params object[] datas)
    {
        switch ( keyData )
        {
            case "CONDITION":
                {
                    return m_SelfCharacter.CONDITION;
                }

            case "TEAM":
                {
                    return m_TeamType;
                }

            case "MAX_HP":
                {
                    return m_SelfCharacter.MAX_HP;
                }

            case "CLASS":
                {
                    return m_SelfCharacter.CLASS;
                }

            case "HP":
                {
                    return m_SelfCharacter.CURRENT_HP;
                }

            case "TARGET":
                {
                    return m_TargetObject;
                }

            case "DESTINATION":
                {
                    return m_Destination;
                }

            case "ATTACK_DATA":
                {
                    return m_SelfCharacter.GetNormalAttackByIndex();
                }

            case "CHARACTER":
                {
                    return m_SelfCharacter;
                }

            case "SKILL_DATA":
                {
                    int nIndex = (int)datas[0];
                    return m_SelfCharacter.GetSkillByIndex(nIndex);
                }                
        }
        return base.GetData(keyData);
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch( keyData )
        {
            case "TOUCH_MOVE":
                {
                    Vector3 position = (Vector3)datas[0];
                    m_TargetObject = null;
                    AI.AddNextAI(eAIStateType.AI_STATE_RUN, null, position);
                }
                break;

            case "SET_TARGET":
                {
                    m_TargetObject = datas[0] as BaseObject;
                }
                break;

            case "SET_DESTINATION":
                {
                    m_Destination = (Vector3)datas[0];
                }
                break;

            case "RESET_TARGET":
                {
                    m_TargetObject = null;
                }
                break;

            case "SELECT_ATTACK":
                {
                    int nIndex = (int)datas[0];
                    SkillData skillData = m_SelfCharacter.GetNormalAttackByIndex();
                    m_SelfCharacter.SELECT_SKILL = skillData;
                }
                break;

            case "SELECT_SKILL":
                {
                    int nIndex = (int)datas[0];
                    SkillData skillData = m_SelfCharacter.GetSkillByIndex(nIndex);
                    m_SelfCharacter.SELECT_SKILL = skillData;
                }
                break;

            case "ATTACK_TARGET":
                {
                    BaseObject targetObject = (BaseObject)datas[0];
                    SkillData skillData = m_SelfCharacter.GetNormalAttackByIndex();

                    AI.ClearAI();

                    if (Vector3.Distance(SelfTransform.position, targetObject.SelfTransform.position) > skillData.RANGE)
                    {
                        AI.AddNextAI(eAIStateType.AI_STATE_RUN, targetObject, targetObject.SelfTransform.position);
                    }
                    else
                    {
                        AI.AddNextAI(eAIStateType.AI_STATE_NORMALATTACK, targetObject);
                    }
                }
                break;

            case "CAST_SKILL":
                {
                    BaseObject targetObject = (BaseObject)datas[0];
                    eAIStateType clickedSkill = (eAIStateType)datas[1];
                    SkillData skillData = m_SelfCharacter.GetSkillByIndex((int)clickedSkill - 3);

                    AI.ClearAI();
                    AI.AddNextAI(clickedSkill, targetObject, targetObject.SelfTransform.position);

                    if (Vector3.Distance(SelfTransform.position, targetObject.SelfTransform.position) > skillData.RANGE)
                    {
                        AI.AddNextAI(eAIStateType.AI_STATE_RUN, targetObject, targetObject.SelfTransform.position);
                    }
                    else
                    {
                        AI.AddNextAI(clickedSkill, targetObject);
                    }
                }
                break;
                
            case "HIT":
                {
                    eConditionType condition = (eConditionType)m_SelfCharacter.CONDITION;

                    if (condition == eConditionType.CONDITION_IMMUNEDAMAGE)
                        return;

                    GameCharacter enemyCharacter = datas[0] as GameCharacter;
                    SkillTemplate skillTemplate = datas[1] as SkillTemplate;

                    enemyCharacter.CHARACTER_FACTOR.AddFactorTable("SKILL", skillTemplate.FACTOR_TABLE);
                    double fAttackDamage = enemyCharacter.CHARACTER_FACTOR.GetFactorData(eFactorData.ATTACK);
                    enemyCharacter.CHARACTER_FACTOR.RemoveFactorTable("SKILL");

                    m_SelfCharacter.IncreaseCurrentHP(-fAttackDamage);

                    BaseBoard hpBoard = BoardManager.Instance.GetBoardData(this, eBoardType.BOARD_HP);
                    if (hpBoard != null)
                    {
                        hpBoard.SetData("HP", GetFactorData(eFactorData.MAX_HP), m_SelfCharacter.CURRENT_HP);
                    }

                    BaseBoard damageBoard = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_DAMAGE);
                    if (damageBoard != null)
                    {
                        damageBoard.SetData("DAMAGE", fAttackDamage);
                    }

                    //+ 보드 HpBar 추가
                    if (m_MakeAIType == eAIType.CHAMPION_AI && m_TeamType == eTeamType.TEAM_PLAYER)
                    {
                        UI_Dungeon_EventHandler.Instance.setHpData("HP_BAR", GetFactorData(eFactorData.MAX_HP), m_SelfCharacter.CURRENT_HP);
                    }

                    Animator ani = GetComponent<Animator>();
                    ani.SetInteger("Hit", 1);

                    EffectManager.Instance.ThrowEffect("HIT", SelfTransform.position);
                }
                break;

            case "TRANS_CONDITION":
                {
                    m_SelfCharacter.CONDITION = (eConditionType)datas[0];
                }
                break;

            case "TRANS_FACTOR":
                {
                    eFactorData factorData = (eFactorData)datas[0];
                    double parameter = (double)datas[1];

                    m_SelfCharacter.CHARACTER_TEMPLATE.FACTOR_TABLE.IncreaseData(factorData, parameter);
                }
                break;

            case "SIDE_EFFECT":
                {
                    //0 : 시전자, 1: 타겟에게 적용되는 함수, 2 : 상태 이상 효과
                    Observer_Component caster = (Observer_Component)datas[0];
                    SkillEvent m_SkillCondition = (SkillEvent)datas[1];
                    eConditionType conditionType = (eConditionType)datas[2];

                    m_SelfCharacter.CONDITION = conditionType;

                    StartCoroutine(m_SkillCondition(caster, this));
                }
                break;
        }
    }

    public void Combo(eComboType ComboType )
    {
        SkillData selectSkill = m_SelfCharacter.SELECT_SKILL;
        if (selectSkill == null)
            return;

        List<string> listSkillTemplate = null;
        if (ComboType == eComboType.COMBO_A)
            listSkillTemplate = selectSkill.LIST_COMBO_A;
        else if (ComboType == eComboType.COMBO_B)
            listSkillTemplate = selectSkill.LIST_COMBO_B;
        else if (ComboType == eComboType.COMBO_C)
            listSkillTemplate = selectSkill.LIST_COMBO_C;

        for( int i = 0; i < listSkillTemplate.Count; ++i )
        {
            SkillManager.Instance.RunSkill(this, listSkillTemplate[i]);
        }
    }
}
