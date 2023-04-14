using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class MapFilters : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text selectedYearLabel;
    
    [SerializeField]
    List<ProductButton> buttons;
    
    [SerializeField]
    PinchSlider yearSlider;
    
    [SerializeField]
    Flat flatPrefab;
    
    [SerializeField]
    Globe globePrefab;
    
    DataMap map = null;
    
    DatasetPrimitives.Product product = DatasetPrimitives.Product.All;
    
    int year = 2004;
    
    void Update()
    {
        if (map == null) return;
        
        transform.position = map.FiltersPosition();
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    
    public void SelectProduct(DatasetPrimitives.Product product)
    {
        this.product = product;
        
        foreach (ProductButton button in buttons) {
            button.MatchSelected(product);
        }
    }
    
    public void DeselectProduct()
    {
        product = DatasetPrimitives.Product.All;
    }
    
    public void SelectYear(SliderEventData data)
    {
        year = 1988 + (int)(32 * data.NewValue);
        
        selectedYearLabel.text = year.ToString();
    }
    
    public void ApplyFilters()
    {
        if (product == DatasetPrimitives.Product.All) return;
        
        map.ApplyFilters(product, year);
        
        Close();
    }
    
    public void DuplicateMap()
    {
        DataMap newMap = (DataMap) Instantiate(
            (map is Flat) ? flatPrefab : globePrefab, 
            Camera.main.transform.position + (Quaternion.AngleAxis(60.0f, Vector3.up) * (map.transform.position - Camera.main.transform.position)), 
            map.transform.rotation
        );
        
        newMap.transform.Rotate(0.0f, 60.0f, 0.0f, Space.World);
        
        List<DatasetPrimitives.Trade> trades = new List<DatasetPrimitives.Trade>();
        
        foreach (TradeArc arc in map.arcs) {
            trades.Add(arc.trade);
        }
        
        newMap.ApplyFilters(map.product, map.year);
        newMap.AddTrades(trades);
    }
    
    public void DeleteMap()
    {
        Destroy(map.gameObject);
        Destroy(gameObject);
    }
    
    public void Open(DataMap map, DatasetPrimitives.Product product = DatasetPrimitives.Product.All, int year = 2004)
    {
        this.map = map;
        
        SelectProduct(product);
        
        yearSlider.SliderValue = (year - 1988) / 32.0f;
    }
    
    public void Close()
    {
        map.FiltersClosed();
        
        Destroy(gameObject);
    }
}
