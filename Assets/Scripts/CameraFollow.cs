using UnityEngine;

// Kamera mashina orqasidan kuzatib boradi
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 12f;
    public float height = 7f;
    public float smooth = 5f;

    void LateUpdate()
    {
        Vector3 wantedPosition = target.position - target.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, wantedPosition, smooth * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up);
    }
}
