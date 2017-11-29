using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W_Restart : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Restart";
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
    
    public void Click_Restart_Yes()
    {
        SceneManager.LoadScene("Stage");
    }
    public void Click_Restart_No()
    {
        Off();
        wm.Get_window("Pause").On();
    }
    #endregion
}
