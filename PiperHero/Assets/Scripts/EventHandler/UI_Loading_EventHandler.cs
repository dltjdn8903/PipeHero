using UnityEngine;
using System.Collections;

public class UI_Loading_EventHandler : MonoBehaviour
{
    [SerializeField]
    UISlider m_Slider = null;

    [SerializeField]
    UILabel m_Label = null;

    [SerializeField]
    GameObject[] m_arrBackground = null;

    void OnEnable()
    {
        int nRandom = Random.Range(0, m_arrBackground.Length);

        for( int i = 0; i < m_arrBackground.Length; ++i )
        {
            if (i == nRandom)
                m_arrBackground[i].SetActive(true);
            else 
                m_arrBackground[i].SetActive(false);
        }

    }

    public void SetValue( float fValue )
    {
        m_Slider.value = fValue;
        m_Label.text = string.Format("{0:f0} %", fValue * 100.0f);
    }
}
