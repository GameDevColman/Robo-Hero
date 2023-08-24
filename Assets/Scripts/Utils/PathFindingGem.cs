using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindingGem : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody.useGravity = false;
    }

    void Update()
    {
        Vector3 newDestination = player.position;
        agent.destination = player.position;
        //agent.SetDestination(newDestination);
    }

}
