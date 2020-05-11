using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropkick : MonoBehaviour
{
    public PlayerDropkickSettings settings;

    public bool IsDropkicking => _isDropkicking;
    public float DropkickCooldownPercentage => Mathf.Min((Time.time - dropkickCooldownTime), settings.dropkickCooldown) / settings.dropkickCooldown;
    public bool CanDropkick => Time.time - dropkickCooldownTime >= settings.dropkickCooldown;
    private bool CanApplyDropkick => _isDropkicking && !hasKicked;

    private float dropkickCooldownTime;
    private bool hasKicked = false;
    private bool _isDropkicking = false;

    private PlayerJump jump;
    private PlayerMovement movement;
    private Player props;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        jump = GetComponent<PlayerJump>();
        movement = GetComponent<PlayerMovement>();
        props = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator = props.Animator;
        jump.OnContactGround += Player_OnContactGround;
    }

    private void Update()
    {
        UpdateDropkick();
        animator.SetBool("IsDropkicking", _isDropkicking);

        if (settings.cooldownPercentageStore)
            settings.cooldownPercentageStore.Value = DropkickCooldownPercentage;
    }

    public bool TriggerDropkick(Dropkickable d, Vector2 contactPoint)
    {
        if (CanApplyDropkick && d.TryGetComponent(out Collider2D collider) && !collider.CompareTag("Ground"))
        {
            hasKicked = true;

            if (collider.attachedRigidbody)
            {
                Vector2 dir = new Vector2(rb.velocity.normalized.x, 1);
                var rbOther = collider.attachedRigidbody;
                rbOther.AddForce(settings.dropkickForce * dir * settings.dropkickSpeedModifier, ForceMode2D.Impulse);
                rbOther.AddTorque(-settings.dropkickTorque * dir.x * settings.dropkickSpeedModifier, ForceMode2D.Impulse);
            }

            GameObject fx = Instantiate(settings.dropkickEffect);
            fx.transform.position = fx.transform.position.SetPosition(contactPoint);

            this.SlowTime(settings.dropkickSlowTimeDuration, settings.dropkickTimeModifier);

            return true;
        }
        return false;
    }

    private void UpdateDropkick()
    {
        if (Input.GetKeyDown(props.keybindings.dropkickKey) && jump.OnGround && CanDropkick)
        {
            _isDropkicking = true;
            hasKicked = false;

            jump.Jump();
            rb.velocity += new Vector2(movement.MovementInput.x, 1) * settings.dropkickBoost;
        }
    }

    private void Player_OnContactGround()
    {
        if (_isDropkicking)
            dropkickCooldownTime = Time.time;
        _isDropkicking = false;
    }
}
