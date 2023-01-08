using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

using System.IO;

[System.Serializable]
public class LuaKeyValueAny
{
    public string key;
    public string value;
    public UnityEngine.Object ovalue;

}

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

public class LuaBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    public TextAsset script;

    protected Script _script;

    protected DynValue _module;

    protected Closure _update;

    [HideInInspector]
    public List<LuaKeyValueAny> _dict;

    public void Awake()
    {

        _script = new Script();

        var loader = new SanboxLuaLoader();

        _script.Options.ScriptLoader = loader;
        _script.Options.DebugPrint = (s) => Debug.Log(s);

        _module = _script.DoString(script.text);

        foreach ( var kk in _dict)
        {
            _module.Table[kk.key] =  kk.value;
        }

        _module.Table.Get("Awake").Function?.Call();

    }

    // Start is called before the first frame update
    public IEnumerator Start()
    {
        _update = _module.Table.Get("Update").Function;

        var sco = _module.Table.Get("Start");

        if (sco.Function == null)
            yield break;

        sco = _script.CreateCoroutine(sco);

        DynValue x = sco.Coroutine.Resume(_module);

        while (true)
        {
            x = sco.Coroutine.Resume();

            yield return new WaitForSeconds((float)x.Number);

            if (sco.Coroutine.State == CoroutineState.Dead)
                break;
        }

        yield break;
    }

    protected DynValue _urq;
    protected float nextUpdate = 0;

    // Update is called once per frame
    public void Update()
    {
        if(_update!=null)
        _update.Call(_module);
    }

    private void OnEnable()
    {
        _module.Table.Get("OnEnable").Function?.Call(_module);
    }

    private void OnDisable()
    {
        _module.Table.Get("OnDisable").Function?.Call();
    }

    public void OnTriggerEnter(Collider other)
    {
        _module.Table.Get("OnTriggerEnter").Function?.Call(_module, UserData.Create( other ) );
    }

    public void OnTriggerExit(Collider other)
    {
        _module.Table.Get("OnTriggerExit").Function?.Call(_module, UserData.Create(other));
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        
    }
}
