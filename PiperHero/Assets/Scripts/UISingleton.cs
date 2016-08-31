using UnityEngine;
using System.Collections;

public class UISingleton<ManagerType> : CacheObject where ManagerType : CacheObject
{ 
    static ManagerType s_Manager = null;

    static bool m_bShutDown = false;

    void OnApplicationQuit()
    {
        m_bShutDown = true;
    }

    public static ManagerType Instance
    {
        get
        {
            if (s_Manager == null)
            {
                if (m_bShutDown == false)
                {
                    ManagerType FindScript = FindObjectOfType<ManagerType>();                    
                    s_Manager = FindScript;
                }
            }

            return s_Manager;
        }
    }
}
