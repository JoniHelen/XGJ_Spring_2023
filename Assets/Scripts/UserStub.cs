using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStub : MonoBehaviour
{
    [SerializeField] private SO_UniversalData _gameData;

    public void MoreAir() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.moreAir);
    public void AddCoal() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.addCoal);
    public void Usehammer() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.useHammer);
    public void ReheatMetal() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.reheatMetal);
    public void UseGlitter() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.useGlitter);
    public void SaveHeart() => _gameData.onPlayerAction.Invoke(UserActionEvent.EventCondition.saveTheHeart);
}
