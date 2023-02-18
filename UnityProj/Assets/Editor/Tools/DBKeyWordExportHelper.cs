using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector.Editor;

public class DBKeyWordExportHelper : OdinEditorWindow
{
    private const string CLIENT_DB_KEYWORD_PATH = "/UnityProj/ClientTool/db_export/cs_client_dbkeyword.xml";
    private string cs_client_dbkeyword_path;

    [Button("Open Directory")]
    private void OpenDirectory()
    {
        if (cs_client_dbkeyword_path == null)
        {
            cs_client_dbkeyword_path = Path.Combine(Application.dataPath.Replace("Assets", ""), "UnityProj/ClientTool/db_export/cs_client_dbkeyword.xml");
        }

        string directory = Path.GetDirectoryName(cs_client_dbkeyword_path);
        if (Directory.Exists(directory))
        {
            Process.Start(directory);
        }
        else
        {
            UnityEngine.Debug.LogError("Directory not exist: " + directory);
        }
    }

    [Button("Open File")]
    private void OpenFile()
    {
        if (cs_client_dbkeyword_path == null)
        {
            cs_client_dbkeyword_path = Path.Combine(Application.dataPath.Replace("Assets", ""), "UnityProj/ClientTool/db_export/cs_client_dbkeyword.xml");
        }

        if (File.Exists(cs_client_dbkeyword_path))
        {
            Process.Start(cs_client_dbkeyword_path);
        }
        else
        {
            UnityEngine.Debug.LogError("File not exist: " + cs_client_dbkeyword_path);
        }
    }

    [Button("Refresh Path")]
    private void RefreshPath()
    {
        cs_client_dbkeyword_path = Path.Combine(Application.dataPath.Replace("Assets", ""), "UnityProj/ClientTool/db_export/cs_client_dbkeyword.xml");
    }

    [PropertyOrder(-1)]
    [LabelText("cs_client_dbkeyword.xml Path")]
    [ReadOnly]
    public string CsClientDbkeywordPath
    {
        get
        {
            if (cs_client_dbkeyword_path == null)
            {
                RefreshPath();
            }
            return cs_client_dbkeyword_path;
        }
    }

    [MenuItem("Tools/DBKeyWord Export Helper")]
    private static void OpenWindow()
    {
        GetWindow<DBKeyWordExportHelper>().Show();
    }
}

