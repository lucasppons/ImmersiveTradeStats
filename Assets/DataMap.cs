using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataMap : MonoBehaviour
{
    [SerializeField]
    DatasetPrimitives.Product product;
    
    [SerializeField]
    int year;
    
    [SerializeField]
    Marker markerPrefab;
    
    [SerializeField]
    TradeArc arcPrefab;
    
    List<Marker> markers = new List<Marker>();
    
    Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    
    void Start() 
    {
        DatasetParser.TradeQuery query = new DatasetParser.TradeQuery(
            DatasetPrimitives.Country.All,
            DatasetPrimitives.Country.WLD,
            product,
            DatasetPrimitives.Indicator.Both,
            year
        );
        
        StartCoroutine(DatasetParser.QueryAPI(query, AddMarkers));
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
    
    public void AddMarkers(List<DatasetPrimitives.Trade> trades)
    {
        foreach (DatasetPrimitives.Trade trade in trades) {
            Marker marker = Instantiate(markerPrefab, transform);
            
            marker.Configure(trade);
            
            marker.transform.localPosition = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
            
            marker.transform.localScale = new Vector3(
                marker.transform.localScale.x / transform.localScale.x,
                marker.transform.localScale.y / transform.localScale.y,
                marker.transform.localScale.z / transform.localScale.z
            );
            
            markers.Add(marker);
        }
    }
    
    public void ApiQueryCallback(List<DatasetPrimitives.Trade> trades) 
    {
        foreach (Marker marker in markers) {
            Destroy(marker);
        }
        
        markers.Clear();
        
        float min = float.PositiveInfinity;
        float max = float.NegativeInfinity;
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            if (trade.value > max) {
                max = trade.value;
            } else if (trade.value < min) {
                min = trade.value;
            }
        }
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            TradeArc arc = Instantiate(arcPrefab, transform);
            
            ConfigureArc(arc, trade, min, max);
        }
    }
    
    protected abstract Vector3 MarkerPosition(Vector2 coord);
    
    protected abstract void ConfigureArc(TradeArc arc, DatasetPrimitives.Trade trade, float min, float max);
}
