using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

using System.IO;

public class SanboxLuaLoader : IScriptLoader
{
    public bool ScriptFileExists(string name)
    {
        return File.Exists(name);
    }

    public object LoadFile(string file, Table globalContext)
    {
        return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    public string ResolveFileName(string filename, Table globalContext)
    {
        return filename;
    }

    public string ResolveModuleName(string modname, Table globalContext)
    {
        return modname;
    }
}

public class LuaBehaviour : MonoBehaviour
{
    public TextAsset script;

    protected Script _script;

    protected DynValue _module;

    protected Closure _update;

    public void Awake()
    {

        _script = new Script();

        var loader = new SanboxLuaLoader();

        _script.Options.ScriptLoader = loader;
        _script.Options.DebugPrint = (s) => Debug.Log(s);

        _module = _script.DoString(script.text);

        _module.Table.Get("Awake").Function?.Call();

    }

    // Start is called before the first frame update
    public void Start()
    {
        _module.Table.Get("Start").Function?.Call();

        _update = _module.Table.Get("Update").Function;
    }

    // Update is called once per frame
    public void Update()
    {
        if (_update!=null)
            _update.Call();
    }

    private void OnEnable()
    {
        _module.Table.Get("OnEnable").Function?.Call();
    }

    private void OnDisable()
    {
        _module.Table.Get("OnDisable").Function?.Call();
    }

}
