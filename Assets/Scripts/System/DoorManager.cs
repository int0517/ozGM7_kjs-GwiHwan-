using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private ItemSO keySO;
    [SerializeField] private Transform doorPivot;

    private float rotateTime = 2f;
    private float rotateAngle = 100f;

    private Coroutine doorCoroutine;
    private bool isOpened = false;

    public void DoorOpen()
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
            // 문 잠김 사운드
        }
        else if (keySO != null && PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(keySO.ID))
        {
            isOpened = true;
            PlayerInventoryManager.Instance.UseItem(keySO);
            doorCoroutine = StartCoroutine(DoorOpenCoroutine());
        }
    }

    private IEnumerator DoorOpenCoroutine()
    {
        Quaternion startRotation = doorPivot.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, rotateAngle, 0f);

        float timer = 0f;

        // 문 열림 사운드

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
