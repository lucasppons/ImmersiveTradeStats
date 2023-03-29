using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DatasetParser
{
    public class TradeQuery 
    {
        public DatasetPrimitives.Country reporter;
        public DatasetPrimitives.Country partner;
        public DatasetPrimitives.Product product;
        public DatasetPrimitives.Indicator indicator;
        public int year;
        
        public TradeQuery(
            DatasetPrimitives.Country reporter, 
            DatasetPrimitives.Country partner, 
            DatasetPrimitives.Product product, 
            DatasetPrimitives.Indicator indicator, 
            int year
        ) {
            this.reporter = reporter;
            this.partner = partner;
            this.product = product;
            this.indicator = indicator;
            this.year = year;
        }
    }
    
    public static IEnumerator QueryAPI(TradeQuery query, System.Action<List<DatasetPrimitives.Trade>> callback)
    {
        string yearString = query.year.ToString();
        string reporterString = query.reporter.ToString();
        string partnerString = query.partner.ToString();
        string productString = productStrings[query.product];
        string indicatorString = indicatorStrings[query.indicator];
        
        string uri = $"http://wits.worldbank.org/API/V1/SDMX/V21/datasource/tradestats-trade/reporter/{reporterString}/year/{yearString}/partner/{partnerString}/product/{productString}/indicator/{indicatorString}?format=JSON";
        
        Debug.Log(uri);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                List<DatasetPrimitives.Trade> trades = ParseJsonResult(webRequest.downloadHandler.text);
                
                callback(trades);
            } else {
                Debug.LogError(webRequest.error);
            }                
        }
    }
    
    static List<DatasetPrimitives.Trade> ParseJsonResult(string json) 
    {
        int reporterIndex = 0, partnerIndex = 0, productIndex = 0, indicatorIndex = 0, dimensionsFound = 0;
        List<Value> reporters = null, partners = null, products = null, indicators = null;
        
        List<DatasetPrimitives.Trade> trades = new List<DatasetPrimitives.Trade>();
        
        Result result = JsonConvert.DeserializeObject<Result>(json);
        
        foreach (Dimension dimension in result.structure.dimensions.series) {
            switch (dimension.id) {
                case "REPORTER": reporterIndex = dimension.keyPosition; reporters = dimension.values; dimensionsFound++; break;
                case "PARTNER": partnerIndex = dimension.keyPosition; partners = dimension.values; dimensionsFound++; break;
                case "PRODUCTCODE": productIndex = dimension.keyPosition; products = dimension.values; dimensionsFound++; break;
                case "INDICATOR": indicatorIndex = dimension.keyPosition; indicators = dimension.values; dimensionsFound++; break;
            }
        }
        
        if (dimensionsFound < 4) {
            Debug.LogError("Dimensions not found");
            
            return trades;
        }
        
        Dictionary<string, DatasetPrimitives.Product> reverseProductStrings = ReverseDict(productStrings);
        Dictionary<string, DatasetPrimitives.Indicator> reverseIndicatorStrings = ReverseDict(indicatorStrings);
        
        foreach (var entry in result.dataSets[0].series) {
            DatasetPrimitives.Trade trade = new DatasetPrimitives.Trade();
            
            string[] indexers = entry.Key.Split(':');
            
            try {
                Value reporter = reporters[int.Parse(indexers[reporterIndex])];
                Value partner = partners[int.Parse(indexers[partnerIndex])];
                Value product = products[int.Parse(indexers[productIndex])];
                
                trade.reporter = (DatasetPrimitives.Country)System.Enum.Parse(typeof(DatasetPrimitives.Country), reporter.id);
                trade.reporterName = reporter.name;
                
                trade.partner = (DatasetPrimitives.Country)System.Enum.Parse(typeof(DatasetPrimitives.Country), partner.id);
                trade.partnerName = partner.name;
                
                trade.product = reverseProductStrings[product.id];
                trade.productName = product.name;
                
                trade.indicator = reverseIndicatorStrings[indicators[int.Parse(indexers[indicatorIndex])].id];
                
                trade.value = entry.Value.observations["0"][0];
                
                if (trade.reporter != DatasetPrimitives.Country.WLD) {
                    trades.Add(trade);            
                }
                
                Debug.Log($"{trade.reporterName} | {trade.partnerName} | {trade.productName} | {trade.indicator} | {trade.value.ToString()}");
            } catch (System.Exception e) {
                Debug.LogWarning(e.Message);
            }
        }
        
        return trades;
    } 
    
    static Dictionary<T2, T1> ReverseDict<T1, T2>(Dictionary<T1, T2> dict)
    {
        Dictionary<T2, T1> reverse = new Dictionary<T2, T1>();
        
        foreach (var item in dict) {
            reverse.Add(item.Value, item.Key);
        }
        
        return reverse;
    }
    
    static Dictionary<DatasetPrimitives.Product, string> productStrings = new Dictionary<DatasetPrimitives.Product, string> 
    {
        {DatasetPrimitives.Product.Animal, "01-05_Animal"},
        {DatasetPrimitives.Product.Vegetable, "06-15_Vegetable"},
        {DatasetPrimitives.Product.FoodProducts, "16-24_FoodProd"},
        {DatasetPrimitives.Product.Minerals, "25-26_Minerals"},
        {DatasetPrimitives.Product.Fuels, "27-27_Fuels"},
        {DatasetPrimitives.Product.Chemicals, "28-38_Chemicals"},
        {DatasetPrimitives.Product.PlasticRubber, "39-40_PlastiRub"},
        {DatasetPrimitives.Product.HidesSkins, "41-43_HidesSkin"},
        {DatasetPrimitives.Product.Wood, "44-49_Wood"},
        {DatasetPrimitives.Product.TextilesClothing, "50-63_TextCloth"},
        {DatasetPrimitives.Product.Footwear, "64-67_Footwear"},
        {DatasetPrimitives.Product.StoneGlass, "68-71_StoneGlas"},
        {DatasetPrimitives.Product.Metals, "72-83_Metals"},
        {DatasetPrimitives.Product.MachElec, "84-85_MachElec"},
        {DatasetPrimitives.Product.Transportation, "86-89_Transport"},
        {DatasetPrimitives.Product.Miscellaneous, "90-99_Miscellan"},
        {DatasetPrimitives.Product.All, "ALL"}
    };
    
    static Dictionary<DatasetPrimitives.Indicator, string> indicatorStrings = new Dictionary<DatasetPrimitives.Indicator, string> 
    {
        {DatasetPrimitives.Indicator.Import, "MPRT-TRD-VL"},
        {DatasetPrimitives.Indicator.Export, "XPRT-TRD-VL"},
        {DatasetPrimitives.Indicator.Both, "MPRT-TRD-VL;XPRT-TRD-VL"},
    };
    
    [System.Serializable]
    class Result 
    {
        public List<DataSet> dataSets;
        public Structure structure;
    }
    
    [System.Serializable]
    class DataSet 
    {
        public Dictionary<string, Series> series;
    }
    
    [System.Serializable]
    class Series
    {
        public Dictionary<string, List<float>> observations;
    }
    
    [System.Serializable]
    class Structure
    {
        public Dimensions dimensions;
    }
    
    [System.Serializable]
    class Dimensions
    {
        public List<Dimension> series;
    }
    
    [System.Serializable]
    class Dimension
    {
        public string id;
        public int keyPosition;
        public List<Value> values;
    }
    
    [System.Serializable]
    class Value
    {
        public string id;
        public string name;
    }
}
