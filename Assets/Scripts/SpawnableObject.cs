using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SpawnableObject : MonoBehaviour
{
    [SerializeField] private int _minTimeLife = 1;
    [SerializeField] private int _maxTimeLife = 5;

    protected Rigidbody _rigidbody;
    protected int _lifetime;

    public event Action<SpawnableObject> Lived;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        OnAwake();
    }

    private void OnEnable()
    {
        int convertRandomMaxTimeLife = _maxTimeLife + 1;
        _lifetime = UnityEngine.Random.Range(_minTimeLife, convertRandomMaxTimeLife);

        InOnEnable();
    }

    public virtual void ApplyDefaultState()
    {
        transform.rotation = Quaternion.identity;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    protected virtual void OnAwake() { }

    protected virtual void InOnEnable() { }

    protected virtual void EndExistence() =>
        Lived?.Invoke(this);
}