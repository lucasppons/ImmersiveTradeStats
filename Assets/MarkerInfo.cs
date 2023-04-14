using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerInfo : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text countryName;    
    
    [SerializeField]
    TMPro.TMP_Text productName;
    
    [SerializeField]
    TMPro.TMP_Text yearNumber;
    
    [SerializeField]
    TMPro.TMP_Text importValue;
    
    [SerializeField]
    TMPro.TMP_Text exportValue;
    
    Marker marker;

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0.0f, 180.0f, 0.0f);        
    }
    
    public void Configure(Marker marker)
    {
        transform.localPosition = marker.label.transform.localPosition * 3;
        
        transform.localScale = new Vector3(
            transform.localScale.x / (marker.transform.localScale.x * marker.map.transform.localScale.x),
            transform.localScale.y / (marker.transform.localScale.y * marker.map.transform.localScale.y),
            transform.localScale.z / (marker.transform.localScale.z * marker.map.transform.localScale.z)
        );
        
        countryName.text = marker.import.reporterName;
        
        productName.text = marker.import.productName;
        
        yearNumber.text = marker.map.year.ToString();
        
        string importText = marker.import.value.ToString("0,0.00");
        
        importValue.text = $"${importText} K";
        
        string exportText = marker.export.value.ToString("0,0.00");
        
        exportValue.text = $"${exportText} K";
        
        this.marker = marker;
    }
    
    public void Close()
    {
        marker.InfoClosed();
        
        Destroy(gameObject);
    }
}
