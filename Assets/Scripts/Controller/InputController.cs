using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        Delay_Pause = 0f;
        timeChecker = false;
        isPaused = false;

        if (PlayerPrefs.HasKey("SwipeSensitivity") == false)
            PlayerPrefs.SetFloat("SwipeSensitivity", 0.2f);
        
        sensitivity = 0.1f / PlayerPrefs.GetFloat("SwipeSensitivity");

        if (PlayerPrefs.GetString("TouchSetting") == "SwipeAndDrop")
            TouchSetting = TouchSetting.SwipeAndDrop;
        else
            TouchSetting = TouchSetting.PointAndDrop;
    }

    void Update()
    {
        #region Input Works
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Stage");
        
        // Android Out button
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        // Check every Update
        tap = false;

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

                //Resume Damper
                if(Delay_Pause > 0 && isPaused == false)
                {
                    Delay_Pause -= Time.deltaTime;
                    return;
                }
                
                #region Android Movment
                if(Input.touchCount > 0 && sm.OnCycle == false && isPaused == false)
                {
                    #region Point & Drop
                    if (TouchSetting == TouchSetting.PointAndDrop)
                    { 
                    // check in zone
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                    bool isInBoundary = false;
                    
                    // Check mino pos
                    float damp = 0.5f;
                    int mino_x0 = 0;
                    int mino_x1 = 0;
                    int mino_y0 = 0;
                    int mino_y1 = 0;

                    switch (sm.Cur_DirIndex)
                    {
                        case 0:
                        case 1:
                            if (touchPos.x > damp +1 && touchPos.x < sm.MapX - 2 - damp)
                            {
                                if (touchPos.y > damp && touchPos.y < sm.MapY - 1 - damp)
                                {
                                    isInBoundary = true;

                                    mino_x0 = (int)Mathf.Round(touchPos.x - 0.5f);
                                    mino_x1 = mino_x0 + 1;
                                }
                            }                                    
                            break;

                        case 2:
                        case 3:
                            if (touchPos.x > damp && touchPos.x < sm.MapX - 1 - damp)
                            {
                                if (touchPos.y > damp +1 && touchPos.y < sm.MapY - 2 - damp)
                                {
                                    isInBoundary = true;

                                    mino_y0 = (int)Mathf.Round(touchPos.y + 0.5f);
                                    mino_y1 = mino_y0 - 1;
                                }
                            }
                             break;

                        default:
                            Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                            break;
                    }

                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        isDraging = true;
                        tap = true;

                        temp_x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                        temp_y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;

                        temp_x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                        temp_y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                        temp_0 = sm.sMinos[sm.Cur_DirIndex].minos[0].MinoType;
                        temp_1 = sm.sMinos[sm.Cur_DirIndex].minos[1].MinoType;

                        if(isInBoundary)
                        {
                            switch (sm.Cur_DirIndex)
                            {
                                case 0:
                                case 1:
                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                    Mino m0_hor = sm.Board[mino_x0, temp_y0];
                                    Mino m1_hor = sm.Board[mino_x1, temp_y1];

                                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_hor;
                                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_hor;

                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    break;

                                case 2:
                                case 3:
                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                    Mino m0_ver = sm.Board[temp_x0, mino_y0];
                                    Mino m1_ver = sm.Board[temp_x1, mino_y1];

                                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_ver;
                                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_ver;

                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    break;

                                default:
                                    Debug.LogError("You didn't select CurIndex. use w,a,s,d key!");
                                    break;
                            }

                            sm.Set_Preview();
                        }
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
                    
                    if(isDraging && isInBoundary)
                    {
                        bool isBiggerThanOne = false;

                        switch (sm.Cur_DirIndex)    
                        {
                            case 0:
                            case 1:
                                if (sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos != mino_x0)
                                    isBiggerThanOne = true;
                                break;
                                
                            case 2:
                            case 3:
                                if (sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos != mino_y0)
                                    isBiggerThanOne = true;

                                break;

                            default:
                                Debug.LogError("Yest, it doesn't work");
                                break;
                        }

                        if(isBiggerThanOne)
                        {
                            switch (sm.Cur_DirIndex)
                            {
                                case 0:
                                case 1:
                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                    Mino m0_hor = sm.Board[mino_x0, temp_y0];
                                    Mino m1_hor = sm.Board[mino_x1, temp_y1];

                                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_hor;
                                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_hor;

                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    break;

                                case 2:
                                case 3:
                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                    Mino m0_ver = sm.Board[temp_x0, mino_y0];
                                    Mino m1_ver = sm.Board[temp_x1, mino_y1];

                                    sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_ver;
                                    sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_ver;

                                    sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                    sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                    break;

                                default:
                                    Debug.LogError("Yest, it doesn't work");
                                    break;
                            }

                            sm.Set_Preview();
                        }
                    }
                     
                    }
                    #endregion
                    
                    #region Swipe & Drop
                    if (TouchSetting == TouchSetting.SwipeAndDrop)
                    {                        
                        if (Input.touches[0].phase == TouchPhase.Began)
                        {
                            isDraging = true;
                            tap = true;
                            startTouch = Input.touches[0].position;
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
                            return;
                        }

                        if(isDraging)
                        {
                            if (startTouch == Vector2.zero)
                                startTouch = Input.touches[0].position;

                            swipeDelta = Camera.main.ScreenToWorldPoint(Input.touches[0].position - startTouch);

                            int movement = 0;

                            int cur_x0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Xpos;
                            int cur_x1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Xpos;
                            int cur_y0 = sm.sMinos[sm.Cur_DirIndex].minos[0].Ypos;
                            int cur_y1 = sm.sMinos[sm.Cur_DirIndex].minos[1].Ypos;

                            switch (sm.Cur_DirIndex)
                            {
                                case 0:
                                case 1:
                                    movement = (int)(swipeDelta.x / sensitivity);
                                    if (temp_x0 + movement != cur_x0)
                                    {
                                        if (temp_x0 + movement >= 1 && temp_x0 + movement <= sm.MapX - 3)
                                        {
                                            sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                            sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                            Mino m0_hor = sm.Board[temp_x0 + movement, temp_y0];
                                            Mino m1_hor = sm.Board[temp_x1 + movement, temp_y1];

                                            sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_hor;
                                            sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_hor;

                                            sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                            sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                            sm.Set_Preview();
                                        }
                                    }
                                    break;

                                case 2:
                                case 3:
                                    movement = (int)(swipeDelta.y / sensitivity);
                                    if (temp_y0 + movement != cur_y0)
                                    {
                                        if (temp_y0 + movement >= 2 && temp_y0 + movement <= sm.MapY - 2)
                                        {
                                            sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(MinoTypes.Empty);
                                            sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(MinoTypes.Empty);

                                            Mino m0_ver = sm.Board[temp_x0, temp_y0 + movement];
                                            Mino m1_ver = sm.Board[temp_x1, temp_y1 + movement];

                                            sm.sMinos[sm.Cur_DirIndex].minos[0] = m0_ver;
                                            sm.sMinos[sm.Cur_DirIndex].minos[1] = m1_ver;

                                            sm.sMinos[sm.Cur_DirIndex].minos[0].Set_MinoType(temp_0);
                                            sm.sMinos[sm.Cur_DirIndex].minos[1].Set_MinoType(temp_1);
                                            sm.Set_Preview();
                                        }
                                    }
                                    break;

                                default:
                                    Debug.LogError("input wrong cur_Dic");
                                    break;
                            }                            
                        }                                 
                    }
                    #endregion
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

    public bool isPaused;
    public float Delay_Pause;

    // Toucing Information
    private bool tap;
    private bool isDraging = false;
    public TouchSetting TouchSetting;
    public float sensitivity;
    
    private Vector2 startTouch, swipeDelta;

    // Starting Information
    public int temp_x0, temp_x1, temp_y0, temp_y1;
    public MinoTypes temp_0, temp_1;
    
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    #endregion

}
