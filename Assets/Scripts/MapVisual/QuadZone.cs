using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadZone : MonoBehaviour {

	void Awake()
    {
        sR = GetComponentsInChildren<SpriteRenderer>();
        tF = new Transform[sR.Length];
        for(int i =0; i<sR.Length; i ++)
        {
            tF[i] = sR[i].transform;
        }
    }

    static Transform[] tF;
    static SpriteRenderer[] sR;

    public static void Chage_View(bool q1, bool q2, bool q3, bool q4, float transparency)
    {
        if (q1)
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, 0);
        else
            sR[0].color = new Color(sR[0].color.r, sR[0].color.g, sR[0].color.b, transparency);
        
        if (q2)
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, 0);
        else                                           
            sR[1].color = new Color(sR[1].color.r, sR[1].color.g, sR[1].color.b, transparency);
        
        if (q3)
            sR[2].color = new Color(sR[2].color.r, sR[2].color.g, sR[2].color.b, 0);
        else   
            sR[2].color = new Color(sR[2].color.r, sR[2].color.g, sR[2].color.b, transparency);
        
        if (q4)
            sR[3].color = new Color(sR[3].color.r, sR[3].color.g, sR[3].color.b, 0);
        else   
            sR[3].color = new Color(sR[3].color.r, sR[3].color.g, sR[3].color.b, transparency);
    }
}
