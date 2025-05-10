using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ChangerColor), typeof(MeshRenderer))]
public class Cube : SpawnableObject
{
    private ChangerColor _changerColor;
    private Coroutine _liveCorutine;
    private bool _isTouched;
    private MeshRenderer _meshRenderer;

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
        _meshRenderer;

    public override void ApplyDefaultState()
    {
        _isTouched = true;

        base.ApplyDefaultState();
    }

    protected override void OnAwake()
    {
        _isTouched = true;
        _changerColor = GetComponent<ChangerColor>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private IEnumerator Live()
    {
        var wait = new WaitForSecondsRealtime(_lifetime);

        yield return wait;

        EndExistence();
    }
}