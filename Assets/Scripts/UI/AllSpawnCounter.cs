using UnityEngine;

public class AllSpawnCounter<T> : Counter where T : SpawnableObject
{
    [SerializeField] Spawner<T> _spawner;

    private void OnEnable() => 
        _spawner.Spawned += ChangeValue;

    private void OnDisable() => 
        _spawner.Spawned += ChangeValue;
}