using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

// Author
// Prometheus

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    public int walkSpeed;
    public int runSpeed;
    public AudioClip damaged1;
    public AudioClip damaged2;
    public AudioClip damaged3;
    public AudioClip attack;
    public AudioClip excited;
    public AudioClip die;
    private float surveyTimeLimit;   // kuinka kauan monsteri kääntyilee korkeintaan
    private float turnSpeed;        // kääntymisnopeus
    private float surveyTimer;      // kuinka kauan monsteri on kääntyillyt paikallaan
    private float surveyStateChangeTimer;
    private float surveyStateChangeTimeLimit;
    private float seed;
    private int totalSearches;
    private int surveyCount;
    private bool wasTurning;
    private bool searchLocationsAdded;
    private Transform body;
    private Transform monster;
    private NavMeshAgent navMeshAgent;      
    private List<Vector3> pointsOfInterest;
    private MonsterMacroBehavior macroBehavior;
    private Animator animator;
    private bool hasCaughtPlayer;
    private float audioCooldown;
    private float timeSinceLastAudioPlayed;
    private AudioClip[] damagedClips;
    private bool canPlaySound;

    public enum SurveyState
    {
        LookLeft,
        LookRight,
        LookForward
    }

    private SurveyState surveyState;    
    
    void Start()
    {
        canPlaySound = true;
        damagedClips = new AudioClip[3];
        damagedClips[0] = damaged1;
        damagedClips[1] = damaged2;
        damagedClips[2] = damaged3;
        timeSinceLastAudioPlayed = 0;
        audioCooldown = 10;
        hasCaughtPlayer = false;
        animator = GetComponent<Animator>();
        animator.SetTrigger("spawn");
        Monster.Health = 4;
        totalSearches = Random.Range(3, 6);
        surveyTimeLimit = Random.Range(4, 7);
        surveyTimer = 0;
        surveyCount = 0;
        surveyStateChangeTimer = 0;
        surveyStateChangeTimeLimit = 1;
        turnSpeed = Random.Range(50, 200);
        searchLocationsAdded = false;
        monster = transform;        
        surveyState = SurveyState.LookForward;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        macroBehavior = GetComponent<MonsterMacroBehavior>();
        body = monster.FindChild("Body");
        Monster.CurrentState = Monster.MonsterState.Idle;
        Monster.CanSeePlayer = false;
        Monster.OriginalPos = monster.position;
        Monster.LastKnownPlayerPosition = player.position;
    }
    
    void Update()
    {
        animator.SetFloat("moveSpeed",navMeshAgent.velocity.sqrMagnitude);
        animator.SetFloat("animationSpeed", navMeshAgent.velocity.sqrMagnitude/50);        

        switch (Monster.Mood)
        {
            case Monster.Mindset.Calm: navMeshAgent.speed = walkSpeed; break;
            case Monster.Mindset.Excited: navMeshAgent.speed = runSpeed; break;
            default: break;
        }

        switch (Monster.CurrentState)
        {
            case Monster.MonsterState.Chase: Chase(); break;
            case Monster.MonsterState.Idle: Idle(); break;
            case Monster.MonsterState.Investigate: Investigate(); break;
            case Monster.MonsterState.Survey: Survey(); break;
            case Monster.MonsterState.Search: Search(); break;
            default: return;
        }
    }

    public void GetHit()
    {
        if(Monster.CurrentState == Monster.MonsterState.Dead){ return; }                
        Monster.ReduceHealth();
        if (Monster.Health <= 0){ Die(); return; }
        
        Monster.LearnPlayerPosition(player.position);
        animator.SetTrigger("isHit");
        MakeSound(damagedClips[Random.Range(0, 3)]);
    }

    private void Die()
    {
        MakeSound(die);
        GetComponent<MonsterHearing>().enabled = false;
        GetComponent<MonsterSight>().enabled = false;
        GetComponent<MonsterMacroBehavior>().enabled = false;
        AudioSource[] audios = GetComponents<AudioSource>();
        audios[1].enabled = false;
        Monster.CurrentState = Monster.MonsterState.Dead;
        animator.SetTrigger("dead");
        this.enabled = false;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EndingKillMonster();
    }

    public void ResetSurvey()
    {
        surveyStateChangeTimer = 0;
        surveyTimer = 0;
        surveyCount = 0;
        searchLocationsAdded = false;
        totalSearches = Random.Range(3, 6);
        surveyTimeLimit = Random.Range(4, 7);
    }

    private void CatchPlayer()
    {
        if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().godmode)
        {
            animator.SetTrigger("attack");
            MakeSound(attack);
            hasCaughtPlayer = true;
            this.enabled = false;
            navMeshAgent.Stop();
            navMeshAgent.velocity = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EndingDie();
            GameObject.FindObjectOfType<FirstPersonController>().Die();
        }
    }



    #region STATEMACHINE_STATES
    private void Chase()
    {
        if (Vector3.Distance(monster.position, player.position) < 3.2 && !hasCaughtPlayer){ CatchPlayer(); }
        timeSinceLastAudioPlayed += Time.deltaTime;
        if (timeSinceLastAudioPlayed > audioCooldown)
        {
            canPlaySound = true;
        }
        if (canPlaySound)
        {
            MakeSound(excited);
            timeSinceLastAudioPlayed = 0;
            canPlaySound = false;
        }

        Monster.OriginalPos = monster.position;

        if (!Monster.CanSeePlayer)
        {
            Monster.CurrentState = Monster.MonsterState.Investigate;
            return;
        }
        
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;
    }

    private void Investigate()
    {
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;
        
        if(PathComplete())
        {
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;         
        }
    }

    private void Survey()
    {
        if (surveyCount == totalSearches)
        {
            Monster.CurrentState = Monster.MonsterState.Idle;
            ResetSurvey();
            return;
        }

        // jos on katseltu ympäriinsä tässä pisteessä riittävän kauan 
        // wasturning siksi ettei monsteri sinkoa muualle kesken kääntymisen (kuuluu olla true)
        if (surveyTimer > surveyTimeLimit && wasTurning)
        {
            surveyTimer = 0;
            surveyCount++;
            Monster.CurrentState = Monster.MonsterState.Search;
            return;
        }
        
        // TODO: innostusasteet jotka määrittää kääntymisen nopeutta ja tiheyttä

        if(surveyStateChangeTimer > surveyStateChangeTimeLimit)
        {                       
            turnSpeed = Random.Range(50, 200);
            surveyStateChangeTimeLimit = Random.Range(200, 400) / (turnSpeed*2); // jos käännytään nopeasti, ei käännytä kauan
            surveyStateChangeTimer = 0;            

            if(wasTurning)
            {
                surveyState = SurveyState.LookForward;
            }
            else
            {
                // randomoidaan suunta
                seed = Random.Range(0, 100);
                if (seed >= 50) surveyState = SurveyState.LookLeft;
                else surveyState = SurveyState.LookRight;
            }
        }

        switch (surveyState)
        {
            case SurveyState.LookLeft:
                wasTurning = true;
                monster.Rotate(Vector3.down * Time.deltaTime * turnSpeed);
                break;
            case SurveyState.LookRight:
                wasTurning = true;
                monster.Rotate(-Vector3.down * Time.deltaTime * turnSpeed);
                break;
            case SurveyState.LookForward:
                wasTurning = false;
                break;
            default: return;
        }

        surveyStateChangeTimer += Time.deltaTime;
        surveyTimer += Time.deltaTime;
    }

    private void Search()
    {        
        if(!searchLocationsAdded)
        {
            pointsOfInterest = new List<Vector3>();            

            // randomoidaan lokaatioita xz-akseleilla ja haetaan niille oikeat korkeudet terrainista
            for (int i = 0; i <= totalSearches; i++)
            {                
                pointsOfInterest.Add(macroBehavior.BuildPointOfInterest(5,12));
            }

            searchLocationsAdded = true;
        }

        navMeshAgent.destination = pointsOfInterest[surveyCount];

        if (PathComplete())
        {
            surveyState = SurveyState.LookForward;
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;
        }
        else if(navMeshAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            pointsOfInterest[surveyCount] = macroBehavior.BuildPointOfInterest(5, 12);
        }
    }

    private void Idle()
    {
        navMeshAgent.destination = Monster.OriginalPos;
    }

    #endregion

    private void MakeSound(AudioClip clip)
    {
        monster.GetComponent<AudioSource>().clip = clip;
        monster.GetComponent<AudioSource>().Play();
    }

    private bool PathComplete()
    {
        float distance = Vector3.Distance(monster.position, navMeshAgent.destination);
        // remainingDistance päivittyy vasta yhden framen päästä siitä kun määränpää on asetettu.
        if (distance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending && (navMeshAgent.velocity.sqrMagnitude == 0f || !navMeshAgent.hasPath))
        {
            return true;
        }
        else return false;
    }
}