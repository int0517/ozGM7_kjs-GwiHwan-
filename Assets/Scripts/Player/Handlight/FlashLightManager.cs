using UnityEngine;
using UnityEngine.UI;

public class FlashLightManager : MonoBehaviour
{
    [SerializeField] private GameObject handlight;
    [SerializeField] private Slider batterySlider;

    private bool isLightOn;
    private float batteryMaxAmount = 10;
    private float batteryCurrentAmount;

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
        if (batteryCurrentAmount <= 0)
        {
            isLightOn = false;
            return;
        }

        if (Input.GetMouseButtonDown(0) && batteryCurrentAmount > 0) isLightOn = !isLightOn;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            batteryCurrentAmount = batteryMaxAmount;
        }
    }
}
