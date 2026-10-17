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
        Application.targetFrameRate = 144;
        CanMove = true;
        IsESC = false;
        IsEnd = false;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene") return;

        if (Input.GetKeyDown(KeyCode.Escape))
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
        ESCMenuUI.SetActive(IsESC);
    }

    public void TitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            if (ESCMenuUI == null) ESCMenuUI = GameObject.Find("ESCPanel");
            if (ESCMenuUI != null) ESCMenuUI.SetActive(false);
        }
    }
}
