-- some debugging functions

_debug = {}

function _debug:PrintListOfDict(obj)
	local it = obj:GetEnumerator()
	while it:MoveNext() do
		print('<pre>')
		local it2 = it.Current:GetEnumerator();
		while it2:MoveNext() do
			print(string.format('[%s] => %s', it2.Current.Key, it2.Current.Value))
		end
		print('</pre>')
		print() -- newline
	end
end

function _debug:PrintList(obj)
	local it = obj:GetEnumerator()
	local i = 0
	while it:MoveNext() do
		print(string.format('[%s] => %s', i, it.Current))
		i = i+1
	end
end

function _debug:PrintDict(obj)
	-- print(obj:GetType().Name)
	local it = obj:GetEnumerator()
	print('<pre>')
	while it:MoveNext() do
		print(string.format('[%s] => %s', it.Current.Key, it.Current.Value))
	end
	print('</pre>')
end