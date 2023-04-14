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
        transform.localPosition = map.MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        
        transform.localScale = new Vector3(
            transform.localScale.x / map.transform.localScale.x,
            transform.localScale.y / map.transform.localScale.y,
            transform.localScale.z / map.transform.localScale.z
        );
        
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
    
    public void SelectImport() 
    {
        map.SelectMarker(this, DatasetPrimitives.Indicator.Import);
    }
    
    public void SelectExport()
    {
        map.SelectMarker(this, DatasetPrimitives.Indicator.Export);
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
