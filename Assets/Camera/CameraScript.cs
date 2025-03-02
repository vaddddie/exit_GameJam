using Unity.Cinemachine;
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
        
        CalculateOffset();
        
        pointTargetEvent.AddListener(MoveCamera);
    }

    private void CalculateOffset()
    {
        var x = m_Camera.pixelWidth / 2;
        var y = m_Camera.pixelHeight / 2;
        var ray = m_Camera.ScreenPointToRay(new Vector3(x, y));
        
        var heightOffset = (Vector3.forward + Vector3.right) / 2;

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, m_LookLayer))
        {
            m_CameraOffset = transform.position - hit.point + heightOffset;
        }
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

        SmoothMovement(targetPosition);
    }

    private void SmoothMovement(Vector3 target)
    {
        var pathLenght = Vector3.Magnitude(target - transform.position);
        transform.position = Vector3.Lerp(transform.position, target, pathLenght * Time.deltaTime);
    }
}
