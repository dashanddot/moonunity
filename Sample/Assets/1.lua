
local script =  {}

function script:Awake()
   print("Awake")
end

function script:Start()
   print( string.format("Start %s", self.var ) );

    wait(5);

   print( "1" );
   wait(0.5);
   print( "2" );
   wait(0.5);
   print( "3" );
   wait(1);

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