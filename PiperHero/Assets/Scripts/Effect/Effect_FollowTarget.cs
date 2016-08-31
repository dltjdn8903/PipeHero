using UnityEngine;
using System.Collections;

public class Effect_FollowTarget : BaseEffect
{
    float m_ElapsedTime = 0;

    void Start()
    {
        SelfTransform.position = TARGET.SelfTransform.position;
    }

    public override void InitEffect()
    {
    }

    public override void UpdateEffect()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        SelfTransform.position = TARGET.SelfTransform.position;
        if (m_ElapsedTime >= 10.0f)
        {
            END = true;
        }
    }
}
