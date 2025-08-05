using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoRotate : MonoBehaviour
{

    GameObject sphere; 
    // Start is called before the first frame update
    void Start()
    {
        sphere = this.gameObject;
        sphere.transform.DORotate(new Vector3(360, 360, 0), 60f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
