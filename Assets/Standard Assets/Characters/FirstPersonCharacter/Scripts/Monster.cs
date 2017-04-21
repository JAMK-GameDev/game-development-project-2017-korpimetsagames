using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Monster {

    public enum MonsterState
    {
        Chase, // näkee pelaajan ja jahtaa
        Investigate, // ei näe pelaajaa, liikkuu sinne missä pelaaja viimeksi havaittiin        
        Survey, // saapunut sinne missä pelaaja viimeksi havaittu, katselee ympärilleen mutta ei liiku
        Search, // liikkuu ja tutkii lähimaastoa
        Idle, // ei merkkejä pelaajasta, palaa sijaintiin _mistä_ pelaaja nähtiin
        Dead // slack
    }

    private static MonsterState currentState;

    public enum Mindset
    {
        Calm,
        Excited        
    }

    private static Mindset mood;

    public static Mindset Mood
    {
        get { return mood; }
        set { mood = value; }
    }

    public static MonsterState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private static int health;

    public static int Health
    {
        get { return health; }
        set { health = value; }
    }

    private static float lastDetectedPlayerTimer;

    public static float LastDetectedPlayerTimer
    {
        get { return  lastDetectedPlayerTimer; }
        set {  lastDetectedPlayerTimer = value; }
    }

    private static bool canSeePlayer;

    public static bool CanSeePlayer
    {
        get { return canSeePlayer; }
        set { canSeePlayer = value; }
    }

    private static Vector3 originalPos;
    public static Vector3 OriginalPos
    {
        get { return originalPos; }
        set { originalPos = value; }
    }
    private static Vector3 lastKnownPlayerPosition;
    public static Vector3 LastKnownPlayerPosition
    {
        get { return lastKnownPlayerPosition; }
        set { lastKnownPlayerPosition = value; }
    }

    public static void LearnPlayerPosition(Vector3 playerPosition)
    {
        lastKnownPlayerPosition = playerPosition;
        currentState = MonsterState.Investigate;
    }

    public static void ReduceHealth()
    {
        health = health - 1;
    }
}
