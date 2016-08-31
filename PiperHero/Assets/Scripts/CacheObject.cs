using UnityEngine;
using System.Collections;

public class CacheObject : MonoBehaviour
{
    protected GameObject m_cacheObject = null;
    protected Transform m_cacheTransform = null;

    protected bool m_bCacheObject = false;
    protected bool m_bCacheTransform = false;

    virtual public GameObject SelfObject
    {
        get
        {
            if( m_cacheObject == null && m_bCacheObject == false )
            {
                m_cacheObject = gameObject;
                m_bCacheObject = true;
            }
            return m_cacheObject;
        }
    }

    virtual public Transform SelfTransform
    {

        
        get
        {
            if ( m_cacheTransform == null && m_bCacheTransform == false )
            {
                m_cacheTransform = transform;
                m_bCacheTransform = true;
            }
            return m_cacheTransform;
        }
    }



}
