using UnityEngine;
using System.Collections;

public enum eNPCName
{
    NPC_SOLIDER = 8,
    NPC_SHOP = 9,
    NPC_RANK = 13,
    NPC_SOLIDERSHOP = 7,
    NPC_NPC  = 14,
}

public class BaseNPC : CacheObject
{
    [SerializeField]
    eNPCName m_NPCName = eNPCName.NPC_RANK;

    public eNPCName NPCName
    {
        get { return m_NPCName; }
        set { m_NPCName = value; }
    }
}
