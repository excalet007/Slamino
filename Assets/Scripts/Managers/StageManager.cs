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
        // ------------------------------Set up Components & Link Singleton--------------------
        mm = MusicManager.Instance;
        mm.SetUp();

        um = UIManager.Instance;

        w_score = FindObjectOfType<W_Score>();
        w_score.SetUp();

        // ------------------------------Settings from game Manager------------------------------
        // This values will be loaded from gameManager (not yet)
        // map Size factor
        mapX = 20;
        mapY = 14;
        zoneX = mapX -1*2;
        zoneY = mapY -1*2;
        gapX = (mapX - zoneX) / 2;
        gapY = (mapY - zoneY) / 2;

        // mino variaty (green, red, yellow, blue)
        minoVariety = 4;
        mm.Change_Volume(mm.Bgm, 0.3f);
        mm.Change_Volume(mm.Sfx_Drop, 0.4f);
        mm.Change_Volume(mm.Sfx_Pop, 0.5f);
        mm.Change_Volume(mm.Sfx_Score_Tap, 0.5f);
        mm.Change_Volume(mm.Sfx_Score_Enter, 0.6f);
        mm.Change_PopStartPoint(1);
        
        // -----------------------------------Initialize Variables----------------------------------------

        cMinos = new List<ChainMino>();
        onCycle = false;
        isHookHor = false;
        isHookVer = false;

        poppedMino_CurBoard = 0;
        poppedChain_CurBoard = 0;

        poppedMino_Total = 0;
        poppedChain_Total = 0;

        pop_Chain_Count = 0;

        curTurn = 1;
        curRound = 1;
        curSMinoIndex = 0;

        totalScore = 0;
        pop_Chain_Count = 0;
        pop_Turn_Count = 0;
        isPop_Turn = false;
        
        swipe_UpLimit = mapY - 2 ;
        swipe_DownLimit = 1 ;

        swipe_LeftLimit = 1 ;
        swipe_RightLimit = mapX - 2 ;

        timeAfterDrop = 0.1f;
        timeAfterPop = 0.02f * 10;

        transparency_blackLayer = 0.85f;

        QuadZone.Chage_View(true, true, false, false, transparency_blackLayer);
        SwipeZone.Chage_View(true, false, false, false, transparency_blackLayer);
        AxisZone.Chage_View(true, false, 0.8f);

        w_score.Input(3, curRound);
        w_score.Input(4, pop_Turn_Count);
        w_score.Input(5, 1 + 0.1f * pop_Turn_Count);

        // ------------------------------------------MapGenarating-----------------------------------

        Initialize_Board(mapX, mapY);
        Initialize_Axis(true, 0, 0);
        Initialize_CenterMinos(horLine, verLine, 3,2, true);
        Initialize_Slaminos(true);

        // ------------------------------------------ Starts Sound --------------------------------------
        mm.Play_BGM();

        // --------------------------------------------Visual Additional Set ---------------------------

        Set_Preview();
    }
    
    void Update()
    {
//#if UNITY_EDITOR 
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Stage");

        // SMino Movement
        if (Input.GetKeyDown(KeyCode.UpArrow) && onCycle == false)
        {
            switch (curSMinoIndex)
            {
                case 0:
                case 1:
                    break;

                case 2:
                case 3:
                    if (sMinos[curSMinoIndex].minos[0].Ypos < swipe_UpLimit)
                    {
                        int x0 = sMinos[curSMinoIndex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoIndex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoIndex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoIndex].minos[1].Ypos;

                        Mino m0 = board[x0, y0 + 1];
                        Mino m1 = board[x1, y1 + 1];
                        Mino m2 = board[x1, y1];
                       
                        m0.Set_MinoType(sMinos[curSMinoIndex].minos[0].MinoType);
                        m1.Set_MinoType(sMinos[curSMinoIndex].minos[1].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoIndex].minos[0] = m0;
                        sMinos[curSMinoIndex].minos[1] = m1;
                    }
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }

            Set_Preview();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && onCycle == false)
        {
            switch (curSMinoIndex)
            {
                case 0:
                case 1:
                    break;

                case 2:
                case 3:
                    if (sMinos[curSMinoIndex].minos[1].Ypos > swipe_DownLimit)
                    {
                        int x0 = sMinos[curSMinoIndex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoIndex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoIndex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoIndex].minos[1].Ypos;

                        Mino m0 = board[x0, y0 - 1];
                        Mino m1 = board[x1, y1 - 1];
                        Mino m2 = board[x0, y0];

                        m1.Set_MinoType(sMinos[curSMinoIndex].minos[1].MinoType);
                        m0.Set_MinoType(sMinos[curSMinoIndex].minos[0].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoIndex].minos[0] = m0;
                        sMinos[curSMinoIndex].minos[1] = m1;
                    }
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }

            Set_Preview();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && onCycle == false)
        {
            switch (curSMinoIndex)
            {
                case 0:
                case 1:
                    if (sMinos[curSMinoIndex].minos[0].Xpos > swipe_LeftLimit )
                    {
                        int x0 = sMinos[curSMinoIndex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoIndex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoIndex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoIndex].minos[1].Ypos;

                        Mino m0 = board[x0-1, y0];
                        Mino m1 = board[x1-1, y1];
                        Mino m2 = board[x1, y1];

                        m0.Set_MinoType(sMinos[curSMinoIndex].minos[0].MinoType);
                        m1.Set_MinoType(sMinos[curSMinoIndex].minos[1].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoIndex].minos[0] = m0;
                        sMinos[curSMinoIndex].minos[1] = m1;
                    }
                    break;

                case 2:
                case 3:
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }

            Set_Preview();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && onCycle == false)
        {
            switch (curSMinoIndex)
            {
                case 0:
                case 1:
                    if (sMinos[curSMinoIndex].minos[1].Xpos < swipe_RightLimit)
                    {
                        int x0 = sMinos[curSMinoIndex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoIndex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoIndex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoIndex].minos[1].Ypos;

                        Mino m0 = board[x0 +1, y0];
                        Mino m1 = board[x1 +1, y1];
                        Mino m2 = board[x0, y0];

                        m1.Set_MinoType(sMinos[curSMinoIndex].minos[1].MinoType);
                        m0.Set_MinoType(sMinos[curSMinoIndex].minos[0].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoIndex].minos[0] = m0;
                        sMinos[curSMinoIndex].minos[1] = m1;
                    }
                    break;

                case 2:
                case 3:
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }

            Set_Preview();
        }


        // Movement
        if (onCycle == false)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                switch(curSMinoIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        Reset_Preview();
                        StartCoroutine(Run_AlgoCycle_Corutine(curSMinoIndex));
                        break;

                    default:
                        Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                        break;
                }
            }
        }

