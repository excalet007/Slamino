using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Proejctor : Window {


    public override void SetUp()
    {
        image = GetComponent<Image>();
        isOn = false;
        imageIndex = 0;
        scaler = 0;

        image.color = new Color(1f, 1f, 1f, 0f);
        image.sprite = sprites[imageIndex];
    }

    public override void On()
    {
        image.color = new Color(1f, 1f, 1f, 55/255f);
        isOn = true;
    }

    public override void Off()
    {
        image.color = new Color(1f, 1f, 1f, 0f);
        isOn = false;
    }

    public Sprite[] sprites;
    bool isOn;
    private Image image;
    int imageIndex;
    int scaler;

    void Awake()
    {
        SetUp();
        On();
    }

    void Update()
    {
        if (isOn)
            Swap_Random(5);
    }

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
}
