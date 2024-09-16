using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _defaultPoolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _isCreating;

    private ObjectPool<GameObject> _pool;

    private float _minZPosition;
    private float _maxZPosition;
    private float _minXPosition;
    private float _maxXPosition;
    private float _yPosition;

    private Color _defaultColor = Color.white;

    private void Awake()
    {
        _defaultPoolSize = 10;
        _maxPoolSize = 100;

        _minZPosition = 316f;
        _maxZPosition = 319f;
        _minXPosition = 490.5f;
        _maxXPosition = 496f;
        _yPosition = 10f;

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

    private void Start()
    {
        StartCoroutine(Spawn(_spawnTime));
    }

    private void DestroyCube(GameObject cube)
    {
        Destroy(cube);
    }

    private void DiactivateCube(GameObject cube)
    {
        cube.GetComponent<Cube>().Lived -= DiactivateCube;

        _pool.Release(cube);
    }

    private void ActionOnGet(GameObject cube)
    {
        cube.GetComponent<Cube>().Lived += DiactivateCube;

        float newZPosition = Random.Range(_minZPosition, _maxZPosition);
        float newXPosition = Random.Range(_minXPosition, _maxXPosition);

        Vector3 position = new(newXPosition, _yPosition, newZPosition);

        cube.GetComponent<Cube>().ApplyDefaultState();

        cube.GetComponent<MeshRenderer>().materials[0].color = _defaultColor;

        cube.transform.position = position;
        cube.transform.rotation = Quaternion.identity;

        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        cube.SetActive(true);
    }

    private IEnumerator Spawn(float time)
    {
        var wait = new WaitForSecondsRealtime(time);

        while (_isCreating)
        {
            yield return wait;

            _pool.Get();
        }
    }
}