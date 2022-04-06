return function(node)
    local v = nil
    
    return {
        init = function()
            v = node.Transform.RotationDeg
        end,
        update = function()
            --v.Y = Time.RunningTime * 200
            if Input.IsKeyDown(Key.E) then v.Y = v.Y + 4 end
            if Input.IsKeyDown(Key.Q) then v.Y = v.Y - 2 end
            node.Transform:RotateDeg(v)
        end
    }
end
