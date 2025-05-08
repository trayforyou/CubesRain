using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ChangerColor : MonoBehaviour
{
    public void ChangeRandomColor()
    {
        Color newColor = Random.ColorHSV();
        gameObject.GetComponent<MeshRenderer>().materials[0].color = newColor;
    }
}