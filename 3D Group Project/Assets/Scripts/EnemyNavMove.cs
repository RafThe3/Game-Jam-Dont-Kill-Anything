using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] float chaseDistance = 10;
    [Min(0), SerializeField] private float attackInterval = 1;
    [Min(0), SerializeField] private int attackDamage = 1;

    Vector3 home;
    NavMeshAgent agent;
    private float attackTimer = 0;

    void Start()
    {
        home = transform.position;
        agent = GetComponent<NavMeshAgent>();
        attackTimer = attackInterval;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        Vector3 moveDir = player.transform.position - transform.position;
        if(moveDir.magnitude < chaseDistance ) 
        {
            agent.destination = player.transform.position;
        }
        else
        {
            agent.destination = home;
        }
    }

    private void Attack(int damage)
    {
        player.GetComponent<Player>().TakeDamage(damage);
        attackTimer = 0;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackTimer >= attackInterval)
        {
            Attack(attackDamage);
        }
    }
}
