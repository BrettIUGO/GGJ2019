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

    protected override void Awake()
    {
        screen = GameObject.Find("AirConsole").GetComponent<ScreenController>();
        ScreenController.onPlayerTap += OnPlayerTap;
        base.Awake();
    }

    protected override void OnDestroy()
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

    public override void InitSequence(int[] sequence, int startingIndex, Color color)
    {
        base.InitSequence(sequence, startingIndex, color);
        // FIXME: assign color to players and init the controller here
        screen.InitSequence(_deviceId, sequence, startingIndex, color);
    }

}