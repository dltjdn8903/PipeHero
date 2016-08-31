using UnityEngine;
using System.Collections;

public class UI_TownMenu_EventHandler : MonoBehaviour {

    //팝업창 떴을시 게임멈춤
    void OnEnable()
    {
        Time.timeScale = 0; // 게임이 멈춤
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f; //1, 2는 2배속, 4는 4배속
    }

    public void OnCilckCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_TownMenu);
    }

    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Exit!");
    }
}
