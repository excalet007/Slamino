using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_Tutorial : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Tutorial";

        texts = GetComponentsInChildren<Text>();
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

    #region Field
    public Text[] texts;
    #endregion

    #region Method

    #endregion
}
