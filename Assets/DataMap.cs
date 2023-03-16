using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataMap : MonoBehaviour
{
    public DatasetPrimitives.Country reporter;
    public DatasetPrimitives.Country partner;
    public DatasetPrimitives.Product product;
    public DatasetPrimitives.Indicator indicator;
    public int year;
    
    [SerializeField]
    protected Marker markerPrefab;
    
    protected List<Marker> markers = new List<Marker>();
    
    void Start() 
    {
        StartCoroutine(DatasetParser.QueryAPI(this));
    }
    
    public void ApiQueryCallback(List<DatasetPrimitives.Trade> trades) 
    {
        foreach (Marker marker in markers) {
            Destroy(marker);
        }
        
        markers.Clear();
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            AddMarker(DatasetPrimitives.coords[trade.reporter]);
            AddMarker(DatasetPrimitives.coords[trade.partner]);
        }
    }
    
    void AddMarker(Vector2 coord)
    {
        Marker marker = Instantiate(markerPrefab, MarkerPosition(coord), Quaternion.identity);
        
        marker.coord = coord;
        
        markers.Add(marker);
    }
    
    protected abstract Vector3 MarkerPosition(Vector2 coord);
}
