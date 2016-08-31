using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObserverManager : BaseManager<ObserverManager>
{
    Dictionary<eTeamType, List<Observer_Component>> m_dicObserver = new Dictionary<eTeamType, List<Observer_Component>>();

    List<Observer_Component> m_Players = null;
    List<Observer_Component> m_Enemys = null;
    List<Observer_Component> m_Player = null;
    List<Observer_Component> m_Boss = null;
    List<Observer_Component> m_Monsters = null;
    List<Observer_Component> m_Minons = null;

    public void AddObserver(Observer_Component addObserver)
    {
        List<Observer_Component> listObserver = null;
        eTeamType teamType = addObserver.TEAM_TYPE;

        if (addObserver.AI is ChampionAI && teamType == eTeamType.TEAM_PLAYER)
        {
            GlobalValue.Instance.Player = addObserver;
        }
        else if (addObserver.AI is ChampionAI && teamType == eTeamType.TEAM_ENEMY)
        {
            addObserver.AI.SetBoss();
        }

        if (m_dicObserver.ContainsKey(teamType) == false)
        {
            listObserver = new List<Observer_Component>();
            m_dicObserver.Add(teamType, listObserver);
        }
        else
        {
            m_dicObserver.TryGetValue(teamType, out listObserver);
        }

        listObserver.Add(addObserver);
    }

    public void RemoveObserver(Observer_Component removeObserver, bool bDelete = true)
    {
        eTeamType teamType = removeObserver.TEAM_TYPE;
        if (m_dicObserver.ContainsKey(teamType) == true)
        {
            List<Observer_Component> listObserver = null;
            m_dicObserver.TryGetValue(teamType, out listObserver);
            listObserver.Remove(removeObserver);
        }

        if (bDelete == true)
        {
            Destroy(removeObserver.SelfObject);
        }
    }

    public Observer_Component GetPlayer()
    {
        List<Observer_Component> PlayerList = m_dicObserver[eTeamType.TEAM_PLAYER];

        for (int i = 0; i < PlayerList.Count; ++i)
        {
            if (PlayerList[i].AI is ChampionAI)
            {
                return PlayerList[i];
            }
        }
        return null;
    }

    public List<Observer_Component> GetPlayerTeam()
    {
        m_Players = m_dicObserver[eTeamType.TEAM_PLAYER];
        return m_Players;
    }

    public List<Observer_Component> GetMinions(eTeamType _team)
    {
        m_dicObserver.TryGetValue(_team, out m_Players);
        //for (int i = 0; i < m_Players.Count; ++i)
        //{
        //    if (m_Players[i].AI is MinionAI)
        //    {
        //        int a = 0;

        //        BaseObject Tesrt = m_Players[i];

        //        m_Minons.Add(Tesrt);

        //        int c = 0;
        //    }
        //}

        return m_Players;
    }

    public List<Observer_Component> GetSameTeam(eTeamType _team)
    {
        List<Observer_Component> otherTeamList = null;
        if (_team == eTeamType.TEAM_ENEMY)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_ENEMY, out otherTeamList);
        }
        else if (_team == eTeamType.TEAM_PLAYER)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_PLAYER, out otherTeamList);
        }
        return otherTeamList;
    }

    public List<Observer_Component> GetOtherTeam(eTeamType _team)
    {
        List<Observer_Component> otherTeamList = null;
        if(_team == eTeamType.TEAM_ENEMY)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_PLAYER, out otherTeamList);
        }
        else if (_team == eTeamType.TEAM_PLAYER)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_ENEMY, out otherTeamList);
        }
        return otherTeamList;
    }

    public BaseObject GetSearchEnemy(eTeamType team, BaseObject observer, float fRadius = 10.0f)
    {
        Vector3 myPosition = observer.SelfTransform.position;

        float fNearDistance = fRadius;
        Observer_Component nearObserver = null;

        List<Observer_Component> dictionary = null;

        if (team == eTeamType.TEAM_PLAYER)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_ENEMY, out dictionary);
        }
        else if (team == eTeamType.TEAM_ENEMY)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_PLAYER, out dictionary);
        }

        if(dictionary == null)
            return null;

        for (int i = 0; i < dictionary.Count; ++i)
        {
            if (dictionary[i].OBJECT_STATE == eBaseObjectState.STATE_DIE || dictionary[i].OBJECT_STATE == eBaseObjectState.STATE_HIDE)
            {
                continue;
            }

            if (dictionary[i].SelfObject.activeSelf == false)
                continue;

            float fDistance = Vector3.Distance(myPosition, dictionary[i].SelfTransform.position);
            if (fDistance < fNearDistance)
            {
                fNearDistance = fDistance;
                nearObserver = dictionary[i];
            }
        }

        return nearObserver;
    }
    public BaseObject GetSearchLongRangeEnemy(eTeamType team, BaseObject observer, float fRadius = 4.0f)
    {
        Vector3 myPosition = observer.SelfTransform.position;

        float fNearDistance = fRadius;
        Observer_Component nearObserver = null;

        List<Observer_Component> dictionary = null;

        if (team == eTeamType.TEAM_PLAYER)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_ENEMY, out dictionary);
        }
        else if (team == eTeamType.TEAM_ENEMY)
        {
            m_dicObserver.TryGetValue(eTeamType.TEAM_PLAYER, out dictionary);
        }

        if (dictionary == null)
            return null;

        for (int i = 0; i < dictionary.Count; ++i)
        {
            if (dictionary[i].OBJECT_STATE == eBaseObjectState.STATE_DIE)
                continue;

            if (dictionary[i].SelfObject.activeSelf == false)
                continue;

            if ((eClassType)dictionary[i].GetData("CLASS") == eClassType.CLASS_SWORD)
                continue;

            float fDistance = Vector3.Distance(myPosition, dictionary[i].SelfTransform.position);
            if (fDistance < fNearDistance)
            {
                fNearDistance = fDistance;
                nearObserver = dictionary[i];
            }
        }

        return nearObserver;
    }

    public BaseObject GetSearchHealTarget(eTeamType team, BaseObject observer, float fRadius = 4.0f)
    {
        Vector3 myPosition = observer.SelfTransform.position;

        float fNearDistance = fRadius;
        Observer_Component nearObserver = null;

        foreach (KeyValuePair<eTeamType, List<Observer_Component>> keyValue in m_dicObserver)
        {
            if (keyValue.Key != team)
                continue;

            for (int i = 0; i < keyValue.Value.Count; ++i)
            {
                if (keyValue.Value[i].OBJECT_STATE == eBaseObjectState.STATE_DIE)
                    continue;

                if (keyValue.Value[i].SelfObject.activeSelf == false)
                    continue;

                if ((double)keyValue.Value[i].GetData("HP") == (double)keyValue.Value[i].GetData("MAX_HP"))
                    continue;

                float fDistance = Vector3.Distance(myPosition, keyValue.Value[i].SelfTransform.position);

                if (fDistance < fNearDistance)
                {
                    fNearDistance = fDistance;
                    nearObserver = keyValue.Value[i];
                }
            }
        }
        return nearObserver;
    }
}
