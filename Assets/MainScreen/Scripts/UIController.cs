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
    public float startAlpha = 0.75f;

    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            return _instance;
        }
    }

    private Queue<SymbolIndicator> symbols;

    void Awake()
    {
        _instance = this;
        symbols = new Queue<SymbolIndicator>();
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void ShowSymbol(Sprite symbolTexture, Color color, Vector3 position)
    {
        SymbolIndicator symbol;
        symbol.startTime = Time.time;
        symbol.obj = Instantiate(symbolTextPrefab, transform);
        symbol.obj.transform.position = position;
        Vector3 localPosition = symbol.obj.GetComponent<RectTransform>().localPosition;
        localPosition.z = 0;
        symbol.obj.GetComponent<RectTransform>().localPosition = localPosition;
        
        Image image = symbol.obj.GetComponent<Image>();
        image.sprite = symbolTexture;
        color.a = startAlpha;
        image.color = color;
        Debug.Log(symbolTexture.name);

        symbols.Enqueue(symbol);
    }

    void Update()
    {       
        for (int i = 0; i < symbols.Count; ++i)
        {
            if (Time.time - symbols.Peek().startTime > iconTime)
                Destroy(symbols.Dequeue().obj);
            else
                break;
        }

        Queue<SymbolIndicator>.Enumerator it = symbols.GetEnumerator();
        while (it.MoveNext())
        {
            Image image = it.Current.obj.GetComponent<Image>();
            Color color = image.color;
            color.a = Mathf.Lerp(startAlpha, 0, (Time.time - it.Current.startTime) / iconTime);
            image.color = color;
        }
    }
}
