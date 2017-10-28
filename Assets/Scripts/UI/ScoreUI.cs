using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    void Awake()
    {
        texts = GetComponentsInChildren<TextMesh>();
        for (int i = 0; i < 3; i++)
            texts[i].text = "";
    }

    static TextMesh[] texts;
    
    public static void Input(int index, float value)
    {
        texts[index].text = value.ToString();
    }

    public static void Input(int index, string s)
    {
        texts[index].text = s;
    }

    public static void BottomToTop()
    {
        texts[0].text = texts[2].text;
        texts[1].text = "";
        texts[2].text = "";
    }

    public static void Input_TypeWriter(int index, float value)
    {
        ;
    }
}
