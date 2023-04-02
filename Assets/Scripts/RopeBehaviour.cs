using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UniRx;

public class RopeBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _maxDragDistance;
    private bool _isDragging;
    private Vector3 _dragStartPosition;
    private Vector3 _dragRbStartPosition;
    private bool _ropePulled = false;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private SO_UniversalData _gameData;
    [SerializeField] private float _failTime;

    Coroutine failRoutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _rb.isKinematic = true;
        _dragStartPosition = eventData.pointerPressRaycast.worldPosition;
        _dragRbStartPosition = _rb.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        _rb.isKinematic = false;
        _ropePulled = false;
    }

    private void Start()
    {
        _gameData.CurrentEvent.Subscribe(e =>
        {
            if(e == UserActionEvent.EventCondition.moreAir)
            {
                failRoutine = StartCoroutine(FailTimer());
            }
        });
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(_failTime);
        _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.moreAir);
    }

    private void Update()
    {
        if (_isDragging)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Pointer.current.position.ReadUnprocessedValue()),
                out RaycastHit hit, 100f, LayerMask.GetMask("Rope Layer"), QueryTriggerInteraction.Collide))
            {
                if (hit.point.y < _dragStartPosition.y)
                {
                    if ((_dragStartPosition.y - hit.point.y) >= _maxDragDistance && !_ropePulled)
                    {
                        _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.moreAir);
                        StopCoroutine(failRoutine);
                        _ropePulled = true;
                    }

                    _rb.position = _dragRbStartPosition + Vector3.down * ((_dragStartPosition.y - hit.point.y) < _maxDragDistance ? (_dragStartPosition.y - hit.point.y) : _maxDragDistance);
                }
            }
        }
    }
}
