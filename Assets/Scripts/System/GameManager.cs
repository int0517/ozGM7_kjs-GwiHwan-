using System.Collections.Generic;
using UnityEngine;

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

    public bool CanMove { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 144;
        CanMove = true;

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

    private void Update()
    {
        if (!CanMove) Time.timeScale = 0f;
        else if (CanMove) Time.timeScale = 1f;
    }

    public void SetMoveAble(bool value)
    {
        CanMove = value;
    }
}
