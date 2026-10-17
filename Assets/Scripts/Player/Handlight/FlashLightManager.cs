using UnityEngine;
using UnityEngine.UI;

public class FlashLightManager : MonoBehaviour
{
    [SerializeField] private GameObject handlight;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private ItemSO batterySO;

    private float batteryMaxAmount = 10f;
    private float batteryCurrentAmount;
    private bool isLightOn;
    private bool hasPlayedEmptySound;

    private void Awake()
    {
        isLightOn = false;
        batteryCurrentAmount = batteryMaxAmount;
        hasPlayedEmptySound = false;
    }

    private void Update()
    {
        handlight.SetActive(isLightOn);

        ReloadBattery();
        LightPower();
        ReduceBattery();
        DisplayBatteryAmount();
    }

    private void LightPower()
    {
        if (!GameManager.Instance.CanMove) return;

        if (Input.GetMouseButtonDown(0))
        {
            // 배터리가 없으면 전원을 켤 수 없음
            if (batteryCurrentAmount <= 0f)
            {
                isLightOn = false;
                return;
            }

            isLightOn = !isLightOn;
            AudioManager.Instance.PlaySystemAudio(7, 0.1f);
        }
    }

    private void ReduceBattery()
    {
        if (!isLightOn || batteryCurrentAmount <= 0f) return;

        batteryCurrentAmount -= Time.deltaTime;

        if (batteryCurrentAmount <= 0f)
        {
            batteryCurrentAmount = 0f;
            isLightOn = false;

            // 배터리가 방전되는 순간 한 번만 재생
            if (!hasPlayedEmptySound)
            {
                hasPlayedEmptySound = true;
                AudioManager.Instance.PlaySystemAudio(7, 0.1f);
            }
        }
    }

    private void DisplayBatteryAmount()
    {
        batterySlider.value = batteryCurrentAmount / batteryMaxAmount;
    }

    private void ReloadBattery()
    {
        if (Input.GetKeyDown(KeyCode.R) &&
            PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(batterySO.ID))
        {
            PlayerInventoryManager.Instance.UseItem(batterySO);

            batteryCurrentAmount = batteryMaxAmount;
            hasPlayedEmptySound = false;
        }
    }

    public void ChangeLightState(bool value)
    {
        if (value && batteryCurrentAmount <= 0f)
            return;

        isLightOn = value;
    }
}