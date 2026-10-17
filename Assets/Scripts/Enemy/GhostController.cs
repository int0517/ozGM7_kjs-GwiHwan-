using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GhostController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;

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

    private NavMeshAgent agent;

    private GhostState currentState;

    private GhostPatrolState patrolState;
    private GhostChaseState chaseState;
    private GhostSearchState searchState;

    private int currentWaypointIndex = 0;

    private GhostAudioController ghostAudio;

    private bool isActive;
    [SerializeField] private GameObject rewardKey;
    [SerializeField] private StairsRoomDoorManager door;

    public GhostStateEnum CurrentState { get; private set; }
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
    public GhostAudioController GhostAudio => ghostAudio;
    public GameObject RewardKey => rewardKey;
    public StairsRoomDoorManager Door => door;

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

        isActive = false;

        agent.isStopped = true;
        agent.ResetPath();
    }

    private void Update()
    {
        if (!isActive) return;

        currentState?.Update();
    }

    public void ChangeState(GhostStateEnum newState)
    {
        currentState?.Exit();

        CurrentState = newState;

        currentState = newState switch
        {
            GhostStateEnum.Patrol => patrolState,
            GhostStateEnum.Chase => chaseState,
            GhostStateEnum.Search => searchState,
            _ => null
        };

        currentState?.Enter();
    }

    public bool CanSeePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Ghost: Player가 연결되지 않았습니다.");
            return false;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > detectRange)
            return false;

        Vector3 direction =
            (player.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > searchAngle)
            return false;

        Vector3 origin = transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(
            origin, direction, out RaycastHit hit, detectRange))
        {
            Debug.DrawRay(
                origin,
                direction * hit.distance,
                hit.transform == player ? Color.green : Color.red
            );

            Debug.Log($"Ghost 감지 Raycast: {hit.transform.name}");

            if (hit.transform == player)
                return true;
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
}