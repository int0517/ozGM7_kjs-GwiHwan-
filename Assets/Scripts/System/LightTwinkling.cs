using UnityEngine;
using System.Collections;

public class LightTwinkling: MonoBehaviour
{
    [SerializeField] private float minInterval = 0.05f;
    [SerializeField] private float maxInterval = 0.5f;

    private Light targetLight;

    private void Awake()
    {
        targetLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        StartCoroutine(Twinkle());
    }

    private IEnumerator Twinkle()
    {
        while (true)
        {
            targetLight.enabled = !targetLight.enabled;

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
