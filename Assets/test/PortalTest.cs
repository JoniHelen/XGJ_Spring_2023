using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class PortalTest : MonoBehaviour
{


    public bool IsPlaying = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPlaying = !IsPlaying;
        }

        if (IsPlaying)
        {
            GetComponent<VisualEffect>().Play();
        }
        else
        {
            GetComponent<VisualEffect>().Stop();
        }
    }
}
