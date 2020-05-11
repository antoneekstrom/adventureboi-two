using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angryboi : MonoBehaviour
{
    [Header("AI")]
    public AISettings aiSettings;
    public Transform target;
    public StringVariable dropkickEvent;

    [Header("functions")]
    public float stunDuration = 0.3f;
    public Vector2 stunForce = new Vector2(100, 100);
    public StringVariable attackEvent;

    private bool stunnedInAir = false;

    private readonly float patrolPointFreq = 2f;
    private float lastPatrolTime;
    private Vector2 home;

    private EventDispatcher ed;
    private IMovementController controller;
    private NpcNavigator navigator;

    private void Awake()
    {
        controller = GetComponent<IMovementController>();
        navigator = GetComponent<NpcNavigator>();
        ed = GetComponent<EventDispatcher>();
    }

    private void Start()
    {
        navigator.OnNextWaypoint += Navigator_OnNextWaypoint;
        ed.OnDispatch += Ed_OnDispatch;
        StartCoroutine(FollowTargetRoutine());

        home = transform.position;
    }

    private void Update()
    {
        if (controller.OnGround)
            stunnedInAir = false;

        controller.Freeze = stunnedInAir;
    }

    private void Ed_OnDispatch(EventAction e)
    {
        if (e.Name == dropkickEvent)
        {
            stunnedInAir = true;
        }
    }

    private void Navigator_OnNextWaypoint(Vector2 w)
    {
        controller.Target = w;
    }

    private IEnumerator FollowTargetRoutine()
    {
        if (!gameObject) yield return null;
        yield return new WaitForSecondsRealtime(0.5f);

        UpdateTarget();

        StartCoroutine(FollowTargetRoutine());
    }

    private void UpdateTarget()
    {
        float sqrDist = Player.ClosestPlayer(transform.position, out Player p);

        if (Mathf.Sqrt(sqrDist) <= aiSettings.detectionRange)
        {
            navigator.Target = p.transform.position;
        }
        else
        {
            if (Time.time - lastPatrolTime >= patrolPointFreq)
            {
                lastPatrolTime = Time.time;
                int tries = 0;
                while (tries < 10)
                {
                    tries++;
                    RaycastHit2D hit = Physics2D.Raycast(home + Vector2.right * Random.Range(-5f, 5f), Vector2.down, 10f);
                    if (hit.collider)
                        navigator.Target = hit.point;
                }
            }
        }
    }

    public void Attack(Player p)
    {
        if (!p.PlayerDropkick.IsDropkicking)
        p.Stun(stunDuration);
        p.GetComponent<Rigidbody2D>().AddForce(stunForce * (p.transform.position - transform.position).normalized, ForceMode2D.Impulse);
        ed.Dispatch(ed.CreateEventAction(attackEvent));
    }
}
