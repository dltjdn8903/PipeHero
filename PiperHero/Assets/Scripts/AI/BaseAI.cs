using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eAIStateType
{
    AI_STATE_NONE,
    AI_STATE_IDLE,
    AI_STATE_NORMALATTACK,
    AI_STATE_SKILL1,
    AI_STATE_SKILL2,
    AI_STATE_SKILL3,
    AI_STATE_POTION,
    AI_STATE_RUN,
    AI_STATE_DIE,
    AI_STATE_COMMEND,
}

public enum eAIType
{
    MINION_AI            ,
    CHAMPION_AI          ,
}

public enum eAIAutoMode
{
    AUTO_NONE           ,
    AUTO_TOUCH          ,
}

public enum eBattleState
{
    BATTLESTATE_NORMAL,
    BATTLESTATE_FIGHT,
}

public struct stNextAI
{
    public eAIStateType m_StateType;
    public BaseObject m_TargetObject;
    public Vector3 m_Position;
}

public class BaseAI : BaseObject
{
    protected List<stNextAI> m_listNextAI = new List<stNextAI>();
    protected eAIStateType m_CurrentAIState = eAIStateType.AI_STATE_IDLE;

    protected bool m_IsForced = false;

    protected Vector3 m_MovePosition = Vector3.zero;

    float m_AgrroTime = 0;

    bool m_bUpdateAI = false;
    bool m_Casting = false;

    Animator m_Animator = null;
    NavMeshAgent m_NavMeshAgent = null;

    Vector3 m_PrevMovePosition = Vector3.zero;

    eBattleState m_BattleState = eBattleState.BATTLESTATE_NORMAL;

    SkillEvent m_Commend = null;

    bool m_bEnd = false;
    
    protected int m_nSkillIndex = 0;

    public eAIStateType CURRENT_AI_STATE
    {
        get { return m_CurrentAIState; }
    }

    public Animator ANIMATOR
    {
        get
        {
            if (m_Animator == null)
            {
                m_Animator = SelfObject.GetComponent<Animator>();
            }

            return m_Animator;
        }
    }

    public NavMeshAgent NAV_MESH_AGENT
    {
        get
        {
            if (m_NavMeshAgent == null)
            {
                m_NavMeshAgent = SelfObject.GetComponent<NavMeshAgent>();
            }
            return m_NavMeshAgent;
        }
    }

    public bool END
    {
        get { return m_bEnd; }
        protected set { m_bEnd = value; }
    }

    public bool IS_CASTING
    {
        set { m_Casting = value; }
        get { return m_Casting; }
    }
    
    public eBattleState BATTLESTATE
    {
        get { return m_BattleState; }
        set { m_BattleState = value; }
    }

    public float AGRROTIME
    {
        get { return m_AgrroTime; }
        set { m_AgrroTime = value; }
    }

    public SkillEvent Commend
    {
        get { return m_Commend; }
        set { m_Commend = value; }
    }

    public void SetBoss()
    {
        InvokeRepeating("BossCastRepeatSkill", 10.0f, 10.0f);
    }

    public void ForceCommendOn()
    {
        m_IsForced = true;
    }
    public void ForceCommendOff()
    {
        m_IsForced = false;
    }

    public void ClearAI()
    {
        m_listNextAI.Clear();
    }

    public void ClearAI(eAIStateType stateType )
    {
        m_listNextAI.RemoveAll(nextAI => nextAI.m_StateType == stateType);
    }

    virtual public void AddNextAI(eAIStateType nextStatetype, BaseObject targetObject = null, Vector3 position = new Vector3())
    {
        stNextAI nextAI = new stNextAI();

        nextAI.m_StateType = nextStatetype;
        nextAI.m_TargetObject = targetObject;
        nextAI.m_Position = position;

        m_listNextAI.Add(nextAI);

        Debug.Log(nextAI.m_StateType);
    }

