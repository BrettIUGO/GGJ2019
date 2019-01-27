using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float moveTime = 3.0f;
    private float _moveStartTime;

    [Range(1.0f, 25.0f)]
    public float forceModifier = 8.0f;

    private Vector3 destination;

    private Rigidbody rb;
    private bool touching;
    private Vector3 avoidanceVector;

    private void Awake()
    {
        _moveStartTime = 0;
        rb = GetComponent<Rigidbody>();
        touching = false;
    }

    public void SetDestination(Vector3 destination)
    {
        Vector3 forceDirection = destination - transform.position;
        if (forceDirection.magnitude > float.Epsilon)
        {
            forceDirection.Normalize();
            if (touching)
            {
                forceDirection = Vector3.Cross(avoidanceVector, forceDirection);
            }

            Vector3 force = forceDirection * forceModifier;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "MapElement")
        {
            avoidanceVector = collision.relativeVelocity.normalized;
            float x = avoidanceVector.x;
            avoidanceVector.x = avoidanceVector.z;
            avoidanceVector.z = -x;
            touching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "MapElement")
        {
            touching = false;
        }
    }
}
