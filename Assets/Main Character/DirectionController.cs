using UnityEngine;

public class DirectionController : MonoBehaviour
{
    private GameObject m_Player;
    private void Start()
    {
        m_Player = GameObject.FindWithTag("Player");
    }
    
    private void LateUpdate()
    {
        transform.position = m_Player.transform.position;
    }
}
