using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    void LateUpdate()
    {
        if (objetivo == null) return;
        transform.position = new Vector3(objetivo.position.x, 0f, -10f);
    }
}
