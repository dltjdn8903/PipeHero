using UnityEngine;
using System.Collections;

public class UI_Town_EventHandler : MonoBehaviour {

    void Awake()
    {
        UILabel m_Level = GameObject.FindWithTag("Level").GetComponent<UILabel>();
        m_Level.text = string.Format("{0}", GlobalValue.Instance.m_Level);

        UILabel m_Money = GameObject.FindWithTag("Money").GetComponent<UILabel>();
        m_Money.text = string.Format("{0}", GlobalValue.Instance.m_Money);
    }

    public void OnClickMenu()
    {
        UIManager.Instance.ShowUI(eUIType.Pf_UI_Popup_TownMenu);
    }

    public void OnClickStatus()
    {
        UIManager.Instance.ShowUI(eUIType.Pf_UI_Popup_Status);
    }

    public void OnClickSkill1()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_NORMALATTACK);
    }

    public void OnClickSkill2()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL2);
    }

    public void OnClickSkill3()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        //observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL3);
    }

}
