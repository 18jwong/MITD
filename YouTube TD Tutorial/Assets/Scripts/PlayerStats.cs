﻿using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 20;

    public static int Rounds;

    private void Awake()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;
    }

    public static void AddMoney(int n)
    {
        Money += n;
    }

    public static void SubtractLives(int n) {
        Lives -= n;
    }
}
