using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.AI;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    
    public Transform player;
    public Transform[] waypoints;
    
    public int speed;
    public int waypointIndex;
    
    private float dist;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSpotted())
        {
            dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            if (dist < 1f)
            {
                increaseIndex();
            }
            patrol();
        }
        else
        {
            agent.SetDestination(player.position);
        }

    }
 
    void patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime );
    }

    void increaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
    }

    private bool playerSpotted()
    {
        bool canSeePlayer = false;
        if (canSeePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
