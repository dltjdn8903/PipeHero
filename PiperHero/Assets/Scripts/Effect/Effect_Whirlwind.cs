using UnityEngine;
using System.Collections;

public class Effect_Whirlwind : BaseEffect
{
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }
}
