using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    public Transform target; // 跟随目标（Player）
    public float smoothSpeed = 0.125f;
    // 调整相机偏移，适配修正后Player和雾块高度
    public Vector3 offset = new Vector3(0, 7, -12);

    void FixedUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}