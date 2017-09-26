using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This controls algorithm of main gameplay
/// (Settings are  controled by GameManager)
/// </summary>
public class StageManager : MonoBehaviour {

    #region MonoBehaviours
    void Awake()
    {
        // ------------------------------Settings from game Manager------------------------------
        // This values will be loaded from gameManager (not yet)
        // map Size factor
        mapX = 14;
        mapY = 12;
        zoneX = mapX -1*2;
        zoneY = mapY -1*2;
        gapX = (mapX - zoneX) / 2;
        gapY = (mapY - zoneX) / 2;

        // mino variaty (green, red, yellow, blue)
        minoVariety = 3;

        // -----------------------------------Initialize Variables----------------------------------------

        //// should be here?? Check
        MMinos = new List<Mino>();
        PMinos = new List<Mino>();

        // ------------------------------------------MapGenarating-----------------------------------

        Initialize_Board(mapX, mapY);
        Initialize_Axis(true, 0, 0);
        Initialize_CenterMinos(horLine, verLine, 1,1, true);
        Initialize_Slaminos();
    }

    void Update()
    {
        #if UNITY_EDITOR
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        // Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Run_AlgoCycle(0);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Run_AlgoCycle(1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Run_AlgoCycle(2);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Run_AlgoCycle(3);
        #endif

    }
    #endregion

