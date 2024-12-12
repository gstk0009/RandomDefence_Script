using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Vector3[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed;

    public void Initialize(float moveSpeed, Vector3[] points)
    {
        speed = moveSpeed;
        waypoints = points;
        ResetPosition();
    }

    public void ResetPosition()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0];
            currentWaypointIndex = 0;
        }
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypoints.Length == 0) return;

        Vector3 target = waypoints[currentWaypointIndex];
        Vector3 moveDir = target - transform.position;
        transform.position += moveDir.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // 다시 처음부터 시작
            }
        }
    }
}
