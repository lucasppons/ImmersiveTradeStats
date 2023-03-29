using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text label;
    
    [SerializeField]
    MeshRenderer meshRenderer;
    
    DataMap map;
    
    [HideInInspector]
    public DatasetPrimitives.Trade baseTrade;

    void Update()
    {
        if(label.enabled) {
            label.transform.LookAt(Camera.main.transform);
            
            label.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    
    public void Configure(DataMap map, DatasetPrimitives.Trade trade) 
    {
        this.map = map;
        baseTrade = trade;
        
        label.text = trade.reporterName;
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
    
    public void Select() 
    {
        map.SelectMarker(this);
    }
    
    public void Highlight()
    {
        meshRenderer.material.color = Color.magenta;
    }
    
    public void Lowlight()
    {
        meshRenderer.material.color = new Color(0.63f, 0.0f, 0.0f, 1.0f);
    }
}
