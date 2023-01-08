
local script =  {}

function script:Awake()
   print("Awake")
end

function script:Start()
   print( string.format("Start %s", self.var ) );

   print( "1" );
   coroutine.yield(0.5);
   print( "2" );
   coroutine.yield(0.5);
   print( "3" );
   coroutine.yield(1);

   gameObject:SetActive(false);
end

function script:Update()
   --print( string.format( "%s %s" , self.var, script.var ) );

end

function script:OnEnable()
   print("OnEnable")
end

function script:OnDisable()
   print("OnDisable")
end

script.var = 32;
script.ovar = nil;

return script;