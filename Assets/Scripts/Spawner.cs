using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public abstract class Spawner<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] private T _object;
    [SerializeField] private int _defaultPoolSize = 5;
    [SerializeField] private int _maxPoolSize = 5;

    protected ObjectPool<T> _objects;

    public event Action Spawned;
    public event Action Created;

    protected virtual void Awake()
    {
        _objects = new ObjectPool<T>(
                    createFunc: () => CreateObject(),
                    actionOnGet: (objectInstance) => TurnOnObject(objectInstance),
                    actionOnRelease: (objectInstance) => objectInstance.gameObject.SetActive(false),
                    actionOnDestroy: (objectInstance) => Destroy(objectInstance.gameObject),
                    collectionCheck: true,
                    defaultCapacity: _defaultPoolSize,
                    maxSize: _maxPoolSize);
    }

    public int GetActiveObjectsCount() =>
        _objects.CountActive;

    protected virtual void DiactivateObject(SpawnableObject objectInstance)
    {
        objectInstance.Lived -= DiactivateObject;

        _objects.Release((T)objectInstance);
    }

    protected virtual void TurnOnObject(T objectInstance)
    {
        Spawned?.Invoke();

        objectInstance.Lived += DiactivateObject;

        objectInstance.gameObject.SetActive(true);
    }

    private T CreateObject()
    {
        Created?.Invoke();

        return Instantiate(_object);
    }
}