using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO keySO;
    [SerializeField] private Transform doorPivot;

    private float rotateTime = 2f;
    private float rotateAngle = 100f;

    private Coroutine doorCoroutine;
    private bool isOpened = false;

    public void TryInteract()
    {
        if (doorCoroutine != null || isOpened) return;

        if (keySO == null)
        {
            isOpened = true;
            doorCoroutine = StartCoroutine(DoorOpenCoroutine());
        }
        else if (keySO != null && !PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(keySO.ID))
        {
            MessageManager.Instance.PlayMessage("문이 잠겨있다.");
            AudioManager.Instance.PlaySystemAudio(2, 0.2f);
        }
        else if (keySO != null && PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(keySO.ID))
        {
            isOpened = true;
            AudioManager.Instance.PlaySystemAudio(3, 0.1f);
            PlayerInventoryManager.Instance.UseItem(keySO);
            doorCoroutine = StartCoroutine(DoorOpenCoroutine());
        }
    }

    private IEnumerator DoorOpenCoroutine()
    {
        Quaternion startRotation = doorPivot.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, rotateAngle, 0f);

        float timer = 0f;

        AudioManager.Instance.PlaySystemAudio(0, 0.1f);

        while (timer < rotateTime)
        {
            timer += Time.deltaTime;

            float t = timer / rotateTime;

            doorPivot.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            yield return null;
        }

        doorPivot.rotation = endRotation;
        doorCoroutine = null;
    }
}
