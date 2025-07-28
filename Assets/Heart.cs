using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public enum heartState {Idle, Clicked, FinalBeats, Completing, Complete};
    heartState currentHeartState = heartState.Idle;

    public float targetClickSize = 3f;
    public float clickSpeed = 0.3f;
    public float clicksNeeded = 10f;

    public Slider slider;

    private float currentClickRate = 0f;
    private float currentSliderValue = 0f;
    private int finalBeats = 3;
    // Start is called before the first frame update
    void Start()
    {
        //currentClickRate -= 1 / clicksNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        //Debug.Log("mouse is over the object");
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


        

    }

    void CheckHeartClicked()
    {
        
    }

    void HeartClicked()
    {
        GameEvents.current.HeartClicked(new Vector3(transform.position.x, transform.position.y, -10));
       
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
            HeartMaxReached();
        }
    }

    void HeartMaxReached()
    {
        //Do Final Animation
        Sequence heartPos = DOTween.Sequence();
        heartPos.Append(transform.DOMoveY(transform.localPosition.y + 3.5f, 0.4f).SetEase(Ease.InSine));
        heartPos.Append(transform.DOMoveY(transform.localPosition.y, 1f).SetDelay(0.1f).SetEase(Ease.InSine));
        
        transform.DORotate(new Vector3(0, 360, 0), 0.75f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart).SetEase(Ease.Linear);

        HideSlider();

        heartPos.OnComplete(SetHeartBeat);
    }

    void SetHeartBeat()
    {
        currentHeartState = heartState.Complete;
        Sequence heartBeat = DOTween.Sequence();
        heartBeat.Append(transform.DOScale(transform.localScale * 1.2f, 0.3f).SetEase(Ease.InSine));
        heartBeat.Append(transform.DOScale(transform.localScale , 01f).SetEase(Ease.InSine));
        heartBeat.SetLoops(-1, LoopType.Restart);

    }

    void HideSlider()
    {
        GameEvents.current.HideSlider(this.GetComponent<Heart>());
    }

    void ShowSlider()
    {
        Debug.Log("getting called");
        GameEvents.current.ShowSlider(this.GetComponent<Heart>());
        //slider.gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(160, 20), 0.8f);

    }

    void IncreaseSlider()
    {
        currentSliderValue += 1 / (clicksNeeded + finalBeats);
        GameEvents.current.IncreaseHeartSlider(this.GetComponent<Heart>(), currentSliderValue); 
    }

}
