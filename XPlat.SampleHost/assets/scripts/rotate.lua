return function(node)
    
    return {
        _node = node,
        init = function()
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.E) then r = 4 end
            if Input.IsKeyDown(Key.Q) then r = -4 end
            self:rotate(0,r,0)
        end,
        rotate = function(self, x,y,z)
            self._node.Transform:RotateDeg(x,y,z)
        end
    }
end
