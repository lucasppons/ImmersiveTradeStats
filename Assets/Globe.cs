using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : DataMap
{
    void Update()
    {
        foreach (Marker marker in markers) {
            marker.transform.position = MarkerPosition(marker.coord);
        }
    }
    
    protected override Vector3 MarkerPosition(Vector2 coord)
    {
        float radius = GetComponent<Renderer>().bounds.extents.x;
        Vector3 position = transform.position;
        Quaternion rotation = transform.localRotation;    
        
        float lon = coord.y * Mathf.PI / 180;
        float lat = coord.x * Mathf.PI / 180;
        
        float x = Mathf.Cos(lat) * Mathf.Cos(lon);
        float y = Mathf.Cos(lat) * Mathf.Sin(lon);
        float z = Mathf.Sin(lat);
        
        return rotation * new Vector3(x, z, y) * radius + position;
    }
}
