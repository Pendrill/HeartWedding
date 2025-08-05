using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class CameraDrag : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

    public Material mat;

    private Vector3 lastHeartPos;

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
        mat.SetTextureOffset("_MainTex", transform.position / 30);
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.DOMove(_origin - _difference, 1f).SetId("cameraDrag");

        Vector3 backgroundTextureVector3 = transform.position;
       
        //transform.position = _origin - _difference;
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
