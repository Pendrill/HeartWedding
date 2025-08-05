using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveRobot : MonoBehaviour
{

    GameObject robot;
    // Start is called before the first frame update
    void Start()
    {
        robot = this.gameObject;
        robot.transform.DOLocalMoveX(11f, 10f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetDelay(0.3f);
    }

    
}
