using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Injectinator.Runtime;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Injectinator.Editor
{

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

        private bool showMethods;
        private bool showFields;
        private bool showSettings;

        private static List<MethodDescriptor> injectedMethodsList = new List<MethodDescriptor>();
        private static List<FieldDescriptor> injectedFieldsList = new List<FieldDescriptor>();

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
            iconRect = new Rect(Screen.width - iconTexture.width * headerScale, 0, iconTexture.width * headerScale,
                iconTexture.height * headerScale);
            bodyRect = new Rect(bodyPadding, iconRect.height + bodyPadding, Screen.width - bodyPadding * 2,
                Screen.height - iconRect.height - bodyPadding * 2);
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
                window.titleContent = new GUIContent(windowTitle,
                    EditorGUIUtility.ObjectContent(CreateInstance<InjectEditorWindow>(), typeof(InjectEditorWindow))
                        .image);

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
                injectedMethodsList.Add(new MethodDescriptor($"Meth_{i}", "void", Random.Range(0, 4)));

            for (int i = 1; i < 9; i++)
                injectedFieldsList.Add(new FieldDescriptor($"Fld_{i}", "float"));
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


            DrawMethods();
            DrawFields();

            DrawSettings();

            DrawRotatingButton();
            GUILayout.EndVertical();
        }

        private void DrawSettings()
        {
            GUILayout.Space(10);
            showSettings =
                EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Settings"); //, skin.GetStyle("HeaderDefault"));
            if (showSettings)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("settings details...");
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Reload Methods", GUILayout.Height(25), GUILayout.Width(Screen.width / 2 - 5)))
                    LoadMethods();

                if (GUILayout.Button("Reload Fields", GUILayout.Height(25), GUILayout.Width(Screen.width / 2 - 5)))
                    LoadFields();

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawMethods()
        {
            GUILayout.Space(20);
            showMethods = EditorGUILayout.BeginFoldoutHeaderGroup(showMethods, "Methods", skin.GetStyle("PanH1"));
            if (showMethods)
            {
                if (injectedMethodsList.Count < 1) LoadMethods();

                var maxSectionHeight = Mathf.Min(injectedFieldsList.Count * 45, Screen.height / 4);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true),
                    GUILayout.Height(maxSectionHeight));

                foreach (var item in injectedMethodsList)
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
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawFields()
        {
            GUILayout.Space(5);
            showFields = EditorGUILayout.BeginFoldoutHeaderGroup(showFields, $"{(showFields?"+":"-")} Fields", skin.GetStyle("PanH1"));
            if (showFields)
            {
                if (injectedFieldsList.Count < 1) LoadFields();

                var maxSectionHeight = Mathf.Min(injectedFieldsList.Count * 20, Screen.height / 4);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true),
                    GUILayout.Height(maxSectionHeight));

                foreach (var item in injectedFieldsList)
                {
                    string caption = $"{item.Name}";
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(caption);
                    GUI.contentColor = Color.cyan;

                    GUILayout.Label($"{item.Type}");
                    //Debug.Log(caption);

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
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
            var pivotPoint = new Vector2(iconRect.x + btnSize.x / 2, iconRect.y + btnSize.y / 2);

            //add padding
            pivotPoint.x -= 5;
            pivotPoint.y += 5;

            GUIUtility.RotateAroundPivot(iconRotation, pivotPoint);
            Vector2 pos = new Vector2(pivotPoint.x - btnSize.x / 2, pivotPoint.y - btnSize.y / 2);
            Rect btnRect = new Rect(pos.x, pos.y, btnSize.x, btnSize.y);
            //GUIContent content = new GUIContent("ROT",iconTexture);
            var btnStyle = skin.GetStyle("PanButton");
            if (GUI.Button(btnRect, iconTexture, btnStyle))
            {
                iconRotation += 30;
            }
        }

        public void LoadMethods()
        {
            Type[] types = GetDefualtAssembly().GetTypes();
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            injectedMethodsList.Clear();
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods(flags))
                {
                    if (method.GetCustomAttribute<Inject>() != null)
                    {
                        injectedMethodsList.Add(new MethodDescriptor(method.Name, method.ReturnType.Name,
                            method.GetParameters().Length));
                        //Debug.Log(method.Name);
                    }
                }
            }
        }

        public void LoadFields()
        {
            Type[] types = GetDefualtAssembly().GetTypes();
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            injectedFieldsList.Clear();
            foreach (Type type in types)
            {
                foreach (FieldInfo field in type.GetFields(flags))
                {
                    if (field.GetCustomAttribute<Inject>() != null)
                    {
                        injectedFieldsList.Add(new FieldDescriptor(field.Name, field.FieldType.Name));
                        //Debug.Log(field.Name);
                    }
                }
            }
        }

        //Helpers
        static Assembly GetDefualtAssembly() => AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
        //static IEnumerable<Type> GetTypesInDefaultAssembly<T>() => GetDefualtAssembly().GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }

    public class MethodDescriptor
    {
        public string Name;
        public string Type;
        public int Params;

        public MethodDescriptor(string name, string type, int @params)
        {
            Name = name;
            Type = type;
            Params = @params;
        }
    }

    public class FieldDescriptor
    {
        public string Name;
        public string Type;

        public FieldDescriptor(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}