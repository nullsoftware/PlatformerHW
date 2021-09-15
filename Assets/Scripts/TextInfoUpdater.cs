using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextInfoUpdater : MonoBehaviour
{
    [SerializeField] string _textFormat;

    private TMP_Text _textMesh;

    private void Start()
    {
        _textMesh = GetComponent<TMP_Text>();
    }

    public void UpdateInfo(int value)
    {
        if (_textMesh == null)
            _textMesh = GetComponent<TMP_Text>();

        _textMesh.text = string.Format(_textFormat, value);
    }
}
