using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmedRico : MonoBehaviour
{
    public StringVariable onHitAction;
    public string drawAnimationTrigger = "Draw";
    public string animationStateName = "Drawing";
    public bool triggerByClick = false;
    public float detectionRange = 8f;
    public float maxVelocity = 5f;
    public float movementAcc = 15f;
    public float drawCooldown = 1f;
    public float stunDuration = 0.35f;
    public Vector2 stunForce = new Vector2(100, 100);

    public bool CanDraw => Time.time - timeOfDraw >= drawCooldown && !animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName);

    private Vector2 movementDirection = Vector2.right;
    private float timeOfDraw;

    private Rigidbody2D rb;
    private EffectTools et;
    private Animator animator;
    private Collider2D col;
    private NpcNavigator navigator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        et = GetComponent<EffectTools>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        navigator = GetComponent<NpcNavigator>();
    }

    private void Start()
    {
        et.OnEffectCreated += EffectTools_OnEffectCreated;
    }

    private void OnDestroy()
    {
        et.OnEffectCreated -= EffectTools_OnEffectCreated;
    }

    private void EffectTools_OnEffectCreated(AnimationEffect animationEffect)
    {
        timeOfDraw = Time.time;

        GunEffect gunEffect = animationEffect.effect.GetComponent<GunEffect>();
        EffectTools gunEffectTools = gunEffect.GetComponent<EffectTools>();

        gunEffect.transform.position = transform.GetChild(0).position;
        gunEffect.transform.rotation = transform.rotation;
        gunEffect.transform.localScale = transform.localScale;

        gunEffect.dispatcher.OnDispatch += Dispatcher_OnDispatch;
    }

    private void Dispatcher_OnDispatch(EventAction e)
    {
        e.Dispatcher.OnDispatch -= Dispatcher_OnDispatch;
        if (onHitAction == e.Name)
            OnHit(((GunEffectAction)e).player);
    }

    private void Update()
    {
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);

        if (!CanDraw) return;

        bool mouseOver = col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (triggerByClick && Input.GetMouseButtonDown(0) && mouseOver)
            Draw(null);
    }

    private void FixedUpdate()
    {
        bool isWall = PhysicsHelper.FindWall(rb, col, movementDirection, out Wall _, distance: 1f);
        bool isGap = PhysicsHelper.FindGap(rb, col, movementDirection, out Gap _, distance: 1f);

        if (isWall || isGap)
            SwitchMovementDirection();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Drawing") && Mathf.Abs(rb.velocity.x) <= maxVelocity - movementAcc * Time.fixedDeltaTime)
            rb.velocity += movementDirection * movementAcc * Time.fixedDeltaTime;

        if (CanDraw)
            Shoot();
    }

    public void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, detectionRange);

        if (hit.collider)
            foreach (Player p in Player.ActivePlayers)
                if (hit.collider.gameObject.Equals(p.gameObject))
                    Draw(p);
    }

    protected void SwitchMovementDirection()
    {
        movementDirection = -movementDirection;
        transform.localScale = Vector2.up + Vector2.right * movementDirection;
        //rb.velocity = Vector2.up * rb.velocity;
    }

    public void Draw(Player _)
    {
        if (CanDraw)
        {
            animator.SetTrigger(drawAnimationTrigger);
            rb.velocity = Vector2.up * rb.velocity;
        }
    }

    private void OnHit(Player target)
    {
        if (target)
        {
            target.Stun(stunDuration);
            target.GetComponent<Rigidbody2D>().AddForce(stunForce * (Vector2.right * transform.localScale.x + Vector2.up));
        }
    }

}
