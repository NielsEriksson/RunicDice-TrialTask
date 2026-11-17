using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target to follow")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] public Vector3 offset;

    public void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;  
        }
    }
}
