using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject[] soundPrefabs;
    public void PlaySound(int soundIndex)
    {
        Instantiate(soundPrefabs[soundIndex], transform);
    }

    public void Start()
    {
        var names = FindObjectsOfType<Button>(includeInactive: true).ToList();
        foreach(Button b in names)
        {
            b.onClick.AddListener(() => PlaySound(0));
        }
    }
}
