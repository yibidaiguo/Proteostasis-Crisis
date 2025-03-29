using UnityEngine;

public static class ConstructionEditorWindowUtility
{
   public static int HexTypeMaxCount = 1;
   public static Color GetHexColor(int value)
   {
      if (value == 0)
         return new Color(0.2f, 0.6f, 1f, 0.5f);
      else if (value == 1)
         return new Color(1f,1f,0,0.9f);
      else
         return new Color(1f, 0, 0, 0.9f);
   }
}
