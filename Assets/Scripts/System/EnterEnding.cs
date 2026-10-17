using UnityEngine;
using UnityEngine.UI;

public class EnterEnding : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject EndingUI;
    [SerializeField] private Image blackScreen;

    [SerializeField] private Button titleB;

    private void OnEnable()
    {
        titleB.onClick.AddListener(GameManager.Instance.TitleButton);
    }

    private void OnDisable()
    {
        titleB.onClick.RemoveAllListeners();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            EndingUI.SetActive(true);
            blackScreen.color = new Color(0f, 0f, 0f, 0.7f);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            GameManager.Instance.SetMoveAble(false);
            GameManager.Instance.SetIsEnd();
        }
    }
}
