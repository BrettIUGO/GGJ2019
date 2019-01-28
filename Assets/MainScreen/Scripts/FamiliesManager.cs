using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliesManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject computerPrefab;
    public GameObject familyPrefab;
    [Range(2, 100)]
    public int maxFamilyMemberCount = 20;
    public float tapListenDuration = 2.5f;

    public float aiMinInitialDelay = 2.0f;
    public float aiMaxInitialDelay = 3.0f;
    public float aiMinDelay = 0.75f;
    public float aiMaxDelay = 1.5f;

    private List<FamilyController> families;

    private PlayerController[] playerControllers;

    private Dictionary<int, List<PlayerController>> matchingSymbols;
    private bool aiInitialized = false;
    private bool resetting;

    private void Awake()
    {
        families = new List<FamilyController>();
        resetting = false;
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
                //if (families[i].PlayerCount >= maxFamilyMemberCount)
                //    continue;
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
        if (!resetting && GameController.Instance.gameOver)
            return;

        GameObject player = Instantiate(playerPrefab);
        var playerController = player.GetComponent<HumanPlayerController>();
        playerController.deviceId = deviceId;

        FamilyController family = GetAvailableFamily();
        player.transform.parent = family.transform;
        family.AddPlayer(deviceId, player);
        playerControllers = GetComponentsInChildren<PlayerController>();

        if(!aiInitialized)
        {
            AddAIPlayer(100001);
            AddAIPlayer(100002);
            aiInitialized = true;
        }
    }

    public void AddAIPlayer(int deviceId)
    {
        GameObject player = Instantiate(computerPrefab);
        var playerController = player.GetComponent<AIPlayerController>();
        playerController.initialTapDelay = Random.Range(aiMinInitialDelay, aiMaxInitialDelay);
        playerController.tapDelay = Random.Range(aiMinDelay, aiMaxDelay);

        FamilyController family = GetAvailableFamily();
        player.transform.parent = family.transform;
        family.AddPlayer(deviceId, player);
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

    public void ResetGame()
    {
        resetting = true;
        List<int> deviceIds = new List<int>();
        for(int i = 0; i < families.Count; ++i)
        {
            deviceIds.AddRange(families[i].GetDeviceIds());
            //while (families[i].transform.childCount > 0)
            //    Destroy(families[i].transform.GetChild(0).gameObject);
            Destroy(families[i].gameObject);
        }
        families.Clear();
        aiInitialized = false;
        for (int i = 0; i < deviceIds.Count; ++i)
        {
            if (deviceIds[i] == -1)
                continue;
            OnPlayerConnect(deviceIds[i]);
        }

        resetting = false;
    }
}
