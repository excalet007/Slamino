using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisZone : MonoBehaviour {

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

    public static void Chage_View(bool horLine, bool verLine, float transparency)
    {
        if (horLine)
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, transparency);
        else
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, 0);

        if (verLine)
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, transparency);
        else
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, 0);
    }

}
