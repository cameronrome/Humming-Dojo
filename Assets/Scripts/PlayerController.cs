using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Vector2 move;
    private Animator animator => GetComponentInChildren<Animator>();

    private bool canMove = true;

    [SerializeField]
    private CharacterController cc;

    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            move = context.ReadValue<Vector2>();
        }
    }

    private void Awake()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
    }

    // Translate movement
    public void MovePlayer()
    {
        if (!canMove) return;

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero) //optional, makes it so rotation angle stays when not moving
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
            animator.SetBool("Running", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        move = Vector2.zero;
        animator.SetBool("Running", false);
    }


    // CharacterController movement
    void CharacterMove()
    {
        if (!canMove) return;

        Vector3 movement = new Vector3(move.x, 0, move.y);

        // Animation and rotation
        if (movement != Vector3.zero) //optional, makes it so rotation angle stays when not moving
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
            animator.SetBool("Running", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
        }


        movement = Vector3.ClampMagnitude(movement, 1f);
        Vector3 finalMove = movement * speed;
        cc.Move(finalMove * Time.deltaTime);
    }

}
