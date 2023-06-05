using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ZombieDestination : MonoBehaviour
{
    public NavMeshAgent agent;
    
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
    
}
