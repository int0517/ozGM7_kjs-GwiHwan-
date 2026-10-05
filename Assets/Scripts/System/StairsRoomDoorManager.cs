using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StairsRoomDoorManager : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private Transform doorPivot;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform movePos1;
    [SerializeField] private Transform movePos2;
    [SerializeField] private Transform mainCamera;
    private Transform playerPos;

    private float rotateTime = 2f;
    private float rotateAngle = 100f;

    private float moveTime = 2f;
    private float moveDistance = 2f;

    private Coroutine doorCoroutine;

    private void Awake()
    {
        playerPos = player.transform;
    }

    public void EnterStairs()
    {
        if (doorCoroutine != null) return;

        doorCoroutine = StartCoroutine(StairsRoomCoroutine());
    }

    private IEnumerator StairsRoomCoroutine()
    {
        GameManager.Instance.SetMoveAble(false);
        playerPos.position = movePos1.position;
        playerPos.rotation = Quaternion.Euler(0f, 180f, 0f);
        mainCamera.rotation = Quaternion.Euler(0f, 180f, 0f);

        Quaternion startRotation = doorPivot.rotation;
        Quaternion openRotation = startRotation * Quaternion.Euler(0f, rotateAngle, 0f);

        float timer = 0f;
        // 문 열림 사운드

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

        playerPos.position = movePos2.position;

        Color clearColor = blackScreen.color;
        clearColor.a = 0f;
        blackScreen.color = clearColor;

        doorPivot.rotation = startRotation;
        GameManager.Instance.SetMoveAble(true);

        doorCoroutine = null;
    }
}
