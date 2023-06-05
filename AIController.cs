using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{
    
    // Patrol Variables
    private GameObject player;
    public Transform[] waypoints;
    
    public int speed;
    public int waypointIndex;
    
    private float dist;

    public NavMeshAgent agent;
    
    // Field of View Variables
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;


    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        DetermineBehaviour();
    }

    private void DetermineBehaviour()
    {
        // if the player hasn't been spotted, check if they are in view of the warden
        if (!canSeePlayer)
            FieldOfViewCheck();
        
        // if the player has been spotted –> chase them
        if (canSeePlayer)
            agent.SetDestination(player.transform.position);
        // otherwise, patrol
        else
        {
            dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            
            if (dist < 1f)
                IncreaseIndex();

            Patrol();
        }
    }
    
    // go to the next waypoint
    private void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime );
    }

    // increase the 
    private void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.position, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
            
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
    
}
