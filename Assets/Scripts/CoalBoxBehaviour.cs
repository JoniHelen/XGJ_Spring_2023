using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UniRx;

public class CoalBoxBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 _startPosition;

    [SerializeField] private SO_UniversalData _gameData;
    [SerializeField] private float _failTime;
    [SerializeField] private CoalBall _coalPrefab;

    private CoalBall _coalBall;

    private bool _isDragging = false;
    private bool _eventActive = false;

    private Rigidbody _coalRb;

    Coroutine failRoutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _coalBall = Instantiate(_coalPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        _coalRb = _coalBall.GetComponent<Rigidbody>();
        _coalBall._box = this;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        Destroy(_coalBall.gameObject, 2f);
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(_failTime);
        _eventActive = false;
        _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.addCoal);
    }

    public void CompleteTask()
    {
        if (_eventActive)
        {
            _eventActive = false;
            _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.addCoal);
            StopCoroutine(failRoutine);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

        _gameData.CurrentEvent.Subscribe(e =>
        {
            if(e == UserActionEvent.EventCondition.addCoal)
            {
                _eventActive = true;
                failRoutine = StartCoroutine(FailTimer());
            }
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            if (_coalBall != null && Physics.Raycast(Camera.main.ScreenPointToRay(Pointer.current.position.ReadUnprocessedValue()),
                out RaycastHit hit, 100f, LayerMask.GetMask("HammerLayer"), QueryTriggerInteraction.Collide))
            {
                _coalRb.transform.position = hit.point;
            }
        }
    }
}
