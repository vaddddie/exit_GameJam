using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{
    private CutsceneManager m_CutsceneManager;
    
    private void Start()
    {
        if (Camera.main is not null)
        {
            m_CutsceneManager = Camera.main.GetComponent<CutsceneManager>();
        }
    }

    private void InvokeEndCutscene0()
    {
        m_CutsceneManager.InvokeEndCutscene0();
    }
}
