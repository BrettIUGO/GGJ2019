using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Symbol
{
    string character;
    Color color;

    public Symbol(string character, Color color)
    {
        this.character = character;
        this.color = color;
    }
}

public class GameController : MonoBehaviour
{
    public static Symbol[] symbols = {
        new Symbol("a", Color.red),
        new Symbol("b", Color.green)
    };
}
