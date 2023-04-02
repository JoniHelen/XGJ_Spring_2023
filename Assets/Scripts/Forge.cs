using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(ForgeFire))]
public class Forge : MonoBehaviour
{
    [Tooltip("The interval of game events in seconds.")]
    [Range(0f, 5f)]
    [SerializeField] private float _eventDelay = 5f;

    [SerializeField] private SO_UniversalData _gameData;

    private WaitForSeconds _eventWait;

    [SerializeField] private Color _flameFlashAir;
    [SerializeField] private Color _flameFlashCoal;
    [SerializeField] private Color _flameFlashHammer;
    [SerializeField] private Color _flameFlashMetal;
    [SerializeField] private Color _flameFlashGlitter;
    [SerializeField] private Color _flameFlashHeart;


    private Queue<UserActionEvent> _events = new();


    private ForgeFire _fire;
    [SerializeField] private VisualEffect _fireEffect;

    private void OnValidate()
    {
        _eventWait = new WaitForSeconds(_eventDelay);
    }

    private void Awake()
    {
        _fire = GetComponent<ForgeFire>();
        _eventWait = new WaitForSeconds(_eventDelay);
        _gameData.onPlayerAction.AddListener(HandleUserAction);
        _gameData.onPlayerActionFailed.AddListener(HandleUserActionFailed);
        _gameData.CurrentEvent.Value = UserActionEvent.EventCondition.none;
    }

    void Start()
    {
        StartCoroutine(CreateEvent());
    }

    private IEnumerator CreateEvent()
    {
        yield return _eventWait;
        var ev = UserActionEvent.RandomEvent;
        _events.Enqueue(ev);
        _gameData.CurrentEvent.Value = ev.Condition;
    }

    private IEnumerator FlashFire(Color from, Color to, float time)
    {
        float elapsed = 0;

        while (elapsed < time)
        {
            Color current = Color.Lerp(from, to, FlashEase(elapsed / time));
            _fireEffect.SetVector4("FlameColor", current);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _fireEffect.SetVector4("FlameColor", from);
    }

    private float FlashEase(float t)
    {
        return -4 * (t - 0.5f) * (t - 0.5f) + 1;
    }

    private void HandleUserActionFailed(UserActionEvent.EventCondition condition)
    {
        if(_events.TryPeek(out var actionEvent) && actionEvent.Condition == condition)
        {
            _events.Dequeue();
            _gameData.CurrentEvent.Value = UserActionEvent.EventCondition.none;
            Debug.Log($"EVENT FAILED!");
            StartCoroutine(CreateEvent());
        }
    }

    private void HandleUserAction(UserActionEvent.EventCondition condition)
    {
        if (_events.TryPeek(out var actionEvent) && actionEvent.Condition == condition)
        {
            _events.Dequeue();
            _gameData.CurrentEvent.Value = UserActionEvent.EventCondition.none;
            _fire.ConsumeEvent(actionEvent);

            Vector4 original = _fireEffect.GetVector4("FlameColor");

            StartCoroutine(FlashFire(original, GetFlameColor(condition) * (original.magnitude * 1.5f), 0.8f));
            StartCoroutine(CreateEvent());
        }
    }

    private Color GetFlameColor(UserActionEvent.EventCondition condition) => condition switch
    {
        UserActionEvent.EventCondition.moreAir => _flameFlashAir,
        UserActionEvent.EventCondition.addCoal => _flameFlashCoal,
        UserActionEvent.EventCondition.useHammer => _flameFlashHammer,
        UserActionEvent.EventCondition.reheatMetal => _flameFlashMetal,
        UserActionEvent.EventCondition.useGlitter => _flameFlashGlitter,
        UserActionEvent.EventCondition.saveTheHeart => _flameFlashHeart,
        _ => Color.black
    };


    // Update is called once per frame
    void Update()
    {
        _gameData.FireStrength.Value = _fire.FireStrength;
        _fireEffect.SetFloat("EffectIntensity", _fire.FireStrength / 100);
    }
}
