using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;

    private void LateUpdate()
    {
        if (_targetTransform == null)
            return;

        Vector3 currentPosition = transform.position;

        currentPosition.x = _targetTransform.position.x;

        transform.position = currentPosition;
    }
}
