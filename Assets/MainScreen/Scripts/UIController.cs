using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct SymbolIndicator {
    public float startTime;
    public GameObject obj;
}

public class UIController : MonoBehaviour
{
    public float iconTime = 1.0f;
    public GameObject symbolTextPrefab;

    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            return _instance;
        }
    }

    private Queue<SymbolIndicator> symbols;

    private void Awake()
    {
        _instance = this;
        symbols = new Queue<SymbolIndicator>();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public void ShowSymbol(string symbolText, Color color, Vector3 position)
    {
        SymbolIndicator symbol;
        symbol.startTime = Time.time;
        symbol.obj = Instantiate(symbolTextPrefab, transform);
        //position.y = 0;
        symbol.obj.transform.position = position;
        Vector3 localPosition = symbol.obj.GetComponent<RectTransform>().localPosition;
        localPosition.z = 0;
        symbol.obj.GetComponent<RectTransform>().localPosition = localPosition;
        Text text = symbol.obj.GetComponent<Text>();
        text.text = symbolText;
        text.color = color;
        symbols.Enqueue(symbol);
    }

    private void Update()
    {
        for (int i = 0; i < symbols.Count; ++i)
        {
            if (Time.time - symbols.Peek().startTime > iconTime)
                Destroy(symbols.Dequeue().obj);
            else
                break;
        }
    }
}
