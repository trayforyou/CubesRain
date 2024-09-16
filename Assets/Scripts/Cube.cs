using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]

public class Cube : MonoBehaviour
{
    private int _minTimeLife;
    private int _maxTimeLife;
    private bool _isFirstTaked;

    public event Action<GameObject> Lived;

    private void Start()
    {
        _minTimeLife = 2;
        _maxTimeLife = 5;

        _isFirstTaked = true;
    }

    public void ApplyDefaultState()
    {
        _isFirstTaked = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out var platform) && _isFirstTaked)
        {
            _isFirstTaked = false;

            UnityEngine.Color newColor = UnityEngine.Random.ColorHSV();
            gameObject.GetComponent<MeshRenderer>().materials[0].color = newColor;

            StartCoroutine(Live(gameObject));
        }
    }

    private IEnumerator Live(GameObject cube)
    {
        int convertRandomMaxTimeLife = _maxTimeLife + 1;
        int lifetime = Random.Range(_minTimeLife, convertRandomMaxTimeLife);

        var wait = new WaitForSecondsRealtime(lifetime);

        yield return wait;

        Lived?.Invoke(cube);
    }

    private void OnDisable()
    {
        StopCoroutine(Live(gameObject));
    }
}