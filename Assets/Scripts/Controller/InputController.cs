using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Control Input
/// Mobile refered : https://www.youtube.com/watch?v=rDK_3qXHAFg
/// </summary>
public class InputController : MonoBehaviour {

    #region MonoBehaviours

    void Awake()
    {
        sm = StageManager.Instance;
        mm = MusicManager.Instance;

        time = 0;
        timeChecker = false;
    }

    void Update()
    {
        #region KeyBoard Works
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Stage");

        // Check every Update
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        switch(sm.GameState)
        {
            case GameState.GameStart:
                if(Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
                {
                    mm.Play_SpotLight(1);
                    
                    mm.Play_Projector(0);

                    WindowManager.Instance.Get_window("Projector").Off();
                    WindowManager.Instance.Get_window("GameStart").Off();

                    sm.GameState = GameState.LoadingPlay;
                    time = 0;
                }

                break;

            case GameState.LoadingPlay:
                if(!mm.Sfx_Projector.isPlaying)
                   mm.Play_Projector(0);

                
                time += Time.deltaTime;
                if (time >= 2.75f)
                {
                    mm.Play_SpotLight(0);
                    mm.Play_Projector(1);
                    mm.Sfx_Projector.loop = true;

                    WindowManager.Instance.Get_window("Score").On();
                    WindowManager.Instance.Get_window("Panel").Off();
                    WindowManager.Instance.Get_window("Projector").On();
                    WindowManager.Instance.Get_window("Button_Pause").On();

                    sm.GameState = GameState.Play;
                    mm.Play_BGM();

                    time = 0;
                }
                break;

            case GameState.Play:
                if(!mm.Bgm.isPlaying)
                {
                    time += Time.deltaTime;
                    if (time >= 4)
                    {
                        mm.Bgm.Play();
                        time = 0;
                    }
                }

                #region smino movement by Touches


                if(Input.touchCount > 0 && sm.OnCycle == false)
                {
                    if(Input.touches[0].phase == TouchPhase.Began)
                    {
                        damper = 0;
                        isDraging = true;
                        tap = true;
                        startTouch = Input.touches[0].position;

                        temp_x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                        temp_y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                        temp_x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                        temp_y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                        temp_0 = sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType;
                        temp_1 = sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType;

                    }
                    else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                    {
                        switch (sm.Cur_DirIndex)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                sm.Reset_Preview();
                                StartCoroutine(sm.Run_AlgoCycle_Corutine(sm.Cur_DirIndex));
                                break;

                            default:
                                Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                                break;
                        }

                        isDraging = false;
                        Reset();
                    }

                    // calc distance
                    swipeDelta = Vector2.zero;
                    if(isDraging)
                    {
                        if (Input.touches.Length > 0)
                            swipeDelta = Input.touches[0].position - startTouch;

                        switch (sm.Cur_DirIndex)
                        {
                            case 0:
                            case 1:
                                int movement = (int)(swipeDelta.x / sensitivity);

                                if (temp_x0 + movement != sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos)
                                {
                                    if ((temp_x0 + movement > 0) && (temp_x0 + movement < sm.MapX - 2))
                                    {
                                        MoveTypes move0 = sm.sMinos[sm.Cur_DirIndex].minos[0].MoveType;
                                        MoveTypes move1 = sm.sMinos[sm.Cur_DirIndex].minos[1].MoveType;

                                        Mino m0 = sm.Board[temp_x0 + movement, temp_y0];
                                        Mino m1 = sm.Board[temp_x1 + movement, temp_y1];

                                        sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                        sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                        sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                        sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;

                                        sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                        sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    }
                                    sm.Set_Preview();
                                }

                                break;

                            case 2:
                            case 3:
                                int movement2 = (int)(swipeDelta.y / sensitivity);

                                if (temp_y0 + movement2 != sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos)
                                {
                                    if ((temp_y0 + movement2 > 1) && (temp_y0 + movement2 < sm.MapY - 1))
                                    {
                                        MoveTypes move0 = sm.sMinos[sm.Cur_DirIndex].minos[0].MoveType;
                                        MoveTypes move1 = sm.sMinos[sm.Cur_DirIndex].minos[1].MoveType;

                                        Mino m0 = sm.Board[temp_x0, temp_y0 + movement2];
                                        Mino m1 = sm.Board[temp_x1, temp_y1 + movement2];

                                        sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                        sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                        sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                        sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;

                                        sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                        sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    }
                                    sm.Set_Preview();
                                }

                                break;
                        }
                    }


                    

                    // New Logic

                    // Deadzone Check
                    //if(swipeDelta.magnitude > 175 * (1+damper))
                    //{
                    //    damper++;

                    //    //which direction?
                    //    float x = swipeDelta.x;
                    //    float y = swipeDelta.y;

                    //    switch (sm.Cur_DirIndex)
                    //    {
                    //        case 0:
                    //        case 1:
                    //            if(x>0)
                    //            {
                    //                if (sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos < sm.MapX - 2)
                    //                {
                    //                    int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                    //                    int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                    //                    int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                    //                    int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                    //                    Mino m0 = sm.Board[x0 + 1, y0];
                    //                    Mino m1 = sm.Board[x1 + 1, y1];
                    //                    Mino m2 = sm.Board[x0, y0];

                    //                    m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                    //                    m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                    //                    m2.Set_MinoType(MinoTypes.Empty);

                    //                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                    //                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                    //                }
                    //            }
                    //            else if(x<0)
                    //            {
                    //                if (sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos > 1)
                    //                {
                    //                    int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                    //                    int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                    //                    int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                    //                    int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                    //                    Mino m0 = sm.Board[x0 - 1, y0];
                    //                    Mino m1 = sm.Board[x1 - 1, y1];
                    //                    Mino m2 = sm.Board[x1, y1];

                    //                    m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                    //                    m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                    //                    m2.Set_MinoType(MinoTypes.Empty);

                    //                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                    //                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                    //                }
                    //            }
                    //            sm.Set_Preview();
                    //            break;

                    //        case 2:
                    //        case 3:
                    //            if(y>0)
                    //            {
                    //                if (sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos < sm.MapY - 2)
                    //                {
                    //                    int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                    //                    int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                    //                    int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                    //                    int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                    //                    Mino m0 = sm.Board[x0, y0 + 1];
                    //                    Mino m1 = sm.Board[x1, y1 + 1];
                    //                    Mino m2 = sm.Board[x1, y1];

                    //                    m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                    //                    m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                    //                    m2.Set_MinoType(MinoTypes.Empty);

                    //                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                    //                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                    //                }
                    //            }
                    //            if(y<0)
                    //            {
                    //                if (sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos > 1)
                    //                {
                    //                    int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                    //                    int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                    //                    int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                    //                    int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                    //                    Mino m0 = sm.Board[x0, y0 - 1];
                    //                    Mino m1 = sm.Board[x1, y1 - 1];
                    //                    Mino m2 = sm.Board[x0, y0];

                    //                    m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                    //                    m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                    //                    m2.Set_MinoType(MinoTypes.Empty);

                    //                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                    //                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                    //                }
                    //            }
                    //            sm.Set_Preview();
                    //            break;

                    //        default:
                    //            Debug.LogError("Youur touch input is wrong");
                    //            break;
                    //    }
                    //}

                    
                }
                #endregion


                #region smino movement by keyboard
                // SMino Movement
                if (Input.GetKeyDown(KeyCode.UpArrow) && sm.OnCycle == false)
                {
                    switch (sm.Cur_DirIndex)
                    {
                        case 0:
                        case 1:
                            break;

                        case 2:
                        case 3:
                            if (sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos < sm.MapY - 2)
                            {
                                int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                                int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                                int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                                int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                                Mino m0 = sm.Board[x0, y0 + 1];
                                Mino m1 = sm.Board[x1, y1 + 1];
                                Mino m2 = sm.Board[x1, y1];

                                m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                                m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                                m2.Set_MinoType(MinoTypes.Empty);

                                sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                            }
                            break;

                        default:
                            Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                            break;
                    }

                    sm.Set_Preview();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && sm.OnCycle == false)
                {
                    switch (sm.Cur_DirIndex)
                    {
                        case 0:
                        case 1:
                            break;

                        case 2:
                        case 3:
                            if (sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos > 1)
                            {
                                int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                                int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                                int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                                int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                                Mino m0 = sm.Board[x0, y0 - 1];
                                Mino m1 = sm.Board[x1, y1 - 1];
                                Mino m2 = sm.Board[x0, y0];

                                m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                                m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                                m2.Set_MinoType(MinoTypes.Empty);

                                sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                            }
                            break;

                        default:
                            Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                            break;
                    }

                    sm.Set_Preview();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) && sm.OnCycle == false)
                {
                    switch (sm.Cur_DirIndex)
                    {
                        case 0:
                        case 1:
                            if (sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos > 1)
                            {
                                int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                                int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                                int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                                int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                                Mino m0 = sm.Board[x0 - 1, y0];
                                Mino m1 = sm.Board[x1 - 1, y1];
                                Mino m2 = sm.Board[x1, y1];

                                m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                                m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                                m2.Set_MinoType(MinoTypes.Empty);

                                sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                            }
                            break;

                        case 2:
                        case 3:
                            break;

                        default:
                            Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                            break;
                    }

                    sm.Set_Preview();
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) && sm.OnCycle == false)
                {
                    switch (sm.Cur_DirIndex)
                    {
                        case 0:
                        case 1:
                            if (sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos < sm.MapX - 2)
                            {
                                int x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                                int y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                                int x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                                int y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                                Mino m0 = sm.Board[x0 + 1, y0];
                                Mino m1 = sm.Board[x1 + 1, y1];
                                Mino m2 = sm.Board[x0, y0];

                                m1.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType);
                                m0.Set_MinoType(sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType);
                                m2.Set_MinoType(MinoTypes.Empty);

                                sm.sMinos[sm.Cur_DirIndex].minos[0] = m0;
                                sm.sMinos[sm.Cur_DirIndex].minos[1] = m1;
                            }
                            break;

                        case 2:
                        case 3:
                            break;

                        default:
                            Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                            break;
                    }

