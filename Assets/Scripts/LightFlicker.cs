using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float _randomRate;
    private float _targetIntensity;
    private Light _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        StartCoroutine(RandomizeIntensity());
    }

    private IEnumerator RandomizeIntensity()
    {
        while (true)
        {
            _targetIntensity = Random.Range(1, 100f);
            yield return new WaitForSeconds(_randomRate * Random.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = _light.intensity + (_targetIntensity - _light.intensity) * 10 * Time.deltaTime;
    }
}
