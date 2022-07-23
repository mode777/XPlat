return function(node)
    
    return {
        init = function(self)
            node.Transform:RotateDeg(0,135,0)
        end,
        update = function(self)
            --print("test")
            if Input.IsKeyDown(Key.LEFT) then node.Transform:RotateDeg(0,1,0) end
            if Input.IsKeyDown(Key.RIGHT) then node.Transform:RotateDeg(0,-1,0) end
            
        end
    }
end