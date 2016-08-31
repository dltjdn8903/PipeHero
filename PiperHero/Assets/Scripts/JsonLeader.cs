using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JsonLeader : MonoBehaviour
{
    string m_FileName = "SKILL_DATA";

    string m_MainKey = string.Empty;
    //string FirstKey = string.Empty;
    //string SecondKey = string.Empty;

    Dictionary<string, List<string>> m_BaseDicList = new Dictionary<string, List<string>>();

    Dictionary<string, object[]> m_BaseDic = new Dictionary<string, object[]>();
    List<string> m_BaseList = new List<string>();

    void Awake()
    {
        string firstKey = string.Empty;

        TextAsset AssetData = Resources.Load<TextAsset>(m_FileName);
        if (AssetData == null)
            return;

        JSONNode rootNode = JSON.Parse(AssetData.text);

        if (rootNode == null)
            return;

        JSONNode mainDic = null;

        foreach (KeyValuePair<string, JSONNode> testValue in rootNode.AsObject)
        {
            m_MainKey = testValue.Key;
            mainDic = testValue.Value;
        }

        JSONArray arrayNode = mainDic as JSONArray;
        JSONClass dictionaryNode = mainDic as JSONClass;
        //int a = 0;
        if (arrayNode != null)
        {
            for (int i = 0; i < arrayNode.Count; ++i)
            {
                JSONNode firstNode = arrayNode[i];
                foreach (KeyValuePair<string, JSONNode> testValue in firstNode.AsObject)
                {
                    string secondKey = testValue.Key;
                    string secondValue = testValue.Value;
                }
            }
        }

        if (dictionaryNode != null)
        {
            Foreacher(firstKey, dictionaryNode);
            //foreach (KeyValuePair<string, JSONNode> testValue in dictionaryNode)
            //{
            //    string firstKey = testValue.Key;

            //    foreach (KeyValuePair<string, JSONNode> testvalue2 in testValue.Value.AsObject)
            //    {
            //        string secondKey = testvalue2.Key;
            //        string secondValue = testvalue2.Value;
            //        if (secondValue == string.Empty)
            //        {
            //            JSONArray econdArrayValue = testvalue2.Value.AsArray;
            //            int dd = 0;
            //            //for (int i = 0; i < econdArrayValue.Count; ++i)
            //            //{
            //            //    string thirdArrayValue = econdArrayValue[i];
            //            //}
            //        }
            //    }
            //}
            //Foreacher(dictionaryNode);
        }
    }

    void Foreacher(string _firstKey, JSONNode _node)
    {
        foreach (KeyValuePair<string, JSONNode> secondNode in _node.AsObject)
        {
            if(_firstKey != string.Empty)
            {
                _firstKey = secondNode.Key;
            }
            object value = secondNode.Value;

            //if ((string)value == string.Empty)
            //{
            //    JSONNode thirdNode = secondNode.Value.AsObject;
            //    JSONArray thirdArray = secondNode.Value.AsArray;
            //    if(thirdNode != null)
            //    {
            //        Foreacher(_firstKey, thirdNode);
            //    }
            //    else if(thirdArray != null)
            //    {
            //        List<string> testList = new List<string>();
            //        for (int i = 0; i < thirdArray.Count; ++i)
            //        {
            //            testList.Add(thirdArray[i]);
            //        }
            //        m_BaseDic.Add(_firstKey, testList);
            //        _firstKey = string.Empty;
            //        int a = 0;
            //    }
            //}
            //else
            //{
            //object[] aas = { value };
            //m_BaseDic.Add(_firstKey, aas);
            //}
        }
        return;
    }
}