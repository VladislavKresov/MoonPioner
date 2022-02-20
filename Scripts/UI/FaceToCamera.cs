using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position - _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.up);
    }
}
