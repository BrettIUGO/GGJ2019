using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

[System.Serializable]
public struct Symbol
{
    //public string name;
    public Sprite texture;
    public Color color;
}

public class GameController : MonoBehaviour
{
    [Range(1, 100)]
    public uint pointsToWin = 10;

    public Symbol[] symbols;
    private Queue<int> shuffledSymbolIndex;

    public bool gameOver
    {
        get
        {
            return _gameOver;
        }
    }
    private bool _gameOver;

    private GameObject gameOverScreen;

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
        _gameOver = false;
        gameOverScreen = GameObject.Find("GameOverCanvas");
        gameOverScreen.SetActive(false);

        _symbolsJSON = new JArray();
        for(int i = 0; i < symbols.Length; ++i)
        {
            _symbolsJSON.Add(new JArray(symbols[i].texture.name,
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

    public void OnFamilyPointsUpdated(FamilyController family)
    {
        if (_gameOver) return;

        Debug.Log(string.Format("Family scored. {0} points", family.points));

        if(family.points >= pointsToWin)
        {
            //Game over!
            _gameOver = true;
            Vector3 centralPosition = family.GetCentralPosition();
            Vector3 delta = centralPosition - Camera.main.transform.position;
            delta *= 0.75f;
            Camera.main.transform.position = Camera.main.transform.position + delta;
            Camera.main.transform.LookAt(centralPosition);
            gameOverScreen.SetActive(true);
            gameOverScreen.GetComponent<AudioSource>().Play();
        }
    }

    public int[] GetSymbolsForFamily()
    {
        if (shuffledSymbolIndex == null)
        {

            int[] shuffledSymbolIndices = new int[GameController.Instance.symbols.Length];
            for (int i = 0; i < shuffledSymbolIndices.Length; ++i)
                shuffledSymbolIndices[i] = i;
            //Shuffle indices
            for (int i = shuffledSymbolIndices.Length - 1; i > 0; --i)
            {
                int j = Random.Range(0, i);
                int oldI = shuffledSymbolIndices[i];
                shuffledSymbolIndices[i] = shuffledSymbolIndices[j];
                shuffledSymbolIndices[j] = oldI;
            }

            shuffledSymbolIndex = new Queue<int>();
            for (int i = 0; i < shuffledSymbolIndices.Length; ++i)
                shuffledSymbolIndex.Enqueue(shuffledSymbolIndices[i]);
        }

        int[] familySymbols = new int[symbols.Length / 2];
        for (int i = 0; i < familySymbols.Length; ++i)
            familySymbols[i] = shuffledSymbolIndex.Dequeue();

        return familySymbols;
    }
}
