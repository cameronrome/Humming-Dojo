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

    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            move = context.ReadValue<Vector2>();
        }
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

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
}
