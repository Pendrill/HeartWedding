using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class CameraDrag : MonoBehaviour
{
    private Vector3 _origin, _difference, _destination;

    private Camera _mainCamera;

    private bool _isDragging;

    public Material mat;

    private Vector3 lastHeartPos = new Vector3(0,0,-10);
    float upperLimit = 35f, rightLimit = 35f;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        GameEvents.current.onHeartClicked += CenterCamera;

    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        mat.SetTextureOffset("_MainTex", transform.position / 4);
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;

        transform.DOMove(ConfirmDestination(_origin - _difference), 1f).SetId("cameraDrag");

        Vector3 backgroundTextureVector3 = transform.position;
       
        //transform.position = _origin - _difference;
    }

    private Vector3 ConfirmDestination(Vector3 destination)
    {
        Vector3 finalDestination = destination;
        if (finalDestination.x > upperLimit)
        {
            finalDestination.x = upperLimit;
        }
        else if (finalDestination.x < (upperLimit * -1))
        {
            finalDestination.x = (upperLimit * -1);
        }

        if (finalDestination.y > rightLimit)
        {
            finalDestination.y = rightLimit;
        }
        else if (finalDestination.y < (rightLimit * -1))
        {
            finalDestination.y = (rightLimit * -1);
        }

        return finalDestination;
    }

    private void CenterCamera(Vector3 newPos)
    {
        //DOTween.KillAll();
        lastHeartPos = newPos;
        DOTween.Kill("cameraDrag", false);
        transform.DOMove(newPos, 0.5f).SetEase(Ease.OutBack);
    }

    public void ReturnToLastHeart()
    {
        if(lastHeartPos != null)
        {
            CenterCamera(lastHeartPos);
        }
    }


    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

}
