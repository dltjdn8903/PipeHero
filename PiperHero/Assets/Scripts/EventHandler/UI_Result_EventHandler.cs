using UnityEngine;
using System.Collections;

public class UI_Result_EventHandler : UISingleton<UI_Result_EventHandler>
{
    [SerializeField]
    UILabel m_Time = null;
    [SerializeField]
    UILabel m_Point = null;
    [SerializeField]
    UILabel m_Money = null;

    

    void Awake()
    {  
        Time.timeScale = 0;


        m_Time.text = "시간 : " + string.Format("{0:00}", UI_Dungeon_EventHandler.Instance.f_minute) + "." + string.Format("{0:F2}", GlobalValue.Instance.m_FlowTime) + " ' "; //m_FlowTime.ToString("F0");//
        m_Point.text = "획득 포인트 : " + GlobalValue.Instance.m_GetPoint;
        m_Money.text = "획득 돈 : " + GlobalValue.Instance.m_GetMoney;
    }


    public void OnClickButton()
    {        
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1.0f;

        //플레이어 위치를 로비로 바꿔줌
        if (TriggerManager.Instance.MAIN_OBJECT == null)
        {
            BaseObject pf_Warrior = Resources.Load<BaseObject>("Warrior");
            TriggerManager.Instance.MAIN_OBJECT = GameObject.Instantiate(pf_Warrior) as BaseObject;
        }
        TriggerManager.Instance.MAIN_OBJECT.GetComponent<NavMeshAgent>().enabled = false;
        Vector3 position = GameObject.FindWithTag("Start").transform.position;
        TriggerManager.Instance.MAIN_OBJECT.transform.position = position;
        TriggerManager.Instance.MAIN_OBJECT.GetComponent<NavMeshAgent>().enabled = true;
    }

}
