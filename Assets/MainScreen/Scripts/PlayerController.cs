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

    private int _lastTapIndex;
    public int lastTapIndex
    {
        get
        {
            return _lastTapIndex;
        }
    }

    public void SetSequenceStartIndex(int index)
    {
        _currentSequenceIndex = index;
    }

    //public int currentSequenceIndex
    //{
    //    get
    //    {
    //        return _currentSequenceIndex;
    //    }
    //}

    public void Tap(int sequenceLength)
    {
        _lastTapTime = Time.time;
        _lastTapIndex = _currentSequenceIndex++;
        _currentSequenceIndex = _currentSequenceIndex % sequenceLength;
        Symbol symbol = GameController.Instance.symbols[_family.sequence[_lastTapIndex]];
        UIController.Instance.ShowSymbol(symbol.character, symbol.color, transform.position);
    }
}
