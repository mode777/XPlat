return function(node, args)
    
    local t = node.Transform

    return {
        init = function(self)
            self.player = node.Scene:FindNode("player")
        end,
        update = function(self)
            t.Translation = self.player.Transform.Translation - Vector3(320,240,0)
        end
    }
end
