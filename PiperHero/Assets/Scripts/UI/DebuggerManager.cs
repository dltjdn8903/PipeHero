using UnityEngine;
using System.Collections;

public class DebuggerManager : BaseManager<DebuggerManager>
{
    [SerializeField]
    UILabel m_MyDebugLabel = null;

    void SetDebugLabel(string _debug)
    {
        m_MyDebugLabel.text = _debug;
    }
}
