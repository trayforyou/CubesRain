using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubesSpawner : Spawner<Cube>
{
    private float _minZPosition;
    private float _maxZPosition;
    private float _minXPosition;
    private float _maxXPosition;
    private float _yPosition;
    private Color _defaultColor = Color.white;

    [SerializeField] private bool _isCreating;
    [SerializeField] private float _spawnTime;

    public event Action<Vector3> CubeDisabled;

    private void Start() =>
    StartCoroutine(Spawn(_spawnTime));

    protected override void OnAwake()
    {
        _minZPosition = 316f;
        _maxZPosition = 319f;
        _minXPosition = 490.5f;
        _maxXPosition = 496f;
        _yPosition = 10f;

        _isCreating = true;
    }

    protected override void TurnOnObject(Cube cube)
    {
        cube.ApplyDefaultState();

        MeshRenderer meshRenderer = cube.GetMeshRenderer();
        Vector3 position = GetNewPosition();
        cube.transform.position = position;
        meshRenderer.materials[0].color = _defaultColor;

        base.TurnOnObject(cube);
    }

    protected override void DiactivateObject(SpawnableObject objectInstance)
    {
        Vector3 cubePosition = objectInstance.transform.position;

        base.DiactivateObject(objectInstance);

        CubeDisabled?.Invoke(cubePosition);
    }

    private Vector3 GetNewPosition()
    {
        float newZPosition = Random.Range(_minZPosition, _maxZPosition);
        float newXPosition = Random.Range(_minXPosition, _maxXPosition);

        return new(newXPosition, _yPosition, newZPosition);
    }

    private IEnumerator Spawn(float time)
    {
        var wait = new WaitForSecondsRealtime(time);

        while (_isCreating)
        {
            yield return wait;

            _objects.Get();
        }
    }
}
