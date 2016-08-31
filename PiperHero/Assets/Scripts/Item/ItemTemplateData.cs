using UnityEngine;
using System.Collections;

using SimpleJSON;

public class ItemTemplateData 
{
    public int ITEM_ID { get; private set; }
    public string ITEM_IMAGE { get; private set; }
    public string ITEM_NAME { get; private set; }
    public string ITEM_EXPLAIN { get; private set; }
    public float ITEM_MONEY { get; private set; }

    public ItemTemplateData(SimpleJSON.JSONNode nodeData)
    {
        ITEM_ID = nodeData["ITEM_ID"].AsInt;
        ITEM_IMAGE = nodeData["ITEM_IMAGE"];
        ITEM_NAME = nodeData["ITEM_NAME"];
        ITEM_EXPLAIN = nodeData["ITEM_EXPLAIN"];
        ITEM_MONEY = nodeData["MONEY"].AsFloat;
    }
    
}
