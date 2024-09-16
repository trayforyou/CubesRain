using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class Cube : MonoBehaviour
{
    private bool _haveDefaultColor;
    private Color _defaultColor;

    private void Start()
    {
        _defaultColor = GetComponent<MeshRenderer>().materials[0].color;
        _haveDefaultColor = true;
    }

    public void ApplyDefaultState()
    {
        _haveDefaultColor = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out var platform) && _haveDefaultColor)
        {
            UnityEngine.Color newColor = UnityEngine.Random.ColorHSV();
            gameObject.GetComponent<MeshRenderer>().materials[0].color = newColor;

            _haveDefaultColor = false;
        }
    }

    private void OnDisable()
    {
        ApplyDefaultState();
    }
}
