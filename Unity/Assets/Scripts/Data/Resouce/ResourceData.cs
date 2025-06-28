using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源种类
/// </summary>
public enum ResourceType
{
    //无效
    useless = -1,
    //ATP(能量货币)
    ATP = 0,                
    //氨基酸
    aminoAcid = 1,          
    //糖
    candy = 2,              
    //核苷酸前体
    preNucleotide = 3,      
    //脂
    fat = 4,                
    //核苷酸
    nucleotide = 5,       
    //丙酮酸
    pyruvate = 6,
    //粗胚转运蛋白
    roughTransPortProtein = 7,
    //简易酶蛋白
    simpleEnzymeProtein = 8,
    //结构蛋白
    structureProtein = 9,
    //标准酶蛋白
    standardEnzymeProtein = 10,
    //标准转运蛋白
    standardTransPortProtein = 11,
    /// <summary>
    /// 细胞器
    /// </summary>
    //囊泡
    bubble,
    //细胞核	
    core,
    //基因研究所
    geneStudying,
    //高尔基体
    golgi,
    //细胞器孵化所
    hatchery,
    //叶绿体
    leaf,
    //线粒体
    powerplant,
    // 溶酶体
    recycle,
    //粗面内质网
    roughNet,
    //光面内质网
    smoothNet,

}


/// <summary>
/// 资源数据，资源种类+资源数量
/// </summary>
public class ResourceData
{
    public ResourceType type;
    public int count;
    
    public ResourceData() 
    { 
        this.type = ResourceType.useless;
        this.count = -1;
    }
    public ResourceData(ResourceType type, int count)
    {
        this.type = type;
        this.count = count;
    }
}

/// <summary>
/// 资源生产命令，包含多个ResourceData   
/// </summary>
public class ResourceCommand
{
    //命令生产资源
    public List<ResourceData> outputs = new List<ResourceData>();
    //命令消耗资源
    public List<ResourceData> resourceDatas = new List<ResourceData>();

    public ResourceCommand() { }

    public ResourceCommand(List<ResourceData> resourceDatas)
    {
        this.resourceDatas = resourceDatas;
    }
    public ResourceCommand(List<ResourceData> resourceDatas, List<ResourceData> outputs)
    {
        this.resourceDatas = resourceDatas;
        this.outputs = outputs;
    }
    public Copy(ResourceCommand resourceCommand)//深拷贝
    {
        this.resourceDatas = resourceCommand.resourceDatas;
        this.outputs = resourceCommand.outputs;
    }
}











