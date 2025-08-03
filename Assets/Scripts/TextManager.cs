using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public enum TextManagerState { Wait, Idle, Typing, Completing, Complete };
    public TextManagerState currentHeartState = TextManagerState.Wait;


    private string tempText = "Hello there! This is some temporary text meant to help gage the animations and the spacing of the text! :)";
    private int currentLineIndex = 0;

    public Febucci.UI.Core.TAnimCore textAnimator;
    public Febucci.UI.Core.TypewriterCore typewriter;
    private GameObject textObject;
    private TextMeshProUGUI tmpElement;


    private List<string> characterOrder, lineOrder;
    // Start is called before the first frame update
    void Start()
    {
        textObject = transform.GetChild(0).gameObject;
        tmpElement = textObject.GetComponent<TextMeshProUGUI>();
        GameEvents.current.onDialogueBoxShown += DialogueBoxIsShown;
        GameEvents.current.onDialogueBoxHidden += DialogueBoxIsHidden;


        typewriter.onTextShowed.AddListener(WaitingForUserClick);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHeartState != TextManagerState.Wait)
        {
            CheckTextManagerStates();
        }
    }

    void CheckTextManagerStates()
    {
        switch (currentHeartState)
        {
            case TextManagerState.Idle:
                if(Input.GetMouseButtonDown(0))
                {
                    ShowNextLineOfText();
                }
                break;
            case TextManagerState.Typing:
                if (Input.GetMouseButtonDown(0))
                {
                    CompleteTheLineOfText();
                }
                break;
            case TextManagerState.Completing:

                break;
            case TextManagerState.Complete:

                break;
        }
    }

    void DialogueBoxIsShown(Heart heart)
    {
        ParseLine(heart.rawText);
        currentLineIndex = 0;
        ShowNextLineOfText();
    }

    void ParseLine(string dataLine)
    {
        characterOrder = new List<string>();
        lineOrder = new List<string>();

        string currentText = "<";
        bool checkingCharacterIdentifier = true;
        for(var i = 2; i < dataLine.Length - 2; i++ ) //Ignore initial and final quotes in string
        {
            if (checkingCharacterIdentifier)
            {
                currentText += dataLine[i];
                if (dataLine[i].Equals('>'))
                {
                    characterOrder.Add(currentText);
                    checkingCharacterIdentifier = false;
                    currentText = "";
                }
            }
            else
            {
                if (dataLine[i].Equals('<'))
                {
                    lineOrder.Add(currentText);
                    currentText = "<";
                    checkingCharacterIdentifier = true;
                }
                else
                {
                    currentText += dataLine[i];
                }
            }
        }

        lineOrder.Add(currentText);

    }

    void ShowNextLineOfText()
    {

        if(currentLineIndex < lineOrder.Count)
        {
            GameEvents.current.CharacterTalk(characterOrder[currentLineIndex]);
            currentHeartState = TextManagerState.Typing;
            typewriter.ShowText(lineOrder[currentLineIndex]);
            
        }
        else
        {
            ClickedThroughAllText();
        }
       
    }

    void CompleteTheLineOfText()
    {
        textAnimator.SetText(lineOrder[currentLineIndex]);
        //WaitingForUserClick();
    }

    void WaitingForUserClick()
    {
        GameEvents.current.StopCharacterTalk(null);
        currentLineIndex += 1;
        currentHeartState = TextManagerState.Idle;
    }

    void ClickedThroughAllText()
    {
        currentHeartState = TextManagerState.Completing;
        ResetText();
        GameEvents.current.DeActivateCharacters(null);

        //Need to hide the text / banner
    }


    void UpdateDialogueText(string text)
    {
        if(text == null)
        {
            typewriter.ShowText(tempText);
        }
        else
        {
            typewriter.ShowText(text);
        }
    }

    void DialogueBoxIsHidden(Heart heart)
    {
        ResetText();
    }

    void ResetText()
    {
        textAnimator.SetText("");
    }
}
