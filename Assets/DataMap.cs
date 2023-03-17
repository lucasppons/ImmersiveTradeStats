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
    
    [SerializeField]
    protected TradeArc arcPrefab;
    
    List<Marker> markers = new List<Marker>();
    
    Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    
    void Start() 
    {
        StartCoroutine(DatasetParser.QueryAPI(this));
    }
    
    void Update()
    {
        Vector3 newScale = transform.localScale;
        
        if (newScale != scale) {
            foreach (Marker marker in markers) {
                marker.transform.localScale = new Vector3(
                    marker.transform.localScale.x * scale.x / newScale.x, 
                    marker.transform.localScale.y * scale.y / newScale.y, 
                    marker.transform.localScale.z * scale.z / newScale.z
                );
            }
            
            scale = newScale;
        }
    }
    
    public void ApiQueryCallback(List<DatasetPrimitives.Trade> trades) 
    {
        foreach (Marker marker in markers) {
            Destroy(marker);
        }
        
        markers.Clear();
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            AddMarker(DatasetPrimitives.coords[trade.partner]);
            AddMarker(DatasetPrimitives.coords[trade.reporter]);
            DrawArc(trade);
        }
    }
    
    void AddMarker(Vector2 coord)
    {
        Marker marker = Instantiate(markerPrefab, transform);
        
        marker.transform.localPosition = MarkerPosition(coord);
        
        marker.transform.localScale = new Vector3(
            marker.transform.localScale.x / transform.localScale.x,
            marker.transform.localScale.y / transform.localScale.y,
            marker.transform.localScale.z / transform.localScale.z
        );
        
        markers.Add(marker);
    }
    
    protected abstract Vector3 MarkerPosition(Vector2 coord);
    
    protected abstract void DrawArc(DatasetPrimitives.Trade trade);
}
