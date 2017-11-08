using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour {

    #region MonoBehaviours

    void Awake()
    {
        sm = StageManager.Instance;
        mm = MusicManager.Instance;

        time = 0;
    }

    void Update()
    {
        #region KeyBoard Works
        // Scene Reset
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Stage");

        switch(sm.GameState)
        {
            case GameState.GameStart:
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    mm.Play_SpotLight(1);

                    mm.Change_Volume(mm.Sfx_Projector, 0.50f);
                    mm.Play_Projector(0);

                    //W_GameStart w_gameStart = WindowManager.Instance.Get_window("GameStart") as W_GameStart;
                    //w_gameStart.Off();

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

                    sm.GameState = GameState.Play;
                    mm.Play_BGM();

                    time = 0;
                }
                break;

            case GameState.Play:
                if(!mm.Bgm.isPlaying)
                {
                    print("bgf over");
                    time += Time.deltaTime;
                    if (time >= 4)
                    {
                        mm.Bgm.Play();
                        time = 0;
                    }
                }

                // gameover
                if (Input.GetKeyDown(KeyCode.E))
                {
                    mm.Sfx_Projector.Stop();
                    mm.Bgm.Stop();
                    mm.Play_Scratch(0);
                    mm.Play_SpotLight(1);

                    WindowManager.Instance.Get_window("Score").Off();
                    WindowManager.Instance.Get_window("Panel").On();
                    WindowManager.Instance.Get_window("Projector").Off(); ;

                    sm.GameState = GameState.LoadingGameOver;
                    
                    time = 0;
                }


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


#if UNITY_ANDROID || UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        lastPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        break;

                    case TouchPhase.Stationary:
                        lastPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        lastPos = touch.position;

                        break;

                    case TouchPhase.Ended:
                        lastPos = touch.position;


                        break;
                }
            }
        }


#endif
    }

    #endregion

    #region Field & Method
    private StageManager sm;
    private MusicManager mm;
    private float time;

    // Toucing Information
    Vector2 startPos; 
    Vector2 lastPos;
    
    float touchStartTime = 0.0f;
    float minSwipeDist = 50.0f;
    #endregion

}
