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
    /// <summary>
    /// Total Score, Credit
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

    public void Touch_Retry()
    {
        SceneManager.LoadScene("Stage");
    }
    #endregion
}
