using UnityEngine;
using UnityEngine.InputSystem;

public class OpenDoor : MonoBehaviour
{
    public Animator m_Animator;       // Reference to the Animator
    public string triggerName = "triggerDoor"; // Name of the trigger parameter in Animator

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        m_Animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the object has the 'Player' tag
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Keyboard.current.eKey.wasPressedThisFrame) // Change to match your Interact key
        {
            m_Animator.SetTrigger(triggerName);
        }
    }
}
