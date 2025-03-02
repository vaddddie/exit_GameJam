using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GameObject;

public class CutsceneManager : MonoBehaviour
{
    public UnityEvent startCutscene = new UnityEvent();
    public UnityEvent endCutscene = new UnityEvent();

    private void Start()
    {
        startCutscene.Invoke();
        FindWithTag("Player").GetComponent<PlayerManagerScript>().standUpEvent.AddListener(InvokeEndCutscene);
    }

    private void InvokeEndCutscene()
    {
        endCutscene.Invoke();
    }
}
