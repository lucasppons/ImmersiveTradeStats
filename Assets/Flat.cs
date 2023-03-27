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
    
    protected override void ConfigureArc(TradeArc arc, DatasetPrimitives.Trade trade, float min, float max)
    {
        Vector3 from = MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        Vector3 to = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        Vector3 outDirection = new Vector3(0.0f, 1.0f, 0.0f);
        Color color = (trade.indicator == DatasetPrimitives.Indicator.Import) ? Color.green : Color.red;
        float width = (trade.value - min) / (max - min);
        float height = Vector3.Distance(from, to) * 0.05f;
        
        arc.Configure(from, to, height, width, outDirection, color);
    }
}
