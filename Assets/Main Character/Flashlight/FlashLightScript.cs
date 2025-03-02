using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLightScript : MonoBehaviour
{
    [SerializeField] private new GameObject light;
    
    private InputAction m_FlashlightAction;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_FlashlightAction = InputSystem.actions.FindAction("Flashlight");
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!m_FlashlightAction.WasPressedThisFrame()) {return;}
        
        light.SetActive(!light.activeSelf);
        m_AudioSource.Play();
    }
}
