﻿using System.Collections;
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
    [Range(1, 100)]
    public uint pointsToWin = 10;

    public Symbol[] symbols;

    public bool gameOver
    {
        get
        {
            return _gameOver;
        }
    }
    public bool _gameOver;

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
        }
    }
}
