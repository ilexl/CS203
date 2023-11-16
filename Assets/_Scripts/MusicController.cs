using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> musicList;
    public AudioSource soundPlayer;
    public Button playButton;

    private bool isPlaying = false;
    private int currentMusicIndex = 0;

    private void Start()
    {
        playButton.onClick.AddListener(ToggleMusic);
        soundPlayer = GetComponent<AudioSource>();

        if (musicList.Count > 0)
        {
            soundPlayer.clip = musicList[currentMusicIndex];
        }
    }

    private void ToggleMusic()
    {
        if (isPlaying)
        {
            soundPlayer.Pause();
        }
        else
        {
            soundPlayer.Play();
        }

        isPlaying = !isPlaying;
    }

    public void ChangeMusic(int index)
    {
        if (index >= 0 && index < musicList.Count)
        {
            currentMusicIndex = index;
            soundPlayer.clip = musicList[currentMusicIndex];


            if (isPlaying)
            {
                soundPlayer.Stop();
                soundPlayer.Play();
            }
        }
    }
} // musicController.ChangeMusic(1); call specified music example

