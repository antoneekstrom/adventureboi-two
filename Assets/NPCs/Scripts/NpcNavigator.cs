using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class NpcNavigator : MonoBehaviour
{
    public Vector2 Target { get => _target; set => SetTarget(value); }
    public float NextWaypointDistanceSquared = 9f;

    public event Action<Vector2> OnNextWaypoint;

    public bool ReachedEndOfPath { get; private set; }
    public Vector2 Waypoint { get; private set; }
    public Path Path => _path;
    public Vector2 WaypointDelta => Waypoint - (Vector2)transform.position;

    private Vector2 _target;
    private int waypointIndex = 0;
    private Path _path;

    private Seeker seeker;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    private void Start()
    {
        seeker.pathCallback += Seeker_OnPathComplete;
    }

    private void Update()
    {
        FollowPath();
    }

    private void OnDestroy()
    {
        seeker.pathCallback -= Seeker_OnPathComplete;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Waypoint, 0.5f);
    }

    protected void SetTarget(Vector2 target)
    {
        if (seeker.IsDone())
        {
            _target = target;
            seeker.StartPath(transform.position, _target);
        }
    }

    protected void Seeker_OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            waypointIndex = 0;
            Waypoint = _path.vectorPath[0];
        }
    }

    protected void FollowPath()
    {
        if (_path == null) return;

        while (true)
        {
            if (WaypointDelta.sqrMagnitude <= NextWaypointDistanceSquared)
            {
                if (waypointIndex + 1 < _path.vectorPath.Count)
                {
                    waypointIndex++;
                    Waypoint = _path.vectorPath[waypointIndex];
                    OnNextWaypoint?.Invoke(Waypoint);
                }
                else
                {
                    ReachedEndOfPath = true;
                    break;
                }
            }
            else
                break;
        }
    }

}
