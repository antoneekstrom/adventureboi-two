using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerMovementSettings settings;

    public Vector2 MovementInput { get; private set; }
    public bool IsRunning => Input.GetKey(props.keybindings.runKey);

    private PlayerJump jump;
    private Player props;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        jump = GetComponent<PlayerJump>();
        props = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator = props.Animator;
    }

    private void Update()
    {
        ComputeMovementInput();

        animator.SetBool("IsRunning", IsRunning);

        animator.SetFloat("InputX", MovementInput.x);
        animator.SetFloat("InputY", MovementInput.y);
        animator.SetFloat("InputMag", MovementInput.magnitude);

        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetFloat("VelocityMag", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        bool onGround = jump.OnGround;

        float inAirMovementModifier = settings.inAirMovementModifier;
        float movementVelocity = settings.movementVelocity;
        float runModifier = settings.runModifier;

        if (settings.constantVelocity)
            rb.velocity = Vector2.up * rb.velocity + Vector2.right * (MovementInput * movementVelocity).x * (onGround ? 1 : inAirMovementModifier) * (IsRunning ? runModifier : 1);
        else
            rb.velocity += Vector2.right * Time.fixedDeltaTime * (MovementInput * movementVelocity).x * (onGround ? 1 : inAirMovementModifier) * (IsRunning ? runModifier : 1);
    }

    private void ComputeMovementInput() => MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

}
