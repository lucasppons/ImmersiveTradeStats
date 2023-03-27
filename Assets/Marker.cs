using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text label;
    
    DatasetPrimitives.Trade trade;

    void Update()
    {
        if(label.enabled) {
            label.transform.LookAt(Camera.main.transform);
            
            label.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }
    
    public void Configure(DatasetPrimitives.Trade trade) {
        this.trade = trade;
        
        label.text = trade.reporterName;
        label.enabled = false;
    }
    
    public void OnFocusEnter() {
        label.enabled = true;
    }
    
    public void OnFocusExit() {
        label.enabled = false;
    }
}
