using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Shadow : Layer {

    #region Override abstract function of Layer

    public override void SetUp(int mapSize_X, int mapSize_Y)
    {
        Id = "Shadow";

        spriteRenderers_corners = new List<SpriteRenderer>();
        spriteRenderers_swipes = new List<SpriteRenderer>();

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
                spriteRenderers_swipes[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_swipes[3].color = new Color(1f, 1f, 1f, trans_Off);

                spriteRenderers_corners[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_corners[3].color = new Color(1f, 1f, 1f, trans_Off);
                break;

            case 1:
                spriteRenderers_swipes[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_swipes[3].color = new Color(1f, 1f, 1f, trans_Off);

                spriteRenderers_corners[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_corners[3].color = new Color(1f, 1f, 1f, trans_Off);
                break;

            case 2:
                spriteRenderers_swipes[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_swipes[3].color = new Color(1f, 1f, 1f, trans_Off);

                spriteRenderers_corners[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_corners[3].color = new Color(1f, 1f, 1f, trans_Off);
                break;

            case 3:
                spriteRenderers_swipes[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_swipes[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_swipes[3].color = new Color(1f, 1f, 1f, trans_Off);

                spriteRenderers_corners[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[1].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers_corners[2].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers_corners[3].color = new Color(1f, 1f, 1f, trans_Off);
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
    float trans_Off = 0f;

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
