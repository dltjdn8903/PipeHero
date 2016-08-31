using UnityEngine;
using System.Collections;

public class UI_NPC_EventHandler : MonoBehaviour
{
    public void OnClickQuest()
    {
        
    }

    public void OnClickChange()
    {
        //UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_NPC);
    }

    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_NPC);
    }
	
}
