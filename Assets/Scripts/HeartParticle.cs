using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeartParticle : MonoBehaviour
{
    GameObject heartContainer, heartImage;

    private float yDestination, xDestinationOffset, rotationSpeed, movementSpeed, xLoopCount;

    // Start is called before the first frame update
    void Start()
    {
        heartContainer = gameObject;
        heartImage = transform.GetChild(0).gameObject;

        heartContainer.transform.localPosition = new Vector3(Random.Range(-1.0f, 1.0f), -0.36f, -1f);
        float newScale = Random.Range(0.1f, 1.3f);
        heartContainer.transform.localScale = new Vector2(newScale, newScale);

        ActivateParticle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateParticle()
    {
        ShowParticle();
        SetSpeeds();
        SetDestinations();
        PlayParticleAnimation();
        GameEvents.current.HeartCollected(1);
    }

    void ShowParticle()
    {
        heartImage.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.2f);
    }

    void SetSpeeds()
    {
        rotationSpeed = Random.Range(0.0f, 3f);
        movementSpeed = Random.Range(1f, 4f);
        xLoopCount = Random.Range(1, 8);
    }

    void SetDestinations()
    {
        yDestination = Random.Range(1.0f, 5.0f);
        xDestinationOffset = Random.Range(-1.5f, 1.5f);
    }

    void PlayParticleAnimation()
    {
        heartContainer.transform.DOLocalMoveY(yDestination, movementSpeed);

        Sequence heartImageSequenceX = DOTween.Sequence();
        heartImageSequenceX.SetLoops(-1, LoopType.Yoyo);
        heartImageSequenceX.Append(heartImage.transform.DOLocalMoveX(xDestinationOffset, movementSpeed / xLoopCount).SetEase(Ease.OutSine));
        heartImageSequenceX.Append(heartImage.transform.DOLocalMoveX(xDestinationOffset * -1, movementSpeed / xLoopCount).SetEase(Ease.InSine));
        heartImage.transform.DORotate(new Vector3(0, 360, 0), movementSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        heartImage.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), (movementSpeed / 2) * 0.98f).SetDelay(movementSpeed / 2).SetEase(Ease.InQuint).OnComplete(ParticleFinished);


    }

    void ParticleFinished()
    {
        
        Destroy(gameObject);
    }
}
