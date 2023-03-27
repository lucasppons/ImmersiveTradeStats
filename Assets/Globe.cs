using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : DataMap
{    
    protected override Vector3 MarkerPosition(Vector2 coord)
    {
        float radius = GetComponent<Renderer>().bounds.extents.x;
        
        float lon = coord.y * Mathf.PI / 180;
        float lat = coord.x * Mathf.PI / 180;
        
        float x = Mathf.Cos(lat) * Mathf.Cos(lon);
        float y = Mathf.Cos(lat) * Mathf.Sin(lon);
        float z = Mathf.Sin(lat);
        
        return new Vector3(x, z, y) * radius;
    }
    
    protected override void ConfigureArc(TradeArc arc, DatasetPrimitives.Trade trade, float min, float max)
    {
        Vector3 from = MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        Vector3 to = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        Vector3 outDirection = from + to;
        Color color = (trade.indicator == DatasetPrimitives.Indicator.Import) ? Color.green : Color.red;
        float width = (trade.value - min) / (max - min);
        float height = Vector3.Distance(from, to) * 0.55f;
        
        arc.Configure(from, to, height, width, outDirection, color);
    }
}
