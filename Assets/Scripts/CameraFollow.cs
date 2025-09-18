using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float smoothTime = .3f;
    [SerializeField] private float zoomSpeed = 2f;


    [SerializeField] private Vector3 normal_offset;
    [SerializeField] private Vector3 zoom_offset;
    

    [SerializeField] private Quaternion normal_rotation;
    [SerializeField] private Quaternion breathing_rotation = Quaternion.Euler(20, 0, 0);

    //Combat rotation and offset is specific, and not needed in inspector
    private Quaternion combat_rotation;
    private Vector3 combat_offset;


    private Vector3 current_offset;
    private Vector3 target_offset;

    private Quaternion current_rotation;
    private Quaternion target_rotation;

    private bool inZoom = false;

    private Vector3 velocity = Vector3.zero;

    //combat camera offset constants
    private Vector3 combatZoomOffsetUP = new(-3.6f, -2.44f, 5.49f);
    private Vector3 combatZoomOffsetDOWN = new(3.6f, -2.44f, -5.49f);
    private Vector3 combatZoomOffsetRIGHT = new(5.49f, -2.44f, 3.5f);
    private Vector3 combatZoomOffsetLEFT = new(-5.49f, -2.44f, -3.5f);

    private Quaternion combatRotationUP = Quaternion.Euler(338.7f, 0, 0);
    private Quaternion combatRotationDOWN = Quaternion.Euler(338.7f, 180, 0);
    private Quaternion combatRotationRIGHT = Quaternion.Euler(338.7f, 90, 0);
    private Quaternion combatRotationLEFT = Quaternion.Euler(338.7f, 270, 0);


    void Start()
    {
        current_offset = normal_offset;
        target_offset = normal_offset;

        current_rotation = normal_rotation;
        target_rotation = normal_rotation;

        combat_rotation = combatRotationRIGHT;
        combat_offset = combatZoomOffsetRIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //target_offset = normal_offset; // to test camera offset
            //target_rotation = normal_rotation; // to test camera angles

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

    public void StartCombatZoom(string direction)
    {
        SetZoomDirection(direction);

        target_offset = combat_offset;
        target_rotation = combat_rotation;
    }

    public void StartCombatZoom() //overloaded function for if there is no direction passed (always goes right)
    {
        target_offset = combatZoomOffsetRIGHT;
        target_rotation = combatRotationRIGHT;

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

    public void StartBreathingZoom(Vector3 breathingOffset)
    {
        target_offset = breathingOffset;
        target_rotation = breathing_rotation;
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
        target_rotation = normal_rotation;
    }

    private void SetZoomDirection(string direction)
    {
        if(direction == "up")
        {
            combat_offset = combatZoomOffsetUP;
            combat_rotation = combatRotationUP;
        }
        else if (direction == "down")
        {
            combat_offset = combatZoomOffsetDOWN;
            combat_rotation = combatRotationDOWN;
        }
        else if (direction == "right")
        {
            combat_offset = combatZoomOffsetRIGHT;
            combat_rotation = combatRotationRIGHT;
        }
        else if (direction == "left")
        {
            combat_offset = combatZoomOffsetLEFT;
            combat_rotation = combatRotationLEFT;
        }
    }
}
