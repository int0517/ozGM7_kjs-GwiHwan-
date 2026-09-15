using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaManager : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [Header("Stamina")]
    [SerializeField] private float staminaMaxAmount = 50;
    [SerializeField] private float staminaCurrentAmount;

    [Header("ExhaustionEffect")]
    [SerializeField] private Image exhaustionEffect;

    private PlayerController playerController;
    private bool isRunning;
    private bool isExhausted;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        isRunning = false;
        isExhausted = false;
        staminaCurrentAmount = staminaMaxAmount;
    }

    void Update()
    {
        Sprint();
        ReduceStamina();
        RecoveryStamina();
        DisplayStamina();
    }

    private void Sprint()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) isExhausted = false;
        if (staminaCurrentAmount <= 0f) isExhausted = true;

        if (isExhausted)
        {
            playerController.SetMoveSpeed(walkSpeed);
            isRunning = false;
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift) && staminaCurrentAmount > 0)
        {
            playerController.SetMoveSpeed(runSpeed);
            isRunning = true;
        }
        else
        {
            playerController.SetMoveSpeed(walkSpeed);
            isRunning = false;
        }
    }

    private void ReduceStamina()
    {
        if (!isRunning || staminaCurrentAmount <= 0) return;

        staminaCurrentAmount -= Time.deltaTime * 10f;

        if (staminaCurrentAmount < 0) staminaCurrentAmount = 0f;
    }

    private void RecoveryStamina()
    {
        float recoveryStaminaAmount = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            recoveryStaminaAmount = 0f;
        else if (playerController.IsMoving && playerController.MoveSpeed == runSpeed)
            recoveryStaminaAmount = 0f;
        else if (playerController.IsMoving && playerController.MoveSpeed == walkSpeed)
            recoveryStaminaAmount = 2f;
        else if (!playerController.IsMoving)
            recoveryStaminaAmount = 5f;

            staminaCurrentAmount += Time.deltaTime * recoveryStaminaAmount;

        if (staminaCurrentAmount > staminaMaxAmount)
            staminaCurrentAmount = staminaMaxAmount;
    }

    private void DisplayStamina()
    {
        float staminaRatio = staminaCurrentAmount / staminaMaxAmount;

        if (staminaRatio >= 0.5f)
        {
            exhaustionEffect.color = new Color(
                exhaustionEffect.color.r,
                exhaustionEffect.color.g,
                exhaustionEffect.color.b,
                0f
            );
        }
        else
        {
            float alpha = 1f - (staminaRatio * 2f);

            exhaustionEffect.color = new Color(
                exhaustionEffect.color.r,
                exhaustionEffect.color.g,
                exhaustionEffect.color.b,
                alpha
            );
        }
    }
}
