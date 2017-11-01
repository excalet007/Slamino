using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_Score : Window {


    #region Override abstract function of Window

    public override void SetUp()
    {
        texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < 3; i++)
            texts[i].text = "";

        this.gameObject.SetActive(true);
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
    /// <summary>
    /// Prev, Turn, Total, Round, Combo, Multiplier
    /// </summary>
    public Text[] texts;

    #endregion

    #region Method
    public void Input(int index, float value)
    {
        texts[index].text = value.ToString();
    }
    
    public void Input(int index, string s)
    {
        texts[index].text = s;
    }
    
    public void Override_BottomToTop()
    {
        texts[0].text = texts[2].text;
        texts[1].text = "";
        texts[2].text = "";
    }

    #endregion
}
