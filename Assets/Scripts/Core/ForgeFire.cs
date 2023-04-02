using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForgeFire : MonoBehaviour
{
    [Tooltip("How quickly the fire decays per second.")]
    [Range(0f, 10f)]
    [SerializeField] private float _fireDecayRate;
    [SerializeField] private SO_UniversalData _gameData;

    private float _fireStrength = 100;
    public float FireStrength { get => _fireStrength; private set => _fireStrength = Mathf.Clamp(value, 0, 100); }

    private void Update()
    {
        if (FireStrength > 0)
            FireStrength -= Time.deltaTime * _fireDecayRate;
        else
        {
            _gameData.onGameOver.Invoke();
        }
    }

    public void ConsumeEvent(UserActionEvent actionEvent)
    {
        Debug.Log($"The fire consumed the Event: {actionEvent.Name}, {actionEvent.FlamePower}FP!");
        FireStrength += actionEvent.OnFulfilled();
    }
}
