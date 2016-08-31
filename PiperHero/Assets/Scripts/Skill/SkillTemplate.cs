using UnityEngine;
using System.Collections;


public class SkillTemplate
{
    string m_strKey = string.Empty;

    eSkillTemplateType m_SkillType = eSkillTemplateType.ATTACK_TARGET;
    eSkillAttackRangeType m_RangeType = eSkillAttackRangeType.RANGE_BOX;
    eConditionType m_SkillConditionType = eConditionType.CONDITION_NONE;
    eSkillTargettingType m_TargetingType = eSkillTargettingType.TARGETTINGTYPE_NONE;
    AudioClip m_AudioClip;

    // Box                  Sphere
    // 1 - X Scale          1 - Radius
    // 2 - Z Scale
    float m_RangeData_1 = 0;
    float m_RangeData_2 = 0;

    FactorTable m_FactorTable = new FactorTable();

    public eSkillTemplateType SKILL_TYPE { get { return m_SkillType; } }
    public eSkillAttackRangeType RANGE_TYPE { get { return m_RangeType; } }

    public float RANGE_DATA_1 { get { return m_RangeData_1; } }
    public float RANGE_DATA_2 { get { return m_RangeData_2; } }

    public eConditionType SKILLCONDITION_TYPE
    {
        get { return m_SkillConditionType; }
    }

    public FactorTable FACTOR_TABLE { get { return m_FactorTable; } }

    public eSkillTargettingType TARGETING_TYPE
    {
        get { return m_TargetingType; }
        set { m_TargetingType = value; }
    }

    public AudioClip AUDIOCLIP { get { return m_AudioClip; } }

    public SkillTemplate( string strKey, SimpleJSON.JSONNode nodeData )
    {
        m_strKey = strKey;
         
        m_SkillType = (eSkillTemplateType)nodeData["SKILL_TYPE"].AsInt;
        m_RangeType = (eSkillAttackRangeType)nodeData["RANGE_TYPE"].AsInt;
        m_RangeData_1 = nodeData["RANGE_DATA_1"].AsFloat;
        m_RangeData_2 = nodeData["RANGE_DATA_2"].AsFloat;
        m_SkillConditionType = (eConditionType)nodeData["EFFECT_TYPE"].AsInt;
        m_TargetingType = (eSkillTargettingType)nodeData["TARGETING_TYPE"].AsInt;
        m_AudioClip = Resources.Load<AudioClip>(nodeData["SOUND_EFFECT"]);

        for ( int i = 0; i < (int)eFactorData.FACTOR_COUNT; ++i )
        {
            eFactorData factorData = (eFactorData)i;
            double valueData = nodeData[factorData.ToString("F")].AsDouble;
            if (valueData > 0)
                m_FactorTable.IncreaseData(factorData, valueData);
        }
    }
}
