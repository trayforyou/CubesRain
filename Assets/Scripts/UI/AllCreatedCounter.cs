using UnityEngine;

public class AllCreatedCounter<T> : Counter where T : SpawnableObject
{
    [SerializeField] Spawner<T> _spawner;

    private void OnEnable() => 
        _spawner.Created += ChangeValue;

    private void OnDisable() => 
        _spawner.Created += ChangeValue;
}