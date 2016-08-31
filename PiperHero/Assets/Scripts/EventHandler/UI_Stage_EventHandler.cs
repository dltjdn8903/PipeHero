using UnityEngine;
using System.Collections;

public class UI_Stage_EventHandler : MonoBehaviour
{
    public void OnClickMenu()
    {
        UIManager.Instance.ShowUI(eUIType.Pf_Ui_Popup);
    }

}
