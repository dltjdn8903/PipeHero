using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public enum eEffectType
{
    EFFECT_ONESHOT,
    EFFECT_FOLLOWTARGET,
}

public class EffectManager : BaseManager<EffectManager>
{
    Dictionary<string, EffectTemplate> m_DicEffect = new Dictionary<string, EffectTemplate>();
    Dictionary<string, GameObject> m_DicEffectObject = new Dictionary<string, GameObject>();

    List<BaseEffect> m_UseEffectList = new List<BaseEffect>();
    List<BaseEffect> m_DeleteEffectList = new List<BaseEffect>();

    public void OnStart()
    {
        GameObject[] sKillprojectiles = Resources.LoadAll<GameObject>("Effects");

        TextAsset effectAssetData = Resources.Load("EFFECT_TEMPLATE") as TextAsset;

        if (effectAssetData == null)
            return;

        JSONNode rootNode = JSON.Parse(effectAssetData.text);
        if (rootNode == null)
            return;

        JSONClass effectTemplateNode = rootNode["EFFECT_TEMPLATE"] as JSONClass;

        foreach (KeyValuePair<string, JSONNode> keyValue in effectTemplateNode)
        {
            EffectTemplate effectTemplate = new EffectTemplate(keyValue.Key, keyValue.Value);
            m_DicEffect.Add(keyValue.Key, effectTemplate);
        }

        for (int i = 0; i < sKillprojectiles.Length; ++i)
        {
            m_DicEffectObject.Add(sKillprojectiles[i].name, sKillprojectiles[i]);
        }
    }

    void Update ()
    {
        if (m_UseEffectList.Count == 0)
            return;
        
        for(int i =0;i<m_UseEffectList.Count;++i)
        {
            m_UseEffectList[i].UpdateEffect();
            if (m_UseEffectList[i].END == true)
            {
                m_DeleteEffectList.Add(m_UseEffectList[i]);
                m_UseEffectList.RemoveAt(i);
            }
        }

        if(m_DeleteEffectList.Count > 0)
        {
            for (int i = 0; i < m_DeleteEffectList.Count; ++i)
            {
                Destroy(m_DeleteEffectList[i].SelfObject);
            }
        }
    }

    public BaseEffect ThrowEffect(string _key, Vector3 _position, BaseObject _target = null)
    {
        EffectTemplate effectTemplate = m_DicEffect[_key];

        BaseEffect baseEffect = null;

        GameObject effectObject = new GameObject();

        switch (effectTemplate.EffectType)
        {
            case eEffectType.EFFECT_ONESHOT:
                {
                    baseEffect = effectObject.AddComponent<Effect_OneShot>();
                }
                break;

            case eEffectType.EFFECT_FOLLOWTARGET:
                {
                    baseEffect = effectObject.AddComponent<Effect_FollowTarget>();
                }
                break;
        }

        baseEffect.EFFECTTEMPLATE = effectTemplate;

        baseEffect.name = effectTemplate.Key;

        baseEffect.InitEffect();


        if (_target != null)
        {
            baseEffect.TARGET = _target;
            effectObject = Instantiate(m_DicEffectObject[effectTemplate.ResourceName]) as GameObject;
        }
        else
        {
            effectObject = Instantiate(m_DicEffectObject[effectTemplate.ResourceName], _position, Quaternion.identity) as GameObject;
        }
        effectObject.transform.parent = baseEffect.SelfTransform;

        m_UseEffectList.Add(baseEffect);

        return baseEffect;
    }
}
