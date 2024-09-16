using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public event Action<GameObject> Taked;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject targetCube = collision.gameObject;

        Taked?.Invoke(targetCube);
    }
}