using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ProductButton : MonoBehaviour
{
    [SerializeField]
    DatasetPrimitives.Product product;
    
    [SerializeField]
    Interactable interactable;
    
    [SerializeField]
    MapFilters filters;
    
    public void Select()
    {
        filters.SelectProduct(product);
    }
    
    public void Deselect()
    {
        filters.DeselectProduct();
    }
    
    public void MatchSelected(DatasetPrimitives.Product product)
    {
        interactable.IsToggled = (product == this.product);
    }
}
