using System.Collections;
using UnityEngine;

public class ActiveObjectsCounter<T> : Counter where T : SpawnableObject
{
    [SerializeField] private float _delayUpdate;
    [SerializeField] private Spawner<T> _spawner;

    private Coroutine _coroutine;

    private void Start() =>
        _coroutine = StartCoroutine(Visualize());

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Visualize()
    {
        WaitForSeconds wait = new WaitForSeconds(_delayUpdate);

        while (enabled)
        {
            _count = _spawner.GetActiveObjectsCount();

            VisualizeValue();

            yield return wait;
        }
    }
}