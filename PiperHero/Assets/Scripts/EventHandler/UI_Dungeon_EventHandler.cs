using UnityEngine;
using System.Collections;

public class UI_Dungeon_EventHandler : UISingleton<UI_Dungeon_EventHandler>
{

    [SerializeField]
    GameObject hpBarBoard = null; //+ 보드 HpBar 추가
    [SerializeField]
    GameObject ExpBarBoard = null; //+ 보드 ExpBar 추가

    public UILabel m_Money = null;

    [SerializeField]
    public UILabel m_Level = null;
    [SerializeField]
    UILabel m_ItemCount = null;
    [SerializeField]
    public UILabel m_Point = null;

    [SerializeField]
    UILabel m_Skill1Time = null;
    [SerializeField]
    UILabel m_Skill2Time = null;
    [SerializeField]
    UILabel m_Skill3Time = null;

    [SerializeField]
    public UILabel m_ShowSoldierCount = null;


    public bool b_Timer = false;
    public float f_minute = 0.0f;

    public float f_CoolTimeSkill1 = 5.0f;
    public bool b_startCoolTime1 = false;
    public float f_CoolTimeSkill2 = 10.0f;
    public bool b_startCoolTime2 = false;
    public float f_CoolTimeSkill3 = 15.0f;
    public bool b_startCoolTime3 = false;

    public bool b_SkillEnd = true;

    [SerializeField]
    TweenScale CoolTimeBar_1 = null;
    [SerializeField]
    TweenScale CoolTimeBar_2 = null;
    [SerializeField]
    TweenScale CoolTimeBar_3 = null;

    void Awake()
    {
        UILabel m_Level = GameObject.FindWithTag("Level").GetComponent<UILabel>();
        m_Level.text = string.Format("{0}", GlobalValue.Instance.m_Level);

        m_Money = GameObject.FindWithTag("Money").GetComponent<UILabel>();
        m_Money.text = string.Format("{0}", GlobalValue.Instance.m_Money);

        m_ItemCount.text = GlobalValue.Instance.m_ItemCount.ToString();
        

    }


    //보류
    void Update()
    {
        m_Point.text = "Point : " + GlobalValue.Instance.m_GetPoint.ToString();
        m_ShowSoldierCount.text = "출전 용병: "
                                   + (GlobalValue.Instance.m_KnightCount + GlobalValue.Instance.m_SaintCount + GlobalValue.Instance.m_MagicianCount + GlobalValue.Instance.m_MagicianIceCount).ToString("F0")
                                       + "명";

        // 타이머를 실행
        if (b_Timer == true)
            TimerOn();


        //60초 이상이면 1분씩 추가
        if (GlobalValue.Instance.m_FlowTime > 60)
        {
            f_minute += 1.0f;
            GlobalValue.Instance.m_FlowTime -= 60; //흐르는 시간 0으로 초기화
        }


        //쿨타임 실행    
        if (b_startCoolTime1 == true)
        {
            CoolTimeStart("Skill1");
        }
        if (b_startCoolTime2 == true)
        {
            CoolTimeStart("Skill2");
        }
        if (b_startCoolTime3 == true)
        {
            CoolTimeStart("Skill3");
        }

    }

    //+ 보드 HpBar 추가
    public void setHpData(string strKey, params object[] datas)
    {
        hpBarBoard.GetComponent<HP_Bar>().SetData(strKey, datas[0], datas[1]);
    }

    //+ 보드 ExpBar 추가
    public void setExpData(string strKey, params object[] datas)
    {
        ExpBarBoard.GetComponent<Exp_Bar>().SetData(strKey, datas[0]);
    }

    public void OnClickMenu()
    {
        if(GlobalValue.Instance.m_AreaType == eAreaType.AREA_TOWN)
            UIManager.Instance.ShowUI(eUIType.Pf_UI_TownMenu);
        else if(GlobalValue.Instance.m_AreaType == eAreaType.AREA_DUNGEON)
            UIManager.Instance.ShowUI(eUIType.Pf_UI_DungeonMenu);
    }

    public void OnClickStatus()
    {
        UIManager.Instance.ShowUI(eUIType.Pf_UI_Popup_Status);
    }

