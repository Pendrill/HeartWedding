using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpdateSlider : MonoBehaviour
{

    public GameObject pivot;
    public GameObject fill;

    public float maxFill = 1f;
    float currentFill = 0f;

    private Heart currentHeart;
    // Start is called before the first frame update
    void Start()
    {
        currentHeart = transform.parent.GetComponent<Heart>();
        GameEvents.current.onIncreaseHeartSlider += SliderNeedsUpdating;
        GameEvents.current.onShowSlider += ShowSlider;
        GameEvents.current.onHideSlider += HideSlider;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SliderNeedsUpdating(Heart heart, float sliderValue)
    {
       if(heart == currentHeart)
        {
            UpdateFill(sliderValue);
        }
       
    }

    public void UpdateFill(float value)
    {
        currentFill = Mathf.Min(value, 1f);
        //fill.transform.DOScaleX(currentFill, 0.5f);
        UpdatePivot(currentFill);
    }

    void UpdatePivot(float value)
    {
        pivot.transform.DOLocalMoveX(-0.75f + (value /2),  0.5f);
    }

    void HideSlider(Heart heart)
    {
        if (heart == currentHeart)
        {
            
            /*transform.DOLocalMoveY(-3.5f, .4f);
            gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 0f), .3f);
            fill.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 1f, 19.6f, 0f), .3f);*/

        }
    }

    void ShowSlider(Heart heart)
    {
        if(heart == currentHeart)
        {
            //transform.DOLocalMoveY(-3f, .4f);
            GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 1f), .3f);
        }
    }
}
