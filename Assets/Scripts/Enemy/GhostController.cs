using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class GhostController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private float runningDetectRange = 8f;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints = new Transform[4];
    [SerializeField] private float waitTime = 2f;

    [Header("Detect")]
    [SerializeField] private float detectRange = 15f;
    [SerializeField] private float searchAngle = 70f;
    [SerializeField] private float chaseMemoryTime = 2f;

    [Header("Search")]
    [SerializeField] private float searchTime = 3f;
    [SerializeField] private float lookSpeed = 90f;
    [SerializeField] private float lookAngle = 70f;

    [Header("Speed")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float searchSpeed = 3f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform rootPos;
    [SerializeField] private Transform ghostRestartPos;
    [SerializeField] private Transform playerRestartPos;
    [SerializeField] private Image blackScreen;

    private NavMeshAgent agent;

    private GhostState currentState;

    private GhostPatrolState patrolState;
    private GhostChaseState chaseState;
    private GhostSearchState searchState;
    private GhostAttackState attackState;

    private int currentWaypointIndex = 0;

    private GhostAudioController ghostAudio;

    private bool isActive;

    [Header("Reward")]
    [SerializeField] private GameObject rewardKey;
    [SerializeField] private StairsRoomDoorManager door;
    [SerializeField] private Transform keyPos;

    [SerializeField] private GhostAudioController ghostAudioController;


    public GhostStateEnum CurrentState { get; private set; }
    public bool IsRestarting { get; private set; }
    public Transform Player => player;
    public NavMeshAgent Agent => agent;
    public Transform[] Waypoints => waypoints;
    public float WaitTime => waitTime;
    public float DetectRange => detectRange;
    public float SearchAngle => searchAngle;
    public float ChaseMemoryTime => chaseMemoryTime;
    public float SearchTime => searchTime;
    public float LookSpeed => lookSpeed;
    public float LookAngle => lookAngle;
    public float PatrolSpeed => patrolSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float SearchSpeed => searchSpeed;
    public bool IsActive => isActive;
    public Transform RootPos => rootPos;
    public Transform GhostRestartPos => ghostRestartPos;
    public Transform PlayerRestartPos => playerRestartPos;
    public Image BlackScreen => blackScreen;
    public GhostAudioController GhostAudio => ghostAudio;
    public GameObject RewardKey => rewardKey;
    public StairsRoomDoorManager Door => door;
    public GhostAudioController GhostAudioController => ghostAudioController;
    public Transform KeyPos => keyPos;


    public int CurrentWaypointIndex
    {
        get => currentWaypointIndex;
        set => currentWaypointIndex = value;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ghostAudio = GetComponent<GhostAudioController>();

        patrolState = new GhostPatrolState(this);
        chaseState = new GhostChaseState(this);
        searchState = new GhostSearchState(this);
        attackState = new GhostAttackState(this);

        isActive = false;

        agent.isStopped = true;
        agent.ResetPath();

        currentState = patrolState;
        CurrentState = GhostStateEnum.Patrol;
    }

    private void Update()
    {
        if (!isActive || IsRestarting) return;

        if (CurrentState != GhostStateEnum.Attack && IsPlayerInAttackRange())
        {
            ChangeState(GhostStateEnum.Attack);
            return;
        }

        currentState?.Update();
    }

    public void ChangeState(GhostStateEnum newState)
    {
        if (CurrentState == newState) return;

        currentState?.Exit();

        CurrentState = newState;
        currentState = newState switch
        {
            GhostStateEnum.Patrol => patrolState,
            GhostStateEnum.Chase => chaseState,
            GhostStateEnum.Search => searchState,
            GhostStateEnum.Attack => attackState,
            _ => null
        };

        currentState?.Enter();
        ghostAudioController?.PlayForState(newState);
    }

    public bool CanSeePlayer()
    {
        if (player == null)
            return false;

        // 감지 시작점과 플레이어의 상체 위치
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 targetPoint = player.position + Vector3.up * 1f;

        Vector3 direction = targetPoint - origin;
        float distance = direction.magnitude;

        // 감지 거리 확인
        if (distance > detectRange)
            return false;

        direction.Normalize();

        // 시야각 확인
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > searchAngle)
            return false;

        // 플레이어까지 Raycast
        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            distance))
        {
            bool isPlayerHit =
                hit.transform == player ||
                hit.transform.IsChildOf(player) ||
                player.IsChildOf(hit.transform);

            return isPlayerHit;
        }

        return false;
    }

    public void MoveNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length) currentWaypointIndex = 0;

        if (waypoints[currentWaypointIndex] != null)
            agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void SetNearestWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            float distance = Vector3.Distance(transform.position, waypoints[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentWaypointIndex = closestIndex;

        MoveNextWaypoint();
    }

    public void GhostActive()
    {
        isActive = true;
        agent.isStopped = false;

        ChangeState(GhostStateEnum.Patrol);
    }

    public bool IsPlayerInAttackRange()
    {
        if (Player == null) return false;

        float distance = Vector3.Distance(
            transform.position,
            Player.position
        );

        return distance <= attackRange;
    }

    public Coroutine StartGhostCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void StopGhostCoroutine(Coroutine routine)
    {
        if (routine != null)
            StopCoroutine(routine);
    }

    public void SetRestarting(bool value)
    {
        IsRestarting = value;
    }

    public bool CanHearRunningPlayer()
    {
        if (Player == null || !Player.TryGetComponent<PlayerController>(out var playerController))
            return false;

        if (!playerController.IsRunning)
            return false;

        float distance = Vector3.Distance(transform.position, Player.position);

        return distance <= runningDetectRange;
    }
}