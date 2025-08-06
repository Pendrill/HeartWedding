using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeartParticle : MonoBehaviour
{
    GameObject heartContainer, heartImage;
    public AudioSource particleAudio;

    private float yDestination, xDestinationOffset, rotationSpeed, movementSpeed, xLoopCount;

    private Vector2 newScaleRef;

    // Start is called before the first frame update
    void Start()
    {
        heartContainer = gameObject;
        heartImage = transform.GetChild(0).gameObject;

        heartContainer.transform.localPosition = new Vector3(Random.Range(-1.0f, 1.0f), -0.36f, -1f);
        float newScale = Random.Range(0.1f, 1.3f);
        newScaleRef = new Vector2(newScale, newScale);
        heartContainer.transform.localScale = new Vector2(0, 0);

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
        //heartImage.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.2f);
        heartContainer.transform.DOScale(newScaleRef, 0.2f).SetEase(Ease.OutBack);
    }

    void SetSpeeds()
    {
        rotationSpeed = Random.Range(0.0f, 3f);
        movementSpeed = Random.Range(1f, 3f);
        xLoopCount = Random.Range(1, 6);
    }

    void SetDestinations()
    {
        yDestination = Random.Range(1.0f, 4.0f);
        xDestinationOffset = Random.Range(0, 4f);
    }

    void PlayParticleAnimation()
    {
        var curX = heartImage.gameObject.transform.localPosition.x;
        var newXMin = curX - xDestinationOffset;
        var newXMax = curX + xDestinationOffset;

        //heartImage.gameObject.transform.localPosition = new Vector3(newXMin, heartImage.gameObject.transform.localPosition.y, heartImage.gameObject.transform.localPosition.z);
        heartContainer.transform.DOLocalMoveY(yDestination, movementSpeed);

        Sequence heartImageSequenceX = DOTween.Sequence();
        //heartImageSequenceX.SetEase(Ease.InSine);
        heartImageSequenceX.Append(heartImage.transform.DOLocalMoveX(newXMax, movementSpeed).SetEase(Ease.InSine));
        //heartImageSequenceX.Append(heartImage.transform.DOLocalMoveX(newXMin, 2f).SetEase(Ease.OutSine));
        heartImage.transform.DORotate(new Vector3(0, 360, 0), movementSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        heartImage.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), (movementSpeed / 2) * 0.98f).SetDelay(movementSpeed / 2).SetEase(Ease.InQuint).OnComplete(PlayParticleAudio);


    }

    void ParticleFinished()
    {
        
        Destroy(gameObject);
    }

    void PlayParticleAudio()
    {
        particleAudio.Play();
        Invoke("ParticleFinished", particleAudio.clip.length);
    }
}
