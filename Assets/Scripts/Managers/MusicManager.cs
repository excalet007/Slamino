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

    // Music Clip lists
    public List<AudioClip> bgmList;
    public List<AudioClip> sfx_DropList;
    public List<AudioClip> sfx_PopList;
    public List<AudioClip> sfx_Score_TapList;
    public List<AudioClip> sfx_Score_EnterList;
    
    public void SetUp()
    {
        bgm = this.gameObject.AddComponent<AudioSource>();
        sfx_Drop = this.gameObject.AddComponent<AudioSource>();
        sfx_Pop = this.gameObject.AddComponent<AudioSource>();
        sfx_Score_Tap = this.gameObject.AddComponent<AudioSource>();
        sfx_Score_Enter = this.gameObject.AddComponent<AudioSource>();

        bgm.playOnAwake = false;
        sfx_Drop.playOnAwake = false;
        sfx_Pop.playOnAwake = false;
        sfx_Score_Tap.playOnAwake = false;
        sfx_Score_Enter.playOnAwake = false;

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

    public void Change_Volume(AudioSource target, float size)
    {
        target.volume = size;
    }

    public void Change_PopStartPoint(int index)
    {
        pop_StartPoint = index;
    }
}
