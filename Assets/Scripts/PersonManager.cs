using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PersonManager : MonoBehaviour
{

    public GameObject personHolder1, persongHolder2;
    public Image Panel;


    private bool _character1Talking = false;
    private bool _character2Talking = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateCharacters();
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
                AnimateCharacterTalking(personHolder1);
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            ActivateCharacters();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            DeactivateCharacters();
        }
    }

    void ActivateCharacters()
    {
        CreateCharacters();
        BringCharactersIn();
        EnableCharacterPanel();
    }

    void DeactivateCharacters()
    {
        MoveCharactersOut();
        DisableCharacterPanel();
    }

    void BringCharactersIn()
    {
        RectTransform p1Rect = personHolder1.GetComponent<RectTransform>();
        RectTransform p2Rect = persongHolder2.GetComponent<RectTransform>();
        p1Rect.DOAnchorPos(new Vector2(-600, -185), 0.3f).SetEase(Ease.OutBack);
        p2Rect.DOAnchorPos(new Vector2(600, -185), 0.3f).SetEase(Ease.OutBack);
    }

    void MoveCharactersOut()
    {
        RectTransform p1Rect = personHolder1.GetComponent<RectTransform>();
        RectTransform p2Rect = persongHolder2.GetComponent<RectTransform>();
        p1Rect.DOAnchorPos(new Vector2(-1400, -185), 0.3f).SetEase(Ease.InSine);
        p2Rect.DOAnchorPos(new Vector2(1400, -185), 0.3f).SetEase(Ease.InSine);
    }

    void AnimateCharacterTalking(GameObject character)
    {
        RectTransform rectTrans = character.GetComponent<RectTransform>();
        Vector2 originalPos = rectTrans.anchoredPosition;

        Sequence charTalkSequence = DOTween.Sequence();
        charTalkSequence.Append(rectTrans.DOAnchorPos(new Vector2(rectTrans.anchoredPosition.x, Random.Range(-186, -58)), 0.15f));
        charTalkSequence.Append(rectTrans.DOAnchorPos(originalPos, 0.15f).OnComplete(() => _AnimateCharacterTalking()));
    }

    void _AnimateCharacterTalking()
    {
        if(_character1Talking)
        {
            AnimateCharacterTalking(personHolder1);
        }
        if(_character2Talking)
        {
            AnimateCharacterTalking(persongHolder2);
        }
    }




    void EnableCharacterPanel()
    {
        Panel.DOColor(new Color(0f, 0f, 0f, 0.95f), 0.6f);
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
        CreateCharacter(personHolder1);
        CreateCharacter(persongHolder2);
    }

    void CreateCharacter(GameObject person)
    {
        _SetupSkin(person);
        _SetupFace(person);
        _SetupHair(person);
        _SetupClothes(person);
    }

    void _SetupSkin(GameObject person)
    {
        Color _color = GetNewColor(false);
        _HeadSkin(person, _color);
        _NeckSkin(person, _color);
        _ArmSkin(person, _color);
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

    void _HeadSkin(GameObject person, Color _color)
    {
        GameObject head = person.transform.GetChild(1).gameObject;
        head.transform.GetChild(0).GetComponent<Image>().color = _color;
    }

    void _NeckSkin(GameObject person, Color _color)
    {
        GameObject neck = person.transform.GetChild(0).gameObject;
        neck.transform.GetChild(0).GetComponent<Image>().color = _color;
    }

    void _ArmSkin(GameObject person, Color _color)
    {
        GameObject arms = person.transform.GetChild(3).GetChild(0).gameObject;
        GameObject armR = arms.transform.GetChild(0).gameObject;
        GameObject armL = arms.transform.GetChild(1).gameObject;

        armR.GetComponent<Image>().color = _color;
        armL.GetComponent<Image>().color = _color;
    }

    void _SetupFace(GameObject person)
    {
        GameObject mainFace = person.transform.GetChild(2).gameObject;
        int faceIndex = Random.Range(0, mainFace.transform.childCount - 1);

        for(var i = 0; i < mainFace.transform.childCount; i++)
        {
            GameObject tempFace = mainFace.transform.GetChild(i).gameObject;
            if(tempFace.activeSelf)
            {
                tempFace.SetActive(false);
            }

            if(faceIndex == i)
            {
                tempFace.SetActive(true);
            }
        }
    }

    void _SetupClothes(GameObject person)
    {
        Color _color = GetNewColor(true);
        _SetupShirt(person, _color);
        _SetupSleeves(person, _color);
    }

    void _SetupShirt(GameObject person, Color _color)
    {
        GameObject mainShirt = person.transform.GetChild(5).gameObject;
        int shirtIndex = Random.Range(0, mainShirt.transform.childCount - 1);

        for( var i = 0; i < mainShirt.transform.childCount; i++)
        {
            GameObject tempShirt = mainShirt.transform.GetChild(i).gameObject;
            if (tempShirt.activeSelf)
            {
                tempShirt.SetActive(false);
            }

            if (shirtIndex == i)
            {
                tempShirt.GetComponent<Image>().color = _color;
                tempShirt.SetActive(true);
            }
        }
    }

    void _SetupSleeves(GameObject person, Color _color)
    {
        GameObject mainSleeves = person.transform.GetChild(4).gameObject;
        int sleevesIndex = Random.Range(0, mainSleeves.transform.childCount - 1);

        for (var i = 0; i < mainSleeves.transform.childCount; i++)
        {
            GameObject tempSleeves = mainSleeves.transform.GetChild(i).gameObject;
            if (tempSleeves.activeSelf)
            {
                tempSleeves.SetActive(false);
            }

            if (sleevesIndex == i)
            {
                GameObject tempSleeveR = tempSleeves.transform.GetChild(0).gameObject;
                GameObject tempSleeveL = tempSleeves.transform.GetChild(1).gameObject;
                tempSleeveL.GetComponent<Image>().color = _color;
                tempSleeveR.GetComponent<Image>().color = _color;
                tempSleeves.SetActive(true);
            }
        }
    }

    void _SetupHair(GameObject person)
    {
        GameObject mainHair = person.transform.GetChild(6).gameObject;
        int hairIndex = Random.Range(0, mainHair.transform.childCount - 1);

        for (var i = 0; i < mainHair.transform.childCount; i++)
        {
            GameObject tempHair = mainHair.transform.GetChild(i).gameObject;
            if (tempHair.activeSelf)
            {
                tempHair.SetActive(false);
            }

            if (hairIndex == i)
            {
             
                tempHair.GetComponent<Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255) ;
                tempHair.SetActive(true);
            }
        }
    }
}
