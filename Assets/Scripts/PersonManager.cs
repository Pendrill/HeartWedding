using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PersonManager : MonoBehaviour
{

    public GameObject personHolder1, persongHolder2, robotHolder, textContainer;
    public PersonData personData1, personData2;
    public Image Panel, coverPanel;


    private bool _character1Talking = false;
    private bool _character2Talking = false;
    private bool _robotIsTalking = false;
    // Start is called before the first frame update
    void Start()
    {
        //CreateCharacters();
        GameEvents.current.onGenerateNewCharacters += CreateCharacters;
        GameEvents.current.onActivateCharacters += ActivateCharacters;
        GameEvents.current.onDeActivateCharacters += DeactivateCharacters;
        GameEvents.current.onCharacterTalk += ActivateCharaterTalking;
        GameEvents.current.onStopCharacterTalk += StopCharactersFromTalking;
        GameEvents.current.onRobotActivate += ActivateRobot;

        TransitionToScreen();
        
    }

    void TransitionToScreen()
    {
        coverPanel.color = new Color(0, 0, 0, 1);
        coverPanel.DOColor(new Color(0, 0, 0, 0), 0.3f).SetDelay(1f).OnComplete(ActivateRobot);
    }

    // Update is called once per frame
    void Update()
    {
        TestCreateCharacters();
    }

    void TestCreateCharacters()
    {
        /*if(Input.GetKeyDown(KeyCode.R))
        {
            CreateCharacters();
        }
*/
        if(Input.GetKeyDown(KeyCode.T))
        {
            _character1Talking = !_character1Talking;
            if(_character1Talking)
            {
                AnimateCharacterTalking(personHolder1, Random.Range(-186, -58));
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            ActivateRobot();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            DeActivateRobot();
        }
    }

    void ActivateCharacters(Heart heart)
    {
        //CreateCharacters();
        BringCharactersIn();
        EnableCharacterPanel();
        ShowDialogueBox(heart);
    }

    void DeactivateCharacters(Heart heart)
    {
        MoveCharactersOut();
        MoveRobotOut();
        DisableCharacterPanel();
        HideDialogueBox();
    }

    
    void BringCharactersIn()
    {
        RectTransform p1Rect = personHolder1.GetComponent<RectTransform>();
        RectTransform p2Rect = persongHolder2.GetComponent<RectTransform>();
        p1Rect.DOAnchorPos(new Vector2(-700, -185), 0.3f).SetEase(Ease.OutBack);
        p2Rect.DOAnchorPos(new Vector2(700, -185), 0.3f).SetEase(Ease.OutBack);
    }

    void MoveCharactersOut()
    {
        RectTransform p1Rect = personHolder1.GetComponent<RectTransform>();
        RectTransform p2Rect = persongHolder2.GetComponent<RectTransform>();
        p1Rect.DOAnchorPos(new Vector2(-1400, -185), 0.3f).SetEase(Ease.InSine);
        p2Rect.DOAnchorPos(new Vector2(1400, -185), 0.3f).SetEase(Ease.InSine);
    }

    void ActivateCharaterTalking(string name)
    {
        if(name.Equals("<P1>"))
        {
            _character1Talking = true;

            AnimateCharacterTalking(personHolder1, Random.Range(-186, -58));
        }
        else if(name.Equals("<P2>"))
        {
            _character2Talking = true;
            AnimateCharacterTalking(persongHolder2, Random.Range(-186, -58));
        }
        else if(name.Equals("<Rob>"))
        {
            _robotIsTalking = true;
            AnimateCharacterTalking(robotHolder, Random.Range(-358, -284));
        }
    }

    void AnimateCharacterTalking(GameObject character, int distance)
    {
        RectTransform rectTrans = character.GetComponent<RectTransform>();
        Vector2 originalPos = rectTrans.anchoredPosition;

        Sequence charTalkSequence = DOTween.Sequence();
        charTalkSequence.Append(rectTrans.DOAnchorPos(new Vector2(rectTrans.anchoredPosition.x, distance), 0.15f));
        charTalkSequence.Append(rectTrans.DOAnchorPos(originalPos, 0.15f).OnComplete(() => _AnimateCharacterTalking()));
    }

    void _AnimateCharacterTalking()
    {
        if(_character1Talking)
        {
            AnimateCharacterTalking(personHolder1, Random.Range(-186, -58));
        }
        if(_character2Talking)
        {
            AnimateCharacterTalking(persongHolder2, Random.Range(-186, -58));
        }
        if(_robotIsTalking)
        {
            AnimateCharacterTalking(robotHolder, Random.Range(-358, -284));
        }
    }

    void StopCharactersFromTalking(string name)
    {
        _character1Talking = false;
        _character2Talking = false;
        _robotIsTalking = false;
    }

    void ActivateRobot()
    {
        BringRobotIn();
        EnableCharacterPanel();
        ShowDialogeBoxForTutorial();
    }

    void DeActivateRobot()
    {
        MoveRobotOut();
        /*DisableCharacterPanel();
        HideDialogueBox();*/
    }

    void BringRobotIn()
    {
        RectTransform robotRect = robotHolder.GetComponent<RectTransform>();
        robotRect.DOAnchorPos(new Vector2(-655, -358), 0.3f).SetEase(Ease.OutBack);

    }

    void MoveRobotOut()
    {
        RectTransform robotRect = robotHolder.GetComponent<RectTransform>();
        robotRect.DOAnchorPos(new Vector2(-1400, -358), 0.3f).SetEase(Ease.OutBack);
    }


    void ShowDialogueBox(Heart heart)
    {
        RectTransform dialRectTrans = textContainer.GetComponent<RectTransform>();
        dialRectTrans.DOAnchorPos(new Vector2(0, 155), 0.4f).SetEase(Ease.OutBack).SetDelay(0.2f).OnComplete(() => DialogueBoxShown(heart));
    }

    void HideDialogueBox()
    {
        RectTransform dialRectTrans = textContainer.GetComponent<RectTransform>();
        dialRectTrans.DOAnchorPos(new Vector2(0, -200), 0.4f).SetEase(Ease.InBack).OnComplete(() => DialogueBoxHidden());
    }

    void ShowDialogeBoxForTutorial()
    {
        RectTransform dialRectTrans = textContainer.GetComponent<RectTransform>();
        dialRectTrans.DOAnchorPos(new Vector2(300, 155), 0.4f).SetEase(Ease.OutBack).SetDelay(0.2f).OnComplete(() => DialogueBoxShownForTutorial());
    }


    void DialogueBoxShown(Heart heart)
    {
        GameEvents.current.DialogueBoxShown(heart); //TODO: need heart ref here.
    }

    void DialogueBoxHidden()
    {
        GameEvents.current.DialogueBoxHidden(null);
    }

    void DialogueBoxShownForTutorial()
    {
        GameEvents.current.ActivateTutorial();
    }

    void EnableCharacterPanel()
    {
        Panel.DOColor(new Color(0f, 0f, 0f, 0.95f), 0.2f);
    }

    void DisableCharacterPanel()
    {
        Panel.DOColor(new Color(1f, 1f, 1f, 0f), 0.6f);
    }

    /*
     * CHARACTER SETUP
     */
    void CreateCharacters()
    {
        CreateCharacter(personHolder1, personData1);
        CreateCharacter(persongHolder2, personData2);
    }

    void CreateCharacter(GameObject person, PersonData data)
    {
        _SetupSkin(person, data);
        _SetupFace(person, data);
        _SetupHair(person, data);
        _SetupClothes(person, data);
    }

    void _SetupSkin(GameObject person, PersonData data)
    {
        Color _color = GetNewColor(false);
        _HeadSkin(person, _color, data);
        _NeckSkin(person, _color, data);
        _ArmSkin(person, _color, data);
    }

    Color GetNewColor(bool random)
    {
        Color _newColor, tempColor;
        if(random)
        {
            Color[] colorArray1 = { new Color32(65, 132, 58, 255), new Color32(208, 236, 86, 255), new Color32(212, 114, 19, 255), new Color32(197, 62, 46, 255), new Color32( 255, 109, 148, 255 ), 
                new Color32(84, 18, 147, 255), new Color32(87, 111, 150, 255), new Color32(6, 148, 176, 255), new Color32(68,68,68,255), new Color32(255,255,255,255)};
            tempColor = colorArray1[Random.Range(0, colorArray1.Length - 1)];
        }
        else
        {
            Color[] colorArray2 = { new Color32(179, 157, 141, 255), new Color32(255,255,255,255), new Color32(118, 94, 76, 255), new Color32(238, 194, 163, 255)  };
            tempColor = colorArray2[Random.Range(0, colorArray2.Length -1)];
        }
        _newColor = tempColor;
        return _newColor;
    }

    void _HeadSkin(GameObject person, Color _color, PersonData data)
    {
        data.currentHead.color = _color;
    }

    void _NeckSkin(GameObject person, Color _color, PersonData data)
    {
        data.currentNeck.color = _color;
    }

    void _ArmSkin(GameObject person, Color _color, PersonData data)
    {
        data.currentArmRight.color = _color;
        data.currentArmLeft.color = _color;
    }

    void _SetupFace(GameObject person, PersonData data)
    {
        if(data.currentFace)
        {
            data.currentFace.color = new Color(1, 1, 1, 0);
        }
        int faceIndex = Random.Range(0, data.Faces.Length - 1);
        data.currentFace = data.Faces[faceIndex];
        data.currentFace.color = new Color(1, 1, 1, 1);
    }

    void _SetupClothes(GameObject person, PersonData data)
    {
        Color _color = GetNewColor(true);
        _SetupShirt(person, _color, data);
        _SetupSleeves(person, _color, data);
    }

    void _SetupShirt(GameObject person, Color _color, PersonData data)
    {
        if(data.currentShirt)
        {
            data.currentShirt.color = new Color(1, 1, 1, 1);
        }
        int shirtIndex = Random.Range(0, data.Shirt.Length - 1);
        data.currentShirt = data.Shirt[shirtIndex];
        data.currentShirt.color = _color;
    }

    void _SetupSleeves(GameObject person, Color _color, PersonData data)
    {
        if(data.currentSleeveLeft)
        {
            data.currentSleeveRight.color = new Color(1, 1, 1, 1);
            data.currentSleeveLeft.color = new Color(1, 1, 1, 1);
        }
        
        int sleevesIndex = Random.Range(0, data.SleevesLeft.Length - 1);
        data.currentSleeveLeft = data.SleevesLeft[sleevesIndex];
        data.currentSleeveRight = data.SleevesRight[sleevesIndex];
        data.currentSleeveLeft.color = _color;
        data.currentSleeveRight.color = _color;
    }

    void _SetupHair(GameObject person, PersonData data)
    {
        if(data.currentHair)
        {
            data.currentHair.color = new Color(1, 1, 1, 0);
        }    
        int hairIndex = Random.Range(0, data.Hair.Length - 1);
        data.currentHair = data.Hair[hairIndex];
        data.currentHair.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
