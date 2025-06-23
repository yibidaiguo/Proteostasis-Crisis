using UnityEngine;

public class ConstructionData
{
   public string constructionName;
   public BluePrintData bluePrintData;
   
   public int rotation
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

   public ConstructionData Clone()
   {
      return new ConstructionData(constructionName, bluePrintData, rotation);
   }
}
