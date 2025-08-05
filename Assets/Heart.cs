using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int index = -1;
    public int heartsNeeded = -1;
    public string rawText = "No Text Has Been Defined";
    public GameObject heartParticle;
    public enum heartState {Wait, Born, BornWait, Idle, Clicked, FinalBeats, Completing, Complete, ExtraClick};
    public heartState currentHeartState = heartState.Wait;

    public float targetClickSize = 2f;
    public float clickSpeed = 0.3f;
    public float clicksNeeded = 10f;

    public Slider slider;

    private float currentClickRate = 0f;
    private float currentSliderValue = 0f;
    private int finalBeats = 3;
    private Vector3 endScale;

    public bool requiresTutorial = false;
    public AudioSource heartAudio;
    public AudioClip grow, complete, extraClick;


    // Start is called before the first frame update
    void Start()
    {

        if(heartsNeeded == -1)
        {
            Debug.LogError("Hearts Needed set to -1");
        }

        transform.localScale = new Vector2(0, 0);
        //currentClickRate -= 1 / clicksNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeartStatus();
    }

    private void OnMouseOver()
    {
        //Debug.Log("mouse is over the object");
    }

    public void ProcessHeartData(List<string> data)
    {
        index = int.Parse(data[0]);
        heartsNeeded = int.Parse(data[1]);
        rawText = data[2];
    }

    public void CheckHeartActivationStatus(int currentHearts)
    {
        if(currentHearts >= heartsNeeded)
        {
            gameObject.tag = "HeartActive";
            currentHeartState = heartState.Born;
        }

    }
    private void CheckHeartStatus()
    {
        if(currentHeartState == heartState.Born)
        {
            currentHeartState = heartState.BornWait;
            ShowInitialHeart();
        }
    }

    private void OnMouseDown()
    {
        if (currentHeartState == heartState.Idle)
        {
            ShowSlider();
        }
        if (currentHeartState == heartState.Idle || currentHeartState == heartState.Clicked)
        {
            HeartClicked();
        }
        else if(currentHeartState == heartState.FinalBeats)
        {
            HeartFinalBeats();
        }
        else if(currentHeartState == heartState.Complete)
        {
            HeartExtraClick();
        }


        

    }

    void CheckHeartClicked()
    {
        
    }

    void HeartClicked()
    {
        GameEvents.current.HeartClicked(new Vector3(transform.position.x, transform.position.y, -10));
        CreateParticle(Random.Range(1,3));
        PlayHeartSound(grow, 1);
       
        currentHeartState = currentClickRate < 1 ? heartState.Clicked : heartState.FinalBeats;
        currentClickRate += 1 / clicksNeeded;
        currentHeartState = currentClickRate < 1 ? heartState.Clicked : heartState.FinalBeats;
        IncreaseSlider();
        Vector3 targetScale = new Vector3(targetClickSize, targetClickSize, targetClickSize) * Mathf.Cos(Mathf.Min(currentClickRate, 1) - 1);

        Sequence heartClickSequence = DOTween.Sequence();
        heartClickSequence.Append(transform.DOScale(targetScale, (1 - currentClickRate) / 3));
        heartClickSequence.Append(transform.DOScale(targetScale * 0.9f, Mathf.Max((1 - currentClickRate) / 3f, 0.1f)));

       
    }

    void HeartFinalBeats()
    {
        GameEvents.current.HeartClicked(new Vector3(transform.position.x, transform.position.y, -10));
        CreateParticle(Random.Range(4, 10));
        PlayHeartSound(grow, 1);
        finalBeats -= 1;
        currentHeartState = finalBeats != 0 ? heartState.FinalBeats : heartState.Completing;
        Vector3 targetScale = transform.localScale * 1.1f;
        IncreaseSlider();
        Sequence heartClickSequence = DOTween.Sequence();
        heartClickSequence.Append(transform.DOScale(targetScale, 0.05f));
        heartClickSequence.Append(transform.DOScale(targetScale * 0.9f, 0.05f).OnComplete(checkHeartReachedMax));
    }

    void checkHeartReachedMax()
    {
        if(currentHeartState == heartState.Completing)
        {
            endScale = transform.localScale;
            HeartMaxReached();
        }
    }

    void HeartMaxReached()
    {
        gameObject.tag = "HeartCompleted";
        //Do Final Animation
        PlayHeartSound(complete, 1);
        Sequence heartPos = DOTween.Sequence();
        heartPos.Append(transform.DOLocalMoveY(transform.localPosition.y + 1.85f, 0.4f).SetEase(Ease.InSine));
        heartPos.Append(transform.DOLocalMoveY(transform.localPosition.y, 2f).SetDelay(0.5f).SetEase(Ease.InSine));
        
        transform.DORotate(new Vector3(0, 360, 0), 1.45f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart).SetEase(Ease.Linear);

        HideSlider();
        HideShadow();

        GameEvents.current.GenerateNewCharacters();

        heartPos.OnComplete(_SetHeartBeat);
    }

    void _SetHeartBeat()
    {
        if(requiresTutorial)
        {
            GameEvents.current.ActivateRobot();
        }
        else
        {
            GameEvents.current.ActivateCharacters(this);
        }
        SetHeartBeat();
    }

    void SetHeartBeat()
    {
        currentHeartState = heartState.Complete;
        Sequence heartBeat = DOTween.Sequence();
        heartBeat.Append(transform.DOScale(endScale * 1.2f, 0.3f).SetEase(Ease.InSine).OnComplete(() => PlayHeartSound(grow, 1)));
        heartBeat.Append(transform.DOScale(endScale , 1f).SetEase(Ease.InSine).OnComplete(() => CreateParticle(1)));
        string heartBeatName = "heartBeat" + index.ToString();
        heartBeat.SetLoops(-1, LoopType.Restart).SetId(heartBeatName);
        ShowShadow();

    }

    void HeartExtraClick()
    {
        string heartBeatName = "heartBeat" + index.ToString();
        PlayHeartSound(extraClick, 0.5f);
        DOTween.Kill(heartBeatName, false);
        currentHeartState = heartState.ExtraClick;

        CreateParticle(5);
        Sequence extraBeat = DOTween.Sequence();
        extraBeat.Append(transform.DOScale(transform.localScale * 0.6f, 0.1f).SetEase(Ease.InSine));
        extraBeat.Append(transform.DOScale(endScale, 0.1f).SetEase(Ease.InSine).OnComplete(() => HeartExtraClickDone()).SetId("extraHeartClick"));

    }

    void HeartExtraClickDone()
    {
        SetHeartBeat();
        //CreateParticle(5);
    }

    void HideSlider()
    {
        GameEvents.current.HideSlider(this.GetComponent<Heart>());
    }

    void ShowSlider()
    {
        GameEvents.current.ShowSlider(this.GetComponent<Heart>());
        //slider.gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(160, 20), 0.8f);

    }

    void IncreaseSlider()
    {
        currentSliderValue += 1 / (clicksNeeded + finalBeats);
        Debug.Log(currentSliderValue);
        GameEvents.current.IncreaseHeartSlider(this.GetComponent<Heart>(), currentSliderValue); 
    }

    void HideShadow()
    {
        GameObject shadow = transform.GetChild(1).gameObject;
        shadow.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), 0.3f);
    }

    void ShowShadow()
    {
        GameObject shadow = transform.GetChild(1).gameObject;
        shadow.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0.7f), 0.1f);
    }

    void ShowInitialHeart()
    {
        transform.DOScale(new Vector3(.95f, .95f, .95f), .3f).SetEase(Ease.OutBack).OnComplete(ShowInitialHeartComplete);
       
    }

    void ShowInitialHeartComplete()
    {
        currentHeartState = heartState.Idle;
        TurnOnHeartCollider();
    }

    public void TurnOnHeartCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void TurnOffHeartCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }


    void CreateParticle(int heartParticleCap)
    {
        for(int i = 0; i < heartParticleCap; i++)
        {
            Instantiate(heartParticle, transform.parent.transform);
        }
        
    }

    void PlayHeartSound(AudioClip clip, float volume)
    {
        heartAudio.clip = clip;
        heartAudio.volume = volume;
        heartAudio.Play();
    }

}
