using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_TouchSetting : Window {

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "TouchSetting";
        wm = WindowManager.Instance;
        im = InputController.Instance;

        switch((int)im.TouchSetting)
        {
            case (int)TouchSetting.SwipeAndDrop:
                Click_SwipeAndDrop();
                break;

            case (int)TouchSetting.PointAndDrop:
            default:
                Click_PointAndDrop();
                break;
        }

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
    InputController im;

    public Image Image_PointAndDrop;
    public Image Image_SwipeAndDrop;
    public List<Image> Image_Sliders;
    public Slider Slider_Sensitivity;

    public void Click_PointAndDrop()
    {
        Image_PointAndDrop.color = new Color(1f, 1f, 1f);
        Image_SwipeAndDrop.color = new Color(0.6f, 0.6f, 0.6f);

        foreach(Image img in Image_Sliders)
            img.color = new Color(0.6f, 0.6f, 0.6f);

        PlayerPrefs.SetString("TouchSetting", "PointAndDrop");
        im.TouchSetting = TouchSetting.PointAndDrop;
    }

    public void Click_SwipeAndDrop()
    {
        Image_PointAndDrop.color = new Color(0.6f, 0.6f, 0.6f);
        Image_SwipeAndDrop.color = new Color(1f, 1f, 1f);

        foreach (Image img in Image_Sliders)
            img.color = new Color(1f, 1f, 1f);

        PlayerPrefs.SetString("TouchSetting", "SwipeAndDrop");
        im.TouchSetting = TouchSetting.SwipeAndDrop;
    }

    public void Change_SwipeSensitivity()
    {
        Image_PointAndDrop.color = new Color(0.6f, 0.6f, 0.6f);
        Image_SwipeAndDrop.color = new Color(1f, 1f, 1f);

        foreach (Image img in Image_Sliders)
            img.color = new Color(1f, 1f, 1f);

        im.sensitivity = Slider_Sensitivity.value;
        PlayerPrefs.SetFloat("SwipeSensitivity", im.sensitivity);
    }

    public void Click_BackToPause()
    {
        Off();

        wm.Get_window("Pause").On();
    }
    #endregion
}
