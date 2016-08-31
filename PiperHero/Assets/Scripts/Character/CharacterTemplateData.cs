using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum eClassType
{
    CLASS_NONE,
    CLASS_SWORD,
    CLASS_GUNNER,
    CLASS_HEALER,

    CLASS_KIGNT = 101,
    CLASS_MAGICIANICE,
    CLASS_MAGICIANFIRE,
    CLASS_PRIEST,

    CLASS_BOSS1 = 201,
    CLASS_BOSS2,
    CLASS_LAST_BOSS = 209,

    CLASS_MON_WF = 301,
    CLASS_MON_DG,
    CLASS_MON_MR,
    CLASS_MON_SK
}

public class CharacterTemplateData 
{
    string m_strKey = string.Empty;

    FactorTable m_FactorTable = new FactorTable();

    List<string> m_listNormalAttack = new List<string>();
    List<string> m_listSkill = new List<string>();
    List<string> m_listPassive = new List<string>();
    AudioClip m_DeadSound;
    AudioClip m_HitSound;

    public string KEY { get { return m_strKey; } }
    public FactorTable FACTOR_TABLE
    {
        get { return m_FactorTable; }
        set { m_FactorTable = value; }
    }

    public List<string> LIST_NORMAL_ATTACK { get { return m_listNormalAttack; } }
    public List<string> LIST_SKILL { get { return m_listSkill; } }
    public List<string> LIST_PASSIVE { get { return m_listPassive; } }
    public AudioClip DEADSOUND { get { return m_DeadSound; } }
    public AudioClip HITSOUND { get { return m_HitSound; } }

    public CharacterTemplateData( string strKey, SimpleJSON.JSONNode nodeData )
    {
        m_strKey = strKey;

        for( int i = 0; i < (int)eFactorData.FACTOR_COUNT; ++i )
        {
            eFactorData factorData = (eFactorData)i;
            double valueData = nodeData[factorData.ToString("F")].AsDouble;
            if( valueData > 0 )
                m_FactorTable.IncreaseData(factorData, valueData);
        }

        SimpleJSON.JSONArray arrNormalAttack = nodeData["NORMAL_ATTACK"].AsArray;
        SimpleJSON.JSONArray arrSkill = nodeData["SKILL"].AsArray;
        SimpleJSON.JSONArray arrPassive = nodeData["PASSIVE"].AsArray;
        m_DeadSound = Resources.Load<AudioClip>(nodeData["DeadSound"]);
        m_HitSound = Resources.Load<AudioClip>(nodeData["HitSound"]);

        if( arrNormalAttack != null )
        {
            for( int i = 0; i < arrNormalAttack.Count; ++i )
            {
                m_listNormalAttack.Add(arrNormalAttack[i]);
            }
        }

        if( arrSkill != null)
        {
            for( int i = 0; i < arrSkill.Count; ++i )
            {
                m_listSkill.Add(arrSkill[i]);
            }
        }

        if ( arrPassive != null)
        {
            for( int i = 0; i < arrPassive.Count; ++i )
            {
                m_listPassive.Add(arrPassive[i]);
            }
        }
    }
}
