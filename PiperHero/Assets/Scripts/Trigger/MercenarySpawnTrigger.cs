using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class MercenarySpawnTrigger : BaseTrigger
{
    [SerializeField]
    List<BaseObject> m_listMonster = new List<BaseObject>();

    public List<BaseObject> LIST_MONSTER
    {
        get { return m_listMonster; }
    }

    public override void InitTrigger()
    {
        for (int i = 0; i < m_listMonster.Count; ++i)
        {
            m_listMonster[i].SelfObject.SetActive(false);
        }
    }

    public override eTriggerResultType UpdateTrigger()
    {
        /*/
        eTriggerResultType resultType = base.UpdateTrigger();
        if (resultType != eTriggerResultType.TRIGGER_RESULT_ING)
            return resultType;

        for (int i = 0; i < m_listMonster.Count;++i)
        {
            m_listMonster[i].SelfObject.SetActive(true);
            m_listMonster[i].SelfTransform.SetParent(null, true);
        }

        if (!COMPLETE)
        {
            GetComponent<SphereCollider>().enabled = false;
            //SelfObject.SetActive(false);
            return eTriggerResultType.TRIGGER_RESULT_END;
        }
        

        COMPLETE = true;

        return eTriggerResultType.TRIGGER_RESULT_ING;        
        /*/

        eTriggerResultType resultType = base.UpdateTrigger();
        if (resultType != eTriggerResultType.TRIGGER_RESULT_ING)
            return resultType;

        if (COMPLETE)
        {
            bool bResult = false;
            for (int i = 0; i < m_listMonster.Count; ++i)
            {
                if (m_listMonster[i] != null) //몬스터가 한마리라도 null이 아니면 살아있다.
                {
                    bResult = true;
                    break;
                }
            }

            if (bResult == true)
                return eTriggerResultType.TRIGGER_RESULT_ING; //살아있으면 ing

            return eTriggerResultType.TRIGGER_RESULT_END; //없으면 end
        }

        for (int i = 0; i < m_listMonster.Count; ++i)
        {
            m_listMonster[i].SelfObject.SetActive(true);
            m_listMonster[i].SelfTransform.SetParent(null, true);
        }

        COMPLETE = true;
        return eTriggerResultType.TRIGGER_RESULT_ING;

    }

    void OnTriggerEnter(Collider other)
    {
        if (COLLISION == true)
            return;

        GameObject colObject = other.gameObject;
        BaseObject colBaseObject = colObject.GetComponent<BaseObject>();

        if (colBaseObject == TriggerManager.Instance.MAIN_OBJECT)
        {
            COLLISION = true;
        }
    }
}
