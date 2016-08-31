using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public sealed class CharacterFactorTable
{
    Dictionary<string, FactorTable> m_dicFactor = new Dictionary<string, FactorTable>();
    FactorTable m_totalFactor = new FactorTable();

    bool m_bRefresh = false;

    public void AddFactorTable( string strKey, FactorTable factorTable )
    {
        m_dicFactor.Remove(strKey);
        m_dicFactor.Add(strKey, factorTable);
        m_bRefresh = true;
    }

    public void RemoveFactorTable( string strKey )
    {
        m_dicFactor.Remove(strKey);
        m_bRefresh = true;
    }

    public double GetFactorData( eFactorData factorData )
    {
        _RefreshTotalFactor();
        return m_totalFactor.GetFactorData(factorData);
    }

    void _RefreshTotalFactor()
    {
        if (m_bRefresh == false)
            return;

        // 초기화
        m_totalFactor.InitData();

        //대입
        foreach( KeyValuePair<string,FactorTable> keyValue in m_dicFactor )
        {
            FactorTable table = keyValue.Value;
            m_totalFactor.Copy(table);
        }

        m_bRefresh = false;
    }
}
