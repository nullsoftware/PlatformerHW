using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;

    void LateUpdate()
    {
        if (_targetTransform == null)
            return;

        Vector3 currentPos = transform.position;

        currentPos.x = _targetTransform.position.x;

        transform.position = currentPos;
    }
}
