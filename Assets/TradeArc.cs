using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeArc : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text label;
    
    DataMap map;
    
    public DatasetPrimitives.Trade trade;
    
    public void Update()
    {        
        if(label.enabled) {
            label.transform.LookAt(Camera.main.transform);
            
            label.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    
    public void Configure(Vector3 from, Vector3 to, float height, float width, Vector3 outDirection, DataMap map, DatasetPrimitives.Trade trade)
    {        
        Color color = (trade.indicator == DatasetPrimitives.Indicator.Import) ? Color.green : Color.red;
        
        Vector3[] points = new Vector3[11];
        
        for (int i = 0; i < 11; i++) {
            points[i] = Parabola(from, to, height, i * 0.1f, outDirection);
        }
        
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 11;
        lineRenderer.SetPositions(points);
        
        SetWidth(width);
        
        this.map = map;
        this.trade = trade;
        
        label.transform.localScale = new Vector3(
            label.transform.localScale.x / map.transform.localScale.x,
            label.transform.localScale.y / map.transform.localScale.y,
            label.transform.localScale.z / map.transform.localScale.z
        );
        label.transform.localPosition = points[5] + (Vector3.up * 0.025f);
        label.text = trade.value.ToString();
        label.enabled = false;
        
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        
        Mesh mesh = new Mesh();
        
        lineRenderer.BakeMesh(mesh, false);
        
        meshCollider.sharedMesh = mesh;
    }
    
    public void SetWidth(float width)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        
        float realWidth = 0.005f + (0.01f * width);
        
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
    
    public void ShowLabel() 
    {
        label.enabled = true;
    }
    
    public void HideLabel() 
    {
        label.enabled = false;
    }
    
    public void Remove()
    {
        map.RemoveArc(this);
    }
}