    public void UpdateAI()
    {
        if (AGRROTIME >= 0.0f)
            AGRROTIME -= Time.smoothDeltaTime;

        if (m_bUpdateAI == true)
            return;

        if (m_listNextAI.Count > 0)
        {
            _NextAI(m_listNextAI[0]);
            m_listNextAI.RemoveAt(0);
        }

        if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
        {
            m_listNextAI.Clear();
            _ProcessDie();
        }
        //else if (OBJECT_STATE == eBaseObjectState.STATE_SKILL2)
        //{
        //    m_listNextAI.Clear();
        //    _ProcessSkill2();
        //}

        m_bUpdateAI = true;

        switch (m_CurrentAIState)
        {
            case eAIStateType.AI_STATE_IDLE:
                {
                    StartCoroutine(_Idle());
                }
                break;

            case eAIStateType.AI_STATE_NORMALATTACK:
                {
                    StartCoroutine(_Attack());
                }
                break;

            case eAIStateType.AI_STATE_SKILL1:
                {
                    StartCoroutine(_Skill1());
                }
                break;

            case eAIStateType.AI_STATE_SKILL2:
                {
                    StartCoroutine(_Skill2());
                }
                break;

            case eAIStateType.AI_STATE_SKILL3:
                {
                    StartCoroutine(_Skill3());
                }
                break;

            case eAIStateType.AI_STATE_POTION:
                {
                    StartCoroutine(_Potion());
                }
                break;

            case eAIStateType.AI_STATE_RUN:
                {
                    StartCoroutine(_Run());
                }
                break;

            case eAIStateType.AI_STATE_DIE:
                {
                    StartCoroutine(_Die());
                }
                break;
        }
    }

    void _NextAI(stNextAI nextAI)
    {
        if (nextAI.m_TargetObject != null)
        {
            OBSERVER_COMPONENT.ThrowEvent("SET_TARGET", nextAI.m_TargetObject);
        }

        if (nextAI.m_Position != Vector3.zero)
        {
            m_MovePosition = nextAI.m_Position;
            OBSERVER_COMPONENT.ThrowEvent("SET_DESTINATION", nextAI.m_Position);
        }

        switch (nextAI.m_StateType)
        {
            case eAIStateType.AI_STATE_IDLE:
                {
                    UI_Dungeon_EventHandler.Instance.b_SkillEnd = true;
                    _ProcessIdle();
                }
                break;

            case eAIStateType.AI_STATE_NORMALATTACK:
                {
                    if(nextAI.m_TargetObject != null )
                    {
                        SelfTransform.forward = (nextAI.m_TargetObject.SelfTransform.position - SelfTransform.position).normalized;
                    }
                    _ProcessAttack();
                }
                break;

            case eAIStateType.AI_STATE_SKILL1:
                {
                    if (nextAI.m_TargetObject != null)
                    {
                        SelfTransform.forward = (nextAI.m_TargetObject.SelfTransform.position - SelfTransform.position).normalized;
                    }

                    UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
                    _ProcessSkill1();
                }
                break;
            case eAIStateType.AI_STATE_SKILL2:
                {
                    if (nextAI.m_TargetObject != null)
                    {
                        SelfTransform.forward = (nextAI.m_TargetObject.SelfTransform.position - SelfTransform.position).normalized;
                    }

                    UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
                    _ProcessSkill2();
                }
                break;
            case eAIStateType.AI_STATE_SKILL3:
                {
                    if (nextAI.m_TargetObject != null)
                    {
                        SelfTransform.forward = (nextAI.m_TargetObject.SelfTransform.position - SelfTransform.position).normalized;
                    }

                    UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
                    _ProcessSkill3();
                }
                break;
            case eAIStateType.AI_STATE_POTION:
                {
                    _ProcessPotion();
                }
                break;

            case eAIStateType.AI_STATE_RUN:
                {
                    _ProcessRun();
                }
                break;

            case eAIStateType.AI_STATE_DIE:
                {
                    _ProcessDie();
                }
                break;

            case eAIStateType.AI_STATE_COMMEND:
                {
                    _ProcessCommend(nextAI.m_StateType);
                }
                break;
        }
    }

    virtual protected void _ProcessDie()
    {
        m_CurrentAIState = eAIStateType.AI_STATE_DIE;
        _ChangeAnimation();
    }

    virtual protected void _ProcessIdle()
    {
        m_CurrentAIState = eAIStateType.AI_STATE_IDLE;
        _ChangeAnimation();
    }

