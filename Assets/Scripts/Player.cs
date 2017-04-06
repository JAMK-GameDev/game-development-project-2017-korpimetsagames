using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
	public enum PsycheState
    {
        Carefree = 1,
        Stressed = 2,
        Panic = 4,
        Paralyzed = 6,
        Berserk = 10           
    }

    private static PsycheState psyche;

    public static PsycheState Psyche
    {
        get { return psyche; }
        set { psyche = value; }
    }

    public enum MoveState
    {
        Walk,
        Sneak,
        Run
    }

    private static MoveState moveMode;

    public static MoveState MoveMode
    {
        get { return moveMode; }
        set { moveMode = value; }
    }

    private static bool isStationary;

    public static bool IsStationary
    {
        get { return isStationary; }
        set { isStationary = value; }
    }


    public static float NoiseLevel
    {
        get
        {
            float moveModeMultiplier;
            switch(moveMode)
            {
                case MoveState.Sneak:
                    moveModeMultiplier = (float)0.5;
                    break;
                case MoveState.Walk:
                    moveModeMultiplier = 1;
                    break;                
                case MoveState.Run:
                    moveModeMultiplier = 2;
                    break;
                default:
                    throw new System.Exception("Noise level defaulted.");
            }

            return moveModeMultiplier * (int)psyche;
        }
    }


    private static float fearLevel;

    public static float FearLevel
    {
        get { return fearLevel; }
        set { fearLevel = value; }
    }

    public static void ImproveState()
    {
        switch(psyche)
        {
            case PsycheState.Carefree:
                break;
            case PsycheState.Stressed:
                psyche = PsycheState.Carefree;
                break;
            case PsycheState.Panic:
                psyche = PsycheState.Stressed;
                break;
            case PsycheState.Paralyzed:
                psyche = PsycheState.Panic;
                break;
            case PsycheState.Berserk:
                psyche = PsycheState.Paralyzed;
                break;
        }
    }

    public static void WorsenState()
    {
        switch (psyche)
        {
            case PsycheState.Carefree:
                psyche = PsycheState.Stressed;
                break;
            case PsycheState.Stressed:
                psyche = PsycheState.Panic;
                break;
            case PsycheState.Panic:
                psyche = PsycheState.Paralyzed;
                break;
            case PsycheState.Paralyzed:
                psyche = PsycheState.Berserk;
                break;
            case PsycheState.Berserk:
                break;
        }
    }
}

