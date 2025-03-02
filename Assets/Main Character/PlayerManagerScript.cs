using UnityEngine;
using UnityEngine.Events;

public class PlayerManagerScript : MonoBehaviour
{
    public UnityEvent standUpEvent = new UnityEvent();

    private void StandUp()
    {
        standUpEvent.Invoke();
    }
}
