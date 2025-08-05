using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{

    private int totalHeartsCollected = 0;
    private GameObject[] hiddenHearts;

    public GameObject HeartCollection;
    public TextMeshProUGUI textCounter;

  

    public enum heartManagerState {Idle, Active,  UnActive, UnActivePause };
    public heartManagerState currentHeartManagerState = heartManagerState.Idle;

    private int actionIndex = -1;

    bool returnToMenu = false;
    public Image panel; //Adding another ref here cause i am lazy

    // Start is called before the first frame update
    void Start()
    {
        var dataSet = Resources.Load<TextAsset>("WeddingHeart_CSV");
        var dataLines = dataSet.text.Split('\n');
        ProcessDataLines(dataLines);
       
        GameEvents.current.onHeartCollected += AddToTotalHearts;
        GameEvents.current.onDialogueBoxHidden += ActivateHeartContainers;
        GameEvents.current.onDialogueBoxShown += DeActivateHeartContainers;
        GameEvents.current.onTutorialActivate += DeActivateHeartContainers;
        GameEvents.current.onSetTutorialAction += SetTutorialAction;
        GameEvents.current.onCompleteTutorialAction += CompleteTutorialAction;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            TestAddHeart();
        }
    }

    void CheckManagerState()
    {
        switch(currentHeartManagerState)
        {
            case heartManagerState.Active:
                ActivateHeartContainers(null);
                break;

            case heartManagerState.UnActive:
                DeActivateHeartContainers(null);
                break;

            case heartManagerState.UnActivePause:

                break;

            case heartManagerState.Idle:

                break;
        }
    }

    void ActivateHeartContainers(Heart heart)
    {
        CheckIfMoreHeartsNeedActivating();
        GameObject[] activeHearts = GameObject.FindGameObjectsWithTag("HeartActive");
        GameObject[] completedHearts = GameObject.FindGameObjectsWithTag("HeartCompleted");
        for (var i = 0; i < activeHearts.Length; i++)
        {
            activeHearts[i].GetComponent<Heart>().TurnOnHeartCollider();
        }

        for (var i = 0; i < completedHearts.Length; i++)
        {
            
            completedHearts[i].GetComponent<Heart>().TurnOnHeartCollider();
        }

        currentHeartManagerState = heartManagerState.Idle;
    }

    void DeActivateHeartContainers() //Workaround
    {
        DeActivateHeartContainers(null);
    }

    void DeActivateHeartContainers(Heart heart)
    {
        GameObject[] activeHearts = GameObject.FindGameObjectsWithTag("HeartActive");
        GameObject[] completedHearts = GameObject.FindGameObjectsWithTag("HeartCompleted");

        for(var i = 0; i < activeHearts.Length; i++)
        {
            
            activeHearts[i].GetComponent<Heart>().TurnOffHeartCollider();
        }

        for (var i = 0; i < completedHearts.Length; i++)
        {
            completedHearts[i].GetComponent<Heart>().TurnOffHeartCollider();
        }

        currentHeartManagerState = heartManagerState.UnActivePause;
    }

    void ProcessDataLines(string[] dataLines)
    {
        int heartContainerCount = HeartCollection.transform.childCount;
        for(int i = 0; i < heartContainerCount; i++)
        {
            string currentLine = dataLines[i + 1];
            Heart currentHeart = HeartCollection.transform.GetChild(i).GetChild(0).GetComponent<Heart>();
            ProcessDataLine(currentLine, currentHeart);
        }
    }

    void ProcessDataLine(string dataLine, Heart heart)
    {
        List<string> processedData = new List<string>();
        string currentText = "";
        int cutoff = 2;

        for(var i = 0; i < dataLine.Length; i++)
        {
            if(cutoff != 0)
            {
                if (dataLine[i].Equals(','))
                {
                    processedData.Add(currentText);
                    cutoff -= 1;
                    currentText = "";
                }
                else
                {
                    currentText += dataLine[i];
                }
            }
            else
            {
                currentText += dataLine[i];
            }
        }
        processedData.Add(currentText);

        heart.ProcessHeartData(processedData);
    }

    void TestAddHeart()
    {
        AddToTotalHearts(1);
    }

    public void AddToTotalHearts(int amount)
    {
        totalHeartsCollected += amount;
        textCounter.text = totalHeartsCollected.ToString();
        _CheckIfMoreHeartsNeedActivating();
    }

    void _CheckIfMoreHeartsNeedActivating()
    {
        if (currentHeartManagerState != heartManagerState.UnActive && currentHeartManagerState != heartManagerState.UnActivePause)
        {
            CheckIfMoreHeartsNeedActivating();
        }
    }

    void CheckIfMoreHeartsNeedActivating()
    {
        hiddenHearts = GameObject.FindGameObjectsWithTag("HeartHidden");
        if (hiddenHearts != null && hiddenHearts.Length > 0)
        {
            for (int i = 0; i < hiddenHearts.Length; i++)
            {
                hiddenHearts[i].GetComponent<Heart>().CheckHeartActivationStatus(totalHeartsCollected);
            }
        }

    }

    void SetTutorialAction(int index)
    {
        actionIndex = index;
    }

    void CompleteTutorialAction()
    {
        
        if(actionIndex == 0)
        {
            AddToTotalHearts(1);
        }

        actionIndex = -1;
    }

    public void ReturnToMenu()
    {
        if(!returnToMenu)
        {
            returnToMenu = true;
            panel.DOColor(new Color(0, 0, 0, 1), 0.3f).OnComplete(_ReturnToMenu);

        }
    }

    void _ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
