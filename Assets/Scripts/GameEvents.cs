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
    public event Action<int> onHeartCollected;
    public event Action<Heart> onActivateCharacters;
    public event Action<Heart> onDeActivateCharacters;
    public event Action<Heart> onDialogueBoxShown;
    public event Action<Heart> onDialogueBoxHidden;
    public event Action<string> onCharacterTalk;
    public event Action<string> onStopCharacterTalk;




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

    public void HeartCollected(int heartValue)
    {
        if(onHeartCollected != null)
        {
            onHeartCollected(heartValue);
        }
    }

    public void ActivateCharacters(Heart heart)
    {
        if(onActivateCharacters != null)
        {
            onActivateCharacters(heart);
        }
    }

    public void DeActivateCharacters(Heart heart)
    {
        if(onDeActivateCharacters != null)
        {
            onDeActivateCharacters(heart);
        }
    }

    public void DialogueBoxShown(Heart heart)
    {
        if(onDialogueBoxShown != null)
        {
            onDialogueBoxShown(heart);
        }
    }

    public void DialogueBoxHidden(Heart heart)
    {
        if(onDialogueBoxHidden != null)
        {
            onDialogueBoxHidden(heart);
        }
    }

    public void CharacterTalk(string name)
    {
        if(onCharacterTalk != null)
        {
            onCharacterTalk(name);
        }
    }

    public void StopCharacterTalk(string name)
    {
        if(onStopCharacterTalk != null)
        {
            onStopCharacterTalk(name);
        }
    }
}
