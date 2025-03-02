using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public UnityEvent<Vector3> pointTargetEvent = new UnityEvent<Vector3>();
    
    [SerializeField] private float maxViewingRadius = 5f;
    
    private InputAction m_LookAction;
    private LayerMask m_LookLayer;
    
    private GameObject m_Player;
    private Camera m_Camera;
    private Vector3 m_CameraOffset;
    
    private void Start()
    {
        m_LookAction = InputSystem.actions.FindAction("Look");
        m_LookLayer = LayerMask.GetMask("LookFloor");
        m_Camera = GetComponent<Camera>();
        
        m_Player = GameObject.FindWithTag("Player");
        m_CameraOffset = transform.position - m_Player.transform.position;
        
        pointTargetEvent.AddListener(MoveCamera);
    }

    private void LateUpdate()
    {
        var tmp = new Vector3(m_LookAction.ReadValue<Vector2>().x, m_LookAction.ReadValue<Vector2>().y);
        var ray = m_Camera.ScreenPointToRay(tmp);
        
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, m_LookLayer))
        {
            var worldPosition = new Vector3(hit.point.x, m_Player.transform.position.y, hit.point.z);
            pointTargetEvent.Invoke(worldPosition);
        }
    }

    private void MoveCamera(Vector3 mousePosition)
    {
        var convertedPlayerCoord = m_Player.transform.position + m_CameraOffset;
        var convertedMouseCoord = mousePosition + m_CameraOffset;

        var viewingRadius = Vector3.Magnitude(convertedMouseCoord - transform.position) / 2;
        if (viewingRadius > maxViewingRadius)
        {
            viewingRadius = maxViewingRadius;
        }

        var movingDirection = Vector3.Normalize(convertedMouseCoord - transform.position);
        var targetPosition =
            new Vector3(movingDirection.x, 0, movingDirection.z) * viewingRadius + convertedPlayerCoord;

        var pathLenght = Vector3.Magnitude(targetPosition - transform.position);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, pathLenght * Time.deltaTime);
    }
}
