using UnityEngine;
using UnityEngine.InputSystem;

public class MovingScript : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    private InputAction m_MoveAction;
    private InputAction m_LookAction;

    private Animator m_Animator;

    private Camera m_MainCamera;
    
    private void Start()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_LookAction = InputSystem.actions.FindAction("Look");
        m_Animator = GetComponent<Animator>();

        if (Camera.main is not null)
        {
            m_MainCamera = Camera.main;
        }
    }

    private void Update()
    {
        m_Animator.SetBool(Run, m_MoveAction.IsPressed());
        
        LookDirection();
    }

    private void LookDirection()
    {
        var tmp = new Vector3(m_LookAction.ReadValue<Vector2>().x, m_LookAction.ReadValue<Vector2>().y);
        var ray = m_MainCamera.ScreenPointToRay(tmp);
        if (!Physics.Raycast(ray, out var hit)) return;
        if (hit.transform.CompareTag("LookFloor"))
        {
            var angle = Vector3.SignedAngle(Vector3.forward, hit.point - transform.position, Vector3.up);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
