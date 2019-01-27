using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public struct GameMessage
{
    public enum MessageType {
        FamilySequenceLength = 0,
        PlayerStartingIndex
    };

    public MessageType type;
    public object value;

    public GameMessage(MessageType type, object value)
    {
        this.type = type;
        this.value = value;
    }
}

public class ScreenController : MonoBehaviour
{
	//Network/Game Interface Methods
	public delegate void OnPlayerConnect(int deviceId);
	public static OnPlayerConnect onPlayerConnect;

	public delegate void OnPlayerDisconnect(int deviceId);
	public static OnPlayerDisconnect onPlayerDisconnect;

    public delegate void OnPlayerTap(int deviceId);
    public static OnPlayerTap onPlayerTap;

    private void OnDestroy()
    {
        onPlayerConnect = null;
        onPlayerDisconnect = null;
        onPlayerTap = null;
    }

	// Start is called before the first frame update
	void Start()
	{
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
		AirConsole.instance.onMessage += OnMessage;
	}

	void OnMessage(int deviceId, JToken data)
	{
		Debug.Log("Got message of type: " + data["type"] + " from " + deviceId);
        if (onPlayerTap != null)
            onPlayerTap(deviceId);
	}

	void OnConnect(int deviceId)
	{
		Debug.Log("Instance connected: " + deviceId);
        	

		if (onPlayerConnect != null)
			onPlayerConnect(deviceId);
	}

    public void InitPlayer(int deviceId, int[] sequence, int startingIndex)
    {
        var message = new
        {
            type = "init",
            data = JObject.FromObject(new
            {
                avatar = "orangered",
                sequence = new JArray(sequence),
                index = startingIndex,
                symbols = GameController.Instance.symbolsJSON
            })
        };
        AirConsole.instance.Message(deviceId, message);
    }

	void OnDisconnect(int deviceId)
	{
		Debug.Log("Instance disconnected: " + deviceId);
		if (onPlayerDisconnect != null)
			onPlayerDisconnect(deviceId);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
