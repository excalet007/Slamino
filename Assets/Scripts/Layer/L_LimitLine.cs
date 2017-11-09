using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_LimitLine : Layer {

    #region Override abstract function of Layer

    public override void SetUp(int mapSize_X, int mapSize_Y)
    {
        Id = "LimitLine";

        foreach (GameObject gameObject in objects)
            spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());

        Off(0);
    }

    public override void On(int Direction)
    {
        for(int i =0; i <spriteRenderers.Count; i++)
        {
            if (i == Direction)
                spriteRenderers[Direction].color = new Color(1f, 1f, 1f, trans_On);
            else
                spriteRenderers[Direction].color = new Color(1f, 1f, 1f, trans_Off);
        }
    }

    public override void Off(int Direction)
    {
        for(int i =0; i < spriteRenderers.Count; i++)
            spriteRenderers[Direction].color = new Color(1f, 1f, 1f, trans_Off);
    }

    #endregion

    #region Field
    float trans_On = 0.5f;
    float trans_Off = 0.02f;

    /// <summary>
    /// Up,Down,Left,Right Lines
    /// </summary>
    public List<GameObject> objects;
    List<SpriteRenderer> spriteRenderers;
    #endregion
}
