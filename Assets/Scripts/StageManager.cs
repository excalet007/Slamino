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
        minoVariety = 5;
        mm.Change_Volume(mm.Bgm, 0.3f);
        mm.Change_Volume(mm.Sfx_Drop, 0.4f);
        mm.Change_Volume(mm.Sfx_Pop, 0.5f);
        mm.Change_PopStartPoint(1);

        // -----------------------------------Algorithm control ---------------------------------------
        

        // -----------------------------------Initialize Variables----------------------------------------

        cMinos = new List<ChainMino>();
        onCycle = false;
        isHookHor = false;
        isHookVer = false;

        poppedMino_CurBoard = 0;
        poppedChain_CurBoard = 0;

        poppedMino_Total = 0;
        poppedChain_Total = 0;

        poppedChain_Combo = 0;

        curSMinoInex = -1;

        swipe_UpLimit = mapY - 2;
        swipe_DownLimit = 1;
        swipe_LeftLimit = 1;
        swipe_RightLimit = mapX - 2;

        timeAfterDrop = 0.1f;
        timeAfterPop = 0.1f;

        // ------------------------------------------MapGenarating-----------------------------------

        Initialize_Board(mapX, mapY);
        Initialize_Axis(true, 0, 0);
        Initialize_CenterMinos(horLine, verLine, 5,3, false);
        Initialize_Slaminos(true);

        // ------------------------------------------ Starts Sound --------------------------------------
        mm.Play_BGM();
    }

    void Update()
    {
        //#if UNITY_EDITOR
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        // SMino Selection & view Change
        if (Input.GetKeyDown(KeyCode.W))
        {
            curSMinoInex = 0;
            QuadZone.Chage_View(true, true, false, false, 0.75f);
            SwipeZone.Chage_View(true, false, false, false, 0.75f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            curSMinoInex = 1;
            QuadZone.Chage_View(false, false, true, true, 0.75f);
            SwipeZone.Chage_View(false, true, false, false, 0.75f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            curSMinoInex = 2;
            QuadZone.Chage_View(false, true, true, false, 0.75f);
            SwipeZone.Chage_View(false, false, true, false, 0.75f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            curSMinoInex = 3;
            QuadZone.Chage_View(true, false, false, true, 0.75f);
            SwipeZone.Chage_View(false, false, false, true, 0.75f);
        }

        // SMino Movement
        if (Input.GetKeyDown(KeyCode.UpArrow) && onCycle == false)
        {
            switch (curSMinoInex)
            {
                case 0:
                case 1:
                    break;

                case 2:
                case 3:
                    if (sMinos[curSMinoInex].minos[0].Ypos < swipe_UpLimit)
                    {
                        int x0 = sMinos[curSMinoInex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoInex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoInex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoInex].minos[1].Ypos;

                        Mino m0 = board[x0, y0 + 1];
                        Mino m1 = board[x1, y1 + 1];
                        Mino m2 = board[x1, y1];

                        m0.Set_MinoType(sMinos[curSMinoInex].minos[0].MinoType);
                        m1.Set_MinoType(sMinos[curSMinoInex].minos[1].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoInex].minos[0] = m0;
                        sMinos[curSMinoInex].minos[1] = m1;
                    }
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && onCycle == false)
        {
            switch (curSMinoInex)
            {
                case 0:
                case 1:
                    break;

                case 2:
                case 3:
                    if (sMinos[curSMinoInex].minos[1].Ypos > swipe_DownLimit)
                    {
                        int x0 = sMinos[curSMinoInex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoInex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoInex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoInex].minos[1].Ypos;

                        Mino m0 = board[x0, y0 - 1];
                        Mino m1 = board[x1, y1 - 1];
                        Mino m2 = board[x0, y0];

                        m1.Set_MinoType(sMinos[curSMinoInex].minos[1].MinoType);
                        m0.Set_MinoType(sMinos[curSMinoInex].minos[0].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoInex].minos[0] = m0;
                        sMinos[curSMinoInex].minos[1] = m1;
                    }
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && onCycle == false)
        {
            switch (curSMinoInex)
            {
                case 0:
                case 1:
                    if (sMinos[curSMinoInex].minos[0].Xpos > swipe_LeftLimit )
                    {
                        int x0 = sMinos[curSMinoInex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoInex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoInex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoInex].minos[1].Ypos;

                        Mino m0 = board[x0-1, y0];
                        Mino m1 = board[x1-1, y1];
                        Mino m2 = board[x1, y1];

                        m0.Set_MinoType(sMinos[curSMinoInex].minos[0].MinoType);
                        m1.Set_MinoType(sMinos[curSMinoInex].minos[1].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoInex].minos[0] = m0;
                        sMinos[curSMinoInex].minos[1] = m1;
                    }
                    break;

                case 2:
                case 3:
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && onCycle == false)
        {
            switch (curSMinoInex)
            {
                case 0:
                case 1:
                    if (sMinos[curSMinoInex].minos[1].Xpos < swipe_RightLimit)
                    {
                        int x0 = sMinos[curSMinoInex].minos[0].Xpos;
                        int y0 = sMinos[curSMinoInex].minos[0].Ypos;

                        int x1 = sMinos[curSMinoInex].minos[1].Xpos;
                        int y1 = sMinos[curSMinoInex].minos[1].Ypos;

                        Mino m0 = board[x0 +1, y0];
                        Mino m1 = board[x1 +1, y1];
                        Mino m2 = board[x0, y0];

                        m1.Set_MinoType(sMinos[curSMinoInex].minos[1].MinoType);
                        m0.Set_MinoType(sMinos[curSMinoInex].minos[0].MinoType);
                        m2.Set_MinoType(MinoTypes.Empty);

                        sMinos[curSMinoInex].minos[0] = m0;
                        sMinos[curSMinoInex].minos[1] = m1;
                    }
                    break;

                case 2:
                case 3:
                    break;

                default:
                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                    break;
            }
        }


        // Movement
        if (onCycle == false)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                switch(curSMinoInex)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        StartCoroutine(Run_AlgoCycle_Corutine(curSMinoInex));
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

    int poppedMino_CurBoard;
    int poppedMino_Total;
    int poppedChain_CurBoard;
    int poppedChain_Total;

    int poppedChain_Combo;
    bool isPopeed;

    int curSMinoInex;

    int swipe_UpLimit;
    int swipe_DownLimit;
    int swipe_LeftLimit;
    int swipe_RightLimit;
    
    bool isHookHor;
    bool isHookVer;

    float timeAfterDrop;
    float timeAfterPop;

    // handle Action
    bool onCycle;

    #endregion

    #region Main Algorithm Cycle
    IEnumerator Run_AlgoCycle_Corutine(int SMinoIndex)
    {
        //Action Handle
        onCycle = true;

        //Initialize Variable
        poppedChain_Combo = 0;
        isPopeed = false;

        //Push Setting
        Set_PushDirectioin(SMinoIndex);

        //Drop Floating minos
        switch (SMinoIndex)
        {
            case 0:
                HookUp();
                break;
            case 1:
                HookDown();
                break;
            case 2:
                HookLeft();
                break;
            case 3:
                HookRight();
                break;

            default:
                Debug.LogError("You input wrong SMinoIndex, check this out!");
                break;
        }
        yield return new WaitForSeconds(timeAfterDrop);

        //Push Slamino
        Push_SMino_Separately(sMinos[SMinoIndex]);
        mm.Play_Drop();
        yield return new WaitForSeconds(timeAfterDrop);
        
        //Check & Pop Connected Minos
        Search_ChainMinos();
        Pop_ChainMinos(MoveTypes.Push);
        if(isPopeed)
        {
            mm.Play_Pop(poppedChain_Combo);
            isPopeed = false;
        }
        yield return new WaitForSeconds(timeAfterPop);
        Reset_Minos_Movement();

        //Hook minos
        isHookHor = true;
        isHookVer = true;
        int chainLimit = 0;
        QuadZone.Chage_View(true, true, true, true, 0f);
        switch (SMinoIndex)
        {
            case 0:
            case 1:
                while (isHookHor == true&& chainLimit <= 5)
                {
                    HookHorizontal();
                    yield return new WaitForSeconds(timeAfterDrop);

                    Search_ChainMinos();
                    if (isHookHor)
                    {
                        Pop_ChainMinos(MoveTypes.Push);
                        if (isPopeed)
                        {
                            mm.Play_Pop(poppedChain_Combo);
                            isPopeed = false;
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

                    Search_ChainMinos();
                    if (isHookVer)
                    {
                        Pop_ChainMinos(MoveTypes.Push);
                        if (isPopeed)
                        {
                            mm.Play_Pop(poppedChain_Combo);
                            isPopeed = false;
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

        // Push whole Minos  OR   Reset  Boards
        if (poppedChain_CurBoard >= 15)
        {
            if (minoVariety < 6)
                minoVariety++;

            poppedChain_CurBoard = 0;
            poppedMino_CurBoard = 0;

            for (int x = gapY; x <= mapX - 1 - gapX; x++)
            {
                for (int y = gapY; y <= mapY - 1 - gapY; y++)
                {
                    board[x, y].Set_MinoType(MinoTypes.Empty);
                }
            }

            Initialize_CenterMinos(horLine, verLine, 5, 3, false);
        }

        //Reset_Variables
        Reset_Minos_Movement();

        //Respawn Slamino
        sMinos[SMinoIndex].Spawn_SMino(true);
        
        //End cycle
        onCycle = false;
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
        return board[m.Xpos + xDif, m.Ypos + yDif];
    }
    
    bool Get_IsCMinos_ContainMovetype(MoveTypes mType)
    {
        for(int i =0; i<cMinos.Count; i++)
        {
            if (cMinos[i].Get_ContainMoveType(mType))
                return true;
        }
        return false;
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
    bool Get_IsFloating_Cross(Mino m)
    {
        if (m.MinoType != MinoTypes.Empty)
        {
            if (((m.Ypos + 1) <= (mapY - 1 - gapY)) && board[m.Xpos, m.Ypos + 1].MinoType != MinoTypes.Empty)
                return false;
            else if (((m.Ypos - 1) >= gapY) && board[m.Xpos, m.Ypos - 1].MinoType != MinoTypes.Empty)
                return false;
            else if (((m.Xpos - 1) >= gapX) && board[m.Xpos - 1, m.Ypos].MinoType != MinoTypes.Empty)
                return false;
            else if (((m.Xpos + 1) <= (mapX - 1 - gapX)) && board[m.Xpos + 1, m.Ypos].MinoType != MinoTypes.Empty)
                return false;
        }
        return true;
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
    List<Mino> Get_Minos_ByMoveType(MoveTypes mType)
    {
        List<Mino> mList = new List<Mino>();

        for (int x = gapX; x <= mapX - 1 - gapX; x++)
        {
            for (int y = gapY; y <= mapY - 1 - gapY; y++)
            {
                if (board[x, y].MoveType == mType)
                    mList.Add(board[x, y]);
            }
        }

        return mList;
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

                poppedChain_Combo++;
                isPopeed = true;

                for (int k = cMinos[i].Minos.Count -1; k > -1; k--)
                {
                    Clear_Mino(cMinos[i].Minos[k]);
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
    void Pop_ChainMinosAll()
    {
        for (int i = 0; i < cMinos.Count; i++)
        {
            for (int k = cMinos[i].Minos.Count - 1; k > -1; k--)
            {
                Clear_Mino(cMinos[i].Minos[k]);
            }
        }
        cMinos.Clear();
    }

    void Push_FloatingSMino()
    {
        List<Mino> FMino = Get_Minos_ByMoveType(MoveTypes.Push);
        for(int i = 0; i<FMino.Count; i++)
        {
            if(Get_IsFloating_Cross(FMino[i]))
            {
                Mino to = Get_ContactPos_ToZone(FMino[i], xPush, yPush);
                Move_Mino(FMino[i], to.Xpos, to.Ypos, MoveTypes.Push);
            }
        }

    }
    void Push_AllMinos(int SMinoIndex)
    {
        switch(SMinoIndex)
        {
            case 0:
                for (int x = gapY; x <= mapX - 1 - gapX; x++)
                {
                    for (int y = gapY; y <= mapY - 1 - gapY; y++)
                    {
                        if (board[x, y].MinoType != MinoTypes.Empty)
                            Move_Mino(board[x, y], x , (y - 1), MoveTypes.Push);
                    }
                }
                break;

            case 1:
                for (int x = gapX; x <= mapX - 1 - gapX; x++)
                {
                    for (int y = mapY-1-gapY; y >= gapY; y--)
                    {
                        if (board[x, y].MinoType != MinoTypes.Empty)
                            Move_Mino(board[x, y], x, (y + 1), MoveTypes.Push);
                    }
                }
                break;

            case 2:
                for (int x = mapX -1 - gapY; x >= gapX; x--)
                {
                    for (int y = gapY; y <= mapY -1 - gapY; y++)
                    {
                        if (board[x, y].MinoType != MinoTypes.Empty)
                            Move_Mino(board[x, y], (x+1), y, MoveTypes.Push);
                    }
                }
                break;

            case 3:
                for (int x = gapY; x <= mapX - 1 -gapX; x++)
                {
                    for (int y = gapY; y <= mapY - 1 - gapY; y++)
                    {
                        if (board[x, y].MinoType != MinoTypes.Empty)
                            Move_Mino(board[x, y], (x -1), y, MoveTypes.Push);
                    }
                }
                break;

            default:
                Debug.LogError("you Input wrong SMinoIndex");
                break;
        }
    }
    void Push_SMino(Slamino s)
    {
        #region Get minimum movement for Slamino
        int minMove = (int)Mathf.Max((int)mapX, (int)mapY);
        for (int i = 0; i < s.minos.Count; i++)
        {
            Mino contact = Get_ContactPos_ToZone(s.minos[i], s.XPush, s.YPush);

            //Xdirection Move
            if (s.XPush == 1 || s.XPush == -1)
            {
                int xDif = s.minos[i].Xpos - contact.Xpos;

                if (Mathf.Abs((float)xDif) < Mathf.Abs((float)minMove))
                    minMove = (int)Mathf.Abs((float)xDif);
            }

            // Ydirection Move
            if (s.YPush == 1 || s.YPush == -1)
            {
                int yDif = s.minos[i].Ypos - contact.Ypos;

                if (Mathf.Abs((float)yDif) < Mathf.Abs((float)minMove))
                    minMove = (int)Mathf.Abs((float)yDif);
            }
        }
        #endregion

        #region push minos using minimum movement
        for (int i = 0; i < s.minos.Count; i++)
        {
            int xPos = s.minos[i].Xpos + s.XPush * minMove;
            int yPos = s.minos[i].Ypos + s.YPush * minMove;

            Move_Mino(s.minos[i], xPos, yPos, MoveTypes.Push);
        }
        #endregion
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
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
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
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
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
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
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
                    Move_Mino(board[x, y], m.Xpos, m.Ypos, MoveTypes.Push);
                }
            }
        }
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
            Debug.LogError("Board[" + xPos + "," + yPos + "] tyep is not Empty it's" + board[xPos, yPos].MinoType);
            return;
        }
        Clear_Mino(m);
    }
    void Clear_Mino(Mino m)
    {
        m.Set_MinoType(MinoTypes.Empty);
        m.Set_MoveType(MoveTypes.None);
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
