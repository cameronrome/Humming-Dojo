using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float smoothTime = .3f;
    [SerializeField] private float zoomSpeed = 2f;


    [SerializeField] private Vector3 normal_offset;
    [SerializeField] private Vector3 zoom_offset;
    [SerializeField] private Vector3 combat_offset;
    [SerializeField] private Vector3 breathing_offset;

    [SerializeField] private Quaternion normal_rotation;
    [SerializeField] private Quaternion combat_rotation;


    private Vector3 current_offset;
    private Vector3 target_offset;

    private Quaternion current_rotation;
    private Quaternion target_rotation;

    private bool inZoom = false;

    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_offset = normal_offset;
        target_offset = normal_offset;

        current_rotation = normal_rotation;
        target_rotation = normal_rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //target_offset = normal_offset; // temporary for testing
            //target_rotation = normal_rotation;

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCombatZoom();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                EndCombatZoom();
            }
            // position
            current_offset = Vector3.Lerp(current_offset, target_offset, Time.deltaTime * zoomSpeed);
            Vector3 targetPosition = target.position + current_offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            //rotation
            current_rotation = Quaternion.Slerp(current_rotation, target_rotation, Time.deltaTime * zoomSpeed);
            transform.rotation = current_rotation;

            
        }
    }

    public void ZoomOut()
    {
        target_offset = zoom_offset;
        inZoom = true;
    }

    public void ResetZoom()
    {
        target_offset = normal_offset;
        inZoom = false;
    }

    public void StartCombatZoom()
    {
        target_offset = combat_offset;
        target_rotation = combat_rotation;
    }

    public void EndCombatZoom()
    {
        if (inZoom)
        {
            ZoomOut();
        }
        else
        {
            ResetZoom();
        }
        target_rotation = normal_rotation;
    }

    public void StartBreathingZoom()
    {
        target_offset = breathing_offset;
    }

    public void StopBreathingZoom()
    {
        if (inZoom)
        {
            ZoomOut();
        }
        else
        {
            ResetZoom();
        }
    }
}
