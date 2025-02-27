using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public GameObject marker;
    NavMeshAgent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Pursue(Vector3 location)
    {
        Vector3 targetDir = location - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));   
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().speed);
        if (target.GetComponent<Drive>().currentSpeed < 0.01f || toTarget > 90 && relativeHeading < 20)
        {
            Seek(target.transform.position);
            return;
        }
        Seek(target.transform.position + target.transform.forward * lookAhead * 5);
    }

    void Evade(Vector3 location)
    {
        Vector3 targetDir = location - transform.position;

        
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().speed);
        
        Flee(target.transform.position + target.transform.forward * lookAhead * 5);
    }
    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f,1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }
    // Update is called once per frame
    void Update()
    {
        Wander();
        marker.transform.position = agent.destination;
    }
}
