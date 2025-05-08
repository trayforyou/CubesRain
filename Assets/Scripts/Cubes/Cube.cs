using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ChangerColor), typeof(MeshRenderer))]
public class Cube : SpawnableObject
{
    private ChangerColor _changerColor;
    private Coroutine _liveCorutine;
    private bool _isTouched;

    private void Start()
    {
        _changerColor = gameObject.GetComponent<ChangerColor>();
        _isTouched = true;
    }

    private void OnDisable()
    {
        if (_liveCorutine != null)
            StopCoroutine(_liveCorutine);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _) && _isTouched)
        {
            _isTouched = false;

            _changerColor.ChangeRandomColor();

            _liveCorutine = StartCoroutine(Live());
        }
    }

    public MeshRenderer GetMeshRenderer() =>
        gameObject.GetComponent<MeshRenderer>();

    public override void ApplyDefaultState()
    {
        _isTouched = true;

        base.ApplyDefaultState();
    }

    protected virtual IEnumerator Live()
    {
        var wait = new WaitForSecondsRealtime(_lifetime);

        yield return wait;

        EndExistence();
    }
}