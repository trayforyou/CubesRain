using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubesSpawner _cubesSpawner;

    private void OnEnable() => 
        _cubesSpawner.CubeDisabled += PlantBomb;

    private void OnDisable() =>
        _cubesSpawner.CubeDisabled -= PlantBomb;

    private void PlantBomb(Vector3 position)
    {
        Bomb currentBomb = Objects.Get();
        currentBomb.transform.position = position;
    }
}