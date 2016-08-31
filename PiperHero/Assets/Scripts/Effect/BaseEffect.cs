using UnityEngine;
using System.Collections;



public class BaseEffect : CacheObject
{
    virtual public EffectTemplate EFFECTTEMPLATE
    {
        get;
        set;
    }

    virtual public float LIFETIME
    {
        get;
        set;
    }

    virtual public Vector3 POSITION
    {
        get;
        set;
    }
    virtual public BaseObject TARGET
    {
        get;
        set;
    }

    virtual public bool END
    {
        get;
        set;
    }

    virtual public void InitEffect()
    {

    }

    virtual public void UpdateEffect()
    {

    }
}