    #region Variables & Property
    // SingleTon
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if(instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "StageManger";
                    instance = container.AddComponent<StageManager>();
                }
            }
            return instance;
        }
    }

    // prefabs for mapGenerating
    public GameObject minoPrefab;
    
    // variables changed by GameOptions
    Mino[,] board;
    int mapX, mapY;
    int zoneX, zoneY;
    int minoVariety;

    // variables for calculation 
    float horLine, verLine;
    int xPush, yPush;
    int gapX, gapY;
    public Slamino[] sMinos;
    public List<Mino> MMinos;
    public List<Mino> PMinos;
    #endregion

    #region Main Algorithm Cycle
    void Run_AlgoCycle(int SMinoIndex)
    {
        //Reset Global Variables
        MMinos.Clear();
        PMinos.Clear();
        
        #region Push Direction Select
        xPush = 0;
        yPush = 0;

        switch (SMinoIndex)
        {
            case 0:
                yPush = -1;
                break;
            case 1:
                yPush = 1;
                break;
            case 2:
                xPush = 1;
                break;
            case 3:
                xPush = -1;
                break;
            default:
                Debug.LogError("You input wrong Slamino Index");
                break;
        }
        #endregion

        //Push
        Move_SMino(sMinos[SMinoIndex]);
        //Check IsConnected

        //Pop Connect

        //Check IsFloating

        //Push Floating

        //Check IsEmpty


        //Push whole Minos

        //Respawn Slamino
        sMinos[SMinoIndex].Spawn_SMino(false);
    }

    #endregion
    
    #region Basic Function(Use no custom function)
        
    public Mino Get_Board(int xPos, int yPos)
    {
        return board[xPos, yPos];
    }
    public MinoTypes Get_RandMinoType(bool noEmpty)
    {
        if (noEmpty)
            return (MinoTypes)UnityEngine.Random.Range(1, minoVariety + 1 - Mathf.Epsilon);
        else
            return (MinoTypes)UnityEngine.Random.Range(0, minoVariety + 1 - Mathf.Epsilon);
    }

    Mino Get_ContactPos_ToAxis(Mino m, int xDir, int yDir)
    {
        if (xDir != 0 && yDir != 0)
        {
            Debug.LogError(" you input diagonal check! it's unacceptable");
            return m;
        }

        int xDif = 0;
        int yDif = 0;

        if (xDir == 1)
        {
            while (board[m.Xpos + 1 + xDif, m.Ypos].MinoType == MinoTypes.Empty)
            {
                xDif++;
                if (m.Xpos + xDif > verLine)
                {
                    xDif--;
                    break;
                }
            }
        }
        else if (xDir == -1)
        {
            while (board[m.Xpos - 1 + xDif, m.Ypos].MinoType == MinoTypes.Empty)
            {
                xDif--;
                if (m.Xpos + xDif < verLine)
                {
                    xDif++;
                    break;
                }
            }
        }
        else if (yDir == 1)
        {
            while (board[m.Xpos, m.Ypos + 1 + yDif].MinoType == MinoTypes.Empty)
            {
                yDif++;
                if (m.Ypos + yDif > horLine)
                {
                    yDif--;
                    break;
                }
            }
        }
        else if (yDir == -1)
        {
            while (board[m.Xpos, m.Ypos - 1 + yDif].MinoType == MinoTypes.Empty)
            {
                yDif--;
                if (m.Ypos + yDif < horLine)
                {
                    yDif++;
                    break;
                }
            }
        }

        return board[m.Xpos + xDif, m.Ypos + yDif];
    }
    Mino Get_ContactPos_ToZone(Mino m, int xDir, int yDir)
    {
        if (xDir != 0 && yDir != 0)
        {
            Debug.LogError(" you input diagonal check! it's unacceptable");
            return m;
        }

        int xDif = 0;
        int yDif = 0;
        int widthDif = mapX - zoneX;
        int heightDif = mapY - zoneY;


        if (xDir == 1)
        {
            while (board[m.Xpos + 1 + xDif, m.Ypos].MinoType == MinoTypes.Empty)
            {
                xDif++;
                if (m.Xpos + xDif >= mapX - widthDif)
                {
                    xDif = (int)(verLine - m.Xpos - 0.5f);
                    break;
                }
            }
        }
        else if (xDir == -1)
        {
            while (board[m.Xpos - 1 + xDif, m.Ypos].MinoType == MinoTypes.Empty)
            {
                xDif--;
                if (m.Xpos + xDif <= widthDif)
                {
                    xDif = (int)(verLine - m.Xpos + 0.5f);
                    break;
                }
            }
        }
        else if (yDir == 1)
        {
            while (board[m.Xpos, m.Ypos + 1 + yDif].MinoType == MinoTypes.Empty)
            {
                yDif++;
                if (m.Ypos + yDif >= mapY - heightDif)
                {
                    yDif = (int)(horLine - m.Ypos - 0.5f);
                    break;
                }
            }
        }
        else if (yDir == -1)
        {
            while (board[m.Xpos, m.Ypos - 1 + yDif].MinoType == MinoTypes.Empty)
            {
                yDif--;
                if (m.Ypos + yDif <= heightDif)
                {
                    yDif = (int)(horLine - m.Ypos + 0.5f);
                    break;
                }
            }
        }

        // for debugging
        int xPos = m.Xpos + xDif;
        int yPos = m.Ypos + yDif;
        print("board[" + xPos + "," + yPos + "]");

        return board[m.Xpos + xDif, m.Ypos + yDif];
    }

    bool Get_IsFloating(Mino m)
    {
        if (m.MinoType != MinoTypes.Empty)
        {
            if (board[m.Xpos, m.Ypos + 1].MinoType != MinoTypes.Empty && ((m.Ypos + 1 ) <= (mapY - 1 - gapY)))
                return false;
            else if (board[m.Xpos, m.Ypos - 1].MinoType != MinoTypes.Empty && ((m.Ypos + 1) >= gapY))
                return false;
            else if (board[m.Xpos - 1, m.Ypos].MinoType != MinoTypes.Empty && ((m.Xpos - 1) >= gapX))
                return false;
            else if (board[m.Xpos + 1, m.Ypos].MinoType != MinoTypes.Empty && ((m.Xpos + 1) <= (mapX - 1 - gapX)))
                return false;
        }
        return true;
    }

    #endregion

    #region Mixed Function(Use basic function)
    public void Move_Mino(Mino m, int xPos, int yPos)
    {
        board[xPos, yPos].Set_MinoType(m.MinoType);

        MMinos.Add(board[xPos, yPos]);
        Check_CMinos(board[xPos, yPos]);

        m.Set_MinoType(MinoTypes.Empty);
    }
    public void Move_SMino(Slamino s)
    {
        int minMove = 0; // means null

        for (int i = 0; i < s.minos.Count; i++)
        {
            Mino contact = Get_ContactPos_ToAxis(s.minos[i], s.XPush, s.YPush);

            //Xdirection Move
            if (s.XPush == 1 || s.XPush == -1)
            {
                int xDif = s.minos[i].Xpos - contact.Xpos;

                if (minMove == 0)
                {
                    minMove = (int)Mathf.Abs((float)xDif);
                    continue;
                }

                if (Mathf.Abs((float)xDif) < Mathf.Abs((float)minMove))
                {
                    minMove = (int)Mathf.Abs((float)xDif);
                }
            }


            // Ydirection Move
            if (s.YPush == 1 || s.YPush == -1)
            {
                int yDif = s.minos[i].Ypos - contact.Ypos;

                if (minMove == 0)
                {
                    minMove = (int)Mathf.Abs((float)yDif);
                    continue;
                }

                if (Mathf.Abs((float)yDif) < Mathf.Abs((float)minMove))
                {
                    minMove = (int)Mathf.Abs((float)yDif);
                }
            }
        }

        for (int i = 0; i < s.minos.Count; i++)
        {
            int xPos = s.minos[i].Xpos + s.XPush * minMove;
            int yPos = s.minos[i].Ypos + s.YPush * minMove;

            Move_Mino(s.minos[i], xPos, yPos);
            PMinos.Add(board[xPos, yPos]);
        }

        Pop_CMinos_InMMinos();

        // Change this as Function
        if (PMinos.Count != 0)
        {
            for (int i = 0; i < PMinos.Count; i++)
            {
                if (Get_IsFloating(PMinos[i]))
                {
                    Mino contact2 = Get_ContactPos_ToZone(PMinos[i], xPush, yPush);
                    int xPos = contact2.Xpos;
                    int yPos = contact2.Ypos;

                    Move_Mino(PMinos[i], xPos, yPos);

                    // 혼자 동동인 경우
                    //PMinos.Add(board[xPos, yPos]);
                    // 혼자 동동이 아닌경우
                }
            }
        }

        Pop_CMinos_InMMinos();

    }

    public void Pop_CMinos_InMMinos()
    {
        for (int i = 0; i < MMinos.Count; i++)
        {
            if (MMinos[i].CMinos.Count > 1)
            {
                for (int j = 0; j < MMinos[i].CMinos.Count; j++)
                    Pop_Mino(MMinos[i].CMinos[j]);
            }
        }
    }

    void Pop_Mino(Mino m)
    {
        m.Set_MinoType(MinoTypes.Empty);

        if (PMinos.Contains(m))
            PMinos.Remove(m);
    }
    void Check_CMinos(Mino m)
    {
        if (m.MinoType != MinoTypes.Empty)
        {
            m.CMinos.Clear();
            m.CMinos.Add(m);

            for (int i = 0; i < m.CMinos.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x + y == -1 || x + y == 1)
                        {
                            int xIndex = m.CMinos[i].Xpos + x;
                            int yIndex = m.CMinos[i].Ypos + y;

                            if (!m.CMinos.Contains(board[xIndex, yIndex])
                                && board[xIndex, yIndex].MinoType == m.MinoType)
                            {
                                m.CMinos.Add(board[xIndex, yIndex]);
                                board[xIndex, yIndex].CMinos = m.CMinos;
                            }
                        }
                    }
                }
            }

        }
    }
    #endregion

    #region Initial Map Generation Fuction
    void Initialize_Board(int width, int height)
    {
        board = new Mino[width, height];

        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                GameObject minoObject = Instantiate<GameObject>
                    (minoPrefab, new Vector3((float)x, (float)y, 0), Quaternion.identity, GameObject.Find("Board").transform);
                minoObject.name = "Mino" + "(" + x.ToString() + "," + y.ToString() + ")";
                board[x, y] = minoObject.GetComponent<Mino>();
                board[x, y].Set_Pos(x, y);
                board[x, y].Set_MinoType(MinoTypes.Empty);
            }
        }
    }
    void Initialize_Axis(bool IsCenter, float x, float y)
    {
        if (IsCenter)
        {
            horLine = (board.GetLength(1) - 1) / 2f;
            verLine = (board.GetLength(0) - 1) / 2f;
        }
        else
        {
            Debug.Log("you Input verLine as " + x + ", and Horline as " + y);
            verLine = x;
            horLine = y;
        }
    }
    void Initialize_CenterMinos(float horLine, float verLine, int xRadius, int yRadius, bool noEmpty)
    {
        for(int x = (int)(verLine + 0.5f) - xRadius; x <= (int)(verLine - 0.5f) + xRadius; x++)
        {
            for (int y = (int)(horLine + 0.5f) - yRadius; y <= (int)(horLine - 0.5f) + yRadius; y++)
            {
                board[x, y].Set_MinoType(Get_RandMinoType(noEmpty));
            }
        }
    }
    void Initialize_Slaminos()
    {
        sMinos[0].Initialize_SMino((board.GetLength(0) - 1) / 2f, board.GetLength(1) - 1, 0, -1);
        sMinos[1].Initialize_SMino((board.GetLength(0) - 1) / 2f, 0, 0, 1);
        sMinos[2].Initialize_SMino(0f, (board.GetLength(1) - 1) / 2f, 1, 0);
        sMinos[3].Initialize_SMino(board.GetLength(0) - 1, (board.GetLength(1) - 1) / 2f, -1, 0);

        for(int i =0; i <sMinos.Length; i ++)
        {
            sMinos[i].Spawn_SMino(false);
        }
    }
    #endregion

    #region Field & Method Not in Use
    int Get_DirToHorizontal(Mino m)
    {
        if (m.Ypos - horLine > 0.5f)
            return -1;
        else if (m.Ypos - horLine < -0.5f)
            return +1;
        else
            return 0;
    }
    int Get_DirToVertical(Mino m)
    {
        if (m.Xpos - verLine > 0.5f)
            return -1;
        else if (m.Xpos - verLine < -0.5f)
            return +1;
        else
            return 0;
    }


    public Mino[,] Get_Board()
    {
        return board;
    }
    #endregion
}
