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
    
    public void Configure(TradeArc arc, Vector3 position) 
    {
        transform.localPosition = position;
        
        transform.localScale = new Vector3(
            transform.localScale.x / arc.map.transform.localScale.x,
            transform.localScale.y / arc.map.transform.localScale.y,
            transform.localScale.z / arc.map.transform.localScale.z
        );
        
        this.arc = arc;
        
        string valueText = arc.trade.value.ToString("0,0.00");
        
        label.text = $"{valueText} K$";
        label.enabled = false;
        
        if (arc.map is Globe) {
            transform.LookAt(arc.map.transform);
            
            transform.Rotate(-90.0f, 0.0f, 0.0f);
        }
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
        arc.Remove();
    }
}
