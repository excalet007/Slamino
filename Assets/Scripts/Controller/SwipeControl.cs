using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Swipe Control
/// refered : https://www.youtube.com/watch?v=rDK_3qXHAFg
/// </summary>
public class SwipeControl : MonoBehaviour {

    public bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    public Vector2 startTouch, swipeDelta;

    public Vector2 SwipeDelta { get { return swipeDelta; } }
}
