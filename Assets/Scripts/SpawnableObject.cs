using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SpawnableObject : MonoBehaviour
{
    [SerializeField] private int _minTimeLife = 1;
    [SerializeField] private int _maxTimeLife = 5;

    protected Rigidbody Rigidbody;
    protected int Lifetime;

    public event Action<SpawnableObject> Lived;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        OnAwake();
    }

    private void OnEnable()
    {
        int convertRandomMaxTimeLife = _maxTimeLife + 1;
        Lifetime = UnityEngine.Random.Range(_minTimeLife, convertRandomMaxTimeLife);

        Enable();
    }

    public virtual void ApplyDefaultState()
    {
        transform.rotation = Quaternion.identity;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.constraints = RigidbodyConstraints.None;
    }

    protected virtual void OnAwake() { }

    protected virtual void Enable() { }

    protected virtual void EndExistence() =>
        Lived?.Invoke(this);
}