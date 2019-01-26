using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    //private GameObject[] players;

    private void Awake()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 destination = new Vector3(15, 0, -17);
        //for (int i = 0; i < players.Length; ++i)
        //    players[i].GetComponent<PlayerMovement>().SetDestination(destination);

        GameObject.Find("Families").GetComponent<FamiliesManager>().CreateFamily(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
