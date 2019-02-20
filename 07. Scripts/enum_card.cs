using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CardSuit
{
    Hearts = 0,
    Diamonds,
    Clubs,
    Spades,
    eMax,
}
[System.Serializable]
public enum CardValue
{
    Ace = 1,
    Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    eMax,
}