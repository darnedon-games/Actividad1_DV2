using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _player;
    public Vector3 offset;

    private void Start()
    {
        _player = FindFirstObjectByType<JugadorTony>().GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        gameObject.transform.position = _player.position + offset;
    }
}
