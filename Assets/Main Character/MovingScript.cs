using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MovingScript : MonoBehaviour
{
    private static readonly int RunX = Animator.StringToHash("RunX");
    private static readonly int RunY = Animator.StringToHash("RunY");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    [SerializeField] private float speed = 10000f;
    [SerializeField] private float sprintMultiple = 2f;
    
    private InputAction m_MoveAction;
    private InputAction m_SprintAction;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    
    private Camera m_MainCamera;

    private Vector3 m_CurrentCourse = Vector3.forward;
    private Vector3 m_OldPosition = Vector3.zero;

    private const float VelocityError = 0.3f;

    private void Start()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
        
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        if (Camera.main is not null)
        {
            m_MainCamera = Camera.main;
        }
        
        m_MainCamera.GetComponent<CameraScript>().pointTargetEvent.AddListener(LookDirection);
    }

    private void LookDirection(Vector3 worldPosition)
    {
        transform.LookAt(worldPosition, Vector3.up);
        m_CurrentCourse = Vector3.Normalize(worldPosition - transform.position);
    }

    private void FixedUpdate()
    {
        var inputDirection = m_MoveAction.ReadValue<Vector2>();

        var velocity = Vector3.Distance(m_OldPosition, transform.position) / Time.fixedDeltaTime;
        m_OldPosition = transform.position;
        
        if (velocity < VelocityError)
        {
            m_Animator.SetFloat(RunX, 0);
            m_Animator.SetFloat(RunY, 0);
        }
        else
        {
            m_Animator.SetFloat(RunX, inputDirection.x);
            m_Animator.SetFloat(RunY, inputDirection.y);                    
        }
        m_Animator.SetBool(IsRunning, m_SprintAction.IsPressed());
        
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

            m_Rigidbody.AddForce(
                movingDirection * (speed * speedMultiple * Time.fixedDeltaTime),
                ForceMode.Acceleration);
        }
    }
}
