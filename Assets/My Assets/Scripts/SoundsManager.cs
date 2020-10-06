using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    static private SoundsManager _inst;
    static public SoundsManager Inst
    {
        get { return _inst; }
    }


    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundEffectSource;


    private void Awake()
    {
        _inst = this;
    }


    public void PlayMusic (string musicTitle, bool loop=false)
    {
        musicSource.clip = Resources.Load ("Sounds/Music/" + musicTitle) as AudioClip;
        musicSource.loop = loop;
        musicSource.Play();
    }


    public void PlaySound (string soundName)
    {
        soundEffectSource.clip = Resources.Load("Sounds/" + soundName) as AudioClip;
        soundEffectSource.loop = false;
        soundEffectSource.Play();
    }
}
