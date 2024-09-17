using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ChangerColor), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    private ChangerColor _changerColor;
    private int _minTimeLife;
    private int _maxTimeLife;
    private bool _isTouched;
    private Coroutine _liveCorutine;

    public event Action<Cube> Lived;

    private void Start()
    {
        _changerColor = gameObject.GetComponent<ChangerColor>();
        _minTimeLife = 2;
        _maxTimeLife = 5;

        _isTouched = true;
    }

    private void OnDisable()
    {
        StopCoroutine(Live(this));
    }

    public void ApplyDefaultState()
    {
        _isTouched = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _) && _isTouched)
        {
            _isTouched = false;

            _changerColor.ChangeRandomColor();

            _liveCorutine = StartCoroutine(Live(this));
        }
    }

    public void GetCubeComponents(out Rigidbody rigidbody, out MeshRenderer mewshRender)
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        mewshRender = gameObject.GetComponent<MeshRenderer>();
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