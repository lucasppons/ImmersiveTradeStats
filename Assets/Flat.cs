using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flat : DataMap
{
    protected override Vector3 MarkerPosition(Vector2 coord) 
    {
        float lon = coord.y / 180;
        float lat = coord.x / 180;
        
        return new Vector3(lon / transform.localScale.x, 0.0f, lat / transform.localScale.z);
    }
    
    protected override void DrawArc(DatasetPrimitives.Trade trade)
    {
        TradeArc arc = Instantiate(arcPrefab, transform);
        
        Vector3 from = MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        Vector3 to = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        Vector3 outDirection = new Vector3(0.0f, 1.0f, 0.0f);
        Color color = (trade.indicator == DatasetPrimitives.Indicator.Import) ? Color.green : Color.red;
        
        arc.Configure(from, to, 0.1f, outDirection, color);
    }
}
