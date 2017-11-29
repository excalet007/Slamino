using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class W_GameOver : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "GameOver"; 

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

    /// <summary>
    /// 0 = total Score, 1 = top Score
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void Input(int index, float value)
    {
        texts[index].text = value.ToString();
    }

    public void Input(int index, string s)
    {
        texts[index].text = s;
    }

    public void Click_Retry()
    {
        SceneManager.LoadScene("Stage");
    }
    #endregion
}
