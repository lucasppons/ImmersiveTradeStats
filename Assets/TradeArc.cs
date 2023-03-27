using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeArc : MonoBehaviour
{
    public void Configure(Vector3 from, Vector3 to, float height, float width, Vector3 outDirection, Color color)
    {
        Vector3[] points = new Vector3[11];
        
        for (int i = 0; i < 11; i++) {
            points[i] = Parabola(from, to, height, i * 0.1f, outDirection);
        }
        
        float realWidth = 0.005f + (0.01f * width);
        
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 11;
        lineRenderer.SetPositions(points);
        lineRenderer.startWidth = realWidth;
        lineRenderer.endWidth = realWidth;
    }    
    
    
    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    {
        float parabolicT = t * 2 - 1;
        //start and end are not level, gets more complicated
        Vector3 travelDirection = end - start;
        Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
        Vector3 right = Vector3.Cross(travelDirection, levelDirection);
        Vector3 up = outDirection;
        Vector3 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }
}
