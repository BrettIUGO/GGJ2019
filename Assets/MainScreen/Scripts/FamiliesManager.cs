using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliesManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject familyPrefab;
    [Range(2, 100)]
    public int maxFamilyMemberCount = 20;
    public float tapListenDuration = 2.5f;

    private List<FamilyController> families;

    private PlayerController[] playerControllers;

    private Dictionary<int, List<PlayerController>> matchingSymbols;

    private void Awake()
    {
        families = new List<FamilyController>();

        matchingSymbols = new Dictionary<int, List<PlayerController>>();

        ScreenController.onPlayerConnect += OnPlayerConnect;
        ScreenController.onPlayerDisconnect += OnPlayerDisconnect;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllers == null)
            return;

        matchingSymbols.Clear();
        for(int i = 0; i < playerControllers.Length; ++i)
        {
            PlayerController player = playerControllers[i];
            if (playerControllers[i].tapConsumed)
            {
                //Debug.Log("Tap consumed");
                continue;
            }

            if (Time.time - playerControllers[i].lastTapTime > tapListenDuration) //Been too long since they last tapped, ignore
                continue;

            //Find which symbol they signalled and store similar players together
            int symbolIndex = player.seqence[player.lastTapIndex];
            if (!matchingSymbols.ContainsKey(symbolIndex))
                matchingSymbols[symbolIndex] = new List<PlayerController>();
            matchingSymbols[symbolIndex].Add(player);                
        }

        //Make each player with matching symbols move toewards each other
        Dictionary<int, List<PlayerController>>.Enumerator it = matchingSymbols.GetEnumerator();
        while(it.MoveNext())
        {
            List<PlayerController> matchingPlayers = it.Current.Value;
            if (matchingPlayers.Count == 1)
                continue;
            //Calculate where to go here
            Vector3 position = Vector3.zero;
            for (int i = 0; i < matchingPlayers.Count; ++i)
                position += matchingPlayers[i].transform.position;
            position /= matchingPlayers.Count;
            position.y = 0.5f;
            for (int i = 0; i < matchingPlayers.Count; ++i)
            {
                matchingPlayers[i].ConsumeTap();
                matchingPlayers[i].GetComponent<PlayerMovement>().SetDestination(position);
            }
        }
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
        if (families.Count < 2)
            return CreateFamily();
        else
        {
            //return families[0];
            int leastMemberCount = int.MaxValue;
            int leastMemberIndex = -1;
            for(int i = 0; i < families.Count; ++i)
            {
                if (families[i].PlayerCount >= maxFamilyMemberCount)
                    continue;
                if(families[i].PlayerCount < leastMemberCount)
                {
                    leastMemberCount = families[i].PlayerCount;
                    leastMemberIndex = i;
                }
            }
            if (leastMemberIndex >= 0)
                return families[leastMemberIndex];
            else
                return CreateFamily();
        }
    }

    private void OnPlayerConnect(int deviceId)
    {
        if (GameController.Instance.gameOver)
            return;

        FamilyController family = GetAvailableFamily();
        family.AddPlayer(deviceId, playerPrefab);
        playerControllers = GetComponentsInChildren<PlayerController>();
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
        else
            playerControllers = GetComponentsInChildren<PlayerController>();
    }
}
