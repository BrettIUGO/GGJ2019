using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public delegate void OnDebugAddPlayer(int deviceId);
    public static OnDebugAddPlayer onDebugAddPlayer;

    public delegate void OnDebugRemovePlayer(int deviceId);
    public static OnDebugRemovePlayer onDebugRemovePlayer;

    private static int nextDeviceId = 1000;
    private List<int> players;

    private void Awake()
    {
        players = new List<int>();
    }

    private void OnDestroy()
    {
        onDebugAddPlayer = null;
        onDebugRemovePlayer = null;
    }

    public void OnClickAddPlayer()
    {
        if(onDebugAddPlayer != null)
        {
            players.Add(nextDeviceId);
            onDebugAddPlayer(nextDeviceId++);
        }
    }

    public void OnClickRemovePlayer()
    {
        if(onDebugRemovePlayer != null && players.Count > 0)
        {
            onDebugRemovePlayer(players[0]);
            players.RemoveAt(0);
        }
    }
}
