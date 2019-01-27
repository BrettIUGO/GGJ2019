using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : PlayerController
{
    public float tapDelay = 1.0f;
    public float initialTapDelay = 2.0f;

    protected float lastTapTime = 0.0f;
    protected bool active = false;

    public override void InitSequence(int[] sequence, int startingIndex, Color color)
    {
        base.InitSequence(seqence, startingIndex, color);

        lastTapTime = Time.time + initialTapDelay - tapDelay;
        Debug.Log("AI init setting tap time to " + lastTapTime + "... current time is " + Time.time + " + " + initialTapDelay + " - " + tapDelay);
        active = true;
    }

    public void FixedUpdate()
    {
        if (active)
        {

            var now = Time.fixedTime;
            if (now >= (lastTapTime + tapDelay))
            {
                Debug.Log("AI tapping at " + now);
                Tap();
                lastTapTime = now;
            }
        }
        else
            return;

        active = !GameController.Instance.gameOver;
    }
}