using UnityEngine;
using System.Collections;

public class UI_MovePopup_EventHandler : MonoBehaviour
{
    public void OnCilckCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_TownMenu);
    }

    public void OnClickLobby()
    {
        // 순간이동
    }

    /* 혹시 몰라서
    public void OnClickLoading()
    {

    }
    */

}
