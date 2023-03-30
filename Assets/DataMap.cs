using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataMap : MonoBehaviour
{
    [SerializeField]
    Marker markerPrefab;
    
    [SerializeField]
    TradeArc arcPrefab;
    
    [SerializeField]
    MapFilters filtersPrefab;
    
    DatasetPrimitives.Product product = DatasetPrimitives.Product.All;
    
    int year = 2004;
    
    List<Marker> markers = new List<Marker>();
    
    List<TradeArc> arcs = new List<TradeArc>();
    
    Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    
    Marker arcOrigin = null;
    
    float minValue = float.PositiveInfinity;
    float maxValue = float.NegativeInfinity;
    
    bool filtersOpen = false;
    
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
            
            foreach (TradeArc arc in arcs) {
                arc.node.transform.localScale = new Vector3(
                    arc.node.transform.localScale.x * scale.x / newScale.x, 
                    arc.node.transform.localScale.y * scale.y / newScale.y, 
                    arc.node.transform.localScale.z * scale.z / newScale.z
                );
            }
            
            scale = newScale;
        }
    }
    
    public void OpenFilters()
    {
        if (filtersOpen) return;
        
        MapFilters filters = Instantiate(filtersPrefab);
        
        filters.Open(this, product, year);
        
        filtersOpen = true;
    }
    
    public void FiltersClosed()
    {
        filtersOpen = false;
    }
    
    public void ApplyFilters(DatasetPrimitives.Product product, int year)
    {
        foreach (Marker marker in markers) {
            Destroy(marker.gameObject);
        }
        
        markers.Clear();
        
        DatasetParser.TradeQuery markersQuery = new DatasetParser.TradeQuery(
            DatasetPrimitives.Country.All,
            DatasetPrimitives.Country.WLD,
            product,
            DatasetPrimitives.Indicator.Export,
            year
        );
        
        StartCoroutine(DatasetParser.QueryAPI(markersQuery, AddMarkers));
        
        List<DatasetPrimitives.Trade> trades = new List<DatasetPrimitives.Trade>();
        
        foreach (TradeArc arc in arcs) {
            trades.Add(arc.trade);
            
            Destroy(arc.node.gameObject);
            Destroy(arc.gameObject);
        }
        
        arcs.Clear();
        
        minValue = float.PositiveInfinity;
        maxValue = float.NegativeInfinity;
        
        foreach (DatasetPrimitives.Trade trade in trades) {
            DatasetParser.TradeQuery arcQuery = new DatasetParser.TradeQuery(
                trade.reporter,
                trade.partner,
                product,
                trade.indicator,
                year
            );
            
            StartCoroutine(DatasetParser.QueryAPI(arcQuery, AddArcs));
        }
        
        this.product = product;
        this.year = year;
    }
    
    public abstract Vector3 FiltersPosition();
    
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
    
    public void SelectMarker(Marker marker, DatasetPrimitives.Indicator indicator)
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
                indicator,
                year
            );
            
            if (!this.arcs.Exists((arc) => arc.trade.SameTradeAs(query))) {            
                StartCoroutine(DatasetParser.QueryAPI(query, AddArcs));
            }
            
            arcOrigin.Lowlight();
            
            arcOrigin = null;
        }
    }
    
    public void AddArcs(List<DatasetPrimitives.Trade> trades) 
    {        
        foreach (DatasetPrimitives.Trade trade in trades) {
            if (trade.value > maxValue) {
                maxValue = trade.value;
            } 
            
            if (trade.value < minValue) {
                minValue = trade.value;
            }
        }
        
        foreach (DatasetPrimitives.Trade trade in trades) {
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
        arc.Configure(this, trade);
    }
    
    public float ArcWidth(DatasetPrimitives.Trade trade)
    {
        float interval = maxValue - minValue;
        
        return (interval == 0.0f) ? 1.0f : ((trade.value - minValue) / interval);
    }
    
    public abstract Vector3 MarkerPosition(Vector2 coord);
    public abstract Vector3 ArcOutDirection(Vector3 from, Vector3 to);
    public abstract float ArcHeight();
    
    public void RemoveArc(TradeArc removedArc)
    {
        arcs.Remove(removedArc);
        
        Destroy(removedArc.node.gameObject);
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
