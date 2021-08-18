using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandAnimatorEventListener : MonoBehaviour
{
    public event UnityAction HandPressed;
    public event UnityAction HandReleased;

    public void OnHandPressed()
    {
        HandPressed?.Invoke();
    }

    public void OnHandReleased()
    {
        HandReleased?.Invoke();
    }
}
