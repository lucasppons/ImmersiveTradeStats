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
    
    protected override Vector3 ArcOutDirection(Vector3 from, Vector3 to)
    {
        return from + to;
    }
    
    protected override float ArcHeight()
    {
        return 0.55f;
    }
}
