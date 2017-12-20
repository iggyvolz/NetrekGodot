local f = io.open("Packets.cs", "r")
local g=""
local cattr
while true do
	local line=f:read()
	if not line then break end
	local attr = line:match("%[Packet%((.+)%)%]")
	if attr then
		cattr=attr
	elseif line:match("public byte type") then
		g=g.."            public byte type="..cattr..";\n"
	else
		g=g..line
	end
end
print(g)
