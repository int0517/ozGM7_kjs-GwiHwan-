using Unity.VisualScripting;
using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen;
    private BoxCollider collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        isOpen = false;
    }

    public void TryInteract()
    {
        isOpen = true;
        animator.SetTrigger("Open");
    }

    public void DoneOpen()
    {
        collider.enabled = false;
    }
}
