using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ChangerAlpha : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Coroutine _coroutine;
    private Color _defaultColor;

    public event Action BecomeInvisible;

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _material.color = _defaultColor;
    }

    public void Activate(int duration)
    {
        if (_material == null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _material = _meshRenderer.material;
            _defaultColor = _material.color;
        }

        _coroutine = StartCoroutine(ChangeValue(duration));
    }

    private IEnumerator ChangeValue(int duration)
    {
        Color color = _material.color;
        float maxValueAlpha = 1;
        float speed = maxValueAlpha / duration;

        while (_material.color.a > 0)
        {
            color.a = Mathf.MoveTowards(_material.color.a, 0, speed * Time.deltaTime);
            _material.color = color;

            yield return null;
        }

        BecomeInvisible?.Invoke();
    }
}
