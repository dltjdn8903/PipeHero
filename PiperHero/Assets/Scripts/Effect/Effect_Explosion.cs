using UnityEngine;
using System.Collections;

public class Effect_Explosion : BaseEffect
{
    void Start()
    {
        Debug.Log("StartDestroy");
        Destroy(gameObject, 1.0f);
    }
}