    virtual protected void _ProcessAttack()
    {
        OBSERVER_COMPONENT.ThrowEvent("SELECT_ATTACK", 0);

        m_CurrentAIState = eAIStateType.AI_STATE_NORMALATTACK;
        _ChangeAnimation();
    }

    virtual protected void _ProcessSkill1()
    {
        OBSERVER_COMPONENT.ThrowEvent("SELECT_SKILL", 0);
        
        m_CurrentAIState = eAIStateType.AI_STATE_SKILL1;
        _ChangeAnimation();
    }
    virtual protected void _ProcessSkill2()
    {
        OBSERVER_COMPONENT.ThrowEvent("SELECT_SKILL", 1);
        
        m_CurrentAIState = eAIStateType.AI_STATE_SKILL2;
        _ChangeAnimation();
    }
    virtual protected void _ProcessSkill3()
    {
        OBSERVER_COMPONENT.ThrowEvent("SELECT_SKILL", 2);
        
        m_CurrentAIState = eAIStateType.AI_STATE_SKILL3;
        _ChangeAnimation();
    }
    virtual protected void _ProcessPotion()
    {
        OBSERVER_COMPONENT.ThrowEvent("SELECT_SKILL", 4);

        m_CurrentAIState = eAIStateType.AI_STATE_POTION;
        _ChangeAnimation();
    }

    virtual protected void _ProcessRun()
    {
        m_CurrentAIState = eAIStateType.AI_STATE_RUN;
        _ChangeAnimation();
    }
    virtual protected void _ProcessCommend(eAIStateType _commend)
    {
        m_CurrentAIState = _commend;
        _ChangeAnimation();
    }

    void _ChangeAnimation()
    {
        ANIMATOR.SetInteger("State", (int)m_CurrentAIState);        
    }

    virtual protected IEnumerator _Idle()
    {
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Attack()
    {
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Skill1()
    {
        //UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Skill2()
    {
        //UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Skill3()
    {
        //UI_Dungeon_EventHandler.Instance.b_SkillEnd = false;
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Potion()
    {
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Run()
    {
        m_bUpdateAI = false;
        yield break;
    }

    virtual protected IEnumerator _Die()
    {
        NavMeshAgent tmpNav = transform.root.GetComponent<NavMeshAgent>();
        CapsuleCollider tmpCol = transform.root.GetComponent<CapsuleCollider>();
        Rigidbody tmpRigi = transform.root.GetComponent<Rigidbody>();

        Destroy(tmpRigi);
        tmpCol.enabled = false;
        tmpNav.enabled = false;

        m_bUpdateAI = false;
        yield break;
    }
    virtual protected IEnumerator _Commend()
    {
        m_bUpdateAI = false;
        yield break;
    }

    protected void Move( Vector3 position )
    {
        if(CURRENT_AI_STATE != eAIStateType.AI_STATE_RUN)
            AddNextAI(eAIStateType.AI_STATE_RUN, null, position);

        NAV_MESH_AGENT.Resume();
        NAV_MESH_AGENT.SetDestination(position);
    }
    
    void BossCastRepeatSkill()
    {
        ClearAI();
        int randomSkill = Random.Range(3, 6);
        AddNextAI((eAIStateType)randomSkill);
    }

    public void Stop()
    {
        NAV_MESH_AGENT.Stop();
        m_MovePosition = Vector3.zero;
        AddNextAI(eAIStateType.AI_STATE_IDLE);
    }

    protected void Attack( BaseObject targetObject = null )
    {
        for(int i =0;i<m_listNextAI.Count;++i)
        {
            Debug.Log(m_listNextAI[i].m_StateType);
        }
        ClearAI(eAIStateType.AI_STATE_IDLE);
        ClearAI(eAIStateType.AI_STATE_NORMALATTACK);
        AddNextAI(eAIStateType.AI_STATE_NORMALATTACK, targetObject);
    }

    protected void CastSkill(eAIStateType _casting, BaseObject targetObject)
    {
        AddNextAI(_casting, targetObject);
    }

    protected void CastSkill(eAIStateType _casting, Vector3 _position)
    {
        AddNextAI(_casting, null, _position);
    }
}
