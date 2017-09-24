using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {

    #region MonoBehaviours
    void Awake()
    {
        // ------------------------------Settings from game Manager------------------------------
        mapWidth = 14;
        mapHeight = 12;
        zoneWidth = mapWidth -1*2;
        zoneHeight = mapHeight -1*2;

        board = new Mino[mapWidth,mapHeight];
        minoVariety = 3;
        horLine = (board.GetLength(1) - 1) / 2f;
        verLine = (board.GetLength(0) -1) / 2f;

        MMinos = new List<Mino>();
        PMinos = new List<Mino>();
        // --------------------------------------------------------------------------------------------------


        // Generating Minos
        for(int x = 0; x < board.GetLength(0); x++)
        {
            for(int y = 0; y < board.GetLength(1); y++)
            {
                GameObject minoObject = Instantiate<GameObject>
                    (minoPrefab, new Vector3((float)x, (float)y, 0), Quaternion.identity, GameObject.Find("Board").transform);
                minoObject.name = "Mino" + "(" + x.ToString() + "," + y.ToString() + ")";
                board[x, y] = minoObject.GetComponent<Mino>();
                board[x, y].Set_Pos(x, y);
                board[x, y].Set_MinoType(MinoTypes.Empty);
            }
        }
         
        // Set intial minos _ Middle Zone
        Set_InitialMino(horLine, verLine, 1,1, true);

        // Set intial Slaminos _ 4Dir
        Set_InitialSMinos();
    }

    void Update()
    {
        #region KeyContorls
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        // Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            xPush = 0;
            yPush = -1;

            MMinos.Clear();
            PMinos.Clear();
            Move_SMino(sMinos[0]);
            sMinos[0].Spawn_SMino(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            xPush = 0;
            yPush = 1;

            MMinos.Clear();
            PMinos.Clear();
            Move_SMino(sMinos[1]);
            sMinos[1].Spawn_SMino(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xPush = 1;
            yPush = 0;

            MMinos.Clear();
            PMinos.Clear();
            Move_SMino(sMinos[2]);
            sMinos[2].Spawn_SMino(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xPush = -1;
            yPush = 0;

            MMinos.Clear();
            PMinos.Clear();
            Move_SMino(sMinos[3]);
            sMinos[3].Spawn_SMino(false);
        }


        // Debugging

        #endregion

    }
    #endregion

    #region Field & Method
    // SingleTone StageManager
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

    // prefab _ Fill board with minos
    public GameObject minoPrefab;
    
    // variables changed by GameOptions
    Mino[,] board;
    int mapWidth, mapHeight;
    int zoneWidth, zoneHeight;
    int minoVariety;
    float horLine, verLine;
    public Slamino[] sMinos;

    // For game Calculation
    int xPush, yPush;
    public List<Mino> MMinos;
    public List<Mino> PMinos;

    // Initial Map Generation Fuction
    void Set_InitialMino(float horLine, float verLine, int xRadius, int yRadius, bool noEmpty)
    {
        for(int x = (int)(verLine + 0.5f) - xRadius; x <= (int)(verLine - 0.5f) + xRadius; x++)
        {
            for (int y = (int)(horLine + 0.5f) - yRadius; y <= (int)(horLine - 0.5f) + yRadius; y++)
            {
                board[x, y].Set_MinoType(Get_RandMinoType(noEmpty));
            }
        }
    }
    void Set_InitialSMinos()
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

    // Movement Function
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
        
        Remove_CMinos_InMMinos();

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

        Remove_CMinos_InMMinos();

    }


    public void Remove_CMinos_InMMinos()
    {
        for(int i =0; i<MMinos.Count; i++)
        {
            if(MMinos[i].CMinos.Count >1)
            {
                for (int j = 0; j < MMinos[i].CMinos.Count; j++)
                    Remove_Mino(MMinos[i].CMinos[j]);
            }
        }
    }

    // Utility Functions
    void Remove_Mino(Mino m)
    {
        m.Set_MinoType(MinoTypes.Empty);

        if (PMinos.Contains(m))
            PMinos.Remove(m);
    }
    void Check_CMinos(Mino m)
    {
        if(m.MinoType != MinoTypes.Empty)
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

    bool Get_IsFloating(Mino m)
    {
        if(m.MinoType != MinoTypes.Empty)
        {
            if (board[m.Xpos, m.Ypos + 1].MinoType != MinoTypes.Empty)
                return false;
            else if (board[m.Xpos, m.Ypos - 1].MinoType != MinoTypes.Empty)
                return false;
            else if (board[m.Xpos -1, m.Ypos].MinoType != MinoTypes.Empty)
                return false;
            else if (board[m.Xpos +1, m.Ypos].MinoType != MinoTypes.Empty)
                return false;
        }
        return true;
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
                if(m.Xpos+xDif > verLine)
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
        else if(yDir == -1)
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
        int widthDif = mapWidth - zoneWidth;
        int heightDif = mapHeight - zoneHeight;


        if (xDir == 1)
        {
            while (board[m.Xpos + 1 + xDif, m.Ypos].MinoType == MinoTypes.Empty)
            {
                xDif++;
                if (m.Xpos + xDif >= mapWidth-widthDif)
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
                if (m.Ypos + yDif >= mapHeight - heightDif)
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

        int xPos = m.Xpos + xDif;
        int yPos = m.Ypos + yDif;
        print("board[" + xPos + "," + yPos + "]"); 

        return board[m.Xpos + xDif, m.Ypos + yDif];
    }

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
    public Mino Get_Board(int xPos, int yPos)
    {
        return board[xPos, yPos];
    }
    
    public MinoTypes Get_RandMinoType(bool noEmpty)
    {
        if(noEmpty)
            return (MinoTypes)UnityEngine.Random.Range(1, minoVariety + 1 - Mathf.Epsilon);
        else
            return (MinoTypes)UnityEngine.Random.Range(0, minoVariety + 1 - Mathf.Epsilon);
    }
        
    #endregion
}
