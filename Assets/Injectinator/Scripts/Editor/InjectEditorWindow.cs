using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class InjectEditorWindow : EditorWindow
{
    private static Texture2D iconTexture;
    private Texture2D headerTexture;
    private Texture2D shadowTexture;
    private static Texture2D bottomTex;

    //private Rect headerBGRect;
    private static Rect iconRect;
    private static Rect shadowDropRect;
    private static Rect fullRect;
    private static Rect headerRect;
    private static Rect bodyRect;
    private static Rect bottomRect;

    private GUIStyle headerStyle;
    private GUISkin skin;

    private static float iconRotation;
    private static float headerScale = 0.1f;
    private static float bodyPadding;
    private Vector2 scrollPos;
    private bool showSettings;
    
    private static List<methodInfo> injectsList = new List<methodInfo>();

    private static InjectEditorWindow window;

    private void OnEnable() => LoadTextures();

    private void LoadTextures()
    {
        iconTexture = Resources.Load<Texture2D>("icons/inject");
        shadowTexture = Resources.Load<Texture2D>("icons/shadowDrop");
        headerTexture = Resources.Load<Texture2D>("icons/headerBG2");
        bottomTex = Resources.Load<Texture2D>("icons/Doofenshmirtz_half");

        skin = Resources.Load<GUISkin>("guiStyles/Default");
        headerStyle = skin.GetStyle("PanHeader");
    }

    private static void DrawRects()
    {
        iconRect = new Rect(Screen.width - iconTexture.width * headerScale, 0, iconTexture.width * headerScale, iconTexture.height * headerScale);
        bodyRect = new Rect(bodyPadding, iconRect.height + bodyPadding, Screen.width - bodyPadding * 2, Screen.height - iconRect.height - bodyPadding * 2);
        fullRect = new Rect(0, 0, Screen.width, Screen.height);
        bottomRect = new Rect(0, Screen.height - bottomTex.height, bottomTex.width, bottomTex.height);
    }

    [MenuItem("Tools/Injection Manager &I")]
    public static void OpenWindow()
    {

        string windowTitle = "Injectinator";
        if (window == null)
        {
            window = GetWindow<InjectEditorWindow>(windowTitle, typeof(SceneView));
        }

        //InitLayout();
        //void InitLayout()
        {
            //Window Setup
            window.titleContent = new GUIContent(windowTitle, EditorGUIUtility.ObjectContent(CreateInstance<InjectEditorWindow>(), typeof(InjectEditorWindow)).image);

            window.minSize = new Vector2(350, 350);
            window.maxSize = new Vector2(1200, 500);
            window.Show();

            //Header
            iconRotation = 0;

            //Body
            DrawRects();
            FakeEntries();
        }
    }
    private static void FakeEntries()
    {
        for (int i = 1; i < 9; i++)
            injectsList.Add(new methodInfo($"Meth_{i}", "void", Random.Range(0, 4)));
    }

    private void OnGUI()
    {
        DrawRects();
        if (EditorApplication.isPlaying) 
        {
            DrawBottom();
            DrawHeader();
            return;
        }
        
        //if (showSettings) DrawBottom();
        DrawHeader();

        GUILayout.BeginVertical();
        GUILayout.Space(20);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height / 4));
        foreach (var item in injectsList)
        {
            string caption = $"{item.Name}:{item.Type}";
            GUILayout.BeginHorizontal();
            GUILayout.Label(caption);
            GUI.contentColor = Color.cyan;

            string plural = item.Params > 1 ? "parameters" : "parameter";
            if (GUILayout.Button($"Inject {item.Params} {plural}", GUILayout.Height(25), GUILayout.Width(150)))
            {
                Debug.Log(caption);
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Settings");//, skin.GetStyle("HeaderDefault"));
        if (showSettings)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("settings details...");
            if (GUILayout.Button("Reload Methods", GUILayout.Height(25)))
            {
                LoadMethods();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

        DrawRotatingButton();
    }

    private static void DrawBottom()
    {
        GUILayout.BeginVertical();
        
        GUI.DrawTexture(bottomRect, bottomTex);
        
        GUILayout.EndVertical();
    }

    private void DrawHeader()
    {
        headerScale = 0.30f;
        //Header BG

        headerRect = new Rect(0, 0, headerTexture.width / 2, headerTexture.height * headerScale + 5);
        GUI.DrawTexture(headerRect, headerTexture);

        shadowDropRect = new Rect(0, headerRect.height, Screen.width, shadowTexture.height * headerScale + 5);
        GUI.DrawTexture(shadowDropRect, shadowTexture);

        //GUI.DrawTexture(iconRect, iconTexture);

        GUILayout.Label("Injectinator", headerStyle);

    }

    private void DrawRotatingButton()
    {
        Vector2 btnSize = iconRect.size * 0.9f;
        var pivotPoint = new Vector2(iconRect.x+btnSize.x/2,iconRect.y+btnSize.y/2);

        //add padding
        pivotPoint.x -= 5;
        pivotPoint.y += 5;

        GUIUtility.RotateAroundPivot(iconRotation, pivotPoint);
        Vector2 pos = new Vector2(pivotPoint.x - btnSize.x / 2,pivotPoint.y - btnSize.y / 2);
        Rect btnRect = new Rect(pos.x, pos.y, btnSize.x, btnSize.y);
        //GUIContent content = new GUIContent("ROT",iconTexture);
        var btnStyle = skin.GetStyle("PanButton");
        if (GUI.Button(btnRect, iconTexture,btnStyle))
        {
            iconRotation += 30;
        }
    }

    public void LoadMethods()
    {
        Type[] types = GetDefualtAssembly().GetTypes();
        var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        injectsList.Clear();
        foreach (Type type in types)
        {
            foreach (MethodInfo method in type.GetMethods(flags))
            {
                if (method.GetCustomAttribute<Inject>() != null)
                {
                    injectsList.Add(new methodInfo(method.Name, method.ReturnType.Name,method.GetParameters().Length));
                    Debug.Log(method.Name);
                }
            }
        }

        //Helpers
        static Assembly GetDefualtAssembly() => AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
        //static IEnumerable<Type> GetTypesInDefaultAssembly<T>() => GetDefualtAssembly().GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }
}
public class methodInfo
{
    public string Name;
    public string Type;
    public int Params;
    public methodInfo(string name, string type, int @params)
    {
        Name = name;
        Type = type;
        Params = @params;
    }
}