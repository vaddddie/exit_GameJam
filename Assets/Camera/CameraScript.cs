using UnityEngine;
using UnityEngine.Serialization;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float widthGreenZone = 10f; 
    [SerializeField] private float heightGreenZone = 10f; 
    
    private GameObject m_Player;
    private Vector3 m_CameraOffset;
    
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player");
        m_CameraOffset = transform.position;
    }

    private void LateUpdate()
    {
        var tmpX = m_Player.transform.position.x - transform.position.x + m_CameraOffset.x;
        var tmpZ = m_Player.transform.position.z - transform.position.z + m_CameraOffset.z;

        if (Mathf.Abs(tmpX) > widthGreenZone / 2)
        {
            var newCoord = m_Player.transform.position.x + m_CameraOffset.x - widthGreenZone * Mathf.Sign(tmpX) / 2;
            transform.position = new Vector3(newCoord, transform.position.y, transform.position.z);
        }

        if (Mathf.Abs(tmpZ) > heightGreenZone / 2)
        {
            var newCoord = m_Player.transform.position.z + m_CameraOffset.z - heightGreenZone * Mathf.Sign(tmpZ) / 2;
            transform.position = new Vector3(transform.position.x, transform.position.y, newCoord);
        }
    }
}
