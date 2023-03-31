using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fireText;
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private SO_UniversalData _gameData;

    private void Awake()
    {
        _gameData.FireStrength.Subscribe(s => _fireText.text = s.ToString());
        _gameData.CurrentEvent.Subscribe(e => _taskText.text = UserActionEvent.EventName(e));
    }
}
