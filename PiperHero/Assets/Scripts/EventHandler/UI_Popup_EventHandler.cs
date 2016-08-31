using UnityEngine;
using System.Collections;

public class UI_Popup_EventHandler : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0;
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    public void OnClickLobby()
    {
        //TriggerManager.Instance.Clear();
        StateManager.Instance.ChangeState(eStateType.STATE_TYPE_LOBBY);
        UIManager.Instance.HideUI(eUIType.Pf_Ui_Popup);
    }

    public void OnClickExit()
    {
        UIManager.Instance.HideUI(eUIType.Pf_Ui_Popup);
    }
}
