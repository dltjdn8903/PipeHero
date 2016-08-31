using UnityEngine;
using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;

public enum eSpellType
{
    SPELL_SINGLE,
    SPELL_ROUND,
}

public enum ClickedSkill
{
    SKILL_FIRST,
    SKILL_SECOND,
    SKILL_THIRD,
    SKILL_POSITION,
    SKILL_NONE,
}

public enum eTouchType
{
    TOUCH_NONE,
    TOUCH_SKILL,
    TOUCH_POTION,
}

public class Touch_EventManager : BaseManager<Touch_EventManager>
{
    delegate void listener(string type, int id, float x, float y, float dx, float dy);

    event listener begin;
    event listener move;
    event listener end;

    bool m_IsSkillPressed = false;
    bool m_IsCastring = false;
    bool prevIsMouseMove = false;

    Vector2[] delta = new Vector2[5];

    BaseObject m_PlayerObject = null;
    Observer_Component m_ClickedObject = null;

    eTeamType m_TargetTeam;

    eSkillTargettingType m_CastingSkillType = eSkillTargettingType.TARGETTINGTYPE_DONTSELECTED;

    eAIStateType m_ClickedSkill = eAIStateType.AI_STATE_NONE;

    public eAIStateType ClickedSkill
    {
        get { return m_ClickedSkill; }
        set { m_ClickedSkill = value; }
    }

    public eTouchType TOUCH_TYPE { get; set; }

    public void SkillPressed()
    {
        m_IsSkillPressed = true;
    }
    public void SkillReleased()
    {
        m_IsSkillPressed = false;
    }

    public void OnStart()
    {
        TOUCH_TYPE = eTouchType.TOUCH_NONE;
        m_TargetTeam = eTeamType.TEAM_NONE;
    }


