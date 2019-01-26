using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float moveTime = 3.0f;
    private float _moveStartTime;

    [Range(0.1f, 100.0f)]
    public float speed;

    private Vector3 destination;
    public bool moving
    {
        get
        {
            return _moving;
        }
    }
    private bool _moving;

    private void Awake()
    {
        _moveStartTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_moving)
        {
            if (Time.time - _moveStartTime > moveTime)
            {
                _moving = false;
            }
            else
            {
                Vector3 movement = (destination - transform.position);
                float distanceToGo = movement.magnitude;
                movement.Normalize();
                movement.y = 0;
                movement *= Mathf.Min(distanceToGo, speed * Time.deltaTime);
                transform.position = transform.position + movement;
                if ((destination - transform.position).magnitude < float.Epsilon)
                    _moving = false; //They've arrived!
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        
        if((transform.position - destination).magnitude > float.Epsilon)
        {
            this.destination = destination;
            _moveStartTime = Time.time;
            _moving = true;
        }
    }
}
