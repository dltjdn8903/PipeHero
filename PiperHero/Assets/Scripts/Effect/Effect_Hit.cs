using UnityEngine;
using System.Collections;

public class Effect_Hit : BaseEffect
{
    public override void InitEffect()
    {
        END = true;
    }    

    void DestroySelf(float _time)
    {
        END = true;
    }
}
