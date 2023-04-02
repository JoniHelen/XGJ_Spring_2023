using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class HeartBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SO_UniversalData _gameData;
    [SerializeField] private float _progressDrain;
    [SerializeField] private float _progressGain;
    private float _progress = 100;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _needsSaving = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_needsSaving)
        {
            _progress = Mathf.Min(_progress + _progressGain, 100);

            if (_progress == 100)
            {
                _needsSaving = false;
                transform.position = _startPosition;
                _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.saveTheHeart);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + Vector3.left * 7f;

        _gameData.CurrentEvent.Subscribe(e =>
        {
            if(e == UserActionEvent.EventCondition.saveTheHeart)
            {
                _needsSaving = true;
                _progress = 99;
            }

        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_needsSaving)
        {
            _progress = Mathf.Max(_progress - _progressDrain * Time.deltaTime, 0);
            if (_progress == 0)
            {
                _needsSaving = false;
                _gameData.onPlayerActionFailed.Invoke(UserActionEvent.EventCondition.saveTheHeart);
            }
            transform.position = Vector3.Lerp(_endPosition, _startPosition, _progress / 100);
        }
    }
}