    void Update()
    {
        if (StageManager.Instance.Marker != null && GlobalValue.Instance.Player != null)
        {
            if (Vector3.Distance(GlobalValue.Instance.Player.transform.position, StageManager.Instance.Marker.transform.position) < 0.1f)
                StageManager.Instance.Marker.gameObject.SetActive(false);

            StageManager.Instance.PlayerPlane.transform.position = GlobalValue.Instance.Player.transform.position;
            StageManager.Instance.PlayerPlane.transform.position = new Vector3(StageManager.Instance.PlayerPlane.transform.position.x, StageManager.Instance.PlayerPlane.transform.position.y + 0.01f, StageManager.Instance.PlayerPlane.transform.position.z);
        }

        //----------------------------------------------클릭을 뗏을때
        if (Input.GetMouseButtonUp(0) == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitUnClick;

            if (Physics.Raycast(ray, out hitUnClick))
            {
                if (m_IsCastring == true)
                {
                    if (hitUnClick.transform.tag == "Environment")
                    {
                        if(m_CastingSkillType == eSkillTargettingType.TARGETTINGTYPE_NONETARGET)
                        {
                            GlobalValue.Instance.Player.AI.ClearAI();
                            GlobalValue.Instance.Player.AI.AddNextAI(ClickedSkill, null, hitUnClick.point);
                        }
                    }
                    else if (hitUnClick.transform.tag == "NPC")
                    {
                        return;
                    }
                    else if (hitUnClick.transform.tag != "Environment")
                    {
                        Observer_Component UnClicked = hitUnClick.collider.gameObject.GetComponent<Observer_Component>();

                        if (m_CastingSkillType == eSkillTargettingType.TARGETTINGTYPE_NONETARGET)
                        {
                            GlobalValue.Instance.Player.AI.ClearAI();
                            GlobalValue.Instance.Player.AI.AddNextAI(ClickedSkill, null, hitUnClick.point);
                        }
                        else if (m_CastingSkillType == eSkillTargettingType.TARGETTINGTYPE_TARGET)
                        {
                            GlobalValue.Instance.Player.AI.ClearAI();
                            GlobalValue.Instance.Player.AI.AddNextAI(ClickedSkill, UnClicked);
                        }
                    }

                    m_IsCastring = false;
                }
                else if (m_IsCastring == false)
                {
                    if (m_ClickedObject != null)
                    {
                        if (hitUnClick.transform.tag == "Environment")
                        {
                            m_ClickedObject.AI.ForceCommendOn();
                            m_ClickedObject.ThrowEvent("TOUCH_MOVE", hitUnClick.point);
                            m_ClickedObject = null;
                        }
                        else if (hitUnClick.transform.tag == "NPC")
                        {
                            return;
                        }
                        else if (hitUnClick.transform.tag != "Environment")
                        {
                            Observer_Component UnClicked = hitUnClick.collider.gameObject.GetComponent<Observer_Component>();

                            if (UnClicked == null)
                                return;

                            m_ClickedObject.ThrowEvent("SET_TARGET", UnClicked);

                            eTeamType UnClickedTeam = (eTeamType)UnClicked.GetData("TEAM");

                            switch (UnClickedTeam)
                            {
                                case eTeamType.TEAM_PLAYER:
                                    {
                                        m_ClickedObject.ThrowEvent("TOUCH_MOVE", UnClicked.SelfTransform.position);
                                    }
                                    break;

                                case eTeamType.TEAM_ENEMY:
                                    {
                                        m_ClickedObject.ThrowEvent("ATTACK_TARGET", UnClicked);
                                        m_ClickedObject.GetComponent<Highlighter>().enabled = false;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (m_ClickedObject == null)
                    {
                        //즉시 발동 스킬 시전
                        if (ClickedSkill > (eAIStateType)2 && ClickedSkill < (eAIStateType)6)
                        {
                            eSkillTargettingType tempTemplate = GlobalValue.Instance.GetSkillMouseTargetingType(ClickedSkill);

                            if (tempTemplate == eSkillTargettingType.TARGETTINGTYPE_NONE)
                            {
                                GlobalValue.Instance.Player.AI.Stop();
                                //GlobalValue.Instance.Player.ISCASTING = true;
                                //GlobalValue.Instance.Player.OBJECT_STATE = eBaseObjectState.STATE_SKILL2;
                                //GlobalValue.Instance.Player.AI.ClearAI();
                                //GlobalValue.Instance.Player.AI.AddNextAI(eAIStateType.AI_STATE_IDLE);
                                GlobalValue.Instance.Player.AI.AddNextAI(ClickedSkill);

                                if (m_IsSkillPressed)
                                {
                                    StageManager.Instance.Marker.SetActive(false);
                                    return;
                                }
                            }
                            else if (tempTemplate == eSkillTargettingType.TARGETTINGTYPE_TARGET || tempTemplate == eSkillTargettingType.TARGETTINGTYPE_NONETARGET)
                            {
                                m_IsCastring = true;
                                m_CastingSkillType = tempTemplate;
                            }
                        }
                    }
                }
            }
            return;
        }

        //----------------------------------------------클릭 눌렀을때
        if (Input.GetMouseButtonDown(0) == false || m_IsCastring == true)
            return;

        if (m_IsSkillPressed)
            return;

        if (Camera.main == null)
            return;

        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(_ray, out hit))
        {
            if (hit.transform.tag == "Environment")//지형지물이면
            {
                //타겟 초기화
                if (GlobalValue.Instance.Player.AI.CURRENT_AI_STATE != eAIStateType.AI_STATE_NORMALATTACK)
                    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("RESET_TARGET");
                m_ClickedObject = null;

                //영웅 이동, 마크 생성
                GlobalValue.Instance.Player.ThrowEvent("TOUCH_MOVE", hit.point);
                StageManager.Instance.Marker.transform.position = hit.point;
                StageManager.Instance.Marker.transform.position = new Vector3(StageManager.Instance.Marker.transform.position.x, StageManager.Instance.Marker.transform.position.y + 0.1f, StageManager.Instance.Marker.transform.position.z);
                StageManager.Instance.Marker.SetActive(true);

                for (int i = 0; i < ObserverManager.Instance.GetPlayerTeam().Count; ++i)
                {
                    if (ObserverManager.Instance.GetPlayerTeam()[i].AI is MinionAI && ObserverManager.Instance.GetPlayerTeam()[i].AI.BATTLESTATE == eBattleState.BATTLESTATE_NORMAL)
                    {
                        ObserverManager.Instance.GetPlayerTeam()[i].ThrowEvent("TOUCH_MOVE", ObserverManager.Instance.GetPlayerTeam()[i].SelfTransform.position - GlobalValue.Instance.Player.SelfTransform.position + hit.point);
                    }
                }
            }

            else if (hit.transform.tag == "NPC")//NPC면
            {
                eNPCName npcName = (eNPCName)hit.transform.GetComponent<BaseNPC>().NPCName;
                UIManager.Instance.ShowUI((eUIType)npcName);
            }
            else
            {
                m_ClickedObject = hit.collider.gameObject.GetComponent<Observer_Component>();

                if (m_ClickedObject == null)
                    return;

                m_TargetTeam = (eTeamType)m_ClickedObject.GetData("TEAM");

                switch (m_TargetTeam)
                {
                    case eTeamType.TEAM_PLAYER://아군 유닛 1. 스킬 아닐시: 타겟 지정, 2. 스킬시: 타겟에 스킬사용(타켓팅 제거)
                        {
                            //if (TOUCH_TYPE == eTouchType.TOUCH_NONE)
                            //{
                            //}
                            //if (TOUCH_TYPE == eTouchType.TOUCH_SKILL)
                            //{
                            //    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", ClickedSkill);
                            //    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("RESET_TARGET");
                            //    m_ClickedObject = null;
                            //}
                            m_ClickedObject.GetComponent<Highlighter>().enabled = true;
                        }
                        break;

                    case eTeamType.TEAM_ENEMY://적클릭 1. 스킬 아닐시: 타겟 공격, 2. 스킬시: 타겟에 스킬 사용 #공용 타켓팅 제거
                        {
                            GlobalValue.Instance.Player.ThrowEvent("SET_TARGET", m_ClickedObject);

                            //if (TOUCH_TYPE == eTouchType.TOUCH_NONE)
                            //{
                            //    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("ATTACK_TARGET", m_ClickedObject);
                            //}
                            //else if (TOUCH_TYPE == eTouchType.TOUCH_SKILL)
                            //{
                            //    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", m_ClickedObject, ClickedSkill);
                            //    TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("RESET_TARGET");
                            //}
                            if (ClickedSkill > (eAIStateType)2 && ClickedSkill < (eAIStateType)6)
                            {
                                TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", m_ClickedObject, ClickedSkill);
                                TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("RESET_TARGET");
                            }
                            else if (ClickedSkill == eAIStateType.AI_STATE_NONE)
                            {
                                TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("ATTACK_TARGET", m_ClickedObject);
                            }
                            m_ClickedObject.GetComponent<Highlighter>().enabled = true;
                            m_ClickedObject = null;
                        }
                        break;
                }
            }
        }
    }
    
    public Observer_Component GetTarget()
    {
        return m_ClickedObject;
    }

    void onTouch(string type, int id, float x, float y, float dx, float dy)
    {
        switch (type)
        {
            case "begin":
                {
                    if (Camera.main == null)
                        return;
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(_ray, out hit))
                    {
                        if (hit.transform.tag == "Environment")//지형지물이면
                        {
                            TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("TOUCH_MOVE", hit.point);
                        }

                        else
                        {
                            m_ClickedObject = hit.collider.gameObject.GetComponent<Observer_Component>();

                            //TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("SET_TARGET", m_TargetObject);

                            m_TargetTeam = (eTeamType)m_ClickedObject.GetData("TEAM");

                            switch (m_TargetTeam)
                            {
                                case eTeamType.TEAM_PLAYER://아군 유닛 1. 스킬시: 타겟에 스킬사용, 2. 스킬 아닐시: 이동
                                    {
                                        if (TOUCH_TYPE == eTouchType.TOUCH_NONE)
                                        {
                                            //TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("TOUCH_MOVE", m_TargetObject.SelfTransform.position);
                                            m_ClickedObject.AI.ForceCommendOn();
                                        }
                                        else if (TOUCH_TYPE == eTouchType.TOUCH_SKILL)
                                        {
                                            TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", ClickedSkill);
                                            TOUCH_TYPE = eTouchType.TOUCH_NONE;
                                        }
                                    }
                                    break;

                                case eTeamType.TEAM_ENEMY://적클릭
                                    {
                                        if (TOUCH_TYPE == eTouchType.TOUCH_NONE)
                                        {
                                            TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("ATTACK_TARGET", m_ClickedObject);
                                        }
                                        else if (TOUCH_TYPE == eTouchType.TOUCH_SKILL)
                                        {
                                            TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", m_ClickedObject, ClickedSkill);
                                            TOUCH_TYPE = eTouchType.TOUCH_NONE;
                                        }
                                        m_ClickedObject = null;
                                        TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("RESET_TARGET");
                                    }
                                    break;

                                case eTeamType.TEAM_NPC://대화, 상점 오픈
                                    {
                                        //TriggerManager.Instance.Dir = m_TargetObject.SelfTransform.position;
                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
            //case "move": {} break;
            case "end":
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitUnClick;
                    if (Physics.Raycast(ray, out hitUnClick) && m_ClickedObject != null)
                    {
                        if (hitUnClick.transform.tag == "Environment")//지형지물이면
                        {
                            m_ClickedObject.ThrowEvent("TOUCH_MOVE", hitUnClick.point);
                        }
                    }

                    if (TOUCH_TYPE == eTouchType.TOUCH_SKILL)
                    {
                        //TriggerManager.Instance.MAIN_OBJECT.ThrowEvent("CAST_SKILL", )
                        TOUCH_TYPE = eTouchType.TOUCH_NONE;
                    }
                    m_ClickedObject = null;
                }
                break;
        }
    }
}
