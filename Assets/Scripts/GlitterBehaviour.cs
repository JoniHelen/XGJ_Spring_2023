using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UniRx;

public class GlitterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _shakeRotation;

    [SerializeField] private float _rotationTime;
    [SerializeField] private VisualEffect _glitter;
    [SerializeField] private SO_UniversalData _gameData;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _failTime;

    private bool _isDragging = false;
    private bool _eventActive = false;

    private byte _glitterCount = 0;

    Coroutine rotationRoutine;
    Coroutine failRoutine;

    private float _elapsed = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        if (rotationRoutine != null)
        {
            StopCoroutine(rotationRoutine);
            rotationRoutine = StartCoroutine(RotateShaker(transform.rotation, _shakeRotation, _rotationTime - _elapsed));
            _elapsed = 0;
        }
        else
            rotationRoutine = StartCoroutine(RotateShaker(_startRotation, _shakeRotation, _rotationTime));


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        if (rotationRoutine != null)
        {
            StopCoroutine(rotationRoutine);
            rotationRoutine = StartCoroutine(RotateShaker(transform.rotation, _startRotation, _rotationTime - _elapsed));
            _elapsed = 0;
        }
        else
            rotationRoutine = StartCoroutine(RotateShaker(_shakeRotation, _startRotation, _rotationTime));

        StartCoroutine(MoveToPosition(transform.position, _startPosition, _rotationTime));
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _shakeRotation = transform.rotation * Quaternion.Euler(0, 110, 0);

        _gameData.CurrentEvent.Subscribe(e =>
        {
            if (e == UserActionEvent.EventCondition.useGlitter)
            {
                _glitterCount = 0;
                _eventActive = true;
                failRoutine = StartCoroutine(FailTimer());
            }
        }).AddTo(this);
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(_failTime);
        _eventActive = false;
        _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.useGlitter);
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

    private IEnumerator RotateShaker(Quaternion from, Quaternion to, float time)
    {
        while (_elapsed < time)
        {
            transform.rotation = Quaternion.Lerp(from, to, _elapsed / time);
            _elapsed += Time.deltaTime;
            yield return null;
        }

        _elapsed = 0;
        transform.rotation = to;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ForgeShaker"))
        {
            _glitter.SendEvent("burst");
            _audioSource.PlayOneShot(_audioSource.clip);

            if (_eventActive)
            {
                _glitterCount++;

                if (_glitterCount == 2)
                {
                    _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.useGlitter);
                    _eventActive = false;
                    StopCoroutine(failRoutine);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isDragging)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Pointer.current.position.ReadUnprocessedValue()),
                out RaycastHit hit, 100f, LayerMask.GetMask("ShakerLayer"), QueryTriggerInteraction.Collide))
            {
                transform.position = hit.point;
            }
        }
    }
}
