using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum eFactorData
{
    //클래스
    CLASS           ,

    // 체력
    MAX_HP          ,

    // 공격
    ATTACK          ,

    // 방어
    DEFENCE         ,

    // 갯수
    FACTOR_COUNT    ,
}


public sealed class FactorTable
{
    Dictionary<eFactorData, double> m_dicData = new Dictionary<eFactorData, double>();

    public void InitData()
    {
        m_dicData.Clear();
    }

    public void Copy( FactorTable factorTable )
    {
        foreach( KeyValuePair<eFactorData, double> keyValue in factorTable.m_dicData)
        {
            IncreaseData(keyValue.Key, keyValue.Value);
        }
    }

    public void IncreaseData( eFactorData factorData, double valueData )
    {
        double prevValue = 0.0;

        m_dicData.TryGetValue(factorData, out prevValue);

        m_dicData[factorData] = prevValue + valueData;
    }

    public void DecreaseData(eFactorData factorData, double valueData)
    {
        double prevValue = 0.0;
        m_dicData.TryGetValue(factorData, out prevValue);

        m_dicData[factorData] = prevValue - valueData;
    }

    public void SetData( eFactorData factorData, double valueData )
    {
        m_dicData[factorData] = valueData;
    }

    public void RemoveData( eFactorData factorData )
    {
        m_dicData.Remove(factorData);
    }

    public double GetFactorData( eFactorData factorData )
    {
        double valueData = 0.0f;
        m_dicData.TryGetValue(factorData, out valueData);
        return valueData;
    }
    


}
