using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {

    #region MonoBehaviours
    /// <summary>
    /// SetUp all linked Layers
    /// </summary>
    void Awake()
    {
        foreach (Layer layer in layers)
            layer.SetUp(StageManager.Instance.MapX, StageManager.Instance.MapY);
    }

    #endregion

    #region Field
    //SingleTon
    private static LayerManager instance;
    public static LayerManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<LayerManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "LayerManager";
                    instance = container.AddComponent<LayerManager>();
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// Layer List, link through hierarchy window
    /// </summary>
    public List<Layer> layers = new List<Layer>();
    #endregion

    #region Method
    /// <summary>
    /// return Layer
    /// </summary>
    /// <param name="name"> </param>
    /// <returns></returns>
    public Layer Get_Layer(string id)
    {
        Layer window = layers.Find(x => x.Id == id);

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
