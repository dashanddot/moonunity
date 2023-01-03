using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class BuildBundle 
{
    static BuildBundle()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(ExportGameBundles2);
    }

    [MenuItem("Window/Kube Build/Build", false, 0)]
    static void ExportGameBundlesAll()
    {
        ExportGameBundles(BuildAssetBundleOptions.ForceRebuildAssetBundle);
    }

    static void ExportGameBundles2(BuildPlayerOptions opt)
    {
        ExportGameBundles(BuildAssetBundleOptions.ForceRebuildAssetBundle, opt.locationPathName );
    }

    static string FindLocalFolder()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low" ;

        return Path.Combine(Application.dataPath, "../");

    }

    static string FindProjectPath()
    {
       

        var locallow = Path.Combine(FindLocalFolder(), "nobodyshot/Sandbox/PublicGames/af865b6ba2e43b458fde2f29aad18eff");

        if (Directory.Exists(locallow))
            return locallow;

       return EditorUtility.SaveFolderPanel("Build game", "", "build");


    }

    static void ExportGameBundles(BuildAssetBundleOptions opt, string path = null )
    {
        if( path == null )
         path = FindProjectPath();
         

        //AssetBundleBuild[] builds

        var scenes = EditorBuildSettings.scenes;

        foreach (var sceneAsset in scenes)
        {
            AssetImporter ai = AssetImporter.GetAtPath(sceneAsset.path);

            string bundleName = Path.GetFileNameWithoutExtension(sceneAsset.path);

            ai.assetBundleName = bundleName + ".unity3d";

            //var realAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.sceneass>(sceneAsset.path);
            Debug.Log(sceneAsset.path);
        }

        BuildPipeline.BuildAssetBundles(path, opt, EditorUserBuildSettings.activeBuildTarget);
    }
}
