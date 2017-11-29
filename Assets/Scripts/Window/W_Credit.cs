using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Credit : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Credit";
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
    WindowManager wm;

    public void Click_BackToPause()
    {
        Off();

        wm.Get_window("Pause").On();
    }
    #endregion
}
