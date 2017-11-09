using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Layer : MonoBehaviour {

    public string Id;

    abstract public void SetUp(int mapSize_X, int mapSize_Y);
    abstract public void On(int Direction);
    abstract public void Off(int Direction);

}
