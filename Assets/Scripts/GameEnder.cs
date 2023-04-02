using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private SO_UniversalData _data;

    [SerializeField] private GameObject _GOScreen;

    private bool _gameIsOver = false;


    private void Start()
    {
        _data.onGameOver.AddListener(() =>
        {
            if (!_gameIsOver)
            {
                _gameIsOver = true;
                _GOScreen.SetActive(true);
                Time.timeScale = 0;
            }
        });
    }
}
