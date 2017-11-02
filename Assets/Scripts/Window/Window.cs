using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Window : MonoBehaviour {

    public string Id;

    abstract public void On();
    abstract public void Off();
    abstract public void SetUp();

}
