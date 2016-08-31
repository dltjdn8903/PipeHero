using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum eComboType
{
    COMBO_A     ,
    COMBO_B     ,
    COMBO_C     ,
}

public class SkillData
{
    string m_strKey = string.Empty;
    float m_Range = 0;
    
    List<string> m_listCombo_A = new List<string>();
    List<string> m_listCombo_B = new List<string>();
    List<string> m_listCombo_C = new List<string>();

    public float RANGE { get { return m_Range; } }
    public List<string> LIST_COMBO_A { get { return m_listCombo_A; } }
    public List<string> LIST_COMBO_B { get { return m_listCombo_B; } }
    public List<string> LIST_COMBO_C { get { return m_listCombo_C; } }
    
    public SkillData( string strKey, SimpleJSON.JSONNode nodeData )
    {
        m_strKey = strKey;
        m_Range = nodeData["RANGE"].AsFloat;

        SimpleJSON.JSONArray arrComboA = nodeData["COMBO_A"].AsArray;
        SimpleJSON.JSONArray arrComboB = nodeData["COMBO_B"].AsArray;
        SimpleJSON.JSONArray arrComboC = nodeData["COMBO_C"].AsArray;

        if( arrComboA != null )
        {
            for( int i = 0; i < arrComboA.Count; ++i )
            {
                m_listCombo_A.Add(arrComboA[i]);
            }
        }

        if( arrComboB != null )
        {
            for( int i = 0; i < arrComboB.Count; ++i )
            {
                m_listCombo_B.Add(arrComboB[i]);
            }
        }

        if( arrComboC != null )
        {
            for( int i = 0; i < arrComboC.Count; ++i )
            {
                m_listCombo_C.Add(arrComboC[i]);
            }
        }
    }
}
