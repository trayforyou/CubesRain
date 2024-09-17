using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private int _defaultPoolSize = 5;
    [SerializeField] private int _maxPoolSize = 5;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _isCreating;

    private ObjectPool<Cube> _pool;

    private float _minZPosition;
    private float _maxZPosition;
    private float _minXPosition;
    private float _maxXPosition;
    private float _yPosition;

    private Color _defaultColor = Color.white;

    private void Awake()
    {
        _minZPosition = 316f;
        _maxZPosition = 319f;
        _minXPosition = 490.5f;
        _maxXPosition = 496f;
        _yPosition = 10f;

        _isCreating = true;

        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _defaultPoolSize,
            maxSize: _maxPoolSize);
    }

    private void Start()
    {
        StartCoroutine(Spawn(_spawnTime));
    }

    private void DestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private void DiactivateCube(Cube cube)
    {
        cube.GetComponent<Cube>().Lived -= DiactivateCube;

        _pool.Release(cube);
    }

    private void ActionOnGet(Cube cube)
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

        cube.gameObject.SetActive(true);
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