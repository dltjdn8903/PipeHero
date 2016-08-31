using UnityEngine;
using System.Collections;

public class ChampionAI : BaseAI
{
    float m_ElapsedTime = 0;

    override protected IEnumerator _Idle()
    {
        //Debug.Log("_Idle");
        eTeamType team = (eTeamType)OBSERVER_COMPONENT.GetData("TEAM");

        if (team == eTeamType.TEAM_ENEMY)
        {
            BaseObject targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
            if (targetObject != null)
            {
                SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;
                float fRange = 0;

                if (skillData != null)
                    fRange = skillData.RANGE;

                float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
                if (fDistance < fRange)
                {
                    Stop();
                    Attack(targetObject);
                }
                else
                {
                    AddNextAI(eAIStateType.AI_STATE_RUN);
                }
            }
        }
        else if (team == eTeamType.TEAM_PLAYER)
        {
            while (true)
            {

                BaseObject targetObject = null;

                targetObject = (BaseObject)OBSERVER_COMPONENT.GetData("TARGET");

                if (targetObject == null)
                {
                    targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
                }

                if (targetObject != null && (eTeamType)targetObject.GetData("TEAM") != team)
                {
                    if (targetObject.OBJECT_STATE == eBaseObjectState.STATE_DIE)
                    {
                        targetObject = null;
                        OBSERVER_COMPONENT.ThrowEvent("RESET_TARGET");
                    }
                    else
                    {
                        targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);

                        SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;
                        float fRange = 0;

                        if (skillData != null)
                            fRange = skillData.RANGE;

                        float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
                        if (fDistance < fRange && StageManager.Instance.Marker.activeSelf == false)
                        {
                            Stop();
                            Attack(targetObject);
                        }
                    }
                }
                break;
            }
        }
        yield return StartCoroutine(base._Idle());
    }

    protected override IEnumerator _Attack()
    {
        yield return new WaitForSeconds(0.3f);

        //Debug.Log("Attack");

        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        if (StageManager.Instance.Marker.activeSelf == true)
            AddNextAI(eAIStateType.AI_STATE_RUN, null, m_MovePosition);
        else
            AddNextAI(eAIStateType.AI_STATE_IDLE);

        yield return StartCoroutine(base._Attack());
    }

    protected override IEnumerator _Skill1()
    {
        yield return new WaitForSeconds(0.3f);

        //Debug.Log("_Skill1");

        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        AddNextAI(eAIStateType.AI_STATE_IDLE);

        Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_NONE;

        yield return StartCoroutine(base._Skill1());
    }

    protected override IEnumerator _Skill2()
    {
        yield return new WaitForSeconds(0.3f);

        //Debug.Log("_Skill2");

        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        AddNextAI(eAIStateType.AI_STATE_IDLE);

        Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_NONE;

        yield return StartCoroutine(base._Skill2());
    }

    protected override IEnumerator _Skill3()
    {
        yield return new WaitForSeconds(0.3f);

        //Debug.Log("_Skill3");
        
        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        AddNextAI(eAIStateType.AI_STATE_IDLE);

        Touch_EventManager.Instance.ClickedSkill = eAIStateType.AI_STATE_NONE;

        yield return StartCoroutine(base._Skill3());
    }

    protected override IEnumerator _Potion()
    {
        //Debug.Log("_Potion");
        yield return new WaitForSeconds(0.3f);

        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        AddNextAI(eAIStateType.AI_STATE_IDLE);

        yield return StartCoroutine(base._Potion());
    }

    protected override IEnumerator _Run()
    {
        //Debug.Log("_Run");
        eTeamType team = (eTeamType)OBSERVER_COMPONENT.GetData("TEAM");

        if (team == eTeamType.TEAM_ENEMY)
        {
            BaseObject targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
            if (targetObject != null && StageManager.Instance.Marker.activeSelf == false)
            {
                SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;
                float fRange = 0;

                if (skillData != null)
                    fRange = skillData.RANGE;

                float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
                if (fDistance < fRange)
                {
                    Stop();
                    Attack(targetObject);
                }
                else
                {
                    Move(targetObject.SelfTransform.position);
                }
            }
        }
        else if (team == eTeamType.TEAM_PLAYER)
        {
            BaseObject targetObject = (BaseObject)OBSERVER_COMPONENT.GetData("TARGET");

            if (targetObject != null)
            {
                SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;
                float fRange = 0;

                if (skillData != null && (eTeamType)targetObject.GetData("TEAM") != team)
                    fRange = skillData.RANGE;

                float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
                if (fDistance < fRange)
                {
                    Stop();
                    Attack(targetObject);
                }
                else
                {
                    Move(m_MovePosition);
                }
            }
            else if (targetObject == null)
            {
                Move(m_MovePosition);
                if (Vector3.Distance(m_MovePosition, SelfTransform.position) < 0.1f)
                {
                    Stop();
                }
            }
        }
        yield return StartCoroutine(base._Run());
    }

    protected override IEnumerator _Die()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        if (m_ElapsedTime > 5.0f)
        {
            END = true;
        }
        yield return StartCoroutine(base._Die());
    }
}
