using UnityEngine;
using UnityEngine.UI;

public class FlashLightManager : MonoBehaviour
{
    [SerializeField] private GameObject handlight;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private ItemSO batterySO;

    private float batteryMaxAmount = 10;
    private float batteryCurrentAmount;
    private bool isLightOn;

    private void Awake()
    {
        isLightOn = false;
        batteryCurrentAmount = batteryMaxAmount;
    }

    void Update()
    {
        handlight.SetActive(isLightOn);

        ReloadBattery();
        DisplayBatteryAmount();
        LightPower();
        ReduceBattery();
    }

    private void LightPower()
    {
        if (!GameManager.Instance.CanMove) return;

        if (batteryCurrentAmount <= 0)
        {
            isLightOn = false;
            AudioManager.Instance.PlaySystemAudio(7, 0.1f);
            return;
        }

        if (Input.GetMouseButtonDown(0) && batteryCurrentAmount > 0)
        {
            AudioManager.Instance.PlaySystemAudio(7, 0.1f);
            isLightOn = !isLightOn;
        }
    }

    private void ReduceBattery()
    {
        if (!isLightOn || batteryCurrentAmount <= 0) return;

        batteryCurrentAmount -= Time.deltaTime;
    }

    private void DisplayBatteryAmount()
    {
        float batteryRatio = batteryCurrentAmount / batteryMaxAmount;
        batterySlider.value = batteryRatio;
    }

    private void ReloadBattery()
    {
        if (Input.GetKeyDown(KeyCode.R) &&
            PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(batterySO.ID))
        {
            PlayerInventoryManager.Instance.UseItem(batterySO);
            batteryCurrentAmount = batteryMaxAmount;
        }
    }

    public void ChangeLightState(bool value)
    {
        isLightOn = value;
    }
}
