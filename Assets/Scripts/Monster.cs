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
        Idle // ei merkkejä pelaajasta, palaa sijaintiin _mistä_ pelaaja nähtiin
    }

    private static MonsterState currentState;

    public static MonsterState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private static float lastDetectedPlayerTimer;

    public static float LastDetectedPlayerTimer
    {
        get { return  lastDetectedPlayerTimer; }
        set {  lastDetectedPlayerTimer = value; }
    }


    private static bool canSeePlayer = false;

    public static bool CanSeePlayer
    {
        get { return canSeePlayer; }
        set { canSeePlayer = value; }
    }

   /* private static bool onRightTrail = false;

    public static bool OnRightTrail
    {
        get { return onRightTrail = false; }
        set { onRightTrail = value; }
    }*/


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
    }
}
