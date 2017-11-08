using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_GameStart : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "GameStart";
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