                    sm.Set_Preview();
                }


                // Movement
                if (sm.OnCycle == false)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        switch (sm.Cur_DirIndex)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                sm.Reset_Preview();
                                StartCoroutine(sm.Run_AlgoCycle_Corutine(sm.Cur_DirIndex));
                                break;

                            default:
                                Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                                break;
                        }
                    }
                }
                #endregion
                break;

            case GameState.LoadingGameOver:
                if(!timeChecker)
                {
                    time = 0;
                    timeChecker = true;
                }

                time += Time.deltaTime;
                if(time > 3f)
                {
                    mm.Play_SpotLight(0);

                    WindowManager.Instance.Get_window("Projector").On();
                    WindowManager.Instance.Get_window("GameOver").On();
                    time = 0;

                    sm.GameState = GameState.GameOver;
                }
                break;

            case GameState.GameOver:
                break;

            default:
                Debug.Log("You inputed other gamestate");
                break;
        }

        #endregion




    }

    #endregion

    #region Field & Method
    private static InputController instance;
    public static InputController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputController>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "InputController";
                    instance = container.AddComponent<InputController>();
                }
            }
            return instance;
        }
    }

    private StageManager sm;
    private MusicManager mm;
    private float time;
    private bool timeChecker;

    // Toucing Information
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;

    private Vector2 startTouch, swipeDelta;
    private int damper;

    // Starting Information
    int temp_x0, temp_x1, temp_y0, temp_y1;
    MinoTypes temp_0, temp_1;
    float sensitivity = 120f;

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    #endregion

}
