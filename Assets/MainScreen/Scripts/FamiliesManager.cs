using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliesManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject familyPrefab;

    private List<FamilyController> families;

    private void Awake()
    {
        Screen.onPlayerConnect += OnPlayerConnect;
        Screen.onPlayerDisconnect += OnPlayerDisconnect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFamily(uint playerCount)
    {
        GameObject familyObj = Instantiate(familyPrefab);
        familyObj.transform.parent = transform;
        FamilyController family = familyObj.GetComponent<FamilyController>();
        for (uint i = 0; i < playerCount; ++i)
            family.AddPlayer(playerPrefab);
        if (families == null)
            families = new List<FamilyController>();
        families.Add(family);

        family.CalculateDestination();
    }

    private void OnPlayerConnect(int deviceId)
    {
        //TODO add player
    }

    private void OnPlayerDisconnect(int deviceId)
    {
        //TODO remove player
    }
}
