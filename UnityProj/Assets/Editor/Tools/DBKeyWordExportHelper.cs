using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.IO;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

public class DBKeyWordExportHelper : OdinEditorWindow
{
    private const string CLIENT_DB_KEYWORD_PATH = @"\UnityProj\ClientTool\db_export\cs_client_dbkeyword.xml";
    private const string GEN_KEYWORD_TOOL_PATH = @"\Server\protocol\GenKeyWord.bat";

    private string clientDBKeywordFullPath;
    private string genKeywordToolFullPath;

    private List<MacrosGroup> macrosGroups = new List<MacrosGroup>();

    [MenuItem("Tools/DB Key Word Export Helper")]
    private static void ShowWindow()
    {
        var window = GetWindow<DBKeyWordExportHelper>();
        window.titleContent = new GUIContent("DB Key Word Export Helper");
        window.minSize = new Vector2(500, 300);
    }

    private void OnEnable()
    {
        clientDBKeywordFullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), CLIENT_DB_KEYWORD_PATH);
        genKeywordToolFullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), GEN_KEYWORD_TOOL_PATH);
    }

    [Button(ButtonSizes.Large)]
    [InfoBox("打开DBKeyword目录")]
    private void OpenClientDBKeywordDir()
    {
        EditorUtility.RevealInFinder(clientDBKeywordFullPath);
    }

    [Button(ButtonSizes.Large)]
    [InfoBox("打开DBKeyword文件")]
    private void OpenClientDBKeywordFile()
    {
        EditorUtility.RevealInFinder(clientDBKeywordFullPath);
    }

    [Button(ButtonSizes.Large)]
    [InfoBox("打开GenKeyWord.bat目录")]
    private void OpenGenKeyWordToolDir()
    {
        EditorUtility.RevealInFinder(genKeywordToolFullPath);
    }
    
    [ShowInInspector]
    [PropertySpace(10)]
    [InfoBox("Client DB Keyword Path")]
    private string ClientDBKeywordPath
    {
        get => clientDBKeywordFullPath;
        set { }
    }

    [ShowInInspector]
    [PropertySpace(10)]
    [InfoBox("Gen Keyword Tool Path")]
    private string GenKeywordToolPath
    {
        get => genKeywordToolFullPath;
        set { }
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
            serializer.Serialize(stream, new MetaLib() {MacrosGroups = macrosGroups});
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

    }
    
    [OnInspectorGUI]
    private void DrawMacroGroup()
    {
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Macro Groups", EditorStyles.boldLabel);

        foreach (var group in groups)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField($"{group.name}: {group.desc}", EditorStyles.boldLabel);

            foreach (var macro in group.macros)
            {
                EditorGUILayout.LabelField(macro.ToString());
            }

            if (GUILayout.Button($"Add new macro to {group.name}"))
            {
                group.macros.Add(new Macro());
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Save to XML"))
        {
            SaveToXml();
        }
    }

    private void SaveToXml()
    {
        XElement metalib = new XElement("metalib", new XAttribute("tagsetversion", "1"),
            new XAttribute("name", "protocol"), new XAttribute("version", "1"));

        foreach (var group in groups)
        {
            XElement macrosgroup = new XElement("macrosgroup", new XAttribute("name", group.name),
                new XAttribute("desc", group.desc));

            foreach (var macro in group.macros)
            {
                XElement macroElem = new XElement("macro", new XAttribute("name", macro.name),
                    new XAttribute("value", macro.value), new XAttribute("desc", macro.desc));

            }
        }
    }
}
