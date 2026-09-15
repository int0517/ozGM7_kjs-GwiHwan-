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
    [SerializeField] private float searchAngle = 70f;

    [Header("Detect")]
    [SerializeField] private float detectRange = 15f;
    [SerializeField] private float lostPlayerTime = 3f;
    
    private float lostTimer = 0f;
    private Vector3 lastKnownPlayerPosition;
    private bool reachedLastKnownPosition = false;

    [Header("Search")]
    [SerializeField] private float searchTime = 3f;
    [SerializeField] private float lookSpeed = 90f;
    [SerializeField] private float lookAngle = 70f;


    [SerializeField] private float chaseMemoryTime = 2f;

    private float chaseMemoryTimer = 0f;

    private Quaternion searchStartRotation;
    private bool searchInitialized = false;

    private NavMeshAgent agent;

    private int currentWaypointIndex = 0;
    private float waitTimer;

    private Renderer ghostRenderer;

    private enum State
    {
        Patrol,
        Chase,
        Search
    }

    private State currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ghostRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        currentState = State.Patrol;

        if (waypoints.Length > 0 && waypoints[0] != null)
        {
            agent.SetDestination(waypoints[0].position);
        }

        ghostRenderer.material.color = Color.red;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                agent.speed = 3f;
                break;

            case State.Chase:
                Chase();
                agent.speed = 5f;
                break;

            case State.Search:
                Search();
                agent.speed = 3f;
                break;
        }
    }

    private void Patrol()
    {
        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = player.position;
            reachedLastKnownPosition = false;
            lostTimer = 0f;
            chaseMemoryTimer = 0f;

            currentState = State.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                MoveNextWaypoint();
            }
        }
    }

    private void Chase()
    {
        if (CanSeePlayer())
        {
            // 플레이어를 보고 있음
            chaseMemoryTimer = 0f;

            lastKnownPlayerPosition = player.position;

            agent.SetDestination(player.position);

            return;
        }

        // 플레이어를 놓침
        chaseMemoryTimer += Time.deltaTime;

        // 2초 동안은 마지막으로 본 위치를 계속 추적
        if (chaseMemoryTimer < chaseMemoryTime)
        {
            agent.SetDestination(lastKnownPlayerPosition);
            return;
        }

        // 2초가 지나면 마지막 위치까지 이동
        if (!reachedLastKnownPosition)
        {
            agent.SetDestination(lastKnownPlayerPosition);

            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                reachedLastKnownPosition = true;

                currentState = State.Search;

                searchInitialized = false;
                lostTimer = 0f;

                agent.ResetPath();
            }
        }
    }

    private bool CanSeePlayer()
    {
        // 1. 거리 체크
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectRange)
            return false;

        // 2. 시야각 체크
        Vector3 direction = (player.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > searchAngle)
            return false;

        // 3. 벽 체크
        Vector3 origin = transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, detectRange))
        {
            if (hit.transform == player)
            {
                Debug.DrawRay(origin, direction * detectRange, Color.green);
                return true;
            }

            Debug.DrawRay(origin, direction * hit.distance, Color.red);
        }

        return false;
    }

    private void MoveNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
            currentWaypointIndex = 0;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void Search()
    {
        if (CanSeePlayer())
        {
            currentState = State.Chase;

            lastKnownPlayerPosition = player.position;
            reachedLastKnownPosition = false;

            return;
        }

        if (!searchInitialized)
        {
            searchInitialized = true;
            searchStartRotation = transform.rotation;

            agent.ResetPath();
        }

        lostTimer += Time.deltaTime;

        float angle = Mathf.Sin(lostTimer * lookSpeed * Mathf.Deg2Rad) * lookAngle;

        transform.rotation = searchStartRotation * Quaternion.Euler(0, angle, 0);

        if (lostTimer >= searchTime)
        {
            transform.rotation = searchStartRotation;

            searchInitialized = false;
            lostTimer = 0f;

            currentState = State.Patrol;

            SetNearestWaypoint();
        }
    }

    private void SetNearestWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // 가장 가까운 웨이포인트를 현재 위치로 설정
        currentWaypointIndex = closestIndex;

        // 다음 웨이포인트로 이동
        MoveNextWaypoint();
    }
}