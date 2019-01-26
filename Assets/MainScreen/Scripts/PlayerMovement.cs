using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0.1f, 100.0f)]
    public float speed;

    private Vector3 destination;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector3 movement = (destination - transform.position);
            float distanceToGo = movement.magnitude;
            movement.Normalize();
            movement.y = 0;
            movement *= Mathf.Min(distanceToGo, speed * Time.deltaTime);
            transform.position = transform.position + movement;
            if ((destination - transform.position).magnitude < float.Epsilon)
                moving = false;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        
        if((transform.position - destination).magnitude > float.Epsilon)
        {
            this.destination = destination;
            moving = true;
        }
    }
}
