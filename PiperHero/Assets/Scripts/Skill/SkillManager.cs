using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SimpleJSON;

public class SkillManager : BaseManager<SkillManager>
{
    Dictionary<BaseObject, List<BaseSkill>> m_dicUseSkill = new Dictionary<BaseObject, List<BaseSkill>>();

    Dictionary<string, SkillData> m_dicSkillData = new Dictionary<string, SkillData>();
    Dictionary<string, SkillTemplate> m_dicSkillTemplate = new Dictionary<string, SkillTemplate>();

    void Awake()
    {
        _LoadSkillData("SKILL_DATA_SUB");
        _LoadSkillTemplate("SKILL_TEMPLATE_SUB");
    }

    void _LoadSkillData(string strFileName)
    {
        TextAsset skillAssetData = Resources.Load<TextAsset>(strFileName);
        if (skillAssetData == null)
            return;

        JSONNode rootNode = JSON.Parse(skillAssetData.text);
        if (rootNode == null)
            return;

        JSONClass skillDataNode = rootNode["SKILL_DATA"] as JSONClass;
        foreach (KeyValuePair<string, JSONNode> keyValue in skillDataNode)
        {
            SkillData skillData = new SkillData(keyValue.Key, keyValue.Value);
            m_dicSkillData.Add(keyValue.Key, skillData);
        }
    }

    void _LoadSkillTemplate(string strFileName)
    {
        TextAsset skillAssetData = Resources.Load<TextAsset>(strFileName);
        if (skillAssetData == null)
            return;

        JSONNode rootNode = JSON.Parse(skillAssetData.text);
        if (rootNode == null)
            return;

        JSONClass skillTemplateNode = rootNode["SKILL_TEMPLATE"] as JSONClass;

        foreach (KeyValuePair<string, JSONNode> keyValue in skillTemplateNode)
        {
            SkillTemplate skillTemplate = new SkillTemplate(keyValue.Key, keyValue.Value);
            m_dicSkillTemplate.Add(keyValue.Key, skillTemplate);
        }
    }

    public SkillData GetSkillData(string strKey)
    {
        SkillData skillData = null;
        m_dicSkillData.TryGetValue(strKey, out skillData);
        return skillData;
    }

    public SkillTemplate GetSkillTemplate(string strKey)
    {
        SkillTemplate skillTemplate = null;
        m_dicSkillTemplate.TryGetValue(strKey, out skillTemplate);
        return skillTemplate;
    }

    List<BaseSkill> listDelete = new List<BaseSkill>();
    void Update()
    {
        Dictionary<BaseObject, List<BaseSkill>>.Enumerator enumerator = m_dicUseSkill.GetEnumerator();
        while (enumerator.MoveNext())
        {
            List<BaseSkill> listSkill = enumerator.Current.Value;
            for (int i = 0; i < listSkill.Count; ++i)
            {
                BaseSkill updateSkill = listSkill[i];
                updateSkill.UpdateSkill();
                if (updateSkill.END == true)
                {
                    listDelete.Add(updateSkill);
                }
            }

            for (int i = 0; i < listDelete.Count; ++i)
            {
                listSkill.Remove(listDelete[i]);
                ObjectPoolManager.Instance.Despawn(listDelete[i].SelfObject);

                Transform skillTransform = listDelete[i].SelfTransform;

                for (int j = 0; j < skillTransform.childCount; ++j)
                {
                    Destroy(skillTransform.GetChild(j));
                }

                Collider[] arrCollider = listDelete[i].SelfObject.GetComponents<Collider>();
                for (int k = 0; k < arrCollider.Length; ++k)
                {
                    Destroy(arrCollider[k]);
                }
                Destroy(listDelete[i]);
            }
            listDelete.Clear();
        }
    }

    public void RunSkill(BaseObject keyObject, string strSkillTemplateKey)
    {
        BaseSkill runSkill = null;
        SkillTemplate skillTemplate = GetSkillTemplate(strSkillTemplateKey);

        if (skillTemplate == null)
            return;

        if(skillTemplate.TARGETING_TYPE == eSkillTargettingType.TARGETTINGTYPE_NONE || skillTemplate.TARGETING_TYPE == eSkillTargettingType.TARGETTINGTYPE_TARGET)
        {
            runSkill = _CreateSkill(keyObject, skillTemplate);
        }
        else if(skillTemplate.TARGETING_TYPE == eSkillTargettingType.TARGETTINGTYPE_NONETARGET)
        {
            runSkill = _CreateSkill(keyObject, skillTemplate, (Vector3)keyObject.GetData("DESTINATION"));
        }

        RunSkill(keyObject, runSkill);
    }

    public void RunSkill(BaseObject keyObject, string strSkillTemplateKey, Vector3 _targetPosition)
    {
        SkillTemplate skillTemplate = GetSkillTemplate(strSkillTemplateKey);
        if (skillTemplate == null)
            return;

        BaseSkill runSkill = _CreateSkill(keyObject, skillTemplate, _targetPosition);
        RunSkill(keyObject, runSkill);
    }

