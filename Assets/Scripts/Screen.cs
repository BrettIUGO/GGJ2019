using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class Screen : MonoBehaviour
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

		var message = new
		{
			type = "init",
			data = JObject.FromObject(new
			{
				avatar = "orangered",
				sequence = new JArray(1, 4, 3, 2, 5),
				index = 3,
				symbols = new JArray(
					new JArray("✤", 255, 0, 0),
					new JArray("★", 255, 127, 0),
					new JArray("♥", 255, 255, 0),
					new JArray("♦", 127, 255, 0),
					new JArray("■", 0, 255, 0),
					new JArray("▲", 0, 127, 255),
					new JArray("●", 127, 0, 255),
					new JArray("❀", 255, 0, 191)
				)
			})
		};
		AirConsole.instance.Message(deviceId, message);

		if (onPlayerConnect != null)
			onPlayerConnect(deviceId);
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
