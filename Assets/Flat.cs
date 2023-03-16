using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flat : DataMap
{
    void Update()
    {
        foreach (Marker marker in markers) {        
            marker.transform.position = MarkerPosition(marker.coord);
        }
    }
    
    protected override Vector3 MarkerPosition(Vector2 coord) 
    {
        float size = GetComponent<Renderer>().bounds.size.z;
        Vector3 position= transform.position;
        Quaternion rotation= transform.localRotation;
        
        float lon = coord.y / 180;
        float lat = coord.x / 180;
        
        return rotation * new Vector3(lon, 0.0f, lat) * size + position;
    }
}
