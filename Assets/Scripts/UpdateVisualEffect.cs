using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UniRx;

[ExecuteAlways]
public class UpdateVisualEffect : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;

    private void Update()
    {
        vfx.SetVector3("HeartTransform_position", transform.position + new Vector3(0, -0.3f, 0));
        vfx.SetVector3("HeartTransform_angles", transform.rotation.eulerAngles);
        vfx.SetVector3("HeartTransform_scale",
            new Vector3(transform.localScale.x * 2.6f,
            transform.localScale.y * 3.3f,
            transform.localScale.z * 3.6f
            ));
    }
}
