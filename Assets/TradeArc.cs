using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeArc : MonoBehaviour
{
    [SerializeField]
    TradeArcNode nodePrefab;
    
    [SerializeField]
    LineRenderer lineRenderer;
    
    DataMap map;
    
    [HideInInspector]
    public DatasetPrimitives.Trade trade;
    
    [HideInInspector]
    public TradeArcNode node;
    
    public void Configure(DataMap map, DatasetPrimitives.Trade trade)
    {
        
        Vector3 from = map.MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
        Vector3 to = map.MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        
        float height = Vector3.Distance(from, to) * map.ArcHeight();
        
        Vector3 outDirection = map.ArcOutDirection(from, to);
        
        Color startColor = Color.gray, endColor = Color.gray;
        
        if (trade.indicator == DatasetPrimitives.Indicator.Import) {
            startColor = Color.green;
            
            outDirection = Quaternion.AngleAxis(15, from - to) * outDirection;
        } else {
            endColor = Color.red;
            
            outDirection = Quaternion.AngleAxis(45, from - to) * outDirection;
        }
        
        Vector3[] points = new Vector3[11];
        
        for (int i = 0; i < 11; i++) {
            points[i] = Parabola(from, to, height, i * 0.1f, outDirection);
        }
        
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.positionCount = 11;
        lineRenderer.SetPositions(points);
        
        SetWidth(map.ArcWidth(trade));
        
        this.map = map;
        this.trade = trade;
        
        node = Instantiate(nodePrefab, map.transform);
        
        node.Configure(this);
        
        node.transform.localPosition = points[5];
        
        node.transform.localScale = new Vector3(
            node.transform.localScale.x / map.transform.localScale.x,
            node.transform.localScale.y / map.transform.localScale.y,
            node.transform.localScale.z / map.transform.localScale.z
        );
    }
    
    public void SetWidth(float width)
    {
        float realWidth = 0.005f + (0.01f * width);
        
        lineRenderer.startWidth = realWidth;
        lineRenderer.endWidth = realWidth;
    }
    
    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    {
        float parabolicT = t * 2 - 1;
        
        return start + (t * (end - start)) + (((-parabolicT * parabolicT + 1) * height) * outDirection.normalized);
    }
    
    public void Remove()
    {
        map.RemoveArc(this);
    }
}
