
local script =  {}

function script.Awake()
   print("Awake")
end

function script.Start()
   print("Start")
end

function script:Update()
   print( string.format( "%s %s" , self.var, script.var ) );
end

function script.OnEnable()
   print("OnEnable")
end

function script.OnDisable()
   print("OnDisable")
end

script.var = 32;

return script;