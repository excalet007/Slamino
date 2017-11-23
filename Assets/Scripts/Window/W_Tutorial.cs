using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_Tutorial : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Tutorial";
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
    public GameObject[] tutorialObjects;
    #endregion

    #region Method
    public void On(int direction)
    {
        switch(direction)
        {
            case 0:
                tutorialObjects[0].SetActive(true);
                tutorialObjects[1].SetActive(false);
                tutorialObjects[2].SetActive(false);
                tutorialObjects[3].SetActive(false);
                break;

            case 1:
                tutorialObjects[0].SetActive(false);
                tutorialObjects[1].SetActive(true);
                tutorialObjects[2].SetActive(false);
                tutorialObjects[3].SetActive(false);
                break;

            case 2:
                tutorialObjects[0].SetActive(false);
                tutorialObjects[1].SetActive(false);
                tutorialObjects[2].SetActive(true);
                tutorialObjects[3].SetActive(false);
                break;

            case 3:
                tutorialObjects[0].SetActive(false);
                tutorialObjects[1].SetActive(false);
                tutorialObjects[2].SetActive(false);
                tutorialObjects[3].SetActive(true);
                break;

            default:
                Debug.LogError("you input wrong number");
                break;

        }
    }
    #endregion
}
