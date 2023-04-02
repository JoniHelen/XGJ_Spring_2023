using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBall : MonoBehaviour
{
    public CoalBoxBehaviour _box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ForgeCoal"))
            _box.CompleteTask();
    }
}
