using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.IO;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class DBKeyWordExportHelper : EditorWindow
{
    private const string CLIENT_DB_KEYWORD_PATH = @"\UnityProj\ClientTool\db_export\cs_client_dbkeyword.xml";
    private const string GEN_KEYWORD_TOOL_PATH = @"\Server\protocol\GenKeyWord.bat";

    private static string clientDBKeywordPath = Application.dataPath.Replace("Assets", "") + CLIENT_DB_KEYWORD_PATH;
    private static string genKeywordToolPath = Application.dataPath.Replace("Assets", "") + GEN_KEYWORD_TOOL_PATH;

    private static string savePath = "";
    private static string openPath = "";

    private List<MacrosGroup> macrosGroups = new List<MacrosGroup>();

    [MenuItem("Tools/DB Key Word Export Helper")]
    private static void ShowWindow()
    {
        var window = GetWindow<DBKeyWordExportHelper>();
        window.titleContent = new GUIContent("DB Key Word Export Helper");
        window.minSize = new Vector2(500, 300);
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("File Panel")]
    private void OpenClientDBKeywordDirectory()
    {
        var dirPath = Path.GetDirectoryName(clientDBKeywordPath);
        openPath = dirPath;
        EditorUtility.RevealInFinder(dirPath);
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("File Panel")]
    private void OpenClientDBKeywordFile()
    {
        openPath = clientDBKeywordPath;
        EditorUtility.OpenWithDefaultApp(clientDBKeywordPath);
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("Tool Panel")]
    private void OpenGenKeywordToolDirectory()
    {
        var dirPath = Path.GetDirectoryName(genKeywordToolPath);
        openPath = dirPath;
        EditorUtility.RevealInFinder(dirPath);
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("Macro Panel")]
    private void AddNewMacro()
    {
        var groupIndex = UnityEditor.EditorGUILayout.Popup("Group", 0, GetGroupNames());
        var groupName = GetGroupNames()[groupIndex];
        var group = GetGroupByName(groupName);

        if (group != null)
        {
            var newMacro = new Macro();
            group.Macros.Add(newMacro);
        }
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("Macro Panel")]
    private void SaveMacros()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MetaLib));
        using (FileStream stream = new FileStream(clientDBKeywordPath, FileMode.Create))
        {
            serializer.Serialize(stream, new MetaLib() { MacrosGroups = macrosGroups });
        }
    }

    protected override void OnGUI()
    {
        GUILayout.Label("File Path:", EditorStyles.boldLabel);
        SirenixEditorGUI.BeginHorizontalToolbar();
        GUILayout.Label("cs_client_dbkeyword.xml:");
        GUILayout.Label(clientDBKeywordPath, SirenixGUIStyles.MultiLineLabel);
        SirenixEditorGUI.EndHorizontalToolbar();

        GUILayout.Space(20);

        GUILayout.Label("File Operation:", EditorStyles.boldLabel);
        SirenixEditorGUI.BeginHorizontalToolbar();
        if (GUILayout.Button("Open Directory"))
        {
            OpenClientDBKeywordDirectory();
        }
        if (GUILayout.Button("Open File"))
        {
            OpenClientDBKeywordFile();
        }
        SirenixEditorGUI.EndHorizontalToolbar();

        GUILayout.Space(20);

