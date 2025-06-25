using UnityEngine;

///
/// 建筑所占有的多个格子所对应的一个建筑数据，只记载动态数据（如正在生产的产品），
///不记载静态数据（如建筑描述文本），
///静态数据由建筑名称索引
/// 
public class ConstructionData             
{
   /// <summary>
   /// 管理游戏建造逻辑的数据(主要让玩家思考的) 
   /// </summary>
   
   // 无

   /// <summary>
   /// 管理实现建造逻辑的数据(主要为程序实现准备的)
   /// </summary>
   public string constructionName;        //建筑名称
   public BluePrintData bluePrintData;    //蓝图数据，用于根据建筑位置找到建筑占用的多个格子
   
   public int rotation                    //旋转角度
   {
      get => _rotation;
      set
      {
         if (Mathf.Abs(value) > 6)
            _rotation = value % 6;
         else
            _rotation = value;
      }
   }
   
   private int _rotation;
   
   public ConstructionData(string constructionName, BluePrintData bluePrintData,int rotation)
   {
      this.constructionName = constructionName;
      this.bluePrintData = bluePrintData;
      this.rotation = rotation;
   }

   public ConstructionData Clone()//深拷贝
   {
      return new ConstructionData(constructionName, bluePrintData, rotation);
   }
}
