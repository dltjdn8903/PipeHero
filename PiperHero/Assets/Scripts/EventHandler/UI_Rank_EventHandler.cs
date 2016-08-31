using UnityEngine;
using System.Collections;

public class UI_Rank_EventHandler : MonoBehaviour
{

    public void OnCilckCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_Rank);
    }
}
