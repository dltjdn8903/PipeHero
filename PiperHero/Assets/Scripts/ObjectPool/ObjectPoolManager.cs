using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePoolType
{
    POOL_EFFECT,
    POOL_ATTACK_PROJECTILES,
    POOL_SKILL_PROJECTILES,
}

public class ObjectPoolManager : BaseManager< ObjectPoolManager >
{
    Stack<GameObject> m_PoolData = new Stack<GameObject>();
    Dictionary<string, BaseSkill> m_SkillPoolData = new Dictionary<string, BaseSkill>();
    List<GameObject> m_listSpawn = new List<GameObject>();
    List<GameObject> m_listSkillSpawn = new List<GameObject>();

    public void OnStart()
    {
        for (int i = 0; i < 50; ++i)
        {
            GameObject newObject = new GameObject();

            newObject.transform.SetParent(SelfTransform);

            newObject.SetActive(false);
            m_PoolData.Push(newObject);
        }
    }

    public GameObject Spawn()
    {
        GameObject spawnObject = m_PoolData.Pop();
        m_listSpawn.Add(spawnObject);
        spawnObject.SetActive(true);
        spawnObject.transform.SetParent(null);

        return spawnObject;
    }

    public void Despawn(GameObject spawnObject)
    {
        if( m_listSpawn.Contains( spawnObject ) == true )
        {
            m_listSpawn.Remove(spawnObject);
            m_PoolData.Push(spawnObject);

            for (int i = 0; i < spawnObject.transform.childCount; ++i)
            {
                Destroy(spawnObject.transform.GetChild(i).gameObject);
            }
            spawnObject.transform.SetParent(SelfTransform);
            spawnObject.SetActive(false);
        }
    }

    //public GameObject SkillSpawn()
    //{
    //    GameObject spawnObject = m_SkillPoolData.Pop();
    //    m_listSkillSpawn.Add(spawnObject);
    //    spawnObject.SetActive(true);
    //    spawnObject.transform.SetParent(null);

    //    return spawnObject;
    //}

    //public void SkillDespawn(GameObject spawnObject)
    //{
    //    if (m_listSkillSpawn.Contains(spawnObject) == true)
    //    {
    //        m_listSkillSpawn.Remove(spawnObject);
    //        m_SkillPoolData.Push(spawnObject);

    //        spawnObject.transform.SetParent(SelfTransform);
    //        spawnObject.SetActive(false);
    //    }
    //}
}