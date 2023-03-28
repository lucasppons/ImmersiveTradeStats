using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.Input;

public class SingleAxisInputHandler : MonoBehaviour, IMixedRealityInputHandler<float>
{    
    [SerializeField]
    MixedRealityInputAction inputAction = MixedRealityInputAction.None;
    
    [SerializeField]
    UnityEvent<float> onChange;
    
    [SerializeField]
    UnityEvent onMax;
    
    [SerializeField]
    UnityEvent onMin;
    
    public void OnInputChanged(InputEventData<float> eventData) 
    {
        if (eventData.MixedRealityInputAction == inputAction) { 
            onChange.Invoke(eventData.InputData);
            
            if (eventData.InputData == 1.0f) {
                onMax.Invoke();
            } else if (eventData.InputData == 0.0f) {
                onMin.Invoke();
            }
        }
    }
}
