using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Heart : MonoBehaviour
{
    public enum heartState {Idle, Clicked, FinalBeats, Completing, Complete};
    heartState currentHeartState = heartState.Idle;

    public float targetClickSize = 3f;
    public float clickSpeed = 0.3f;
    public float clicksNeeded = 10f;

    private float currentClickRate = 0;
    private int finalBeats = 3;
    // Start is called before the first frame update
    void Start()
    {
        currentClickRate -= 1 / clicksNeeded;
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
        if(currentHeartState == heartState.Idle || currentHeartState == heartState.Clicked)
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
        currentHeartState =currentClickRate < 1 ?  heartState.Clicked : heartState.FinalBeats;
        //DOTween.KillAll();
        currentClickRate += 1 / clicksNeeded;
        currentHeartState =currentClickRate < 1 ?  heartState.Clicked : heartState.FinalBeats;
        Vector3 targetScale = new Vector3(targetClickSize, targetClickSize, targetClickSize) * Mathf.Cos(Mathf.Min(currentClickRate, 1) - 1);
 
        Sequence heartClickSequence = DOTween.Sequence();
        heartClickSequence.Append(transform.DOScale(targetScale, (1 - currentClickRate) / 3));
        heartClickSequence.Append(transform.DOScale(targetScale * 0.9f, Mathf.Max((1 - currentClickRate) / 3f, 0.1f)));        
    }

    void HeartFinalBeats()
    {
        Debug.Log("Here");
        finalBeats -= 1;
        currentHeartState = finalBeats != 0 ? heartState.FinalBeats : heartState.Completing;
        Vector3 targetScale = transform.localScale * 1.1f;

        Sequence heartClickSequence = DOTween.Sequence();
        heartClickSequence.Append(transform.DOScale(targetScale, 0.05f));
        heartClickSequence.Append(transform.DOScale(targetScale * 0.9f, 0.05f).OnComplete(checkHeartReachedMax));
        Debug.Log("here" + currentHeartState);
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

}
