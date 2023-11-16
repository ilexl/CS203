using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> musicList;
    public AudioSource _audio;

    private int currentMusicIndex = 0;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        if (musicList.Count > 0)
        {
            _audio.clip = musicList[currentMusicIndex];
            _audio.Play();
        }
    }

    public void ToggleMusic(Toggle toggle)
    {
        bool play = toggle.isOn;
        if (play)
        {
            _audio.Pause();
            _audio.volume = 0.0f;
        }
        else
        {
            _audio.Play();
            _audio.volume = 1.0f;
        }
    }
}

