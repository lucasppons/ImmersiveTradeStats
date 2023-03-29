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
    
    DataMap map = null;
    
    DatasetPrimitives.Product product = DatasetPrimitives.Product.All;
    
    int year = 2004;
    
    void Update()
    {
        if (map == null) return;
        
        transform.position = map.FiltersPosition() + new Vector3(0.066f, 0.0f, 0.0f);
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
    
    public void Open(DataMap map, DatasetPrimitives.Product product = DatasetPrimitives.Product.All, int year = 2004)
    {
        this.map = map;
        
        SelectProduct(product);
        
        Debug.Log(year);
        Debug.Log((year - 1988) / 32);
        Debug.Log((year - 1988) / 32.0f);
        
        yearSlider.SliderValue = (year - 1988) / 32.0f;
    }
    
    public void Close()
    {
        map.FiltersClosed();
        
        Destroy(gameObject);
    }
}
