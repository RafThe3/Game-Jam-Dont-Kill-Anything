using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMove : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] float chaseDistance = 10;
    Vector3 home;

    void Start()
    {
        home = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 moveDir = player.transform.position - transform.position;
        agent.destination = moveDir.magnitude < chaseDistance ? player.transform.position : home;
    }
}
