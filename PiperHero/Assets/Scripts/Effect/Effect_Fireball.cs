using UnityEngine;
using System.Collections;

public class Effect_Fireball : BaseEffect
{
    void Start()
    {
        Debug.Log("StartDestroy");
        Destroy(gameObject, 1.0f);
    }
}
