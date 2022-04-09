return function(node)
    
    return {
        update = function()
            local r = 0
            if Input.IsKeyDown(Key.E) then r = 4 end
            if Input.IsKeyDown(Key.Q) then r = -4 end
            node.Transform:RotateDeg(0,r,0)
        end
    }
end
