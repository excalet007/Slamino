using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Pause : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Pause";
        wm = WindowManager.Instance;
        ic = InputController.Instance;
        sm = StageManager.Instance;
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
    WindowManager wm;
    InputController ic;
    StageManager sm;

    public void Click_Credit()
    {
        Off();

        wm.Get_window("Credit").On();
    }
    public void Click_Restart()
    {
        Off();

        wm.Get_window("Restart").On();
    }
    public void Click_Resume()
    {
        Off();

        if (!Json.Check_Exsits("PlayData") && StageManager.Instance.Cur_Round <= 4)
            wm.Get_window("Tutorial").On();

        W_Button_Pause w_button_pause = wm.Get_window("Button_Pause") as W_Button_Pause;
        w_button_pause.SetAcitve_GameBoard(true);
        wm.Get_window("Button_Pause").On();

        sm.isPaused = false;
        ic.isPaused = false;
    }

    #endregion
}
