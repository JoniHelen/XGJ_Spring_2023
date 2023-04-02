using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UpdateCoalMaterial : MonoBehaviour
{
    [SerializeField] Material _coalMaterial;
    [SerializeField] Material _coalMaterial2;
    [SerializeField] Material _coalMaterial3;
    [SerializeField] Material _coalMaterial4;
    void Update()
    {
        _coalMaterial.SetVector("_HeartPosition", transform.position);
        _coalMaterial2.SetVector("_HeartPosition", transform.position);
        _coalMaterial3.SetVector("_HeartPosition", transform.position);
        _coalMaterial4.SetVector("_HeartPosition", transform.position);
    }
}
