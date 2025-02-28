using UnityEngine;
using UnityEngine.InputSystem;

public class MovingScript : MonoBehaviour
{
    private static readonly int RunX = Animator.StringToHash("RunX");
    private static readonly int RunY = Animator.StringToHash("RunY");

    [SerializeField] private float speed = 10000f;
    [SerializeField] private float sprintMultiple = 2f;
    
    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_TouchAction;
    private InputAction m_SprintAction;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    
    private Camera m_MainCamera;

    private Vector3 m_CurrentCourse = Vector3.forward;
    
    private void Start()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_LookAction = InputSystem.actions.FindAction("Look");
        m_TouchAction = InputSystem.actions.FindAction("MouseTouch");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
        
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        if (Camera.main is not null)
        {
            m_MainCamera = Camera.main;
        }
    }

    private void Update()
    {
        var inputDirection = m_MoveAction.ReadValue<Vector2>();
        m_Animator.SetFloat(RunX, inputDirection.x);
        m_Animator.SetFloat(RunY, inputDirection.y);
        
        if (m_MoveAction.IsPressed())
        {
            var lookDirection = new Vector3(m_CurrentCourse.x, 0, m_CurrentCourse.z);
            var horizontalLookDirection = Quaternion.Euler(0, 90, 0) * lookDirection;
            var movingDirection = horizontalLookDirection * inputDirection.x + lookDirection * inputDirection.y;

            var speedMultiple = 1f;
            
            if (m_SprintAction.IsPressed())
            {
                speedMultiple = sprintMultiple;
            }

            m_Rigidbody.AddForce(movingDirection * (speed * speedMultiple * Time.deltaTime));
        }
        
        LookDirection();
    }

    private void LookDirection()
    {
        if (!m_TouchAction.IsPressed()) return;
        var tmp = new Vector3(m_LookAction.ReadValue<Vector2>().x, m_LookAction.ReadValue<Vector2>().y);
        var ray = m_MainCamera.ScreenPointToRay(tmp);
        if (Physics.Raycast(ray, out var hit))
        {
            if (!hit.transform.CompareTag("LookFloor")) return;
            var angle = Vector3.SignedAngle(Vector3.forward, hit.point - transform.position, Vector3.up);
            m_CurrentCourse = Vector3.Normalize(hit.point - transform.position);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
