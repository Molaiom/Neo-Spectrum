using UnityEngine;

public class CameraScaler : MonoBehaviour
{

    public float orthographicSize = 8;
    public float aspect = 1.33333f;

    void Start()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                gameObject.GetComponent<Camera>().nearClipPlane, gameObject.GetComponent<Camera>().farClipPlane);
    }
}
