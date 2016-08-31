using UnityEngine;
using System.Collections;

public class UI_Scene_EventHandler : MonoBehaviour
{
    public void OnClickScene_1()
    {
        StateManager.Instance.ChangeState(eStateType.STATE_TYPE_LOGO);
    }

    public void OnClickScene_2()
    {
        StateManager.Instance.ChangeState(eStateType.STATE_TYPE_LOBBY);
    }

    public void OnClickScene_3()
    {
        StateManager.Instance.ChangeState(eStateType.STATE_TYPE_STAGE);
    }

}
