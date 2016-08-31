using UnityEngine;
using System.Collections;

public class ButtonTriggers : MonoBehaviour {

    UIEventTrigger m_EventTrigger = null;

    void Start ()
    {
        gameObject.AddComponent<UIEventTrigger>();

        EventDelegate eventBtnPress = new EventDelegate(Touch_EventManager.Instance, "SkillPressed");
        EventDelegate eventBtnRelease = new EventDelegate(Touch_EventManager.Instance, "SkillReleased");

        EventDelegate.Add(gameObject.GetComponent<UIEventTrigger>().onPress, eventBtnPress);
        EventDelegate.Add(gameObject.GetComponent<UIEventTrigger>().onRelease, eventBtnRelease);
    }
}
