using UnityEngine;

[RequireComponent(typeof(ChangerAlpha))]
public class Bomb : SpawnableObject
{
    [SerializeField] private float _explosionRadius = 1;
    [SerializeField] private float _explosionForce = 1;

    private ChangerAlpha _changerAlpha;

    protected override void Awake()
    {
       base.Awake();
        
        _changerAlpha = GetComponent<ChangerAlpha>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _changerAlpha.Activate(_lifetime);

        _changerAlpha.BecomeInvisible += EndExistence;
    }

    private void OnDisable() =>
        _changerAlpha.BecomeInvisible -= EndExistence;

    protected override void EndExistence()
    {
        Explode();
        base.EndExistence();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}