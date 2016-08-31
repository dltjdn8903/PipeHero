using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class MinionAI : BaseAI
{
    float m_ElapsedTime = 0;

    override protected IEnumerator _Idle()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (!m_IsForced)
            {
                BaseObject targetObject = null;
                BaseObject targetedObject = null;

                eTeamType team = (eTeamType)OBSERVER_COMPONENT.GetData("TEAM");

                if (AGRROTIME <= 0)
                {
                    targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
                    if(targetObject != null)
                        AGRROTIME = 3.0f;
                }

                if (team == eTeamType.TEAM_ENEMY)
                {
                    double myHP = (double)OBSERVER_COMPONENT.GetData("HP");

                    if (targetObject == null)
                    {
                        targetObject = (BaseObject)OBSERVER_COMPONENT.GetData("TARGET");
                    }

                    if (myHP < (double)OBSERVER_COMPONENT.GetData("MAX_HP") * 0.3f)
                    {
                        targetObject = ObserverManager.Instance.GetSearchLongRangeEnemy(team, OBSERVER_COMPONENT);
                        if (targetObject == null)
                            targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
                    }
                }
                else if (team == eTeamType.TEAM_PLAYER)
                {
                    targetedObject = OBSERVER_COMPONENT.GetData("TARGET") as Observer_Component;

                    if (targetedObject != null)
                    {
                        targetObject = targetedObject;
                    }
                }

                if (((GameCharacter)OBSERVER_COMPONENT.GetData("CHARACTER")).CLASS == eClassType.CLASS_HEALER)
                {
                    targetObject = ObserverManager.Instance.GetSearchHealTarget(team, OBSERVER_COMPONENT);
                }

                if (targetObject != null)
                {
                    if (targetObject.OBJECT_STATE == eBaseObjectState.STATE_DIE)
                    {
                        targetObject = null;
                        OBSERVER_COMPONENT.ThrowEvent("RESET_TARGET");
                        ClearAI();
                        AGRROTIME = 0;
                    }
                    else
                    {
                        SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;

                        float fRange = 0;

                        if (skillData != null)
                            fRange = skillData.RANGE;

                        float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
                        if (fDistance < fRange)
                        {
                            if (team == eTeamType.TEAM_ENEMY)
                                SelfObject.transform.GetComponent<Highlighter>().enabled = false;
                            Stop();
                            Attack(targetObject);
                        }
                        else
                        {
                            AddNextAI(eAIStateType.AI_STATE_RUN, null, targetObject.SelfTransform.position);
                        }
                    }
                }
            }
            yield return StartCoroutine(base._Idle());
            break;
        }
    }

    protected override IEnumerator _Attack()
    {
        yield return new WaitForEndOfFrame();

        while (IS_CASTING)
        {
            if (OBJECT_STATE == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        if (m_IsForced == true)
            AddNextAI(eAIStateType.AI_STATE_RUN, null, m_MovePosition);
        else
            AddNextAI(eAIStateType.AI_STATE_IDLE);


        yield return StartCoroutine(base._Attack());
    }

    protected override IEnumerator _Run()
    {
        Debug.Log("_RUN");
        yield return new WaitForEndOfFrame();

        BaseObject targetObject = null;
        BaseObject targetedObject = null;

        eTeamType team = (eTeamType)OBSERVER_COMPONENT.GetData("TEAM");

        if (team == eTeamType.TEAM_ENEMY)
        {
            double myHP = (double)OBSERVER_COMPONENT.GetData("HP");
            targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);
        }

        else if (team == eTeamType.TEAM_PLAYER)
        {
            targetObject = ObserverManager.Instance.GetSearchEnemy(team, OBSERVER_COMPONENT);

            if (targetObject != null)
            {
                targetedObject = OBSERVER_COMPONENT.GetData("TARGET") as Observer_Component;

                if (targetedObject != null)
                    targetObject = targetedObject;
            }
        }

        if (targetObject == null)
        {
            if (Vector3.Distance(SelfTransform.position, m_MovePosition) > 0.1f)
            {
                Move(m_MovePosition);
            }
            else
            {
                AddNextAI(eAIStateType.AI_STATE_IDLE);
                if (team == eTeamType.TEAM_PLAYER)
                    SelfObject.transform.GetComponent<Highlighter>().enabled = false;
                Stop();
            }
        }
        else if (m_IsForced && targetObject != null)
        {
            if (Vector3.Distance(m_MovePosition, SelfTransform.position) > 0.1f)
            {
                AddNextAI(eAIStateType.AI_STATE_RUN);
                Move(m_MovePosition);
            }
            else
            {
                if (team == eTeamType.TEAM_PLAYER)
                    SelfObject.transform.GetComponent<Highlighter>().enabled = false;
                ForceCommendOff();
                Stop();
            }
        }
        else if (!m_IsForced && targetObject != null)
        {
            SkillData skillData = OBSERVER_COMPONENT.GetData("ATTACK_DATA") as SkillData;

            float fRange = 0;

            if (skillData != null)
                fRange = skillData.RANGE;

            float fDistance = Vector3.Distance(targetObject.SelfTransform.position, SelfTransform.position);
            if (fDistance < fRange)
            {
                if (team == eTeamType.TEAM_ENEMY)
                    SelfObject.transform.GetComponent<Highlighter>().enabled = false;
                Stop();
                Attack(targetObject);
            }
            else
            {
                Move(targetObject.SelfTransform.position);
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
