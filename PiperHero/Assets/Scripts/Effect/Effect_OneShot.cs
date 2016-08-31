using UnityEngine;
using System.Collections;

public class Effect_OneShot : BaseEffect
{
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }
}
