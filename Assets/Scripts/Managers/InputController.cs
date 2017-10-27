using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    #region MonoBehaviours

    void Awake()
    {
        sm = StageManager.Instance;
    }

    void Update()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
        
        if(Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                switch(touch.phase)
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
    // SingleTon 
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
                    container.name = "StageManger";
                    instance = container.AddComponent<InputController>();
                }
            }
            return instance;
        }
    }

    private StageManager sm;

    // Toucing Information
    Vector2 startPos; 
    Vector2 lastPos;
    
    float touchStartTime = 0.0f;
    float minSwipeDist = 50.0f;
    #endregion

}
