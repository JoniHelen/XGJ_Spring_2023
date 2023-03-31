using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ForgeFire))]
public class Forge : MonoBehaviour
{
    [Tooltip("The interval of game events in seconds.")]
    [Range(5f, 10f)]
    [SerializeField] private float _eventDelay = 5f;
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
            yield return _eventWait;
            _events.Enqueue(UserActionEvent.RandomEvent);
        }
    }

    private IEnumerator ConsumeEvents()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            yield return _eventWait;
            if (_events.TryDequeue(out var result))
                _fire.ConsumeEvent(result);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
