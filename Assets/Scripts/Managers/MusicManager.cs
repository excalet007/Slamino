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
    int pop_CurPoint;

    // Speakers
    AudioSource bgm;
    AudioSource sfx_Drop;
    AudioSource sfx_Pop;
    AudioSource sfx_Score_Tap;
    AudioSource sfx_Score_Enter;
    AudioSource sfx_Projector;
    AudioSource sfx_SpotLight;
    AudioSource sfx_Cheer;
    AudioSource sfx_Scratch;
    AudioSource sfx_Swipe;

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
    public AudioSource Sfx_Score_Tap
    {
        get { return sfx_Score_Tap; }
    }
    public AudioSource Sfx_Score_Enter
    {
        get { return sfx_Score_Enter; }
    }
    public AudioSource Sfx_Projector
    { get { return sfx_Projector; } }
    public AudioSource Sfx_SpotLight
    { get { return sfx_SpotLight; } }
    public AudioSource Sfx_Cheer
    { get { return sfx_Cheer; } }
    public AudioSource Sfx_Scratch
    { get { return sfx_Scratch; } }
    public AudioSource Sfx_Swipe
    { get { return sfx_Swipe; } }


    // Music Clip lists
    public List<AudioClip> bgmList;
    public List<AudioClip> sfx_DropList;
    public List<AudioClip> sfx_PopList;
    public List<AudioClip> sfx_Score_TapList;
    public List<AudioClip> sfx_Score_EnterList;
    public List<AudioClip> sfx_ProjectorList;
    public List<AudioClip> sfx_SpotLightList;
    public List<AudioClip> sfx_CheerList;
    public List<AudioClip> sfx_ScratchList;
    public List<AudioClip> sfx_SwipeList;


    public void SetUp()
    {
        bgm = this.gameObject.AddComponent<AudioSource>();
        sfx_Drop = this.gameObject.AddComponent<AudioSource>();
        sfx_Pop = this.gameObject.AddComponent<AudioSource>();
        sfx_Score_Tap = this.gameObject.AddComponent<AudioSource>();
        sfx_Score_Enter = this.gameObject.AddComponent<AudioSource>();
        sfx_Projector = this.gameObject.AddComponent<AudioSource>();
        sfx_SpotLight = this.gameObject.AddComponent<AudioSource>();
        sfx_Cheer = this.gameObject.AddComponent<AudioSource>();
        sfx_Scratch = this.gameObject.AddComponent<AudioSource>();
        sfx_Swipe = this.gameObject.AddComponent<AudioSource>(); 

        bgm.playOnAwake = false;
        sfx_Drop.playOnAwake = false;
        sfx_Pop.playOnAwake = false;
        sfx_Score_Tap.playOnAwake = false;
        sfx_Score_Enter.playOnAwake = false;
        sfx_Projector.playOnAwake = false;
        sfx_SpotLight.playOnAwake = false; 
        sfx_Cheer.playOnAwake = false;
        sfx_Scratch.playOnAwake = false;
        sfx_Swipe.playOnAwake = false;

        bgm.loop = true;
       
        bgm.clip = bgmList[0];
        sfx_Drop.clip = sfx_DropList[0];
        sfx_Pop.clip = sfx_PopList[0];
        sfx_Score_Tap.clip = sfx_Score_TapList[0];
        sfx_Score_Enter.clip = sfx_Score_EnterList[1];
    }

    public void Play_BGM()
    {
        bgm.Play();
    }

    public void Play_Drop()
    {
        sfx_Drop.Play();
    }
    
    public void Play_Pop()
    {
        sfx_Pop.Play();
    }

    public void Play_Pop(int combo)
    {
        if (pop_StartPoint + combo < sfx_PopList.Count)
            sfx_Pop.clip = sfx_PopList[pop_StartPoint + combo];
        else
            sfx_Pop.clip = sfx_PopList[sfx_PopList.Count-1];
        sfx_Pop.Play();
    }

    public void Play_Pop_Continuous()
    {
        if ((pop_StartPoint + pop_CurPoint) > sfx_PopList.Count)
            pop_CurPoint = 0;

        sfx_Pop.clip = sfx_PopList[pop_StartPoint + pop_CurPoint];
        pop_CurPoint++;

        sfx_Pop.Play();

    }

    public void Reset_Pop_Continuous()
    {
        pop_CurPoint = 0;
    }

    public void Play_Score_Tap()
    {
        //int index = (int)UnityEngine.Random.Range(0, sfx_Score_TapList.Count + 1 - Mathf.Epsilon);
        //sfx_Score_Tap.clip = sfx_Score_TapList[index];
        
        sfx_Score_Tap.Play();
    }

    public void Play_Score_Enter()
    {
        sfx_Score_Enter.Play();
    }

    /// <summary>
    /// 0 = on, 1 = loop
    /// </summary>
    /// <param name="num"></param>
    public void Play_Projector(int index)
    {
        if (sfx_Projector.isPlaying)
            sfx_Projector.Stop();

        sfx_Projector.clip = sfx_ProjectorList[index];
        sfx_Projector.Play();
    }

    /// <summary>
    /// 0 = on, 1 = off
    /// </summary>
    /// <param name="index"></param>
    public void Play_SpotLight(int index)
    {
        sfx_SpotLight.clip = sfx_SpotLightList[index];
        sfx_SpotLight.Play();
    }

    public void Play_Cheer(int index)
    {
        if (sfx_Cheer.isPlaying)
            sfx_Cheer.Stop();

        sfx_Cheer.clip = sfx_CheerList[index];
        sfx_Cheer.Play();
    }

    public void Play_Scratch(int index)
    {
        sfx_Scratch.clip = sfx_ScratchList[index];
        sfx_Scratch.Play();
    }

    public void Play_Swipe(int index)
    {
        sfx_Swipe.clip = sfx_SwipeList[index];
        sfx_Swipe.Play();
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
