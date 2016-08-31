using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillHide : BaseSkill
{
    Renderer m_MyRenderer = null;
    Material m_NoneHideMaterial = null;
    Material m_HideMaterial = null;
    string NoneHidePath = string.Empty;
    string HidePath = string.Empty;

    public override void InitSkill()
    {
        m_MyRenderer = OWNER.GetComponentInChildren<SkinnedMeshRenderer>();
        NoneHidePath = "Material/NoneHide/" + OWNER.name.ToString();
        HidePath = "Material/Hide/" + OWNER.name.ToString();

        m_NoneHideMaterial = Resources.Load(NoneHidePath) as Material;
        m_HideMaterial = Resources.Load(HidePath) as Material;

        SKILLEVENT = Hide;
        OWNER.ThrowEvent("SIDE_EFFECT", OWNER, SKILLEVENT, eConditionType.CONDITION_HIDE);
    }

    public override void UpdateSkill() { }   

    IEnumerator Hide(BaseObject _caster, BaseObject _target)
    {
        List<Observer_Component> enemyList = ObserverManager.Instance.GetOtherTeam((eTeamType)OWNER.GetData("TEAM"));

        for(int i =0;i< enemyList.Count;++i)
        {
            if((Observer_Component)enemyList[i].GetData("TARGET") == OWNER)
            {
                enemyList[i].ThrowEvent("RESET_TARGET");
            }
        }

        while (true)
        {
            m_MyRenderer.material = m_HideMaterial;
            (_caster.GetComponent<NavMeshAgent>()).speed = 5.0f;
            OWNER.OBJECT_STATE = eBaseObjectState.STATE_HIDE;
            yield return new WaitForSeconds(8.0f);
            m_MyRenderer.material = m_NoneHideMaterial;
            (_caster.GetComponent<NavMeshAgent>()).speed = 3.5f;
            OWNER.OBJECT_STATE = eBaseObjectState.STATE_NORMAL;
            break;
        }
        yield return null;
    }
}