//#endif
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

    MusicManager mm;
    UIManager um;
    W_Score w_score;
    
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
    [Header("Minos")]
    public Slamino[] sMinos;
    [SerializeField]
    List<ChainMino> cMinos;

    int upHor
    {
        get { return (int)(horLine + 0.5f); }
    }
    int downHor
    {
        get { return (int)(horLine - 0.5f); }
    }
    int leftVer
    {
        get { return (int)(verLine - 0.5f); }
    }
    int rightVer
    {
        get { return (int)(verLine + 0.5f); }
    }

    int curTurn;
    int curRound;

    int poppedMino_CurBoard;
    int poppedMino_Total;
    int poppedChain_CurBoard;
    int poppedChain_Total;

    int totalScore;

    int pop_Turn_Count;
    int pop_Chain_Count;
    int pop_Block_Count;
    bool isPop_Turn;

    int curSMinoIndex;

    int swipe_UpLimit;
    int swipe_DownLimit;
    int swipe_LeftLimit;
    int swipe_RightLimit;
    
    bool isHookHor;
    bool isHookVer;

    float timeAfterDrop;
    float timeAfterPop;


    /// <summary>
    /// 0 is perfect transparency, 1 is none transparency
    /// </summary>
    float transparency_blackLayer;

    // handle Action
    bool onCycle;

    #endregion

    #region Main Algorithm Cycle
    IEnumerator Run_AlgoCycle_Corutine(int SMinoIndex)
    {
        //Action Handle
        onCycle = true;

        //Initialize Variable
        int turnScore = 0;
        pop_Chain_Count = 0;

        //Push Setting
        Set_PushDirectioin(SMinoIndex);
        
        //Push Slamino
        Push_SMino_Separately(sMinos[SMinoIndex]);
        mm.Play_Drop();
        yield return new WaitForSeconds(timeAfterDrop);
        
        //Check & Pop Connected Minos
        Search_ChainMinos(curSMinoIndex);
        Pop_ChainMinos(MoveTypes.Push);
        if(pop_Block_Count != 0)
        {
            mm.Play_Pop_Continuous();

            turnScore += Get_TurnScore(pop_Block_Count, pop_Chain_Count, pop_Turn_Count);
            w_score.Input(1, turnScore);

            pop_Block_Count = 0;
        }
        yield return new WaitForSeconds(timeAfterPop);
        Reset_Minos_Movement();

        //Hook minos
        isHookHor = true;
        isHookVer = true;
        int chainLimit = 0;
        switch (SMinoIndex)
        {
            case 0:
            case 1:
                while (isHookHor == true&& chainLimit <= 5)
                {
                    HookHorizontal();
                    yield return new WaitForSeconds(timeAfterDrop);

                    Search_ChainMinos(curSMinoIndex);
                    if (isHookHor)
                    {
                        Pop_ChainMinos(MoveTypes.Push);
                        if (pop_Block_Count!= 0)
                        {
                            mm.Play_Pop_Continuous();

                            turnScore +=Get_TurnScore(pop_Block_Count, pop_Chain_Count, pop_Turn_Count);
                            w_score.Input(1, turnScore);

                            pop_Block_Count = 0;
                            yield return new WaitForSeconds(timeAfterPop);
                        }
                    }
                    Reset_Minos_Movement();
                    chainLimit++;
                }
                break;

            case 2:
            case 3:
                while (isHookVer == true && chainLimit <= 5)
                {
                    HookVertical();
                    yield return new WaitForSeconds(timeAfterDrop);

                    Search_ChainMinos(curSMinoIndex);
                    if (isHookVer)
                    {
                        Pop_ChainMinos(MoveTypes.Push);
                        if (pop_Block_Count != 0)
                        {
                            mm.Play_Pop_Continuous();

                            turnScore +=Get_TurnScore(pop_Block_Count, pop_Chain_Count, pop_Turn_Count);
                            w_score.Input(1, turnScore);

                            pop_Block_Count = 0;
                            yield return new WaitForSeconds(timeAfterPop);
                        }
                    }
                    Reset_Minos_Movement();
                    chainLimit++;
                }
                break;

            default:
                Debug.LogError("Wrong Index Number");
                break;
        }

        //Total Score Update
        totalScore += turnScore;
        List<string> totalScore_String = Get_Score_InDigit(totalScore);

        if(totalScore != 0 && turnScore!= 0)
        {
            for(int i = 1; i <= totalScore_String.Count; i++) // 반복횟수
            {
                string input = "";

                for(int index = totalScore_String.Count - i; index <= totalScore_String.Count - 1; index++)
                {
                    input += totalScore_String[index];
                }

                mm.Play_Score_Tap();
                w_score.Input(2, input);

                yield return new WaitForSeconds(0.06f);
            }
        }
        // Check Is Game Over
        if(!Get_IsDropAble(curSMinoIndex))
        {
            um.Open_GameOver();
        }

        //Reset_Sount Value
        mm.Reset_Pop_Continuous();

        //Reset_Variables
        Reset_Minos_Movement();

        //Respawn Slamino && Adjust to normal Position
        if (Get_IsSwipeLineEmpty(curSMinoIndex))
        {
            Reset_SMino_Position();
            sMinos[SMinoIndex].Spawn_SMino(true);
        }
        else
            um.Open_GameOver();


        //Turn Turn the Table
        switch (curSMinoIndex)
        {
            case 0:
                curSMinoIndex = 3;
                break;

            case 1:
                curSMinoIndex = 2;
                break;

            case 2:
                curSMinoIndex = 0;
                break;

            case 3:
                curSMinoIndex = 1;
                break;

            default:
                Debug.LogError("you input wrong Index!!");
                break;

        }
        
        switch (curSMinoIndex)
        {
            case 0:
                QuadZone.Chage_View(true, true, false, false, transparency_blackLayer);
                SwipeZone.Chage_View(true, false, false, false, transparency_blackLayer);
                AxisZone.Chage_View(true, false, 0.8f);

                yield return new WaitForSeconds(timeAfterDrop * 0.7f);
                HookUp();
                break;

            case 1:
                QuadZone.Chage_View(false, false, true, true, transparency_blackLayer);
                SwipeZone.Chage_View(false, true, false, false, transparency_blackLayer);
                AxisZone.Chage_View(true, false, 0.8f);

                yield return new WaitForSeconds(timeAfterDrop * 0.7f);
                HookDown();
                break;

            case 2:
                QuadZone.Chage_View(false, true, true, false, transparency_blackLayer);
                SwipeZone.Chage_View(false, false, true, false, transparency_blackLayer);
                AxisZone.Chage_View(false, true, 0.8f);

                yield return new WaitForSeconds(timeAfterDrop * 0.7f);
                HookLeft();
                break;

            case 3:
                QuadZone.Chage_View(true, false, false, true, transparency_blackLayer);
                SwipeZone.Chage_View(false, false, false, true, transparency_blackLayer);
                AxisZone.Chage_View(false, true, 0.8f);

                yield return new WaitForSeconds(timeAfterDrop*0.7f);
                HookRight();
                break;

            default:
                Debug.LogError("Wrong curSMinoIndex Number. Check!");
                break;
        }

        //Scoring Initialize
        if (isPop_Turn)
        {
            pop_Turn_Count++;
            isPop_Turn = false;
        }
        else
            pop_Turn_Count = 0;
        pop_Chain_Count = 0;

        w_score.Input(3, curRound);
        w_score.Input(4, pop_Turn_Count);
        w_score.Input(5, 1 + 0.1f * pop_Turn_Count);

        // TotalScore Line Update
        if(turnScore != 0)
            w_score.Override_BottomToTop();
           
        yield return new WaitForSeconds(timeAfterDrop);
        
        curTurn++;
        if ((curTurn % 4) - 1 == 0)
            curRound++;

        // color change
        switch(curRound)
        {
            case 4:
                minoVariety = 5;
                break;

            case 8:
                minoVariety = 6;
                break;
        }

        if(curRound% 2 == 0)
        {
            switch (curSMinoIndex)
            {
                case 0:
                    Add_Minos(Direction.Up, 1, 4, 4, 0, 0);
                    break;

                case 1:
                    Add_Minos(Direction.Down, 1, 4, 4, 0, 0);
                    break;

                case 2:
                    Add_Minos(Direction.Left, 1, 0, 0, 3, 3);
                    break;

                case 3:
                    Add_Minos(Direction.Right, 1, 0, 0, 3, 3);
                    break;
            }

            if(Get_IsDropAble(curSMinoIndex))
            {
                mm.Play_Score_Enter();
            }
            else
            {
                print("Unable");
            }
        }
        yield return new WaitForSeconds(timeAfterDrop);

        // Visual Change as turn
        Set_Preview();

        //End cycle
        onCycle = false;
    }
    #endregion
    
    #region Utility Functions
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

    bool Get_IsConnected(Mino m, MinoTypes targetType)
    {
        if (m.MinoType != MinoTypes.Empty)
        {
            if (((m.Ypos + 1) <= (mapY - 1 - gapY)) && board[m.Xpos, m.Ypos + 1].MinoType == m.MinoType)
                return true;
            else if (((m.Ypos - 1) >= gapY) && board[m.Xpos, m.Ypos - 1].MinoType == m.MinoType)
                return true;
            else if (((m.Xpos - 1) >= gapX) && board[m.Xpos - 1, m.Ypos].MinoType == m.MinoType)
                return true;
            else if (((m.Xpos + 1) <= (mapX - 1 - gapX) && board[m.Xpos + 1, m.Ypos].MinoType == m.MinoType))
                return true;
        }
        return false;
    }
    List<Mino> Get_ConnectedMinos(Mino m)
    {
        List<Mino> ms = new List<Mino>();
        ms.Add(m);

        for(int i=0; i<ms.Count; i++)
        {
            if (((ms[i].Ypos + 1) <= (mapY - 1 - gapY)) && board[ms[i].Xpos, ms[i].Ypos + 1].MinoType == ms[i].MinoType)
            {
                if (!ms.Contains(board[ms[i].Xpos, ms[i].Ypos + 1]))
                    ms.Add(board[ms[i].Xpos, ms[i].Ypos + 1]);
            }
            if (((ms[i].Ypos - 1) >= gapY) && board[ms[i].Xpos, ms[i].Ypos - 1].MinoType == ms[i].MinoType)
            {
                if (!ms.Contains(board[ms[i].Xpos, ms[i].Ypos - 1]))
                    ms.Add(board[ms[i].Xpos, ms[i].Ypos - 1]);
            }
            if (((ms[i].Xpos - 1) >= gapX) && board[ms[i].Xpos - 1, ms[i].Ypos].MinoType == ms[i].MinoType)
            {
                if (!ms.Contains(board[ms[i].Xpos - 1, ms[i].Ypos]))
                    ms.Add(board[ms[i].Xpos - 1, ms[i].Ypos]);
            }
            if (((ms[i].Xpos + 1) <= (mapX - 1 - gapX) && board[ms[i].Xpos + 1, ms[i].Ypos].MinoType == ms[i].MinoType))
            {
                if (!ms.Contains(board[ms[i].Xpos + 1, ms[i].Ypos]))
                    ms.Add(board[ms[i].Xpos + 1, ms[i].Ypos]);
            }
                
        }
        
        return ms;
    }
    void Search_ChainMinos()
    {
        //Reset
        cMinos.Clear();

        for(int x = gapX; x <= mapX -1 -gapX; x++)
        {
            for(int y = gapY; y <= mapY -1 - gapY; y++)
            {
                Mino m = board[x, y];

                if (Get_IsConnected(m,m.MinoType))
                {
                    bool IsAdded = false;

                    for(int i =0; i<cMinos.Count; i++)
                    {
                        if(cMinos[i].Get_ContainMino(m))
                        {
                            IsAdded = true;
                            break;
                        }
                    }

                    if(!IsAdded)
                    {
                        ChainMino cMino = new ChainMino(m.MinoType, Get_ConnectedMinos(m));
                        cMinos.Add(cMino);
                    }
                }
            }
        }
    }
    void Search_ChainMinos(int index)
    {
        //Reset
        cMinos.Clear();

        for (int x = gapX; x <= mapX - 1 - gapX; x++)
        {
            for (int y = gapY; y <= mapY - 1 - gapY; y++)
            {
                Mino m = board[x, y];

                if (Get_IsConnected(m, m.MinoType))
                {
                    bool IsAdded = false;

                    for (int i = 0; i < cMinos.Count; i++)
                    {
                        if (cMinos[i].Get_ContainMino(m))
                        {
                            IsAdded = true;
                            break;
                        }
                    }

                    if (!IsAdded)
                    {
                        ChainMino cMino = new ChainMino(m.MinoType, Get_ConnectedMinos(m));
                        cMinos.Add(cMino);
                    }
                }
            }
        }

        if(index == 0)
        {
            List<int> removeIndex = new List<int>();

            for (int i = 0; i < cMinos.Count; i++)
            {
                for (int x = gapX; x <= mapX - 1 - gapX; x++)
                {
                    for (int y = gapY; y <= downHor; y++)
                    {
                        Mino mino = board[x, y];
                        if (cMinos[i].Get_ContainMino(mino))
                        {
                            cMinos[i].Minos.Remove(mino);
                        }
                    }
                }

                if (cMinos[i].Minos.Count < 2)
                {
                    removeIndex.Add(i);
                }
            }

            for (int i = removeIndex.Count - 1; i >= 0; i--)
            {
                cMinos.Remove(cMinos[removeIndex[i]]);
            }
        }
        else if (index == 1)
        {
            List<int> removeIndex = new List<int>();

            for (int i = 0; i < cMinos.Count; i++)
            {
                for (int x = gapX; x <= mapX - 1 - gapX; x++)
                {
                    for (int y = mapY -1 - gapY; y >= upHor; y--)
                    {
                        Mino mino = board[x, y];
                        if (cMinos[i].Get_ContainMino(mino))
                        {
                            cMinos[i].Minos.Remove(mino);
                        }
                    }
                }

                if (cMinos[i].Minos.Count < 2)
                {
                    removeIndex.Add(i);
                }
            }

            for (int i = removeIndex.Count - 1; i >= 0; i--)
            {
                cMinos.Remove(cMinos[removeIndex[i]]);
            }
        }
        else if (index == 2)
        {
            List<int> removeIndex = new List<int>();

            for (int i = 0; i < cMinos.Count; i++)
            {
                for (int x = mapX -1 - gapX; x >= rightVer; x--)
                {
                    for (int y = mapY -1 - gapY; y >= gapY; y--)
                    {
                        Mino mino = board[x, y];
                        if (cMinos[i].Get_ContainMino(mino))
                        {
                            cMinos[i].Minos.Remove(mino);
                        }
                    }
                }

                if (cMinos[i].Minos.Count < 2)
                {
                    removeIndex.Add(i);
                }
            }

            for (int i = removeIndex.Count - 1; i >= 0; i--)
            {
                cMinos.Remove(cMinos[removeIndex[i]]);
            }
        }
        else if (index == 3)
        {
            List<int> removeIndex = new List<int>();

            for (int i = 0; i < cMinos.Count; i++)
            {
                for (int x = gapX; x <= leftVer; x++)
                {
                    for (int y = mapY - 1 - gapY; y >= gapY; y--)
                    {
                        Mino mino = board[x, y];
                        if (cMinos[i].Get_ContainMino(mino))
                        {
                            cMinos[i].Minos.Remove(mino);
                        }
                    }
                }

                if (cMinos[i].Minos.Count < 2)
                {
                    removeIndex.Add(i);
                }
            }

            for (int i = removeIndex.Count - 1; i >= 0; i--)
            {
                cMinos.Remove(cMinos[removeIndex[i]]);
            }
        }
        else
            Debug.LogError("you intput Wrong Number, It should be between 0~3(inclusive)");
    }

    void Pop_ChainMinos(MoveTypes mType)
    {
        List<int> indexList = new List<int>();

        for(int i = 0; i < cMinos.Count; i++)
        {
            if(cMinos[i].Get_ContainMoveType(mType))
            {
                indexList.Add(i);

                poppedChain_CurBoard++;
                poppedChain_Total++;

                if (isPop_Turn == false)
                    isPop_Turn = true;

                pop_Chain_Count++;
                
                for (int k = cMinos[i].Minos.Count -1; k > -1; k--)
                {
                    Clear_Mino(cMinos[i].Minos[k]);
                    pop_Block_Count++;
                    poppedMino_CurBoard++;
                    poppedMino_Total++;
                }
            }
        }

        for(int j = indexList.Count -1; j > -1; j--)
        {
            cMinos.Remove(cMinos[j]);
        }
    }
    
    void Set_PushDirectioin(int SMinoIndex)
    {
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
    }
    void Push_SMino_Separately(Slamino s)
    {
        for (int i = 0; i < s.minos.Count; i++)
        {
            Mino contact = Get_ContactPos_ToAxis(s.minos[i], xPush, yPush);

            int xPos = contact.Xpos;
            int yPos = contact.Ypos;

            Move_Mino(s.minos[i], xPos, yPos, MoveTypes.Push);
        }
    }

    void Reset_Minos_Movement()
    {
        for (int x = gapX; x <= mapX - 1 - gapX; x++)
        {
            for (int y = gapY; y <= mapY - 1 - gapY; y++)
            {
                board[x, y].Set_MoveType(MoveTypes.None);
            }
        }

        cMinos.Clear();
    }
    void Reset_SMino_Position()
    {
        for(int index=0; index<sMinos.Length; index++)
        {
            switch(index)
            {
                case 0:
                case 1:
                    int xLeft = mapX / 2 - 1;
                    int xRight = mapX / 2;
                    int y =  -1;
                    if (index == 0)
                        y = mapY - 1;
                    else if (index == 1)
                        y = 0;
                    else
                        Debug.LogError("Check your index and y value");


                    MinoTypes mT0_hor = sMinos[index].minos[0].MinoType;
                    MinoTypes mT1_hor = sMinos[index].minos[1].MinoType;

                    Clear_Mino(sMinos[index].minos[0]);
                    Clear_Mino(sMinos[index].minos[1]);

                    Mino m0_hor = board[xLeft, y];
                    Mino m1_hor = board[xRight, y];

                    m0_hor.Set_MinoType(mT0_hor);
                    m1_hor.Set_MinoType(mT1_hor);

                    sMinos[index].minos[0] = m0_hor;
                    sMinos[index].minos[1] = m1_hor;
                    break;

                case 2:
                case 3:
                    int yUp = mapY / 2;
                    int yDown = mapY /2 - 1;
                    int x = -1;
                    if (index == 2)
                        x = 0;
                    else if (index == 3)
                        x = mapX -1;
                    else
                        Debug.LogError("Check your index and x value");

                    MinoTypes mT0_ver = sMinos[index].minos[0].MinoType;
                    MinoTypes mT1_ver = sMinos[index].minos[1].MinoType;

                    Clear_Mino(sMinos[index].minos[0]);
                    Clear_Mino(sMinos[index].minos[1]);

                    Mino m0_ver = board[x, yUp];
                    Mino m1_ver = board[x, yDown];

                    m0_ver.Set_MinoType(mT0_ver);
                    m1_ver.Set_MinoType(mT1_ver);

                    sMinos[index].minos[0] = m0_ver;
                    sMinos[index].minos[1] = m1_ver;
                    break;

                default:
                    Debug.Log("Wrong Index error, it should be no more than 0~3(up down left right), check this out");
                    break;
            }
        }
    }

    int Get_TurnScore(int block_Count, int Chain_Count, int Turn_Count)
    {
        int score = 0;
        int block_Value = 100;
        
        float mult_Chain = Mathf.Pow(2, Chain_Count - 1);
        float mult_Turn = 1 + 0.1f * Turn_Count;
        float mult_Round = 1 +  (int)(curRound / 10);

        score = (int)(block_Value * block_Count *  mult_Chain * mult_Turn * mult_Round);

        return score;
    }
    int Get_ScoreToCredit(int score)
    {
        int credit = 0;

        credit = score / 10000;

        return credit;
    }

    List<string> Get_Score_InDigit(int value)
    {
        string stringValue  = value.ToString();
        char[] charArray = stringValue.ToCharArray();

        List<string> stringList = new List<string>();
        foreach (char c in charArray)
            stringList.Add(c.ToString());
        
        return stringList;
    }

    void HookHorizontal()
    {
        isHookHor = false;

        for (int x = gapX; x < mapX -1 -gapX; x++)
        {
            for (int y = upHor; y <= mapY -1 -gapY; y++)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], 0, -1);
                if (m.Ypos != board[x, y].Ypos)
                {
                    isHookHor = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
                }
            }

            for (int y = downHor; y >= gapY; y--)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], 0, 1);
                if (m.Ypos != board[x, y].Ypos)
                {
                    isHookHor = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
                }
            }
        }
    }
    void HookVertical()
    {
        isHookVer = false;

        for(int y = gapY; y <= mapY -1 - gapY; y++)
        {
            for(int x = leftVer; x>=gapX; x--)
            {
                Mino m = Get_ContactPos_ToAxis(board[x,y], 1, 0);
                if (m.Xpos != board[x,y].Xpos)
                {
                    isHookVer = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
                }
            }

            for(int x = rightVer; x<=mapX -1 -gapX; x++)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], -1, 0);
                if (m.Xpos != board[x, y].Xpos)
                {
                    isHookVer = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
                }
            }
        }
    }
    void HookUp()
    {
        isHookHor = false;

        for (int x = gapX; x < mapX - 1 - gapX; x++)
        {
            for (int y = upHor; y <= mapY - 1 - gapY; y++)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], 0, -1);
                if (m.Ypos != board[x, y].Ypos)
                {
                    isHookHor = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.None);
                }
            }
        }
    }
    void HookDown()
    {
        isHookHor = false;

        for (int x = gapX; x < mapX - 1 - gapX; x++)
        {
            for (int y = downHor; y >= gapY; y--)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], 0, 1);
                if (m.Ypos != board[x, y].Ypos)
                {
                    isHookHor = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.None);
                }
            }
        }
    }
    void HookLeft()
    {
        isHookVer = false;

        for (int y = gapY; y <= mapY - 1 - gapY; y++)
        {
            for (int x = leftVer; x >= gapX; x--)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], 1, 0);
                if (m.Xpos != board[x, y].Xpos)
                {
                    isHookVer = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.None);
                }
            }
        }
    }
    void HookRight()
    {
        isHookVer = false;

        for (int y = gapY; y <= mapY - 1 - gapY; y++)
        {
            for (int x = rightVer; x <= mapX - 1 - gapX; x++)
            {
                Mino m = Get_ContactPos_ToAxis(board[x, y], -1, 0);
                if (m.Xpos != board[x, y].Xpos)
                {
                    isHookVer = true;
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.None);
                }
            }
        }
    }

    /// <summary>
    /// hard to test this. how can i check?
    /// </summary>
    /// <param name="sIndex"></param>
    /// <returns></returns>
    bool Get_IsDropAble(int sIndex)
    {
        bool able = false;

        if(sIndex == 0 || sIndex == 1)
        {
            for (int x = swipe_LeftLimit; x <= swipe_RightLimit-1; x++)
            {
                if(sIndex ==0)
                {
                    if(board[x,mapY-1-gapY].MinoType == MinoTypes.Empty && board[x+1, mapY - 1 - gapY].MinoType == MinoTypes.Empty)
                    {
                        able = true;
                        return able;
                    }
                }
                if(sIndex ==1)
                {
                    if (board[x, gapY].MinoType == MinoTypes.Empty && board[x + 1, gapY].MinoType == MinoTypes.Empty)
                    {
                        able = true;
                        return able;
                    }
                }
            }
        }

        if (sIndex == 2 || sIndex == 3)
        {
            for (int y = swipe_UpLimit; y >= swipe_DownLimit + 1; y--)
            {
                if (sIndex == 2)
                {
                    if (board[gapX, y].MinoType == MinoTypes.Empty && board[gapX, y - 1].MinoType == MinoTypes.Empty)
                    {
                        able = true;
                        return able;
                    }
                }
                if (sIndex == 3)
                {
                    if (board[mapX -1 - gapX, y].MinoType == MinoTypes.Empty && board[mapX - 1 - gapX, y - 1].MinoType == MinoTypes.Empty)
                    {
                        able = true;
                        return able;
                    }
                }
            }
        }

        return able;
    }
    bool Get_IsSwipeLineEmpty(int sIndex)
    {
        bool empty = true;

        switch(sIndex)
        {
            case 0:
                for(int x = swipe_LeftLimit; x <= swipe_RightLimit; x++)
                {
                    if (board[x, mapY - 1].MinoType != MinoTypes.Empty)
                        empty = false;
                }
                break;

            case 1:
                for (int x = swipe_LeftLimit; x <= swipe_RightLimit; x++)
                {
                    if (board[x, 0].MinoType != MinoTypes.Empty)
                        empty = false;
                }
                break;

            case 2:
                for (int y = swipe_UpLimit; y >= swipe_DownLimit; y--)
                {
                    if (board[0, y].MinoType != MinoTypes.Empty)
                        empty = false;
                }
                break;

            case 3:
                for (int y = swipe_UpLimit; y >= swipe_DownLimit; y--)
                {
                    if (board[mapX -1, y].MinoType != MinoTypes.Empty)
                        empty = false;
                }
                break;

        }

        return empty;
    }

    void Move_Mino(Mino m, int xPos, int yPos, MoveTypes moveType)
    {
        if (board[xPos, yPos].MinoType == MinoTypes.Empty)
        {
            board[xPos, yPos].Set_MinoType(m.MinoType);
            board[xPos, yPos].Set_MoveType(moveType);
        }
        else
        {
            ;
        }

        if (m.Xpos == xPos && m.Ypos == yPos)
        {
            return;
        }
        else
            Clear_Mino(m);
    }
    void Clear_Mino(Mino m)
    {
        m.Set_MinoType(MinoTypes.Empty);
        m.Set_MoveType(MoveTypes.None);
    }
    void Add_Minos(Direction dir, int Refeat, int xLeft, int xRight, int yUp, int yDown)
    {
        for(int i =0; i < Refeat; i ++)
        {
            switch (dir)
            {
                case Direction.Up:
                    for (int y = mapY - 1 - gapY; y >= upHor; y--)
                    {
                        for (int x = leftVer - xLeft + 1; x <= rightVer + xRight - 1; x++)
                        {
                            Mino m = board[x, y];
                            if (y == mapY - 1 - gapY && m.MinoType != MinoTypes.Empty && board[x, y + 1].MinoType != MinoTypes.Empty)
                            {
                                um.Open_GameOver();
                            }
                            else
                                Move_Mino(m, m.Xpos, m.Ypos + 1, m.MoveType);
                        }
                    }

                    for (int x = leftVer - xLeft + 1; x <= rightVer + xRight - 1; x++)
                    {
                        board[x, upHor].Set_MinoType(Get_RandMinoType(true));
                    }
                    break;

                case Direction.Down:
                    for (int y = gapY; y <= downHor; y++)
                    {
                        for (int x = leftVer - xLeft + 1; x <= rightVer + xRight - 1; x++)
                        {
                            Mino m = board[x, y];
                            if (y == gapY && m.MinoType != MinoTypes.Empty && board[x, y - 1].MinoType != MinoTypes.Empty)
                            {
                                um.Open_GameOver();
                            }
                            else
                                Move_Mino(m, m.Xpos, m.Ypos - 1, m.MoveType);
                        }
                    }

                    for (int x = leftVer - xLeft + 1; x <= rightVer + xRight - 1; x++)
                    {
                        board[x, downHor].Set_MinoType(Get_RandMinoType(true));
                    }
                    break;

                case Direction.Left:
                    for (int x = gapX; x <= leftVer; x++)
                    {
                        for (int y = downHor - yDown + 1; y <= upHor + yUp - 1; y++)
                        {
                            Mino m = board[x, y];
                            if (x == gapX && m.MinoType != MinoTypes.Empty && board[x-1, y].MinoType != MinoTypes.Empty)
                            {
                                um.Open_GameOver();
                            }
                            else
                                Move_Mino(m, m.Xpos - 1, m.Ypos, m.MoveType);
                        }
                    }

                    for (int y = downHor - yDown + 1; y <= upHor + yUp - 1; y++)
                    {
                        board[leftVer, y].Set_MinoType(Get_RandMinoType(true));
                    }
                    break;

                case Direction.Right:
                    for (int x = mapX - 1 - gapX; x >= rightVer; x--)
                    {
                        for (int y = downHor - yDown + 1; y <= upHor + yUp - 1; y++)
                        {
                            Mino m = board[x, y];
                            if (x == mapX -1 - gapX && m.MinoType != MinoTypes.Empty && board[x + 1, y].MinoType != MinoTypes.Empty)
                            {
                                um.Open_GameOver();
                            }
                            else
                                Move_Mino(m, m.Xpos + 1, m.Ypos, m.MoveType);
                        }
                    }

                    for (int y = downHor - yDown + 1; y <= upHor + yUp - 1; y++)
                    {
                        board[rightVer, y].Set_MinoType(Get_RandMinoType(true));
                    }
                    break;

                default:
                    break;
            }
        }
    }

    void Set_Preview()
    {
        for(int x = gapX; x<= mapX - 1 - gapX; x++)
        {
            for(int y = gapY; y<= mapY -1 - gapY; y++)
            {
                board[x, y].Reset_ShadowType();
            }
        }

        int tempX =0;
        int tempY =0;

        switch (curSMinoIndex)
        {
            case 0:
                tempX = 0;
                tempY = -1;
                break;

            case 1:
                tempX = 0;
                tempY = 1;
                break;

            case 2:
                tempX = 1;
                tempY = 0;
                break;

            case 3:
                tempX = -1;
                tempY = 0;
                break;

            default:
                Debug.Log("Wrong cur Smino index error");
                break;
        }

        for (int i = 0; i < sMinos[curSMinoIndex].minos.Count; i++)
        {
            Mino m = Get_ContactPos_ToAxis(sMinos[curSMinoIndex].minos[i], tempX, tempY);
            m.Set_ShadowType(sMinos[curSMinoIndex].minos[i].MinoType);
        }
    }
    void Reset_Preview()
    {
        for (int x = gapX; x <= mapX - 1 - gapX; x++)
        {
            for (int y = gapY; y <= mapY - 1 - gapY; y++)
            {
                board[x, y].Reset_ShadowType();
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
    void Initialize_Slaminos(bool acceptSame)
    {
        sMinos[0].Initialize_SMino((board.GetLength(0) - 1) / 2f, board.GetLength(1) - 1, 0, -1);
        sMinos[1].Initialize_SMino((board.GetLength(0) - 1) / 2f, 0, 0, 1);
        sMinos[2].Initialize_SMino(0f, (board.GetLength(1) - 1) / 2f, 1, 0);
        sMinos[3].Initialize_SMino(board.GetLength(0) - 1, (board.GetLength(1) - 1) / 2f, -1, 0);

        for(int i =0; i <sMinos.Length; i ++)
        {
            sMinos[i].Spawn_SMino(acceptSame);
        }
    }
    #endregion
}
