using UnityEngine;

public class CameraController : MonoBehaviour {

    //SingleTon
    private static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraController>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "CameraController";
                    instance = container.AddComponent<CameraController>();
                }
            }
            return instance;
        }
    }


    void SetUp()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
     
    void Awake()
    {
        SetUp();
    }
}
    