using UnityEngine;
using System.Collections;

public class BaseBoard : CacheObject
{
    [SerializeField]
    float m_DestroyTime = 0.0f;

    [SerializeField]
    bool m_AttachBoard = true;

    BaseObject m_Observer = null;
    Camera m_UICamera = null;
    Camera m_WorldCamera = null;
    Transform m_BoardTransform = null;

    Vector3 m_Position = Vector3.zero;

    float m_ElapsedTime = 0.0f;

    public BaseObject OBSERVER_COMPONENT
    {
        set
        {
            m_Observer = value;
            m_BoardTransform = m_Observer.GetChild("Board");
            // 보드 오브젝트를 찾고 UPdateBoard에서 뷰포트랑 캐릭터의 위치의 <위치>를 설정해준다.
        }
    }

    public Camera UI_CAMERA
    {
        get
        {
            if( m_UICamera == null )
            {
                m_UICamera = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
            }

            return m_UICamera;
        }
    }

    public Camera WORLD_CAMERA
    {
        get
        {
            if( m_WorldCamera == null )
            {
                GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                m_WorldCamera = cameraObject.GetComponent<Camera>();
            }

            return m_WorldCamera;
        }

    }

    virtual public eBoardType BOARD_TYPE
    {
        get { return eBoardType.BOARD_NONE; }
    }

    virtual public void SetData(string strKey, params object[] datas) { }

    public bool CheckDestroyTime()
    {
        if (m_DestroyTime == 0.0f)
            return false;

        if (m_DestroyTime < m_ElapsedTime)
            return true;

        return false;
    }


    virtual public void UpdateBoard()
    {
        m_ElapsedTime += Time.smoothDeltaTime;

        if (UI_CAMERA == null || WORLD_CAMERA == null)
            return;

        if (m_BoardTransform == null)
            return;

        if( m_AttachBoard == true )
        {
            m_Position = m_BoardTransform.position;

        }
        else
        {
            if (m_Position == Vector3.zero)
                m_Position = m_BoardTransform.position;
        }

        Vector3 viewport = WORLD_CAMERA.WorldToViewportPoint(m_Position);
        Vector3 boardPosition = UI_CAMERA.ViewportToWorldPoint(viewport);

        SelfTransform.position = boardPosition;
    }


}
