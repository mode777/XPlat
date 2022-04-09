return function(node, args)
    
    return {
        __component = "rotate",
        _node = node,
        init = function()
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.E) then r = 4 end
            if Input.IsKeyDown(Key.Q) then r = -4 end
            local x,y,z = (args.axis == 'x' and r or 0), (args.axis == 'y' and r or 0), (args.axis == 'z' and r or 0)
            self:rotate(x,y,z)
        end,
        rotate = function(self, x,y,z)
            self._node.Transform:RotateDeg(x,y,z)
        end
    }
end
