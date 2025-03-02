using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    public UnityEvent startCutscene0 = new UnityEvent();
    public UnityEvent endCutscene0 = new UnityEvent();

    public void InvokeEndCutscene0()
    {
        endCutscene0.Invoke();
    }

    private void Start()
    {
        startCutscene0.Invoke();
    }


}
