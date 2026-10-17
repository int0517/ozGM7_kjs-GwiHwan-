using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject titleSprite2;
    [SerializeField] private GameObject denySprite;
    private Color ambientColor = new Color(50/255f, 0f, 0f, 1f);

    private void Awake()
    {
        if (GameManager.Instance == null) return;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (GameManager.Instance.IsEnd)
        {
            titleSprite2.SetActive(true);
            denySprite.SetActive(true);
            RenderSettings.ambientLight = ambientColor;
        }
    }

    public void StartButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GameScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
