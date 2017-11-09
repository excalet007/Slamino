using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Shadow : Layer {

    #region Override abstract function of Layer

    public override void SetUp(int mapSize_X, int mapSize_Y)
    {
        Id = "Shadow";

        foreach (GameObject gameObject in swipes)
            spriteRenderers_swipes.Add(gameObject.GetComponent<SpriteRenderer>());
        
        foreach (GameObject gameObject in corners)
            spriteRenderers_corners.Add(gameObject.GetComponent<SpriteRenderer>());

        On(0);
    }

    public override void On(int Direction)
    {
        switch(Direction)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.Log("you input wrong direction numbers");
                break;
        }
    }

    public override void Off(int Direction)
    {
        for (int i = 0; i < spriteRenderers_swipes.Count; i++)
            spriteRenderers_swipes[i].color = new Color(1f, 1f, 1f, trans_Off);

        for (int i = 0; i < spriteRenderers_corners.Count; i++)
            spriteRenderers_corners[i].color = new Color(1f, 1f, 1f, trans_Off);
    }

    #endregion

    #region Field
    float trans_On = 0.8f;
    float trans_Off = 0.1f;

    /// <summary>
    /// Up,Down,Left,Right
    /// </summary>
    public List<GameObject> swipes;
    List<SpriteRenderer> spriteRenderers_swipes;
    
    /// <summary>
    /// UpLeft,UpRight,DownLeft,DownRight
    /// </summary>
    public List<GameObject> corners;
    List<SpriteRenderer> spriteRenderers_corners;
    #endregion
}
