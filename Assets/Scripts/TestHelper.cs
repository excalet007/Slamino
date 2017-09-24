using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHelper : MonoBehaviour {
    
    public int xPos, yPos;
    public MinoTypes minotypes;
    private bool xInput;
    private bool yInput;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            StageManager.Instance.Get_Board(xPos, yPos).Set_MinoType(minotypes);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            xInput = true;
            yInput = false;
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            xInput = false;
            yInput = true;
        }
    }

}
