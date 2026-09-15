using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 200f;

    private CharacterController controller;
    private Renderer pRenderer;
    private Transform cam;

    private float xRotation = 0f;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        pRenderer = GetComponent<Renderer>();
        cam = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pRenderer.material.color = Color.blue;
    }

    private void Update()
    {
        Move();
        Look();
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
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") *
                       mouseSensitivity *
                       Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") *
                       mouseSensitivity *
                       Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void SetMoveSpeed(float amount)
    {
        moveSpeed = amount;
    }
}