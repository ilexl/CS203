using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject[] soundPrefabs;
    public void PlaySound(int soundIndex)
    {
        Instantiate(soundPrefabs[soundIndex], transform);
    }
}
