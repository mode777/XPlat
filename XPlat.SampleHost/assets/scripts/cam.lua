return function(node, args)
    
    local t = node.Transform

    return {
        init = function(self)
            self.player = node.Scene:FindNode("player")
        end,
        update = function(self)
            if(Input.IsKeyDown(Key.RIGHT)) then t.Translation = t.Translation + Vector3(3,0,0) end
            if(Input.IsKeyDown(Key.LEFT)) then t.Translation = t.Translation - Vector3(3,0,0) end
            t.Translation = self.player.Transform.Translation - Vector3(320,240,0)
        end
    }
end
