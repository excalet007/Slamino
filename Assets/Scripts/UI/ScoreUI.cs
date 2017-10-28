using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    void Awake()
    {
        texts = GetComponentsInChildren<TextMesh>();
        for (int i = 0; i < 3; i++)
            texts[i].text = "";

        ScoreUI.Input(0, 2017);
        ScoreUI.Input(1, 1028);

    }

    static TextMesh[] texts;
    
    public static void Input(int index, int value)
    {
        texts[index].text = value.ToString();
    }

    public static void BottomToTop()
    {
        texts[0].text = texts[2].text;
        texts[1].text = "";
        texts[2].text = "";
    }


}
