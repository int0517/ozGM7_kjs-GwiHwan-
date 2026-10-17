using UnityEngine;
using UnityEngine.UI;

public class ESCMenuButtonOnClick : MonoBehaviour
{
    [SerializeField] private Button resumeB;
    [SerializeField] private Button titleB;

    private void OnEnable()
    {
        resumeB.onClick.AddListener(GameManager.Instance.ResumeButton);
        titleB.onClick.AddListener (GameManager.Instance.TitleButton);
    }

    private void OnDisable()
    {
        resumeB.onClick.RemoveAllListeners();
        titleB.onClick.RemoveAllListeners();
    }
}
