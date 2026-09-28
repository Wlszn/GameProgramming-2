using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sends the current parameteres to the blend tree to change the animation based on the current state of the player
        animator.SetFloat("CharacterSpeed", rb.linearVelocity.magnitude);

        //tell the animator if we are on the ground
        animator.SetBool("IsGrounded", movement.IsGrounded);

        //when Fire1 is released trigger the roll
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetTrigger("doRoll");
        }

    }
}
