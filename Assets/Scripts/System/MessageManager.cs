using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private static MessageManager instance;
    public static MessageManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private TextMeshProUGUI messageT;
    [SerializeField] private float fadeTime = 2f;
    private Coroutine messageCoroutine;

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
    }

    public void PlayMessage(string text)
    {
        if (messageCoroutine != null) StopCoroutine(messageCoroutine);

        messageT.text = text;
        messageCoroutine = StartCoroutine(MessageCoroutine());
    }

    public IEnumerator MessageCoroutine()
    {
        Color color = messageT.color;

        color.a = 0f;
        messageT.color = color;

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeTime);
            messageT.color = color;

            yield return null;
        }

        timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            messageT.color = color;

            yield return null;
        }

        color.a = 0f;
        messageT.color = color;

        messageCoroutine = null;
    }
}
