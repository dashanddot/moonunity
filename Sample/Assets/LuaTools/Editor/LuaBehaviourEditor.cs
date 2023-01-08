using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEditor;
using System;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

// Adds a nice editor to edit JSON files as well as a simple text editor incase
// the editor doesn't support the types you need. It works with strings, floats
// ints and bools at the moment.
// 
// * Requires the latest version of JSON.net compatible with Unity


//If you want to edit a JSON file in the "StreammingAssets" Folder change this to DefaultAsset.
//Hacky solution to a weird problem :/
[CustomEditor(typeof(LuaBehaviour), true)]
public class LuaBehaviourEditor : Editor
{
    private string Path => AssetDatabase.GetAssetPath(target);
    private bool isCompatible => true;
    private bool unableToParse => false;

    protected TextAsset _asset;
    protected DynValue _module;
    private void OnEnable()
    {
        _asset = serializedObject.FindProperty("script").objectReferenceValue as TextAsset;

        var script = new Script();


        if(_asset)
            _module = script.DoString(_asset.text);
    }

    private void OnDisable()
    {
       
    }

    public override void OnInspectorGUI()
    {
        if (isCompatible)
        {
            JsonInspectorGUI();
            return;
        }
        base.OnInspectorGUI();
    }

    private void JsonInspectorGUI()
    {
        serializedObject.Update();

        Rect subHeaderRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 2.5f);
        Rect helpBoxRect = new Rect(subHeaderRect.x, subHeaderRect.y, subHeaderRect.width - subHeaderRect.width / 6 - 5f, subHeaderRect.height);
        Rect rawTextModeButtonRect = new Rect(subHeaderRect.x + subHeaderRect.width / 6 * 5, subHeaderRect.y, subHeaderRect.width / 6, subHeaderRect.height);
        EditorGUI.HelpBox(helpBoxRect, "You can change public propertiest of script here", MessageType.Info);

        Rect fieldRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight );
        EditorGUILayout.ObjectField(serializedObject.FindProperty("script"));

        if (_module != null)
        {

            var dict = serializedObject.FindProperty("_dict");


            int dictSize = 0;

            foreach (var xx in _module.Table.Keys)
            {
                var cfield = _module.Table.Get(xx);

                if (cfield?.Type == DataType.Function)
                    continue;

                

                

                if (dictSize >= dict.arraySize)
                {
                    dict.InsertArrayElementAtIndex(dictSize);
                   
                }

                var row = dict.GetArrayElementAtIndex(dictSize);

                var propType = cfield.Type;
                SerializedProperty rowVal = null;

                if (propType == DataType.String || propType == DataType.Number)
                {

                    rowVal = row.FindPropertyRelative("value");

                    string kv = rowVal != null ? rowVal.stringValue : cfield.CastToString();

                    kv = EditorGUILayout.TextField(new GUIContent(xx.String), kv);

                    row.FindPropertyRelative("key").stringValue = xx.String;
                    rowVal.stringValue = kv;
                }
                else
                {
                    rowVal = row.FindPropertyRelative("ovalue");

                    EditorGUILayout.ObjectField(rowVal, typeof(GameObject), new GUIContent(xx.String));

                    //rowVal.objectReferenceValue = EditorGUILayout.ObjectField( new GUIContent(xx.String), (GameObject)rowVal.serializedObject.context, typeof(GameObject) , true );


                }

                dictSize++;
            }

            dict.arraySize = dictSize;

        }

        serializedObject.ApplyModifiedProperties();
    }



    [MenuItem("Assets/Create/Lua File", priority = 81)]
    public static void CreateNewJsonFile()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
            path = "Assets";
        else if (System.IO.Path.GetExtension(path) != "")
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

        path = System.IO.Path.Combine(path, "New LUA File.lua");

        JObject jObject = new JObject();
        File.WriteAllText(path, "" );
        AssetDatabase.Refresh();
    }
}