    public void OnClickSkill1()
    {
        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_NORMALATTACK);
        if (b_SkillEnd == true)
        {
            if (f_CoolTimeSkill1 == 5.0f)
            {
                Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_SKILL1;

                CoolTimeBar_1.gameObject.SetActive(true);
                CoolTimeBar_1.enabled = true;
                CoolTimeBar_1.ResetToBeginning();
            }
        }

    }

    public void OnClickSkill2()
    {
        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL2);
        if (b_SkillEnd == true)
        {
            if (f_CoolTimeSkill2 == 10.0f)
            {
                Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_SKILL2;

                CoolTimeBar_2.gameObject.SetActive(true);
                CoolTimeBar_2.enabled = true;
                CoolTimeBar_2.ResetToBeginning();
            }
        }

    }

    public void OnClickSkill3()
    {
        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL3);
        if (b_SkillEnd == true)
        {
            if (f_CoolTimeSkill3 == 15.0)
            {
                Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_SKILL3;
                
                CoolTimeBar_3.gameObject.SetActive(true);
                CoolTimeBar_3.enabled = true;
                CoolTimeBar_3.ResetToBeginning();
            }
        }
    }

    public void OnClickPotion()
    {
        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL3);
        //Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_POTION;

        //포션 갯수가 0보다 크면 

        if(GlobalValue.Instance.m_ItemCount > 0)
        {
            Observer_Component player = GlobalValue.Instance.Player;
            GameCharacter playerChacrecter = (GameCharacter)GlobalValue.Instance.Player.GetData("CHARACTER");

            playerChacrecter.IncreaseCurrentHP(100);

            BaseBoard hpBoard = BoardManager.Instance.GetBoardData(player, eBoardType.BOARD_HP);
            if (hpBoard != null)
            {
                hpBoard.SetData("HP", player.GetFactorData(eFactorData.MAX_HP), playerChacrecter.CURRENT_HP);
            }

            GlobalValue.Instance.m_ItemCount -= 1;
        }


    }

    // 타이머 온오프
    public void TimerOn()
    {
        GlobalValue.Instance.m_FlowTime += Time.smoothDeltaTime;
    }


    void CoolTimeStart(string SkillType)
    {
        switch (SkillType)
        {
            case "Skill1":
                {
                    if (f_CoolTimeSkill1 > 1) // 스킬이 시전됐을때
                    {
                        m_Skill1Time.gameObject.SetActive(true);
                        f_CoolTimeSkill1 -= Time.smoothDeltaTime;
                        //m_Skill1Time.text = f_CoolTimeSkill1.ToString("F0");
                    }
                    else // 스킬이 끝났을때
                    {
                        f_CoolTimeSkill1 = 5.0f;
                        b_startCoolTime1 = false;
                        m_Skill1Time.gameObject.SetActive(false);
                        
                    }

                    m_Skill1Time.text = f_CoolTimeSkill1.ToString("F0");

                }
                break;

            case "Skill2":
                {
                    if (f_CoolTimeSkill2 > 1) // 스킬이 시전됐을때
                    {
                        m_Skill2Time.gameObject.SetActive(true);
                        f_CoolTimeSkill2 -= Time.smoothDeltaTime;
                        //m_Skill2Time.text = f_CoolTimeSkill2.ToString("F0");
                    }
                    else // 스킬이 끝났을때
                    {
                        f_CoolTimeSkill2 = 10.0f;
                        b_startCoolTime2 = false;
                        m_Skill2Time.gameObject.SetActive(false);

                    }

                    m_Skill2Time.text = f_CoolTimeSkill2.ToString("F0");

                }
                break;

            case "Skill3":
                {
                    if (f_CoolTimeSkill3 > 1) // 스킬이 시전됐을때
                    {
                        m_Skill3Time.gameObject.SetActive(true);
                        f_CoolTimeSkill3 -= Time.smoothDeltaTime;
                        //m_Skill3Time.text = f_CoolTimeSkill3.ToString("F0");
                    }
                    else // 스킬이 끝났을때
                    {
                        f_CoolTimeSkill3 = 15.0f;
                        b_startCoolTime3 = false;                       
                        m_Skill3Time.gameObject.SetActive(false);
                        
                    }

                    m_Skill3Time.text = f_CoolTimeSkill3.ToString("F0");

                }
                break;
        }
    }
}
