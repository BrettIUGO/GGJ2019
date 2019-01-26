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
    	AirConsole.instance.onMessage += OnMessage;
    }

	void OnMessage(int from, JToken data) {
		Debug.Log("Got message");
    	AirConsole.instance.Message(from, "Full of pixels!");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
