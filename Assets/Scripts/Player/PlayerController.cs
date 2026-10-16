using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5f;

    private CharacterController controller;
    private Transform cam;

    private float xRotation = 0f;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    public bool IsMoving { get; private set; }

    [Header("Floor")]
    [SerializeField] private CurrentFloor currentFloor;
    private float floor1YPos = 1f;
    private float floor2YPos = 7f;

    [Header("Footstep Audio")]
    [SerializeField] private float walkFootstepInterval = 0.5f;
    [SerializeField] private float runFootstepInterval = 0.3f;

    private float footstepTimer;
    private int footstepIndex;
    private PlayerStaminaManager staminaManager;
    private bool wasRunning;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        staminaManager = GetComponent<PlayerStaminaManager>();

        cam = Camera.main.transform;
        currentFloor = CurrentFloor.Floor1;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManager.Instance.CanMove)
        {
            Move();
            Look();
            PlayFootstep();
        }
        else
        {
            IsMoving = false;
            footstepTimer = 0f;
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(x, 0f, z);
        IsMoving = input.sqrMagnitude > 0f;

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;

        switch (currentFloor)
        {
            case CurrentFloor.Floor1:
                pos.y = floor1YPos;
                transform.position = pos;
                break;
            case CurrentFloor.Floor2:
                pos.y = floor2YPos;
                transform.position = pos;
                break;
        }
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void SetMoveSpeed(float amount)
    {
        moveSpeed = amount;
    }

    public void ChangeFloor()
    {
        if (currentFloor == CurrentFloor.Floor1)
        {
            currentFloor = CurrentFloor.Floor2;
        }
        else if (currentFloor == CurrentFloor.Floor2)
        {
            currentFloor = CurrentFloor.Floor1;
        }
    }


    private void PlayFootstep()
    {
        if (!IsMoving)
        {
            footstepTimer = 0f;
            wasRunning = staminaManager != null &&
                         staminaManager.IsRunning;
            return;
        }

        bool isRunning = staminaManager != null &&
                         staminaManager.IsRunning;

        float currentInterval = isRunning
            ? runFootstepInterval
            : walkFootstepInterval;

        // 걷기와 달리기 상태가 바뀌면 타이머를 새 간격에 맞춤
        if (isRunning != wasRunning)
        {
            footstepTimer = Mathf.Min(footstepTimer, currentInterval);
            wasRunning = isRunning;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            AudioManager.Instance.PlayPlayerAudio(footstepIndex, 0.1f);

            footstepIndex = (footstepIndex + 1) % 2;

            footstepTimer = currentInterval;
        }
    }

}