using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeArcNode : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text label;
    
    TradeArc arc;
    
    void Update()
    {
        if(label.enabled) {
            label.transform.LookAt(Camera.main.transform);
            
            label.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    
    public void Configure(TradeArc arc) 
    {
        this.arc = arc;
        
        label.text = arc.trade.value.ToString();
        label.enabled = false;
    }
    
    public void ShowLabel() 
    {
        label.enabled = true;
    }
    
    public void HideLabel() 
    {
        label.enabled = false;
    }
    
    public void RemoveArc()
    {
        Destroy(gameObject);
        
        arc.Remove();
    }
}
