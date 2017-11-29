using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Button_Pause : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Button_Pause";
        sm = StageManager.Instance;
        wm = WindowManager.Instance;
    }

    public override void On()
    {
        this.gameObject.SetActive(true);
    }

    public override void Off()
    {
        this.gameObject.SetActive(false);
    }

    #endregion

    #region Field & Method
    StageManager sm;
    WindowManager wm;

    public List<GameObject> hideList;

    public void Click_Button_Pause()
    {
        if(sm.GameState == GameState.Play && sm.OnCycle == false)
        {
            SetAcitve_GameBoard(false);

            Off();
            wm.Get_window("Pause").On();
        }

    }
    public void SetAcitve_GameBoard(bool isOn)
    {
        foreach (GameObject each in hideList)
            each.SetActive(isOn);
    }
    #endregion

}
