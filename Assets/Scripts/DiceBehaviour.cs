using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DiceBehaviour : MonoBehaviour
{
    private bool _isSpinning = true;
    [SerializeField] private SO_UniversalData _data;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioClip _good;
    [SerializeField] private AudioClip _bad;

    private void Start()
    {
        _data.CurrentEvent.Subscribe(Show).AddTo(this);
    }

    private void Update()
    {
        if (_isSpinning)
            transform.Rotate(new Vector3(90, 180, 45) * Time.deltaTime, Space.Self);
    }

    public void Show(UserActionEvent.EventCondition condition)
    {
        _isSpinning = false;

        Vector3 dir;

        switch(condition)
        {
            case UserActionEvent.EventCondition.none:
                dir = Vector3.zero;
                break;
            case UserActionEvent.EventCondition.moreAir:
                dir = Vector3.forward;
                break;
            case UserActionEvent.EventCondition.addCoal:
                dir = Vector3.down;
                break;
            case UserActionEvent.EventCondition.useHammer:
                dir = Vector3.left;
                break;
            case UserActionEvent.EventCondition.reheatMetal:
                dir = Vector3.right;
                break;
            case UserActionEvent.EventCondition.useGlitter:
                dir = Vector3.back;
                break;
            case UserActionEvent.EventCondition.saveTheHeart:
                dir = Vector3.up;
                break;
            default:
                dir = Vector3.zero;
                break;
        }

        if (condition != UserActionEvent.EventCondition.none)
        {
            if (condition == UserActionEvent.EventCondition.saveTheHeart)
                _audioSource2.PlayOneShot(_bad);
            else
                _audioSource2.PlayOneShot(_good, 0.3f);
        }

        if (dir != Vector3.zero)
        {
            StartCoroutine(RotateDie(transform.rotation, Quaternion.LookRotation(dir), 0.25f));
            _audioSource.Stop();
            _audioSource.PlayOneShot(_clip);
        }
        else
        {
            _audioSource.Play();
            _isSpinning = true;
        }
    }

    private IEnumerator RotateDie(Quaternion from, Quaternion to, float time)
    {
        float elapsed = 0;

        while (elapsed < time)
        {
            transform.rotation = Quaternion.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0;
        transform.rotation = to;
    }
}
