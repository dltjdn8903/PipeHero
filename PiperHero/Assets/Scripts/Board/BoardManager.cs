using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eBoardType
{
    BOARD_NONE          ,
    BOARD_HP            ,
    BOARD_DAMAGE        ,
    BOARD_EXP        , //+ Exp보드 추가
    BOARD_MONEY, //+ Money보드 추가
    BOARD_LEVELUP, //+ 레벨업보드 추가
}

public class BoardManager : BaseManager< BoardManager >
{
    Dictionary<BaseObject, List<BaseBoard>> m_dicBoard = new Dictionary<BaseObject, List<BaseBoard>>();
    GameObject m_BoardUI = null;

    void Awake()
    {
        m_BoardUI = UIManager.Instance.ShowUI(eUIType.Pf_Ui_BoardManager);
    }

    public BaseBoard AddBoard(BaseObject keyObject, eBoardType boardType)
    {
        List<BaseBoard> listBoard = null;

        if( m_dicBoard.ContainsKey( keyObject ) == false )
        {
            listBoard = new List<BaseBoard>();
            m_dicBoard.Add(keyObject, listBoard);
        }
        else
        {
            listBoard = m_dicBoard[keyObject];
        }

        BaseBoard boardData = _MakeBoard(boardType);
        boardData.OBSERVER_COMPONENT = keyObject;

        listBoard.Add(boardData);
        return boardData;
    }

    public BaseBoard GetBoardData(BaseObject keyObject, eBoardType boardType)
    {
        if (m_dicBoard.ContainsKey(keyObject) == false)
            return null;

        List<BaseBoard> listBoard = m_dicBoard[keyObject];
        for (int i = 0; i < listBoard.Count; ++i)
        {
            if (listBoard[i].BOARD_TYPE == boardType)
                return listBoard[i];
        }

        return null;
    }

    public void RemoveBoard( BaseObject keyObject, BaseBoard boardData )
    {
        if (m_dicBoard.ContainsKey(keyObject) == false)
            return;

        m_dicBoard[keyObject].Remove(boardData);
        Destroy(boardData.SelfObject);
    }

    public void ClearBoard( BaseObject keyObject, eBoardType boardType )
    {
        if (m_dicBoard.ContainsKey(keyObject) == false)
            return;

        List<BaseBoard> listDelete = new List<BaseBoard>();
        List<BaseBoard> listBoard = m_dicBoard[keyObject];
        

        for( int i = 0; i < listBoard.Count; ++i )
        {
            if( listBoard[i].BOARD_TYPE == boardType )
            {
                listDelete.Add(listBoard[i]);
            }
        }

        for( int i = 0; i < listDelete.Count; ++i )
        {
            listBoard.Remove(listDelete[i]);
        }

        if( listBoard.Count == 0 )
        {
            m_dicBoard.Remove(keyObject);
        }
    }

    public void ClearBoard( BaseObject keyObject )
    {
        if (m_dicBoard.ContainsKey(keyObject) == false)
            return;

        List<BaseBoard> listBoard = m_dicBoard[keyObject];

        for (int i = 0; i < listBoard.Count; ++i)
        {
            Destroy(listBoard[i].SelfObject);
        }

        m_dicBoard.Remove(keyObject);
    }

    public void ClearBoard()
    {
        foreach( KeyValuePair<BaseObject, List<BaseBoard>> keyValue in m_dicBoard )
        {
            for( int i = 0; i < keyValue.Value.Count; ++i )
            {
                Destroy(keyValue.Value[i]);
            }
        }

        m_dicBoard.Clear();
    }

    BaseBoard _MakeBoard( eBoardType boardType )
    {
        BaseBoard boardData = null;

        switch( boardType )
        {
            case eBoardType.BOARD_HP:
                {
                    GameObject prefabHPBoard = Resources.Load<GameObject>("HP_Board");
                    GameObject hpBoard = NGUITools.AddChild(m_BoardUI, prefabHPBoard);
                    boardData = hpBoard.GetComponent<HPBoard>();
                }
                break;

            case eBoardType.BOARD_DAMAGE:
                {
                    GameObject prefabDamageBoard = Resources.Load<GameObject>("Damage_Board");
                    GameObject damageBoard = NGUITools.AddChild(m_BoardUI, prefabDamageBoard);
                    boardData = damageBoard.GetComponent<DamageBoard>();
                }
                break;

            case eBoardType.BOARD_EXP:
                {
                    GameObject prefabExpBoard = Resources.Load<GameObject>("Pf_Exp_Board");
                    GameObject expBoard = NGUITools.AddChild(m_BoardUI, prefabExpBoard);
                    boardData = expBoard.GetComponent<ExpBoard>();
                }
                break;

            case eBoardType.BOARD_MONEY:
                {
                    GameObject prefabMoneyBoard = Resources.Load<GameObject>("Pf_Money_Board");
                    GameObject moneyBoard = NGUITools.AddChild(m_BoardUI, prefabMoneyBoard);
                    boardData = moneyBoard.GetComponent<MoneyBoard>();
                }
                break;

            case eBoardType.BOARD_LEVELUP:
                {
                    GameObject prefabLevelUpBoard = Resources.Load<GameObject>("Pf_LevelUP_Board");
                    GameObject levelUpBoard = NGUITools.AddChild(m_BoardUI, prefabLevelUpBoard);
                    boardData = levelUpBoard.GetComponent<LevelUpBoard>();
                }
                break;
        }

        return boardData;
    }

    List<BaseBoard> listDelete = new List<BaseBoard>();
    void Update()
    {
        Dictionary<BaseObject, List<BaseBoard> >.Enumerator enumerator = m_dicBoard.GetEnumerator();
        while( enumerator.MoveNext() )
        {
            List<BaseBoard> listBoard = enumerator.Current.Value;
            for( int i = 0; i < listBoard.Count; ++i )
            {
                listBoard[i].UpdateBoard();
                if( listBoard[i].CheckDestroyTime() == true )
                {
                    listDelete.Add(listBoard[i]);
                }
            }

            for( int i = 0; i < listDelete.Count; ++i )
            {
                listBoard.Remove(listDelete[i]);
                Destroy(listDelete[i].SelfObject);
            }
            listDelete.Clear();
        }
    }


    public void ShowBoard( BaseObject keyObject, bool bEnable = true )
    {
        if (m_dicBoard.ContainsKey(keyObject) == false)
            return;

        List<BaseBoard> listBoard = m_dicBoard[keyObject];
        for( int i = 0; i < listBoard.Count; ++i )
        {
            if( listBoard[i].SelfObject != null)
                listBoard[i].SelfObject.SetActive(bEnable);
        }
    }

}
