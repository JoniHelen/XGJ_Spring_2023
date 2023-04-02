using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UniRx;

public class MetalBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 _startPosition;

    [SerializeField] private SO_UniversalData _gameData;
    [SerializeField] private float _failTime;
    [SerializeField] private float _targetHeat;

    private bool _isDragging = false;
    private bool _eventActive = false;
    private bool _isHeatingUp = false;

    private float _currentHeat = 0;

    Coroutine failRoutine;

    Renderer rend;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        StartCoroutine(MoveToPosition(transform.position, _startPosition, 0.1f));
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        _startPosition = transform.position;
        _gameData.CurrentEvent.Subscribe(e =>
        {
            if (e == UserActionEvent.EventCondition.reheatMetal)
            {
                _eventActive = true;
                failRoutine = StartCoroutine(FailTimer());
            }
        }).AddTo(this);
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(_failTime);
        _eventActive = false;
        _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.reheatMetal);
    }

    private IEnumerator MoveToPosition(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0;

        while (elapsed < time)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ForgeShaker"))
        {
            _isHeatingUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ForgeShaker"))
        {
            _isHeatingUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Pointer.current.position.ReadUnprocessedValue()),
                out RaycastHit hit, 100f, LayerMask.GetMask("HammerLayer"), QueryTriggerInteraction.Collide))
            {
                transform.position = hit.point;
            }
        }

        if (_isHeatingUp)
            _currentHeat = Mathf.Min(_currentHeat + Time.deltaTime, _targetHeat);
        else
            _currentHeat = Mathf.Max(_currentHeat - Time.deltaTime * 0.5f, 0);

        if (_currentHeat == _targetHeat && _eventActive)
        {
            _eventActive = false;
            _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.reheatMetal);
            StopCoroutine(failRoutine);
        }

        rend.sharedMaterial.SetFloat("_Heat", _currentHeat / _targetHeat);
    }
}
