using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour {

    #region MonoBehaviours
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coordinate = GetComponentInChildren<TextMesh>();
    }
    #endregion

    #region Field 
    // Mechanism Data
    int x, y;
    private MinoTypes minoType;
    private MoveTypes moveType;
  
    // Graphical Data
    private SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;

    // Debugging Tool
    private TextMesh coordinate;    


    #endregion

    #region Method & Property
    public void Set_MinoType(MinoTypes type)
    {
        minoType = type;
        spriteRenderer.sprite = sprites[(int)type];
    }
    public void Set_MoveType(MoveTypes type)
    {
        moveType = type;
    }
    public void Set_Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
        coordinate.text = this.x + "," + this.y;
    }

    public int Xpos { get { return x; } }
    public int Ypos { get { return y; } }
    public MinoTypes MinoType { get { return minoType; } }
    public MoveTypes MoveType { get { return moveType; } }
    #endregion
}
