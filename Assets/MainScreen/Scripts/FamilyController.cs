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

    [Range(0.1f, 10.0f)]
    public float detectionRange = 3;

    [Range(0.1f, 5.0f)]
    public float timePerPointCalculation = 1.0f;
    private float timeAtLastPointCalculation;

    private Dictionary<int, Player> familyMembers;

    public int[] sequence
    {
        get
        {
            return _sequence;
        }
    }
    private int[] _sequence;

    private FamilyController family;

    public uint points
    {
        get
        {
            return _points;
        }
    }
    private uint _points;

    private void Awake()
    {
        familyMembers = new Dictionary<int, Player>();

        timeAtLastPointCalculation = -timePerPointCalculation;

        GenerateSequence();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.gameOver)
            return;

        if (Time.time - timeAtLastPointCalculation >= timePerPointCalculation && familyMembers.Count > 1)
        {
            timeAtLastPointCalculation = Time.time;
            Dictionary<int, Player>.Enumerator it = familyMembers.GetEnumerator();
            int playerIndex = 1;
            while (it.MoveNext())
            {
                Dictionary<int, Player>.Enumerator jt = familyMembers.GetEnumerator();
                for (int i = 0; i < playerIndex; ++i)
                    jt.MoveNext();
                playerIndex++;
                while (jt.MoveNext())
                {
                    if (it.Current.Key == jt.Current.Key)
                        continue;
                    if ((it.Current.Value.transform.position - jt.Current.Value.transform.position).magnitude < detectionRange)
                    {
                        _points++;
                        GameController.Instance.OnFamilyPointsUpdated(this);
                        //TODO add effect
                    }
                }
            }
        }
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

        int[] shuffledSymbolIndices = new int[GameController.Instance.symbols.Length];
        for (int i = 0; i < shuffledSymbolIndices.Length; ++i)
            shuffledSymbolIndices[i] = i;
        //Shuffle indices
        for(int i = shuffledSymbolIndices.Length - 1; i > 0; --i)
        {
            int j = Random.Range(0, i);
            int oldI = shuffledSymbolIndices[i];
            shuffledSymbolIndices[i] = shuffledSymbolIndices[j];
            shuffledSymbolIndices[j] = oldI;
        }

        for(int i = 0; i < defaultSequenceLength; ++i)
        {
            _sequence[i] = shuffledSymbolIndices[i];
        }
    }

    public void AddPlayer(int deviceId, GameObject player)
    {
        MapController map = MapController.Instance;

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
                       
        Material material = new Material(player.GetComponent<MeshRenderer>().material);
        Color color = new Color(Random.value, Random.value, Random.value, 1);
        material.color = color;
        player.GetComponent<MeshRenderer>().material = material;       

        var playerController = player.GetComponent<PlayerController>();
        playerController.InitSequence(_sequence, startingIndex, color);
    }

    public bool RemovePlayer(int deviceId)
    {
        if (familyMembers.ContainsKey(deviceId))
            Destroy(familyMembers[deviceId].game.gameObject);
        return familyMembers.Remove(deviceId);
    }

    public Vector3 GetCentralPosition()
    {
        Vector3 position = Vector3.zero;
        for(int i = 0; i < familyMembers.Count; ++i)
        {
            position += familyMembers[i].transform.position;
        }
        position /= familyMembers.Count;
        return position;
    }
}
