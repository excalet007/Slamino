using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionChecker : MonoBehaviour {

    public static PositionChecker Instance;

    public Text text;

    void Awake()
    {
        Instance = this;
        print(Screen.width + "" + Screen.height);
    }

    public void Show_Text(Vector2 vector)
    {
        text.text = vector.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Show_Text(Input.mousePosition);
    }
}
