using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_LimitLine : Layer {

    #region Override abstract function of Layer

    public override void SetUp(int mapSize_X, int mapSize_Y)
    {
        Id = "LimitLine";

        spriteRenderers = new List<SpriteRenderer>();

        foreach (GameObject gameObject in objects)
            spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());

        Off(0);
    }

    public override void On(int Direction)
    {
        for(int i = 0; i < 4; i++)
        {
            spriteRenderers[i].sprite = sprites[0];

            if(i == Direction)
                spriteRenderers[i].color = new Color(1f, 1f, 1f, trans_On);
            else
                spriteRenderers[i].color = new Color(1f, 1f, 1f, trans_Off);
        }

    }

    /// <summary>
    /// 0 is gray, 1 is white, 2 is yellow
    /// </summary>
    /// <param name="Direction"></param>
    /// <param name="danger"></param>
    public void On_Warning(int Direction, int danger)
    {
        switch(danger)
        {
            case 0:
                spriteRenderers[Direction].sprite = sprites[0];
                spriteRenderers[Direction].color = new Color(1f, 1f, 1f, trans_On);
                break;

            case 1:
                spriteRenderers[Direction].sprite = sprites[1];
                spriteRenderers[Direction].color = new Color(1f, 1f, 1f, 1);
                break;

            case 2:
                spriteRenderers[Direction].sprite = sprites[2];
                spriteRenderers[Direction].color = new Color(1f, 1f, 1f, 1);
                break;

            default:
                Debug.LogError("you input wrong number");
                break;
        }

    }

    public override void Off(int Direction)
    {
        for(int i =0; i < spriteRenderers.Count; i++)
            spriteRenderers[Direction].color = new Color(1f, 1f, 1f, trans_Off);
    }

    #endregion

    #region Field
    float trans_On = 0f;
    float trans_Off = 0f;

    /// <summary>
    /// Up,Down,Left,Right Lines
    /// </summary>
    public List<GameObject> objects;
    List<SpriteRenderer> spriteRenderers;

    /// <summary>
    /// white, yellow
    /// </summary>
    public List<Sprite> sprites;
    #endregion
}