    public void RunSkill(BaseObject keyObject, BaseSkill runSkill)
    {
        List<BaseSkill> listSkill = null;
        if (m_dicUseSkill.ContainsKey(keyObject) == false)
        {
            listSkill = new List<BaseSkill>();
            m_dicUseSkill.Add(keyObject, listSkill);
        }
        else
        {
            listSkill = m_dicUseSkill[keyObject];
        }

        listSkill.Add(runSkill);
    }

    BaseSkill _CreateSkill(BaseObject owner, SkillTemplate skillTemplate, Vector3 _targetPosition = new Vector3())
    {
        BaseSkill makeSkill = null;
        GameObject skillObject = ObjectPoolManager.Instance.Spawn();

        Transform parentTransform = null;

        switch (skillTemplate.SKILL_TYPE)
        {
            case eSkillTemplateType.ATTACK_TARGET:
                {
                    makeSkill = skillObject.AddComponent<MeleeBasic>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.ATTACK_MISSILE:
                {
                    makeSkill = skillObject.AddComponent<RangeBasic>();
                    parentTransform = owner.GetChild("ShootingPoint");
                }
                break;

            case eSkillTemplateType.BASIC_HEAL:
                {
                    makeSkill = skillObject.AddComponent<HealBasic>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_WHIRLWIND:
                {
                    makeSkill = skillObject.AddComponent<SkillWhirlwind>();
                    parentTransform = owner.SelfTransform;
                }
                break;
            case eSkillTemplateType.SKILL_AGRRO:
                {
                    makeSkill = skillObject.AddComponent<SkillAgrro>();
                    parentTransform = owner.SelfTransform;
                }
                break;
            case eSkillTemplateType.SKILL_IMMUNEDAMAGE:
                {
                    makeSkill = skillObject.AddComponent<SkillImmune>();
                    parentTransform = owner.SelfTransform;
                }
                break;
            case eSkillTemplateType.SKILL_PIERCING:
                {
                    makeSkill = skillObject.AddComponent<SkillPiercing>();
                    parentTransform = owner.GetChild("ShootingPoint");
                }
                break;
            case eSkillTemplateType.SKILL_RAMPAGE:
                {
                    makeSkill = skillObject.AddComponent<RangeBasic>();
                    parentTransform = owner.GetChild("ShootingPoint");
                }
                break;
            case eSkillTemplateType.SKILL_HIDE:
                {
                    makeSkill = skillObject.AddComponent<SkillHide>();
                    parentTransform = owner.SelfTransform;
                }
                break;
            case eSkillTemplateType.SKILL_HEAL:
                {
                    makeSkill = skillObject.AddComponent<SkillHeal>();
                    parentTransform = owner.SelfTransform;
                }
                break;
            case eSkillTemplateType.SKILL_FIREBALL:
                {
                    makeSkill = skillObject.AddComponent<SkillFireball>();
                    parentTransform = owner.GetChild("ShootingPoint");
                }
                break;
            case eSkillTemplateType.SKILL_BUFF:
                {
                    makeSkill = skillObject.AddComponent<SkillBuff>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_FIRENOVA:
                {
                    makeSkill = skillObject.AddComponent<SkillFireNova>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_FIRENOVABULLET:
                {
                    makeSkill = skillObject.AddComponent<SkillFireNovaBullet>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_SUMMON:
                {
                    makeSkill = skillObject.AddComponent<SkillSummon>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_METEOR:
                {
                    makeSkill = skillObject.AddComponent<SkillMeteor>();
                    parentTransform = owner.SelfTransform;
                }
                break;

            case eSkillTemplateType.SKILL_METEORBULLET:
                {
                    makeSkill = skillObject.AddComponent<SkillMeteorBullet>();
                    parentTransform = GameObject.FindWithTag("MeteorPoint").transform;
                }
                break;
        }

        skillObject.name = skillTemplate.SKILL_TYPE.ToString("F");

        if (makeSkill != null)
        {
            makeSkill.SelfTransform.position = parentTransform.position;
            makeSkill.SelfTransform.rotation = parentTransform.rotation;

            makeSkill.OWNER = owner;
            makeSkill.SKILL_TEMPLATE = skillTemplate;

            if (_targetPosition == Vector3.zero)
                makeSkill.TARGET = owner.GetData("TARGET") as BaseObject;
            else
            {
                makeSkill.TARGET = null;
                makeSkill.DESTINATION = _targetPosition;
            }

            makeSkill.InitSkill();
        }

        if (skillTemplate.RANGE_TYPE == eSkillAttackRangeType.RANGE_BOX)
        {
            BoxCollider collider = skillObject.AddComponent<BoxCollider>();

            collider.size = new Vector3(skillTemplate.RANGE_DATA_1, 1, skillTemplate.RANGE_DATA_2);
            collider.center = new Vector3(0, 0, skillTemplate.RANGE_DATA_2 * 0.5f);
            collider.isTrigger = true;
        }
        else if (skillTemplate.RANGE_TYPE == eSkillAttackRangeType.RANGE_SPHERE)
        {
            SphereCollider collider = skillObject.AddComponent<SphereCollider>();
            collider.radius = skillTemplate.RANGE_DATA_1;
            collider.isTrigger = true;
        }

        GlobalValue.Instance.PlaySound(skillTemplate.AUDIOCLIP);

        return makeSkill;
    }
}
