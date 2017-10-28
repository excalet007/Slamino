﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeZone : MonoBehaviour {

    void Awake()
    {
        sR = GetComponentsInChildren<SpriteRenderer>();
        tF = new Transform[sR.Length];
        for (int i = 0; i < sR.Length; i++)
        {
            tF[i] = sR[i].transform;
        }
    }

    static Transform[] tF;
    static SpriteRenderer[] sR;

    public static void Chage_View(bool up, bool down, bool left, bool right, float transparency)
    {
        if (up)
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, 0);
        else
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, transparency);

        if (down)
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, 0);
        else
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, transparency);

        if (left)
            sR[2].color = new Color(sR[2].color.r, sR[2].color.g, sR[2].color.b, 0);
        else
            sR[2].color = new Color(sR[2].color.r, sR[2].color.g, sR[2].color.b, transparency);

        if (right)
            sR[3].color = new Color(sR[3].color.r, sR[3].color.g, sR[3].color.b, 0);
        else
            sR[3].color = new Color(sR[3].color.r, sR[3].color.g, sR[3].color.b, transparency);
    }
}