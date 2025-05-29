using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class ConstructionEditorWindow : EditorWindow
{
    private Dictionary<Vector3Int, HexData> hexagons = new();
    private int curSize;
    private BluePrintDataConfig loadedDataConfig;
    private string savePath = "Assets/Blueprints";
    private string fileName = "NewBlueprint";
    
    private int layers = 2;
    private int Layers 
    {
        get { return layers; }
        set
        {
            if (value != layers)
            {
                layers = value;
                GenerateBlueprint();
            }
        }
    }
    
    private int hexSize = 30;
    private int HexSize
    {
        get { return hexSize; }
        set
        {
            if (value != hexSize)
            {
                hexSize = value;
                GenerateBlueprint();
            }
        }
    }

    private float hexOffset => (Layers - 0.5f) * (HexSize * BluePrintUtility.sqrt3);

    [MenuItem("Tools/建筑编辑器")]
    private static void ShowWindow()
    {
        var window = GetWindow<ConstructionEditorWindow>();
        window.titleContent = new GUIContent("建筑编辑器");
    }

    private void OnEnable()
    {
        GenerateBlueprint();
    }
    
    private void OnGUI()
    {
        // 修改层数输入和新增矩形尺寸
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        {
            // 层数输入
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("层数:", GUILayout.Width(40));
            Layers = EditorGUILayout.IntField(Layers, GUILayout.Width(60));
            Layers = Mathf.Clamp(Layers, 1, 10); // 限制1-10层
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("矩形尺寸:", GUILayout.Width(60));
            HexSize = EditorGUILayout.IntField(HexSize, GUILayout.Width(60));
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        EditorGUI.BeginChangeCheck();
        loadedDataConfig = (BluePrintDataConfig)EditorGUILayout.ObjectField(
            "蓝图文件", 
            loadedDataConfig, 
            typeof(BluePrintDataConfig), 
            false
        );
        
        if (EditorGUI.EndChangeCheck())
        {
            if (loadedDataConfig != null)
            {
                LoadDataToEditor();
            }
            else
            {
                loadedDataConfig = null;
                hexagons.Clear();
                GenerateBlueprint();
            }
        }
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        // 文件名输入
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("文件名:", GUILayout.Width(60));
        fileName = EditorGUILayout.TextField(fileName);
        EditorGUILayout.EndHorizontal();

        // 保存路径输入
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("保存路径:", GUILayout.Width(60));
        savePath = EditorGUILayout.TextField(savePath);
        if (GUILayout.Button("浏览", GUILayout.Width(50)))
        {
            string selectedPath = EditorUtility.SaveFolderPanel("选择保存路径", savePath, "");
            if (!string.IsNullOrEmpty(selectedPath))
                if (selectedPath.StartsWith(Application.dataPath))
                    savePath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                else
                    savePath = selectedPath;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        Vector2 windowCenter = new Vector2(position.width / 2, position.height / 2);
        if (GUILayout.Button("生成蓝图", GUILayout.Width(120)))
        {
            GenerateBlueprint();
        }

        if (GUILayout.Button("保存蓝图", GUILayout.Width(120)))
        {
            SaveBlueprint();
        }

        foreach (var hex in hexagons)
        {
            hex.Value.center = BluePrintUtility.GetCoordsWithinRadius(windowCenter, hex.Key, HexSize); // 更新中心坐标
            DrawHexagon(hex.Value);
            CheckClickEvent(hex.Value);
        }
    }

    /// <summary>
    /// 绘制六边形
    /// </summary>
    /// <param name="hex"></param>
    private void DrawHexagon(HexData hex)
    {
        Vector2 center = new Vector2(hex.center.x, hex.center.y + hexOffset);
        
        Vector2[] corners = GetHexCorners(center, hex.radius);

        // 填充六边形
        Handles.color = ConstructionEditorWindowUtility.GetHexColor(hex.value);
        Handles.DrawAAConvexPolygon(corners.Select(p => (Vector3)p).ToArray());

        // 绘制边框
        Handles.color = Color.white;
        for (int i = 0; i < 6; i++)
        {
            Handles.DrawLine(corners[i], corners[(i + 1) % 6]);
        }

        // 显示数值
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
        Handles.Label(center, hex.value.ToString(), style);
    }

    /// <summary>
    /// 平顶六边形顶点计算
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    Vector2[] GetHexCorners(Vector2 center, float radius)
    {
        Vector2[] points = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = 60 * i; //平顶六边形角度
            float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            points[i] = new Vector2(x, y);
        }
        return points;
    }

    /// <summary>
    /// 处理点击事件
    /// </summary>
    /// <param name="hex"></param>
    private void CheckClickEvent(HexData hex)
    {
        Vector2 center = new Vector2(hex.center.x, hex.center.y + hexOffset);
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2[] corners = GetHexCorners(center, hex.radius);
            if (IsPointInPolygon(e.mousePosition, corners))
            {
                if (hex.value <= ConstructionEditorWindowUtility.HexTypeMaxCount)
                    hex.value++;
                else
                    hex.value = 0;

                Repaint();
                e.Use();
            }
        }
    }

    /// <summary>
    /// 射线法判断点是否在多边形内
    /// </summary>
    /// <param name="point"></param>
    /// <param name="polygon"></param>
    /// <returns></returns>
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int intersections = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 p1 = polygon[i];
            Vector2 p2 = polygon[(i + 1) % polygon.Length];
            if ((point.y > Mathf.Min(p1.y, p2.y)) &&
                (point.y <= Mathf.Max(p1.y, p2.y)) &&
                (point.x <= Mathf.Max(p1.x, p2.x)))
            {
                float xIntersection = (point.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
                if (p1.x == p2.x || point.x <= xIntersection)
                    intersections++;
            }
        }

        return intersections % 2 == 1;
    }

    /// <summary>
    /// 将数据加载到编辑器
    /// </summary>
    private void LoadDataToEditor()
    {
        foreach (var hex in hexagons.Values) hex.value = 0;
        
        if (loadedDataConfig != null)
        {
            int maxLayer = loadedDataConfig.hexagons.Keys
                .Select(BluePrintUtility.GetLayer)
                .DefaultIfEmpty(Layers)
                .Max();
            
            Layers = maxLayer;
            
            foreach (var pair in loadedDataConfig.hexagons)
            {
                if (hexagons.TryGetValue(pair.Key, out HexData hex))
                {
                    hex.value = pair.Value;
                }
                else
                {
                    Vector2 pos = BluePrintUtility.GetCoordsWithinRadius(
                        new Vector2(position.width / 2, position.height / 2),
                        pair.Key,
                        HexSize
                    );
                    hexagons[pair.Key] = new HexData(pos, HexSize) { value = pair.Value };
                }
            }
            
        }
        
        Repaint();
    }
    
    /// <summary>
    /// 生成蓝图
    /// </summary>
    private void GenerateBlueprint()
    {
        var allCoords = BluePrintUtility.GetCoordsWithinLayers(Vector3Int.zero, Layers);
        foreach (var coord in allCoords)
        {
            if (curSize != HexSize)
            {
                hexagons.Clear();
            }
            if (hexagons.ContainsKey(coord))
            {
                hexagons[coord].value = 0; // 重置现有坐标
            }
            else
            {
                Vector2 pos = BluePrintUtility.GetCoordsWithinRadius(
                    new Vector2(position.width / 2,position.height / 2), 
                    coord, 
                    HexSize
                );
                hexagons.Add(coord, new HexData(pos, HexSize));
                curSize = HexSize;
            }
        }
        
        var toRemove = hexagons.Keys.Where(k => !allCoords.Contains(k)).ToList();
        foreach (var key in toRemove) hexagons.Remove(key);

        Repaint();
    }

    /// <summary>
    /// 保存蓝图
    /// </summary>
    private void SaveBlueprint()
    {
        BluePrintDataConfig dataConfig = loadedDataConfig ?? CreateInstance<BluePrintDataConfig>();
        dataConfig.hexagons.Clear();
        
        foreach (var hex in hexagons)
        {
            if(hex.Value.value > 0)
                dataConfig.hexagons.Add(hex.Key, hex.Value.value);
        }
        
        string assetPath = $"{savePath}/{fileName}.asset".Replace("\\", "/");
        string fullPath = Path.Combine(
            Directory.GetParent(Application.dataPath).FullName, 
            assetPath
        ).Replace("/", Path.DirectorySeparatorChar.ToString());
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        
        if (loadedDataConfig == null)
        {
            // 确保路径在Assets目录下
            if (!assetPath.StartsWith("Assets/"))
            {
                Debug.LogError("保存路径必须在Assets目录下！");
                return;
            }
        
            AssetDatabase.CreateAsset(dataConfig, assetPath);
        }
        else
        {
            EditorUtility.SetDirty(dataConfig);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

public class HexData
{
    public int value;
    public Vector2 center; //六边形中心坐标
    public float radius; //外接圆半径

    public HexData(Vector2 pos, float size)
    {
        center = pos;
        radius = size;
        value = 0;
    }
}