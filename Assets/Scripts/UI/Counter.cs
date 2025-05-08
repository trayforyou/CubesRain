using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;

    protected int _count = 0;
    private string _unchangeableText;

    private void Awake()
    {
        _unchangeableText = _textMeshProUGUI.text;

        VisualizeValue();
    }

    protected void ChangeValue()
    {
        _count++;

        VisualizeValue();
    }

    protected void VisualizeValue() => 
        _textMeshProUGUI.text = _unchangeableText + _count;
}