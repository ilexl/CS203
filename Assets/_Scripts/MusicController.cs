using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> musicList;
    public AudioSource audio;

    private int currentMusicIndex = 0;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        if (musicList.Count > 0)
        {
            audio.clip = musicList[currentMusicIndex];
            audio.Play();
        }
    }

    public void ToggleMusic(Toggle toggle)
    {
        bool play = toggle.isOn;
        if (play)
        {
            audio.Pause();
            audio.volume = 0.0f;
        }
        else
        {
            audio.Play();
            audio.volume = 1.0f;
        }
    }
}

