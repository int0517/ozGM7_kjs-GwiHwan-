
using UnityEngine;
using System.Collections;

public class GhostJumpScare : MonoBehaviour
{
    [Header("Jump Scare")]
    [SerializeField] private GameObject ghost;
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    [SerializeField] private Light light;

    [Header("Settings")]
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float scareDuration = 3f;

    private bool isPlaying;

    public void PlayJumpScare()
    {
        if (ghost == null || player == null || camera == null ||
            target1 == null || target2 == null)
            return;

        if (isPlaying)
            return;

        StartCoroutine(JumpScareCoroutine());
    }

    private IEnumerator JumpScareCoroutine()
    {
        isPlaying = true;

        ghost.transform.position = ghost.transform.localPosition + new Vector3(0f, 2f, 0f);
        light.color = new Color(140/255f, 0f, 0f);
        light.intensity = 10f;

        AudioManager.Instance.PlayGhostAudio(0, 0.2f);
        GameManager.Instance.SetMoveAble(false);

        // 카메라가 target1을 향해 빠르게 이동하고 회전
        while (Vector3.Distance(camera.position, target1.position) > 0.02f ||
               Quaternion.Angle(camera.rotation, target1.rotation) > 0.5f)
        {
            camera.position = Vector3.Lerp(
                camera.position,
                target1.position,
                rotateSpeed * Time.deltaTime
            );

            camera.rotation = Quaternion.Slerp(
                camera.rotation,
                target1.rotation,
                rotateSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 목표 위치와 회전을 정확하게 적용
        camera.SetPositionAndRotation(target1.position, target1.rotation);

        // 유령 활성화
        ghost.SetActive(true);

        // 점프 스케어 유지
        yield return new WaitForSeconds(scareDuration);

        // 플레이어 위치 이동
        player.position = target2.position + Vector3.up;

        // 플레이어의 Y축 회전 적용
        Vector3 playerRotation = player.eulerAngles;
        playerRotation.y = target2.eulerAngles.y;
        player.eulerAngles = playerRotation;

        // 카메라의 X축 회전 적용
        Vector3 cameraRotation = camera.eulerAngles;
        cameraRotation.x = target2.eulerAngles.x;
        camera.eulerAngles = cameraRotation;

        camera.localPosition = new Vector3(0f, 1f, 0f);

        GameManager.Instance.SetMoveAble(true);
        light.enabled = false;
        isPlaying = false;

        ghost.SetActive(false);
    }
}
