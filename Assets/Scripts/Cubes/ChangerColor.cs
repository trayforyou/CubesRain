using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ChangerColor : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public void ChangeRandomColor()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        Color newColor = Random.ColorHSV();
        _meshRenderer.materials[0].color = newColor;
    }
}