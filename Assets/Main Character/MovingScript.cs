using UnityEngine;
using UnityEngine.InputSystem;

public class MovingScript : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    private InputAction m_MoveAction;
    private Animator m_Animator;
    private void Start()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_MoveAction.IsPressed())
        {
            Debug.Log("Moving...");
        }
        m_Animator.SetBool(Run, m_MoveAction.IsPressed());
    }
}
