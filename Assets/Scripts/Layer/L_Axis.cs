using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Axis : Layer {

    #region Override abstract function of Layer

    public override void SetUp(int mapSize_X, int mapSize_Y)
    {
        Id = "Axis";

        spriteRenderers = new List<SpriteRenderer>();

        foreach (GameObject gameObject in objects)
            spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());

        On(0);
    }

    public override void On(int Direction)
    {
        switch(Direction)
        {
            case 0:
            case 1:
                spriteRenderers[0].color = new Color(1f, 1f, 1f, trans_On);
                spriteRenderers[1].color = new Color(1f, 1f, 1f, trans_Off);
                break;
                
            case 2:
            case 3:
                spriteRenderers[0].color = new Color(1f, 1f, 1f, trans_Off);
                spriteRenderers[1].color = new Color(1f, 1f, 1f, trans_On);
                break;

            default:
                Debug.LogError("you input wrong direction");
                break;
        }
    }

    public override void Off(int Direction)
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
            spriteRenderers[i].color = new Color(1f, 1f, 1f, trans_Off);
    }

    #endregion

    #region Field
    float trans_On = 0.6f;
    float trans_Off = 0.1f;

    /// <summary>
    /// Horizontal, Vertical
    /// </summary>
    public List<GameObject> objects;
    List<SpriteRenderer> spriteRenderers;
    #endregion
}
