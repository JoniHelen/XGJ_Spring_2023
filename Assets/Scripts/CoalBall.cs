using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBall : MonoBehaviour
{
    public CoalBoxBehaviour _box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ForgeCoal") && _box._eventActive)
        {
            var a = other.GetComponent<AudioSource>();
            a.PlayOneShot(a.clip);
            _box.CompleteTask();
        }
            
    }
}
