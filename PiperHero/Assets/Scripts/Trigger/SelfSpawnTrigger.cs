using UnityEngine;
using System.Collections;

public class SelfSpawnTrigger : BaseTrigger
{
    [SerializeField]
    BaseObject m_SelfCharacter = null;

    eTriggerResultType resultType = eTriggerResultType.TRIGGER_RESULT_WAIT;

    public BaseObject SELF_CHARACTER
    {
        get { return m_SelfCharacter; }
        set { m_SelfCharacter = value; }
    }

    public override void InitTrigger()
    {
        base.InitTrigger();

        m_SelfCharacter.SelfObject.SetActive(true);
        m_SelfCharacter.SelfTransform.SetParent(null, true);

        TriggerManager.Instance.MAIN_OBJECT = m_SelfCharacter;
    }

}
