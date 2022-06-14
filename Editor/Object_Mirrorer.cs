using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;
using UnityEngine.Rendering;
using UnityEditorInternal;

public class Object_Mirrorer : EditorWindow
{
    Transform sourceObject;
    Transform targetParentObject;

    bool addSuffix = true;

    [MenuItem("PeanutTools/Object Mirrorer")]
    public static void ShowWindow()
    {
        var window = GetWindow<Object_Mirrorer>();
        window.titleContent = new GUIContent("Object Mirrorer");
        window.minSize = new Vector2(250, 50);
    }

    void HorizontalRule() {
       Rect rect = EditorGUILayout.GetControlRect(false, 1);
       rect.height = 1;
       EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
    }

    void LineGap() {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    void ForceRefresh() {
        GUI.FocusControl(null);
    }

    void RenderLink(string label, string url) {
        Rect rect = EditorGUILayout.GetControlRect();

        if (rect.Contains(Event.current.mousePosition)) {
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            if (Event.current.type == EventType.MouseUp) {
                Help.BrowseURL(url);
            }
        }

        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(0.5f, 0.5f, 1);

        GUI.Label(rect, label, style);
    }

    void OnGUI()
    {
        GUIStyle italicStyle = new GUIStyle(GUI.skin.label);
        italicStyle.fontStyle = FontStyle.Italic;

        GUILayout.Label("Object Mirrorer", EditorStyles.boldLabel);
        GUILayout.Label("Mirrors a Unity game object.", italicStyle);

        LineGap();

        sourceObject = (Transform)EditorGUILayout.ObjectField("Source object", sourceObject, typeof(Transform));

        LineGap();

        targetParentObject = (Transform)EditorGUILayout.ObjectField("New parent object", targetParentObject, typeof(Transform));

        EditorGUI.BeginDisabledGroup(sourceObject == null);
        
        if (GUILayout.Button("Use same parent", GUILayout.Width(150), GUILayout.Height(25))) {
            UseSameParent();
        }

        LineGap();

        addSuffix = EditorGUILayout.Toggle("Add suffix", addSuffix);

        LineGap();

        EditorGUI.BeginDisabledGroup(targetParentObject == null);

        if (GUILayout.Button("Mirror", GUILayout.Width(75), GUILayout.Height(25))) {
            Mirror();
        }

        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        GUILayout.Label("Links:");

        RenderLink("  Download new versions from GitHub", "https://github.com/imagitama/object-mirrorer");
        RenderLink("  Get support from my Discord", "https://discord.gg/R6Scz6ccdn");
        RenderLink("  Follow me on Twitter", "https://twitter.com/@HiPeanutBuddha");
    }

    void UseSameParent() {
        targetParentObject = sourceObject.transform.parent;
        ForceRefresh();
    }

    void Mirror() {
        GameObject tempObject = new UnityEngine.GameObject("Mirrorer_Temp");
        tempObject.transform.position = new Vector3(0, 0, 0);

        Transform clonedSourceObjectTransform = UnityEngine.GameObject.Instantiate(sourceObject, sourceObject.transform.parent);
        
        clonedSourceObjectTransform.gameObject.name = sourceObject.name + (addSuffix ? " - Mirrored" : "");
        
        clonedSourceObjectTransform.SetParent(tempObject.transform, true);

        tempObject.transform.localScale = new Vector3(-1, 1, 1);

        clonedSourceObjectTransform.SetParent(targetParentObject.transform, true);



        DestroyImmediate(tempObject);
    }

//     How to mirror fins 100% perfectly:
// 1) duplicate the fin
// 2) create an empty object in root of the scene, make sure its on 0, 0, 0 location
// 3) move the new fin into it from your avatar
// 4) set the X scale of this empty to -1
// 5) move the fin back onto the avatar
}
