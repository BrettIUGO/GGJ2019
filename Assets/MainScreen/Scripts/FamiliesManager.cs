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
        families = new List<FamilyController>();

        Screen.onPlayerConnect += OnPlayerConnect;
        Screen.onPlayerDisconnect += OnPlayerDisconnect;

        //Debug
        //DebugController.onDebugAddPlayer += OnPlayerConnect;
        //DebugController.onDebugRemovePlayer += OnPlayerDisconnect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private FamilyController CreateFamily()
    {
        GameObject familyObj = Instantiate(familyPrefab);
        familyObj.transform.parent = transform;
        FamilyController family = familyObj.GetComponent<FamilyController>();
        
        if (families == null)
            families = new List<FamilyController>();
        families.Add(family);

        return family;
    }

    private FamilyController GetAvailableFamily()
    {
        if (families.Count == 0)
            return CreateFamily();
        else
        {
            return families[0];
        }
    }

    private void OnPlayerConnect(int deviceId)
    {
        FamilyController family = GetAvailableFamily();
        family.AddPlayer(deviceId, playerPrefab);
    }

    private void OnPlayerDisconnect(int deviceId)
    {
        bool playerRemoved = false;
        for(int i = 0; i < families.Count; ++i)
        {
            if (playerRemoved = families[i].RemovePlayer(deviceId))
                    break;
        }
        if (!playerRemoved)
            Debug.LogWarning(string.Format("Player {0} was not found and could not be removed!", deviceId));
    }
}
