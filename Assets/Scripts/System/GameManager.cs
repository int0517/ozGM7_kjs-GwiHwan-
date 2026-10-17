using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    [SerializeField] private GameObject ESCMenuUI;

    public bool CanMove { get; private set; }
    public bool IsESC { get; private set; }
    public bool IsEnd { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 144;
        CanMove = true;
        IsESC = false;
        IsEnd = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene") return;

        if (Input.GetKeyDown(KeyCode.Escape)
            && SceneManager.GetActiveScene().name == "GameScene"
            && !InventoryUIManager.Instance.IsOpen)
        {
            IsESC = !IsESC;
            Time.timeScale = IsESC ? 0f : 1f;
            CanMove = !IsESC;
            ESCMenuUI.SetActive(IsESC);
            Cursor.lockState = IsESC ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = IsESC;
        }
    }

    public void SetMoveAble(bool value)
    {
        CanMove = value;
    }

    public void SetIsEnd()
    {
        IsEnd = true;
    }

    public void ResumeButton()
    {
        IsESC = false;
        Time.timeScale = 1f;
        CanMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ESCMenuUI.SetActive(false);
    }

    public void TitleButton()
    {
        IsESC = false;
        Time.timeScale = 1f;
        CanMove = true;
        SceneManager.LoadScene("TitleScene");
    }

    public void Bind(GameObject ESCMenuUI)
    {
        this.ESCMenuUI = ESCMenuUI;

        if (this.ESCMenuUI != null)
        {
            this.ESCMenuUI.SetActive(false);
        }
    }
}
