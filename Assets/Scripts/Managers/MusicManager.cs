using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    
    // SingleTon
    private static MusicManager instance;
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "StageManger";
                    instance = container.AddComponent<MusicManager>();
                }
            }
            return instance;
        }
    }

    // Setting Variable
    int pop_StartPoint;

    // Speakers
    AudioSource bgm;
    AudioSource sfx_Drop;
    AudioSource sfx_Pop;
    public AudioSource Bgm
    {
        get { return bgm; }
    }
    public AudioSource Sfx_Drop
    {
        get { return sfx_Drop; }
    }
    public AudioSource Sfx_Pop
    {
        get { return sfx_Pop; }
    }
            
    // Music Clip lists
    public List<AudioClip> bgmList;
    public List<AudioClip> sfx_DropList;
    public List<AudioClip> sfx_PopList;
    
    public void SetUp()
    {
        bgm = this.gameObject.AddComponent<AudioSource>();
        sfx_Drop = this.gameObject.AddComponent<AudioSource>(); 
        sfx_Pop = this.gameObject.AddComponent<AudioSource>();

        bgm.playOnAwake = false;
        sfx_Drop.playOnAwake = false;
        sfx_Pop.playOnAwake = false;

        bgm.clip = bgmList[0];
        sfx_Drop.clip = sfx_DropList[0];
        sfx_Pop.clip = sfx_PopList[0];
    }

    public void Play_BGM()
    {
        bgm.Play();
    }

    public void Play_Drop()
    {
        sfx_Drop.Play();
    }

    public void Play_Pop(int combo)
    {
        if (pop_StartPoint + combo < sfx_PopList.Count)
            sfx_Pop.clip = sfx_PopList[pop_StartPoint + combo];
        else
            Sfx_Pop.clip = sfx_PopList[sfx_PopList.Count-1];
        Sfx_Pop.Play();
    }

    public void Change_Volume(AudioSource target, float size)
    {
        target.volume = size;
    }

    public void Change_PopStartPoint(int index)
    {
        pop_StartPoint = index;
    }
}
