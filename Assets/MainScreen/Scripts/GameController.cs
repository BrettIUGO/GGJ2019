using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

[System.Serializable]
public struct Symbol
{
    public string character;
    public Color color;
}

public class GameController : MonoBehaviour
{
    public Symbol[] symbols;

    public JArray symbolsJSON
    {
        get
        {
            return _symbolsJSON;
        }
    }
    private JArray _symbolsJSON;
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _symbolsJSON = new JArray();
        for(int i = 0; i < symbols.Length; ++i)
        {
            _symbolsJSON.Add(new JArray(symbols[i].character,
                (int)(symbols[i].color.r * 255),
                (int)(symbols[i].color.g * 255),
                (int)(symbols[i].color.b * 255)
                ));
        }
    }



    private void OnDestroy()
    {
        _instance = null;
    }
}
