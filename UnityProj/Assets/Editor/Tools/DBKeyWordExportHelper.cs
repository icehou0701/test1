using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

public class DBKeyWordExportHelper : EditorWindow
{
    private static readonly string CLIENT_DB_KEYWORD_PATH = "/UnityProj/ClientTool/db_export/cs_client_dbkeyword.xml";
    private static readonly string GEN_KEYWORD_BAT_PATH = "/Server/protocol/GenKeyWord.bat";

    private static readonly string CLIENT_DB_KEYWORD_FULL_PATH =
        Path.Combine(Application.dataPath.Replace("Assets", ""), CLIENT_DB_KEYWORD_PATH);
    private static readonly string GEN_KEYWORD_BAT_FULL_PATH =
        Path.Combine(Application.dataPath.Replace("Assets", ""), GEN_KEYWORD_BAT_PATH);

    private Vector2 scrollPosition = Vector2.zero;
    private string selectedMacroGroupName = "";
    private string newMacroName = "";
    private string newMacroValue = "";
    private string newMacroDesc = "";

    private bool isInit = false;
    private List<XElement> macroGroupElements;

    [MenuItem("Tools/DBKeywordExportHelper")]
    static void Init()
    {
        DBKeyWordExportHelper window = (DBKeyWordExportHelper)EditorWindow.GetWindow(typeof(DBKeyWordExportHelper));
        window.Show();
        window.InitMacroGroupElements();
    }

    void InitMacroGroupElements()
    {
        XDocument doc = XDocument.Load(CLIENT_DB_KEYWORD_FULL_PATH);
        macroGroupElements = doc.Root.Elements("macrosgroup").ToList();
        if (macroGroupElements.Count > 0)
        {
            selectedMacroGroupName = macroGroupElements[0].Attribute("name").Value;
        }
    }

    void OnGUI()
    {
        if (!isInit)
        {
            InitMacroGroupElements();
            isInit = true;
        }

        GUILayout.Label("cs_client_dbkeyword.xml Path: " + CLIENT_DB_KEYWORD_FULL_PATH);
        GUILayout.Label("GenKeyWord.bat Path: " + GEN_KEYWORD_BAT_FULL_PATH);
        EditorGUILayout.Space();

        if (GUILayout.Button("Open DBKeyword Export Directory"))
        {
            string directory = Path.Combine(Application.dataPath.Replace("Assets", ""), "/UnityProj/ClientTool/db_export");
            if (Directory.Exists(directory))
            {
                Process.Start(directory);
            }
        }

        if (GUILayout.Button("Open DBKeyword File"))
        {
            if (File.Exists(CLIENT_DB_KEYWORD_FULL_PATH))
            {
                EditorUtility.OpenWithDefaultApp(CLIENT_DB_KEYWORD_FULL_PATH);
            }
        }

        if (GUILayout.Button("Open GenKeyWord.bat Directory"))
        {
            string directory = Path.Combine(Application.dataPath.Replace("Assets", ""), "/Server/protocol");
            if (Directory.Exists(directory))
            {
                Process.Start(directory);
            }
        }

        EditorGUILayout.Space();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Macro Group List:");
        if (macroGroupElements.Count > 0)
        {
            List<string> macroGroupNames = macroGroupElements.Select(x => x.Attribute("name").Value).ToList();
            int selectedIndex = macroGroupNames.IndexOf(selectedMacroGroupName);
            int newSelectedIndex = EditorGUILayout.Popup(selectedIndex, macroGroupNames.ToArray());
            if (newSelectedIndex != selectedIndex)
            {
                selectedMacroGroupName = macroGroupNames[newSelectedIndex];
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        XElement selectedMacroGroup = macroGroupElements.FirstOrDefault(x => x.Attribute("name").Value == selectedMacroGroupName);
       
