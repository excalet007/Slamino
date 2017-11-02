using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_Proejctor : Window {

    #region Monobehaviours
    void Update()
    {
        Swap_Random(5);
    }
    #endregion

    #region Override abstract function of Window

    public override void SetUp()
    {
        Id = "Projector";

        image = GetComponent<Image>();
        imageIndex = 0;
        scaler = 0;
        
        image.sprite = sprites[imageIndex];
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
    public Sprite[] sprites;
    private Image image;
    int imageIndex;
    int scaler;
    #endregion

    #region Method
    void Swap_InOrder(int cap)
    {
        image.sprite = sprites[imageIndex];
        scaler++;
        if (scaler > cap)
        {
            scaler = 0;
            imageIndex++;
            if (imageIndex >= sprites.Length)
                imageIndex = 0;
        }
    }
    void Swap_Random(int cap)
    {
        scaler++;
        if (scaler > cap)
        {
            scaler = 0;
            image.sprite = sprites[(int)UnityEngine.Random.Range(0, sprites.Length - Mathf.Epsilon)];
        }
    }
    #endregion
}
