using UnityEngine;
using System.Collections;

public class LevelUpBoard : BaseBoard
{
    override public eBoardType BOARD_TYPE
    {
        get { return eBoardType.BOARD_LEVELUP; }
    }

    //+ 보드 LevelUp보드 추가
    //public override void SetData(string strKey, params object[] datas)
    //{
    //    switch (strKey)
    //    {
    //        case "LEVELUP_BOARD":
    //            {

    //            }
    //            break;
    //    }

    //}
}
