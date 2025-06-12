using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class PlayerPrefsRemove : EditorWindow
{
    [MenuItem("Tools/Player Prefs Remover")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player Prefs Deleted!");
    }
}