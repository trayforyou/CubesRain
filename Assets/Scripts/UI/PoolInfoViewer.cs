using System.Collections;
using TMPro;
using UnityEngine;

public abstract class PoolInfoViewer<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private TextMeshProUGUI _textSpawnInfo;
    [SerializeField] private TextMeshProUGUI _textCreatedInfo;
    [SerializeField] private TextMeshProUGUI _textActiveInfo;
    [SerializeField] private float _delayUpdateActive;

    private int _countSpawn = 0;
    private int _countCreated = 0;
    private int _countActive = 0;
    private string _unchangeableSpawnInfo;
    private string _unchangeableCreatedInfo;
    private string _unchangeableActiveInfo;
    private Coroutine _coroutine;

    private void Awake()
    {
        _unchangeableSpawnInfo = _textSpawnInfo.text;
        _unchangeableCreatedInfo = _textCreatedInfo.text;
        _unchangeableActiveInfo = _textActiveInfo.text;

        VisualizeSpawnedValue();
    }

    private void Start() =>
        _coroutine = StartCoroutine(UpdateActiveValue());

    private void OnEnable()
    {
        _spawner.Spawned += VisualizeSpawnedValue;
        _spawner.Created += VisualizeCreatedValue;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _spawner.Spawned -= VisualizeSpawnedValue;
        _spawner.Created -= VisualizeCreatedValue;
    }

    private void VisualizeCreatedValue()
    {
        _countCreated++;
        _textCreatedInfo.text = _unchangeableCreatedInfo + _countCreated;
    }

    private void VisualizeSpawnedValue()
    {
        _countSpawn++;
        _textSpawnInfo.text = _unchangeableSpawnInfo + _countSpawn;
    }

    private IEnumerator UpdateActiveValue()
    {
        WaitForSeconds wait = new WaitForSeconds(_delayUpdateActive);

        while (enabled)
        {
            _countActive = _spawner.GetActiveObjectsCount();

            VisualizeActiveValue();

            yield return wait;
        }
    }

    private void VisualizeActiveValue() =>
        _textActiveInfo.text = _unchangeableActiveInfo + _countActive;
}