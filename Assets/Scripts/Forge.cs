using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ForgeFire))]
public class Forge : MonoBehaviour
{
    [Tooltip("The interval of game events in seconds.")]
    [Range(5f, 10f)]
    [SerializeField] private float _eventDelay = 5f;

    [SerializeField] private SO_UniversalData _gameData;

    private WaitForSeconds _eventWait;


    private Queue<UserActionEvent> _events = new();


    private ForgeFire _fire;

    private void OnValidate()
    {
        _eventWait = new WaitForSeconds(_eventDelay);
    }

    private void Awake()
    {
        _fire = GetComponent<ForgeFire>();
        _eventWait = new WaitForSeconds(_eventDelay);
        _gameData.onPlayerAction.AddListener(HandleUserAction);
    }

    void Start()
    {
        StartCoroutine(CreateEvents());
        StartCoroutine(ConsumeEvents());
    }

    private IEnumerator CreateEvents()
    {
        while (true)
        {
            _events.Enqueue(UserActionEvent.RandomEvent);
            yield return _eventWait;
        }
    }

    private IEnumerator ConsumeEvents()
    {
        yield return new WaitForSeconds(3);
        while (true)
        {
            yield return _eventWait;
            if (_events.TryDequeue(out var result))
                Debug.Log($"EVENT FAILED: {result.Name}");
        }
    }

    private void HandleUserAction(UserActionEvent.EventCondition condition)
    {
        if (_events.TryPeek(out var actionEvent) && actionEvent.Condition == condition)
        {
            _events.Dequeue();
            _fire.ConsumeEvent(actionEvent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_events.TryPeek(out var userAction))
            _gameData.CurrentEvent.Value = userAction.Condition;
        else
            _gameData.CurrentEvent.Value = UserActionEvent.EventCondition.none;

        _gameData.FireStrength.Value = _fire.FireStrength;
    }
}
