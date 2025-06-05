using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float smoothTime = .3f;
    [SerializeField] private float zoomSpeed = 2f;


    [SerializeField] private Vector3 normal_offset;

    [SerializeField] private Vector3 zoom_offset;

    private Vector3 current_offset;
    private Vector3 target_offset;

    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_offset = normal_offset;
        target_offset = normal_offset;
    }

    // Update is called once per frame
    void Update()
    {
       if(target != null)
        {
            current_offset = Vector3.Lerp(current_offset, target_offset, Time.deltaTime * zoomSpeed);
            Vector3 targetPosition = target.position + current_offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void ZoomOut()
    {
        target_offset = zoom_offset;
    }

    public void ResetZoom()
    {
        target_offset = normal_offset;
    }
}
