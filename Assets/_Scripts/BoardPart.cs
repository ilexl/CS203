using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPart : MonoBehaviour
{
    // This class holds data only
    public Vector3 realPos;
    public DropHolder dropHolder;

    private void Awake()
    {
        dropHolder = GetComponent<DropHolder>();
    }
}
