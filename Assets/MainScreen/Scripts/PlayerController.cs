using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class PlayerController : MonoBehaviour
{
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
    private string _currentSymbol;
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

    public bool tapConsumed
    {
        get
        {
            return _tapConsumed;
        }
    }
    private bool _tapConsumed;

    protected void Awake()
    {
        _tapConsumed = false;
    }

    protected void OnDestroy()
    {

    }

    public void SetSequenceStartIndex(int index)
    {
        _currentSequenceIndex = index;
    }
    
    public virtual void InitSequence(int[] sequence, int startingIndex)
    {

    }

    public void Tap()
    {
        _tapConsumed = false;
        _lastTapTime = Time.time;
        _lastTapIndex = _currentSequenceIndex++;        
        _currentSequenceIndex = _currentSequenceIndex % _family.sequence.Length;
        Symbol symbol = GameController.Instance.symbols[_family.sequence[_lastTapIndex]];
        _currentSymbol = symbol.character;
        UIController.Instance.ShowSymbol(symbol.character, symbol.color, transform.position);
    }

    public void ConsumeTap()
    {
        _tapConsumed = true;
    }
}
