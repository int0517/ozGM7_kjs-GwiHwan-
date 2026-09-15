using UnityEngine;

public class LightEnemyHit : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float hitRange = 25f;
    private Light handlight;

    private void Awake()
    {
        handlight = GetComponent<Light>();
    }

    void Update()
    {
        EnemyHit();
    }

    private void EnemyHit()
    {
        if (Physics.Raycast(
            handlight.transform.position,
            handlight.transform.forward,
            out RaycastHit hit,
            hitRange,
            enemyLayer))
        {
            if (hit.collider.TryGetComponent(out GhostController ghostController))
            {
                Camera cam = Camera.main;

                Vector3 viewportPos = cam.WorldToViewportPoint(hit.collider.bounds.center);

                bool isVisible =
                    viewportPos.z > 0 &&
                    viewportPos.x >= 0 && viewportPos.x <= 1 &&
                    viewportPos.y >= 0 && viewportPos.y <= 1;

                if (isVisible)
                {
                    Debug.Log("적 파괴");
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
