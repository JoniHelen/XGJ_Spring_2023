using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UniRx;

public class HammerBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private HingeJoint _joint;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _failTime;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private VisualEffect _sparks;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private SO_UniversalData _gameData;

    private Vector3 _localAnchor;

    private byte _strikes = 0;

    private bool _hasPassedMinAngle = false;
    private bool _isDragging = false;

    private bool _eventActive = false;

    Coroutine failRoutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _localAnchor = transform.InverseTransformPoint(eventData.pointerPressRaycast.worldPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(_failTime);
        _eventActive = false;
        _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.useHammer);
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameData.CurrentEvent.Subscribe(e =>
        {
            if(e == UserActionEvent.EventCondition.useHammer)
            {
                _strikes = 0;
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Pointer.current.position.ReadUnprocessedValue()),
                out RaycastHit hit, 100f, LayerMask.GetMask("HammerLayer"), QueryTriggerInteraction.Collide))
            {
                _rb.AddForce((hit.point - transform.TransformPoint(_localAnchor)) * (Time.deltaTime * 1000), ForceMode.Impulse);
            }
        }

        if (Mathf.Abs(_joint.angle) > _minAngle && !_hasPassedMinAngle)
            _hasPassedMinAngle = true;

        if (Mathf.Abs(_joint.angle) < 0.05f && _hasPassedMinAngle)
        {
            _hasPassedMinAngle = false;
            _sparks.SendEvent("smith");
            _audioSource.PlayOneShot(_audioSource.clip);

            if (_eventActive)
            {
                _strikes++;

                if (_strikes == 2)
                {
                    _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.useHammer);
                    StopCoroutine(failRoutine);
                    _eventActive = false;
                }
            }
        }
    }
}
