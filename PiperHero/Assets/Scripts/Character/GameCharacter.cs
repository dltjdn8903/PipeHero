using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCharacter : BaseObject
{
    double m_CurrentHP = 0;

    double m_MaxHP = 0;

    double m_Speed = 0;

    eClassType m_ClassType = eClassType.CLASS_NONE;

    eConditionType m_ConditionType = eConditionType.CONDITION_NONE;

    CharacterTemplateData m_TemplateData = null;
    CharacterFactorTable m_CharacterFactorTable = new CharacterFactorTable();

    List<SkillData> m_listNormalAttack = new List<SkillData>();
    List<SkillData> m_listSkill = new List<SkillData>();
    List<SkillData> m_listPassive = new List<SkillData>();

    public SkillData SELECT_SKILL
    {
        get;
        set;
    }

    public CharacterFactorTable CHARACTER_FACTOR { get { return m_CharacterFactorTable; } }
    public double CURRENT_HP
    {
        get { return m_CurrentHP; }
        set { m_CurrentHP = value; }
    }
    public double MAX_HP
    {
        get { return m_MaxHP; }
        set { m_MaxHP = value; }
    }

    public double SPEED
    {
        get { return m_Speed; }
        set { m_Speed = value; }
    }

    public CharacterTemplateData CHARACTER_TEMPLATE { get { return m_TemplateData; } }

    public eClassType CLASS { get { return m_ClassType; } }

    public eConditionType CONDITION
    {
        get { return m_ConditionType; }
        set { m_ConditionType = value; }
    }


    public void IncreaseCurrentHP(double valueData)
    {
        if (valueData < 0)
        {
            GlobalValue.Instance.PlaySound(m_TemplateData.HITSOUND);
        }

        m_CurrentHP += valueData;
        if (m_CurrentHP < 0)
            m_CurrentHP = 0;

        if (m_CurrentHP > CHARACTER_FACTOR.GetFactorData(eFactorData.MAX_HP))
            m_CurrentHP = CHARACTER_FACTOR.GetFactorData(eFactorData.MAX_HP);

        if (m_CurrentHP == 0)
        {
            OBJECT_STATE = eBaseObjectState.STATE_DIE;
            GlobalValue.Instance.PlaySound(m_TemplateData.DEADSOUND);
        }
    }
    
    public void SetTemplate( CharacterTemplateData templateData )
    {
        m_TemplateData = templateData;
        m_CharacterFactorTable.AddFactorTable("CHARACTER", m_TemplateData.FACTOR_TABLE);
        m_MaxHP = CHARACTER_FACTOR.GetFactorData(eFactorData.MAX_HP);
        m_CurrentHP = CHARACTER_FACTOR.GetFactorData(eFactorData.MAX_HP);
        m_ClassType = (eClassType)CHARACTER_FACTOR.GetFactorData(eFactorData.CLASS);
    }

    public void AddAttack(SkillData skillData)
    {
        m_listNormalAttack.Add(skillData);
    }

    public void AddSkill(SkillData skillData)
    {
        m_listSkill.Add(skillData);
    }

    public SkillData GetNormalAttackByIndex()
    {
        if (m_listNormalAttack.Count > 0)
            return m_listNormalAttack[0];

        return null;
    }

    public SkillData GetSkillByIndex(int nIndex)
    {
        if (m_listSkill.Count >= nIndex)
            return m_listSkill[nIndex];

        return null;
    }
}
