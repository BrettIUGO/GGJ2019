using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Player
{
    public PlayerMovement movement;
    public PlayerController game;
    public Transform transform
    {
        get
        {
            return movement.transform;
        }
    }
}

public class FamilyController : MonoBehaviour
{
    [Range(3, 10)]
    public int defaultSequenceLength = 6;

    private Dictionary<int, Player> familyMembers;

    private Screen screen;

    public int[] sequence
    {
        get
        {
            return _sequence;
        }
    }
    private int[] _sequence;

    private FamilyController family;

    private void Awake()
    {
        familyMembers = new Dictionary<int, Player>();

        Screen.onPlayerTap += OnPlayerTap;

        screen = GameObject.Find("AirConsole").GetComponent<Screen>();

        GenerateSequence();
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

        //TODO: Win condition here
    }

    public int PlayerCount
    {
        get
        {
            return familyMembers.Count;
        }
    }

    private void GenerateSequence()
    {
        _sequence = new int[defaultSequenceLength];
        for(int i = 0; i < defaultSequenceLength; ++i)
        {
            _sequence[i] = Random.Range(0, GameController.Instance.symbols.Length - 1);
        }
    }

    public void AddPlayer(int deviceId, GameObject playerPrefab)
    {
        MapController map = MapController.Instance;


        GameObject player = Instantiate(playerPrefab, transform);
        Player playerData;
        playerData.movement = player.GetComponent<PlayerMovement>();
        playerData.game = player.GetComponent<PlayerController>();
        playerData.game.family = this;

        familyMembers.Add(deviceId, playerData);
        player.transform.position = new Vector3(
            Random.Range(map.minMapExtents.x, map.maxMapExtents.x), 
            0.5f, 
            Random.Range(map.minMapExtents.y, map.maxMapExtents.y));

        int startingIndex = Random.Range(0, defaultSequenceLength - 1);
        playerData.game.SetSequenceStartIndex(startingIndex);
        screen.InitPlayer(deviceId, _sequence, startingIndex);
    }

    public bool RemovePlayer(int deviceId)
    {
        if (familyMembers.ContainsKey(deviceId))
            Destroy(familyMembers[deviceId].game.gameObject);
        return familyMembers.Remove(deviceId);
    }

    private void OnPlayerTap(int deviceId)
    {
        if (!familyMembers.ContainsKey(deviceId))
            return;

        //Don't allow taps when moving
        if (familyMembers[deviceId].movement.moving)
            return;

        familyMembers[deviceId].game.Tap(defaultSequenceLength);
    }
}
