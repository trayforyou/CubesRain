using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _defaultPoolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private int _spawnTime;
    [SerializeField] private bool _isCreating;
    [SerializeField] private List<GameObject> _platforms;

    private ObjectPool<GameObject> _pool;
    private int _minTimeLife;
    private int _maxTimeLife;
    private Color _defaultColor = Color.white;

    private void Awake()
    {
        _defaultPoolSize = 10;
        _maxPoolSize = 100;
        _spawnTime = 1;
        _minTimeLife = 2;
        _maxTimeLife = 5;
        _isCreating = true;

        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.SetActive(false),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _defaultPoolSize,
            maxSize: _maxPoolSize);
    }

    private void OnEnable()
    {
        foreach (var platform in _platforms)
        {
            platform.GetComponent<Platform>().Taked += TurnOnTimer;
        }
    }

    private void Start()
    {
        StartCoroutine(Spawn(_spawnTime));
    }

    private void DestroyCube(GameObject cube)
    {
        Destroy(cube);
    }

    private IEnumerator Spawn(int time)
    {
        var wait = new WaitForSecondsRealtime(time);

        while (_isCreating)
        {
            yield return wait;

            _pool.Get();
        }
    }

    private void ActionOnGet(GameObject cube)
    {
        Vector3 position = new Vector3(493, 9, 317);

        cube.transform.position = position;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.GetComponent<MeshRenderer>().materials[0].color = _defaultColor;
        cube.transform.Rotate(0, 0, 0);
        cube.GetComponent<Cube>().ApplyDefaultState();
        cube.SetActive(true);
    }

    private void TurnOnTimer(GameObject cube)
    {
        StartCoroutine(Live(cube));
    }

    private IEnumerator Live(GameObject cube)
    {
        int convertRandomMaxTimeLife = _maxTimeLife + 1;
        int lifetime = Random.Range(_minTimeLife, convertRandomMaxTimeLife);

        var wait = new WaitForSecondsRealtime(lifetime);
        yield return wait;

        _pool.Release(cube);
    }

    private void OnDisable()
    {
        foreach (var platform in _platforms)
        {
            platform.GetComponent<Platform>().Taked -= TurnOnTimer;
        }
    }
}
