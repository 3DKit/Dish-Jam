using UnityEngine;
using UnityEditor;

public class CompileFix : MonoBehaviour
{
    [MenuItem("Tools/Force Recompile")]
    public static void ForceRecompile()
    {
        AssetDatabase.Refresh();
        EditorUtility.RequestScriptReload();
    }
} 