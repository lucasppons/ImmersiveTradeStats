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
    
    List<TradeArc> arcs= new List<TradeArc>();
    
    Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    
    Marker arcOrigin = null;
    
    float minValue = float.PositiveInfinity;
    float maxValue = float.NegativeInfinity;
    
    void Start() 
    {
        DatasetParser.TradeQuery query = new DatasetParser.TradeQuery(
            DatasetPrimitives.Country.All,
            DatasetPrimitives.Country.WLD,
            product,
            DatasetPrimitives.Indicator.Export,
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
            
            marker.Configure(this, trade);
            
            marker.transform.localPosition = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);
            
            marker.transform.localScale = new Vector3(
                marker.transform.localScale.x / transform.localScale.x,
                marker.transform.localScale.y / transform.localScale.y,
                marker.transform.localScale.z / transform.localScale.z
            );
            
            markers.Add(marker);
        }
    }
    
    public void SelectMarker(Marker marker)
    {
        if (arcOrigin == null) {            
            marker.Highlight();
            
            arcOrigin = marker;
        } else if (arcOrigin == marker) {            
            marker.Lowlight();
            
            arcOrigin = null;
        } else {
            DatasetParser.TradeQuery query = new DatasetParser.TradeQuery(
                arcOrigin.baseTrade.reporter,
                marker.baseTrade.reporter,
                product,
                DatasetPrimitives.Indicator.Export,
                year
            );
            
            if (!this.arcs.Exists((arc) => arc.trade.SameRouteAs(query))) {            
                StartCoroutine(DatasetParser.QueryAPI(query, AddArcs));
            }
            
            arcOrigin.Lowlight();
            
            arcOrigin = null;
        }
    }
    
    public void AddArcs(List<DatasetPrimitives.Trade> trades) 
    {        
        foreach (DatasetPrimitives.Trade trade in trades) {
            if (this.arcs.Exists((arc) => arc.trade.SameRouteAs(trade))) continue;
            
            if (trade.value > maxValue) {
                maxValue = trade.value;
            } 
            
            if (trade.value < minValue) {
                minValue = trade.value;
            }
        }
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            if (this.arcs.Exists((arc) => arc.trade.SameRouteAs(trade))) continue;
            
            TradeArc arc = Instantiate(arcPrefab, transform);
            
            ConfigureArc(arc, trade);
            
            arcs.Add(arc);
        }
        
        foreach (TradeArc arc in arcs) {
            arc.SetWidth(ArcWidth(arc.trade));
        }
    }
    
    void ConfigureArc(TradeArc arc, DatasetPrimitives.Trade trade)
    {
        Vector3 from = MarkerPosition(DatasetPrimitives.coords[trade.partner]);
        Vector3 to = MarkerPosition(DatasetPrimitives.coords[trade.reporter]);   
        float height = Vector3.Distance(from, to) * ArcHeight();
        
        arc.Configure(from, to, height, ArcWidth(trade), ArcOutDirection(from, to), this, trade);
    }
    
    float ArcWidth(DatasetPrimitives.Trade trade)
    {
        float interval = maxValue - minValue;
        
        return (interval == 0.0f) ? 1.0f : ((trade.value - minValue) / interval);
    }
    
    protected abstract Vector3 MarkerPosition(Vector2 coord);
    protected abstract Vector3 ArcOutDirection(Vector3 from, Vector3 to);
    protected abstract float ArcHeight();
    
    public void RemoveArc(TradeArc removedArc)
    {
        arcs.Remove(removedArc);
        
        Destroy(removedArc.gameObject);
        
        minValue = float.PositiveInfinity;
        maxValue = float.NegativeInfinity;
        
        foreach (TradeArc arc in arcs) {            
            if (arc.trade.value > maxValue) {
                maxValue = arc.trade.value;
            } 
            
            if (arc.trade.value < minValue) {
                minValue = arc.trade.value;
            }
        }
        
        foreach (TradeArc arc in arcs) {
            arc.SetWidth(ArcWidth(arc.trade));
        }        
    }
}
