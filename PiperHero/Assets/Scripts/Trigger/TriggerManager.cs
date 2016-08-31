using UnityEngine;
using System.Collections;

using System.Collections.Generic;


//UI에서 스킬을 누르면 이벤트 핸들러에서 TOUCH_TYPE을 변경해준다.

public class TriggerManager : BaseManager< TriggerManager >
{
    List<BaseTrigger> m_listTrigger = new List<BaseTrigger>();

    public BaseObject MAIN_OBJECT { get; set; }
    public Camera MAIN_CAMERA { get; set; }

    public Vector3 Dir { get; set; }

    public void AddTrigger( BaseTrigger trigger )
    {
        m_listTrigger.Add(trigger);
        trigger.InitTrigger();
    }

    public void RemoveTrigger( BaseTrigger trigger, bool bDelete = false )
    {
        m_listTrigger.Remove(trigger);
        if (bDelete)
            Destroy(trigger.SelfObject);
    }

    List<BaseTrigger> listDelete = new List<BaseTrigger>();
    void Update()
    {
        for( int i = 0; i < m_listTrigger.Count; ++i )
        {
            BaseTrigger trigger = m_listTrigger[i];
            eTriggerResultType resultType = trigger.UpdateTrigger();
            switch( resultType )
            {
                case eTriggerResultType.TRIGGER_RESULT_END:
                    {
                        trigger.CallTrigger();
                        listDelete.Add(trigger);
                    }
                    break;
            }
        }

        for( int i = 0; i < listDelete.Count; ++i )
        {
            RemoveTrigger(listDelete[i], true);
        }
        listDelete.Clear();
    }

    public bool HasMonsterSpawn()
    {
        for( int i = 0; i < m_listTrigger.Count; ++i )
        {
            if (m_listTrigger[i] is MonsterSpawnTrigger)
                return true;
        }
        return false;
    }

    //public void Clear()
    //{
    //    for( int i = 0; i < m_listTrigger.Count; ++i )
    //    {
    //        Destroy(m_listTrigger[i].SelfObject);
    //    }
    //    m_listTrigger.Clear();
    //}
}
