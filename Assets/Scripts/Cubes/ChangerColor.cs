using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ChangerColor : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeRandomColor()
    {
        Color newColor = Random.ColorHSV();
        _meshRenderer.materials[0].color = newColor;
    }
}