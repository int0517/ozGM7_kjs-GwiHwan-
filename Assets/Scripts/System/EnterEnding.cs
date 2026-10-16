using UnityEngine;
using UnityEngine.UI;

public class EnterEnding : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject EndingT;
    [SerializeField] private Image blackScreen;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            EndingT.SetActive(true);
            blackScreen.color = new Color(0f, 0f, 0f, 0.7f);
            Time.timeScale = 0f;
            GameManager.Instance.SetMoveAble(false);
        }
    }
}
