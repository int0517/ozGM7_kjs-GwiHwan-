using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagersBinder : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField] private GameObject ESCMenuUI;

    [Header("MessageManager")]
    [SerializeField] private TextMeshProUGUI messageT;

    [Header("InventoryUIManager")]
    [SerializeField] private GameObject invenPanel;
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI itemDescriptionT;

    [Header("AudioManager")]
    [SerializeField] private Slider slider;


    void Start()
    {
        GameManager.Instance.Bind(ESCMenuUI);
        MessageManager.Instance.Bind(messageT);
        InventoryUIManager.Instance.Bind(invenPanel, content, itemDescriptionT);
        AudioManager.Instance.GetComponent<AudioVolumeController>().Bind(slider);
    }
}
