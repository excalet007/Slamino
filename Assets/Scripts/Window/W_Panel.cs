using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Panel : Window {
    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Panel";
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

}
