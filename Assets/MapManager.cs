using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    FollowMeToggle toggler;
    
    [SerializeField]
    Flat flatPrefab;
    
    [SerializeField]
    Globe globePrefab;
    
    void Start()
    {
        toggler.ToggleFollowMeBehavior();
    }
    
    public void AddFlat()
    {
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0.0f;
        
        Flat flat = Instantiate(flatPrefab, Camera.main.transform.position + (direction * 2), Quaternion.identity);
        
        flat.transform.LookAt(Camera.main.transform);
        
        flat.transform.Rotate(-75.0f, 180.0f, 0.0f);
    }
    
    public void AddGlobe()
    {
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0.0f;
        
        Globe globe = Instantiate(globePrefab, Camera.main.transform.position + (direction * 2), Quaternion.identity);
        
        globe.transform.LookAt(Camera.main.transform);
        
        globe.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
