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
    
    protected override void DrawArc(DatasetPrimitives.Trade trade)
    {
        TradeArc arc = Instantiate(arcPrefab, transform);
        
        Vector3 from = MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        Vector3 to = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        Color color = (trade.indicator == DatasetPrimitives.Indicator.Import) ? Color.green : Color.red;
        
        float distance = Vector3.Distance(from, to);
        
        arc.Configure(from, to, distance / 1.85f, from + to, color);
    }
}
