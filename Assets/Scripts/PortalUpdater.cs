using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UniRx;

public class PortalUpdater : MonoBehaviour
{
    private VisualEffect _effect;
    [SerializeField] private SO_UniversalData _data;

    private void Start()
    {
        _effect = GetComponent<VisualEffect>();

        _data.CurrentEvent.Subscribe(e =>
        {
            if (e == UserActionEvent.EventCondition.saveTheHeart)
                _effect.Play();
            else
                _effect.Stop();
        }).AddTo(this);
    }
}
