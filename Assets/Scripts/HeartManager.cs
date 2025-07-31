using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartManager : MonoBehaviour
{

    private int totalHeartsCollected = 0;
    private GameObject[] hiddenHearts;

    public TextMeshProUGUI textCounter;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onHeartCollected += AddToTotalHearts;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            TestAddHeart();
        }
    }

    void TestAddHeart()
    {
        AddToTotalHearts(1);
    }

    public void AddToTotalHearts(int amount)
    {
        totalHeartsCollected += amount;
        textCounter.text = totalHeartsCollected.ToString();
        CheckIfMoreHeartsNeedActivating();
    }

    void CheckIfMoreHeartsNeedActivating()
    {
        hiddenHearts = GameObject.FindGameObjectsWithTag("HeartHidden");
        if(hiddenHearts != null && hiddenHearts.Length > 0)
        {
            for (int i = 0; i < hiddenHearts.Length; i++)
            {
                hiddenHearts[i].GetComponent<Heart>().CheckHeartActivationStatus(totalHeartsCollected);
            }
        }
        
    }
}
