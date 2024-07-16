using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float chaseDistance = 10f;
    public float stopDistance = 2f;
    public float moveSpeed = 3f;
    public float interval = 0.3f;


    NavMeshAgent agent;
    float distanceToPlayer;
    float tempTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void FixedUpdate()
    {
        tempTime += Time.fixedDeltaTime;
        if (tempTime > interval)
        {

            Vector3 direction = player.position - transform.position;
            distanceToPlayer = Vector3.SqrMagnitude(direction);
            if (distanceToPlayer <= chaseDistance * chaseDistance)
            {
                if (distanceToPlayer > stopDistance * stopDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
            }
            tempTime = 0;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
