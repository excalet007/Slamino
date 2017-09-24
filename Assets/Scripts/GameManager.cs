using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region MonoBehaviours
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    #endregion


    #region Field & Method
    // Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "GameManger";
                    instance = container.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    
    // Settings

    #endregion

}
