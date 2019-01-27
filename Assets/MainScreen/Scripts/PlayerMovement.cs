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

    private void Awake()
    {
        _moveStartTime = 0;
        rb = GetComponent<Rigidbody>();
    }

    public void SetDestination(Vector3 destination)
    {
        if((transform.position - destination).magnitude > float.Epsilon)
        {
            Vector3 force = (destination - transform.position).normalized * forceModifier;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
