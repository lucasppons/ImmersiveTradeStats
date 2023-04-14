using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeArcInfo : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text tradeValue;    
    
    [SerializeField]
    TMPro.TMP_Text productName;
    
    [SerializeField]
    TMPro.TMP_Text indicator;
    
    [SerializeField]
    TMPro.TMP_Text yearNumber;
    
    [SerializeField]
    TMPro.TMP_Text fromCountry;
    
    [SerializeField]
    TMPro.TMP_Text toCountry;
    
    TradeArcNode node;

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0.0f, 180.0f, 0.0f);        
    }
    
    public void Configure(TradeArcNode node)
    {
        transform.localPosition = node.label.transform.localPosition * 3;
        
        transform.localScale = new Vector3(
            transform.localScale.x / (node.transform.localScale.x * node.arc.map.transform.localScale.x),
            transform.localScale.y / (node.transform.localScale.y * node.arc.map.transform.localScale.y),
            transform.localScale.z / (node.transform.localScale.z * node.arc.map.transform.localScale.z)
        );
        
        string valueText = node.arc.trade.value.ToString("0,0.00");
        
        tradeValue.text = $"${valueText} K";
        
        indicator.text = node.arc.trade.indicator.ToString();
        
        if (node.arc.trade.indicator == DatasetPrimitives.Indicator.Import) {
            fromCountry.text = node.arc.trade.partnerName;
            toCountry.text = node.arc.trade.reporterName;
        } else {
            fromCountry.text = node.arc.trade.reporterName;
            toCountry.text = node.arc.trade.partnerName;
        }
        
        productName.text = node.arc.trade.productName;
        
        yearNumber.text = node.arc.map.year.ToString();
        
        this.node = node;
    }
    
    public void Close()
    {
        node.InfoClosed();
        
        Destroy(gameObject);
    }
}
