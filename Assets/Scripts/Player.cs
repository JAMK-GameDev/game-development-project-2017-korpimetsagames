using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player {

	public enum State
    {
        Carefree = 1,
        Stressed = 2,
        Panic = 4,
        Paralyzed = 6,
        Screaming = 10           
    }

    private static State currentState;

    public static State CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private static float fearLevel;

    public static float FearLevel
    {
        get { return fearLevel; }
        set { fearLevel = value; }
    }

    public static void ImproveState()
    {
        switch(currentState)
        {
            case State.Carefree:
                break;
            case State.Stressed:
                currentState = State.Carefree;
                break;
            case State.Panic:
                currentState = State.Stressed;
                break;
            case State.Paralyzed:
                currentState = State.Panic;
                break;
            case State.Screaming:
                currentState = State.Paralyzed;
                break;
        }
    }

    public static void WorsenState()
    {
        switch (currentState)
        {
            case State.Carefree:
                currentState = State.Stressed;
                break;
            case State.Stressed:
                currentState = State.Panic;
                break;
            case State.Panic:
                currentState = State.Paralyzed;
                break;
            case State.Paralyzed:
                currentState = State.Screaming;
                break;
            case State.Screaming:
                break;
        }
    }
}
