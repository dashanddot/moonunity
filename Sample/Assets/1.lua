
local script =  {}

function script.Awake()
   print("Awake")
end

function script.Start()
   print("Start");

   print( "1" );
   coroutine.yield(0.5);
   print( "2" );
   coroutine.yield(0.5);
   print( "3" );
   coroutine.yield(1);
end

function script:Update()
   --print( string.format( "%s %s" , self.var, script.var ) );

end

function script.OnEnable()
   print("OnEnable")
end

function script.OnDisable()
   print("OnDisable")
end

script.var = 32;

return script;