using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 细胞核数据
/// </summary>
public class CoreData : ConstructionData 
{
    public Data<int> coreLevel = new Data<int>();          //细胞核等级
    public CoreData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }



    
}
