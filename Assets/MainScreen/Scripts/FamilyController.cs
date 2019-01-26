using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyController : MonoBehaviour
{
    public float listenTime = 2.5f;

    private Dictionary<int, PlayerMovement> familyMembers;
    private uint playerTapCount;
    private float listenStartTime;

    private void Awake()
    {
        familyMembers = new Dictionary<int, PlayerMovement>();
        playerTapCount = 0;
        listenStartTime = 0;

        Screen.onPlayerTap += OnPlayerTap;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        Screen.onPlayerTap -= OnPlayerTap;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTapCount > 0 && Time.time - listenStartTime >= listenTime)
        {
            Debug.Log("TIMEOUT");
            playerTapCount = 0;
        }
    }

    public void AddPlayer(int deviceId, GameObject playerPrefab)
    {
        MapController map = MapController.Instance;


        GameObject player = Instantiate(playerPrefab, transform);
        familyMembers.Add(deviceId, player.GetComponent<PlayerMovement>());
        player.transform.position = new Vector3(
            Random.Range(map.minMapExtents.x, map.maxMapExtents.x), 
            0, 
            Random.Range(map.minMapExtents.y, map.maxMapExtents.y));
    }

    public bool RemovePlayer(int deviceId)
    {
        if (familyMembers.ContainsKey(deviceId))
            Destroy(familyMembers[deviceId]);
        return familyMembers.Remove(deviceId);
    }

    private void OnPlayerTap(int deviceId)
    {
        if (!familyMembers.ContainsKey(deviceId))
            return;

        //Don't allow taps when moving
        if (familyMembers[deviceId].moving)
            return;

        if(playerTapCount == 0)
            listenStartTime = Time.time;
        playerTapCount++;

        Debug.Log("TAP");

        if(playerTapCount == familyMembers.Count)
        {
            Debug.Log("FAMILY MOVE");
            playerTapCount = 0;
            CalculateDestination();
        }
    }

    private void CalculateDestination()
    {
        Vector3 destination = Vector3.zero;
        Dictionary<int, PlayerMovement>.Enumerator it = familyMembers.GetEnumerator();
        while(it.MoveNext())
            destination += it.Current.Value.transform.position;
        
        destination /= familyMembers.Count;
        destination.y = 0;
        it = familyMembers.GetEnumerator();
        while (it.MoveNext())
            it.Current.Value.SetDestination(destination);
            
    }
}
