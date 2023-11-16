using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject obj in gameObjects)
        {
            if(obj.activeSelf == false)
            {
                obj.SetActive(true);
                obj.SetActive(false);
            }
        }
    }
}
