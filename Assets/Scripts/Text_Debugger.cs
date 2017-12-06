using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Debugger : MonoBehaviour {

    public Text textBot;
    public Text textTop;

    public static Text_Debugger Instance;
    
    void Awake()
    {
        Instance = this;
    }

    public void Change_Text_Bot(string str)
    {
        textBot.text = str;
    }

    public void Change_Text_Top(string str)
    {
        textTop.text = str;
    }

}
