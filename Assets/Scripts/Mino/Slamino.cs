using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slamino : MonoBehaviour {

    #region Monobehaviours
    // keycontrol
    #endregion

    #region Field
    public List<Mino> minos;
    float xPivot, yPivot;
    int xPush, yPush; 
   
    // Singleton
    private StageManager sm;
    #endregion

    #region Property
    public int XPush
    {
        get { return xPush; }
    }
    public int YPush
    {
        get { return yPush; }
    }
    #endregion

    #region Method
    void Awake()
    {
        sm = StageManager.Instance;
        minos = new List<Mino>();
    }

    public void Initialize_SMino(float xPivot, float yPivot, int xPush, int yPush)
    {

        this.xPivot = xPivot;
        this.yPivot = yPivot;

        this.xPush = xPush;
        this.yPush = yPush;

        if (xPush == 0 && yPush == -1)
        {
            minos.Add(sm.Get_Board((int)(this.xPivot - 0.5f), (int)(this.yPivot)));
            minos.Add(sm.Get_Board((int)(this.xPivot + 0.5f), (int)(this.yPivot)));
        }
        else if (xPush == 0 && yPush == 1)
        {
            minos.Add(sm.Get_Board((int)(this.xPivot - 0.5f), (int)(this.yPivot)));
            minos.Add(sm.Get_Board((int)(this.xPivot + 0.5f), (int)(this.yPivot)));
        }
        else if (xPush == -1 && yPush == 0)
        {
            minos.Add(sm.Get_Board((int)this.xPivot, (int)(this.yPivot + 0.5f)));
            minos.Add(sm.Get_Board((int)this.xPivot, (int)(this.yPivot - 0.5f)));
        }
        else if (xPush == 1 && yPush == 0)
        {
            minos.Add(sm.Get_Board((int)this.xPivot, (int)(this.yPivot + 0.5f)));
            minos.Add(sm.Get_Board((int)this.xPivot, (int)(this.yPivot - 0.5f)));
        }
        else
            Debug.LogError("you input wrong Dir!");

    }
    public void Spawn_SMino(bool acceptSame)
    {
        if(acceptSame)
        for(int i =0; i < minos.Count; i++)
        {
                minos[i].Set_MinoType(sm.Get_RandMinoType(true));
        }
        else
        {
            List<MinoTypes> types = new List<MinoTypes>();
            for (int i = 0; i < minos.Count; i++)
            {
                MinoTypes input = sm.Get_RandMinoType(true);
                while(types.Contains(input))
                {
                    input = sm.Get_RandMinoType(true);
                }

                minos[i].Set_MinoType(input);
                types.Add(input);
            }
        }
    }
    public void Recover_SMino()
    {
        if(xPush == 0 && yPush == -1)
        {

        }
        else if(XPush == 0 && yPush == 1)
        {

        }
        else if(XPush == 1 && yPush == 0)
        {

        }
        else if(XPush == -1 && yPush ==0)
        {

        }

    }
    #endregion
}
