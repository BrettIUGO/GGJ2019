using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyController : MonoBehaviour
{
    private List<GameObject> players;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(GameObject playerPrefab)
    {
        MapController map = MapController.Instance;

        if (players == null)
            players = new List<GameObject>();

        GameObject player = Instantiate(playerPrefab, transform);
        players.Add(player);
        player.transform.position = new Vector3(
            Random.Range(map.minMapExtents.x, map.maxMapExtents.x), 
            0, 
            Random.Range(map.minMapExtents.y, map.maxMapExtents.y));
    }

    public void CalculateDestination()
    {
        Vector3 destination = Vector3.zero;
        for(int i = 0; i < players.Count; ++i)
        {
            destination += players[i].transform.position;
        }
        destination /= players.Count;
        destination.y = 0;
        for (int i = 0; i < players.Count; ++i)
            players[i].GetComponent<PlayerMovement>().SetDestination(destination);
    }
}
