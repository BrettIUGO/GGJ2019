using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class Screen : MonoBehaviour
{
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
	}

	void OnConnect(int deviceId)
	{
		Debug.Log("Instance connected: " + deviceId);

		var message = new
		{
			type = "init",
			data = JObject.FromObject(new
			{
				avatar = "avatar" + deviceId + ".png",
				sequence = new JArray(1, 2, 3, 4, 5)
			})
		};
		AirConsole.instance.Message(deviceId, message);
	}

	void OnDisconnect(int deviceId)
	{
		Debug.Log("Instance disconnected: " + deviceId);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
