using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<Vector3> onHeartClicked; 

    public void HeartClicked(Vector3 newPos)
    {
        if(onHeartClicked !=null)
        {
            onHeartClicked(newPos);
        }
    }
}
