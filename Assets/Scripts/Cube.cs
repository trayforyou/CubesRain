using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(ChangerColor))]

public class Cube : MonoBehaviour
{
    private int _minTimeLife;
    private int _maxTimeLife;
    private bool _isFirstTaked;

    public event Action<Cube> Lived;

    private void Start()
    {
        _minTimeLife = 2;
        _maxTimeLife = 5;

        _isFirstTaked = true;
    }

    private void OnDisable()
    {
        StopCoroutine(Live(this));
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

            gameObject.GetComponent<ChangerColor>().ChangeRandomColor();

            StartCoroutine(Live(this));
        }
    }

    private IEnumerator Live(Cube cube)
    {
        int convertRandomMaxTimeLife = _maxTimeLife + 1;
        int lifetime = Random.Range(_minTimeLife, convertRandomMaxTimeLife);

        var wait = new WaitForSecondsRealtime(lifetime);

        yield return wait;

        Lived?.Invoke(cube);
    }
}