using UnityEngine;
using UnityEngine.UI;

public class EnterEnding : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject EndingT;
    [SerializeField] private Image blackScreen;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == playerLayer)
        {
            EndingT.SetActive(true);
            blackScreen.color = new Color(0f, 0f, 0f, 0.7f);
            Time.timeScale = 0f;
        }
    }
}
