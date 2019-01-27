using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class HumanPlayerController : PlayerController
{
        public int deviceId
    {
        set
        {
            _deviceId = value;
        }
    }
    private int _deviceId;

    private ScreenController screen;

    protected void Awake()
    {

        screen = GameObject.Find("AirConsole").GetComponent<ScreenController>();
        ScreenController.onPlayerTap += OnPlayerTap;
        base.Awake();
    }

    protected void OnDestroy()
    {
        ScreenController.onPlayerTap -= OnPlayerTap;
        base.OnDestroy();
    }

        private void OnPlayerTap(int deviceId)
    {
        if(deviceId != _deviceId)
            return;
            
        Tap();
    }

    public override void InitSequence(int[] sequence, int startingIndex)
    {
        base.InitSequence(sequence, startingIndex);
        screen.InitSequence(_deviceId, sequence, startingIndex);
    }

}