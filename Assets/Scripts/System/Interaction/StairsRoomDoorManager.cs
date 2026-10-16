using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StairsRoomDoorManager : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO keySO;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Transform doorPivot;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform movePos1;
    [SerializeField] private Transform movePos2;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private FlashLightManager flashLightManager;
    
    private Transform playerPos;

    private float rotateTime = 2f;
    private float rotateAngle = 100f;

    private float moveTime = 2f;
    private float moveDistance = 2f;

    private Coroutine doorCoroutine;
    private bool isOpened = false;

    private void Awake()
    {
        playerPos = player.transform;
    }

    public void TryInteract()
    {
        if (doorCoroutine != null) return;

        if (keySO == null)
        {
            isOpened = true;
            doorCoroutine = StartCoroutine(StairsRoomCoroutine());
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
            doorCoroutine = StartCoroutine(StairsRoomCoroutine());
        }
    }

    private IEnumerator StairsRoomCoroutine()
    {
        GameManager.Instance.SetMoveAble(false);
        flashLightManager.ChangeLightState(false);
        playerPos.position = movePos1.position;
        playerPos.rotation = Quaternion.Euler(0f, 180f, 0f);
        mainCamera.rotation = Quaternion.Euler(0f, 180f, 0f);

        Quaternion startRotation = doorPivot.rotation;
        Quaternion openRotation = startRotation * Quaternion.Euler(0f, rotateAngle, 0f);

        float timer = 0f;
        AudioManager.Instance.PlaySystemAudio(0, 0.1f);

        while (timer < rotateTime)
        {
            timer += Time.deltaTime;
            float t = timer / rotateTime;

            doorPivot.rotation = Quaternion.Lerp(startRotation, openRotation, t);

            yield return null;
        }

        doorPivot.rotation = openRotation;

        Vector3 startPosition = movePos1.position;
        Vector3 endPosition = movePos1.position + Vector3.back * moveDistance;

        timer = 0f;

        while ( timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = timer / moveTime;

            playerPos.position = Vector3.Lerp(startPosition, endPosition, t);

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            blackScreen.color = color;

            yield return null;
        }

        playerPos.position = endPosition;

        Color blackColor = blackScreen.color;
        blackColor.a = 1f;
        blackScreen.color = blackColor;

        yield return new WaitForSeconds(1f);
        
        Vector3 movePos2Start = movePos2.position;
        movePos2Start.z -= 1f;

        playerPos.position = movePos2Start;
        playerPos.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainCamera.rotation = Quaternion.Euler(0f, 0f, 0f);
        player.ChangeFloor();

        timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = timer / moveTime;

            playerPos.position = Vector3.Lerp(movePos2Start, movePos2.position, t);

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(1f, 0f, t);
            blackScreen.color = color;

            yield return null;
        }

        AudioManager.Instance.PlaySystemAudio(1, 0.1f);
        playerPos.position = movePos2.position;

        Color clearColor = blackScreen.color;
        clearColor.a = 0f;
        blackScreen.color = clearColor;

        doorPivot.rotation = startRotation;
        GameManager.Instance.SetMoveAble(true);

        doorCoroutine = null;
    }
}
