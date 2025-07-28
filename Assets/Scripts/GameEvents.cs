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
    public event Action<Heart, float> onIncreaseHeartSlider;
    public event Action<Heart> onShowSlider;
    public event Action<Heart> onHideSlider;



    public void HeartClicked(Vector3 newPos)
    {
        if(onHeartClicked !=null)
        {
            onHeartClicked(newPos);
        }
    }

    public void IncreaseHeartSlider(Heart heart, float sliderValue)
    {
        if(onIncreaseHeartSlider != null)
        {
            onIncreaseHeartSlider(heart, sliderValue) ;
        }
    }

    public void ShowSlider(Heart heart)
    {
        if (onShowSlider != null)
        {
            onShowSlider(heart);
        }
    }

    public void HideSlider(Heart heart)
    {
        if (onHideSlider != null)
        {
            onHideSlider(heart);
        }
    }
}
