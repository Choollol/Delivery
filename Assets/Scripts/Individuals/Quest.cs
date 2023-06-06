using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public bool isCompleted;

    private int coinReward;

    public Quest(string name, int coinReward)
    {
        this.name = name;
        this.coinReward = coinReward;
    }
}
