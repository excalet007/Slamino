using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// connected-sameType-minos
/// </summary>
public class ChainMino {

    MinoTypes minotype;
    List<Mino> minos;

    public MinoTypes Minotype
    {
        get { return minotype; }
    }
    public List<Mino> Minos
    {
        get { return minos; }
    }

    public ChainMino(MinoTypes mType, List<Mino> mList)
    {
        minotype = mType;
        minos = mList;

    }
    public ChainMino()
    {
        minotype = MinoTypes.Empty;
        minos = new List<Mino>();
    }

    public bool Get_ContainMino(Mino m)
    {
        if (m.MinoType == minotype && minos.Contains(m))
        {
            return true;
        }
        else
            return false;
    }

    public bool Get_ContainMoveType(MoveTypes mType)
    {
        for(int i =0; i<minos.Count; i++)
        {
            if (minos[i].MoveType == mType)
                return true;
        }
        return false;
    }
}
