using UnityEngine;
using System.Collections;

public class CurtainManager : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform curtain;
    [SerializeField] private float duration = 2f;

    private bool isAnimating;

    public void TryInteract()
    {
        if (isAnimating) return;

        StartCoroutine(ShrinkCurtain());
    }

    private IEnumerator ShrinkCurtain()
    {
        isAnimating = true;

        Vector3 startScale = curtain.localScale;
        Vector3 targetScale = startScale;
        targetScale.x = 0.3f;

        float elapsed = 0f;
        AudioManager.Instance.PlaySystemAudio(4, 0.1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 scale = curtain.localScale;
            scale.x = Mathf.Lerp(startScale.x, targetScale.x, t);

            curtain.localScale = scale;

            yield return null;
        }

        curtain.localScale = targetScale;
        isAnimating = false;
    }
}
