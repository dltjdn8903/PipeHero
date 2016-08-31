using UnityEngine;
using System.Collections;

public abstract class BaseSkill : CacheObject
{
    public BaseObject OWNER
    {
        get;
        set;
    }

    public BaseObject TARGET
    {
        get;
        set;
    }

    public Vector3 DESTINATION
    {
        get;
        set;
    }

    public SkillTemplate SKILL_TEMPLATE
    {
        get;
        set;
    }
    
    public bool END
    {
        get;
        protected set;
    }

    public SkillEvent SKILLEVENT
    {
        get;
        set;
    }

    abstract public void InitSkill();
    abstract public void UpdateSkill();
}