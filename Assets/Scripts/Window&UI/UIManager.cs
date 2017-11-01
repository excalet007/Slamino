using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //Single Ton
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "StageManger";
                    instance = container.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    public GameObject GameOver;
    public Text TotalScore;
    public Text Credit;

    public GameObject pause;



    private GameState gs;
    public GameState GS
    {
        get { return gs; }
    }

    public void SetUp()
    {
       ;
    }

    public void Open_GameOver()
    {
        GameOver.SetActive(true);
    }

    public void Close_GameOver()
    {
        GameOver.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Stage");
    }

    public void Pause()
    {

    }

    public void UnPause()
    {

    }
}
