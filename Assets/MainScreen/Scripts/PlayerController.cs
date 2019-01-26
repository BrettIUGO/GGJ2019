using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class PlayerController : MonoBehaviour
{
    public int deviceId
    {
        set
        {
            _deviceId = value;
        }
    }
    private int _deviceId;

    private FamilyController _family;
    public FamilyController family
    {
        set
        {
            _family = value;
        }
    }

    public int[] seqence
    {
        get
        {
            return _family.sequence;
        }
    }

    [SerializeField]
    private int _currentSequenceIndex;
    private float _lastTapTime;

    public float lastTapTime
    {
        get
        {
            return _lastTapTime;
        }
    }

    public void SetSequenceStartIndex(int index)
    {
        _currentSequenceIndex = index;
    }

    public int currentSequenceIndex
    {
        get
        {
            return _currentSequenceIndex;
        }
    }

    public int Tap(int sequenceLength)
    {
        _lastTapTime = Time.time;
        int index = _currentSequenceIndex++;
        _currentSequenceIndex = _currentSequenceIndex % sequenceLength;
        return _currentSequenceIndex;
    }
}
