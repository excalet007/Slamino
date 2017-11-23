using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    #region MonoBehaviours
    /// <summary>
    /// SetUp all linked windows
    /// </summary>
    void Awake()
    {
        foreach (Window w in windows)
            w.SetUp();

        Get_window("Score").Off();
        Get_window("Tutorial").Off();
        Get_window("Panel").On();
        Get_window("Projector").On();
        Get_window("GameOver").Off();
        Get_window("GameStart").On();
    }

    #endregion

    #region Field
    //SingleTon
    private static WindowManager instance;
    public static WindowManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<WindowManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "StageManger";
                    instance = container.AddComponent<WindowManager>();
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// window List, link through hierarchy window
    /// </summary>
    public List<Window> windows = new List<Window>();
    #endregion

    #region Method
    /// <summary>
    /// return Window, You should change Type
    /// </summary>
    /// <param name="name"> Score, GameOver, Proejctor, etc </param>
    /// <returns></returns>
    public Window Get_window(string id)
    {
        Window window = windows.Find(x => x.Id == id);

        if (window != null)
            return window;
        else
        {
            Debug.LogError("cannot find 'Id' in window Manager");
            return null;
        }
    }
    #endregion


}
