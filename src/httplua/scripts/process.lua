-- System.Diagnostics
-- Process

luanet.load_assembly("System");

Process = luanet.import_type("System.Diagnostics.Process")
ProcessStartInfo = luanet.import_type("System.Diagnostics.ProcessStartInfo")