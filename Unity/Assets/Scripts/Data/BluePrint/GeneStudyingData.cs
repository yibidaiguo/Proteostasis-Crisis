using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基因研究所数据
/// </summary>
public class GeneStudyingData : ConstructionData 
{
    //一次只研究一个核苷酸
    int studyprogress = -1;  //研究进度，-1表示未开始研究
    public GeneStudyingData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
    
    }
}
