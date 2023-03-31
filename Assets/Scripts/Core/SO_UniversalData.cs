using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

[CreateAssetMenu(fileName = "New Universal Data", menuName = "ScriptableObjects/UniversalData")]
public class SO_UniversalData : ScriptableObject
{
    public FloatReactiveProperty FireStrength = new(100);
    public ReactiveProperty<UserActionEvent.EventCondition> CurrentEvent = new(UserActionEvent.EventCondition.none);

    public UnityEvent<UserActionEvent.EventCondition> onPlayerAction = new();
}
