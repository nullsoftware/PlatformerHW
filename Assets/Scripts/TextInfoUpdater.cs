using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextInfoUpdater : MonoBehaviour
{
    private TMP_Text _textMesh;

    [SerializeField] string _textFormat;

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
