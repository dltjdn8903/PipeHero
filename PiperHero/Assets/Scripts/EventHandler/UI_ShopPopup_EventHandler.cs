using UnityEngine;
using System.Collections;

public class UI_ShopPopup_EventHandler : MonoBehaviour
{

    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_Shop);
    }

}
