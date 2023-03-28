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
    
    protected override Vector3 ArcOutDirection(Vector3 from, Vector3 to)
    {
        return new Vector3(0.0f, 1.0f, 0.0f);
    }
    
    protected override float ArcHeight()
    {
        return 0.05f;
    }
}
