return function(node, args)
    
    return {
        init = function(self)
            self.forward = 0
            self.t = node.Transform
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.D) then r = 4 end
            if Input.IsKeyDown(Key.A) then r = -4 end
            if Input.IsKeyDown(Key.W) then self.forward = math.min(self.forward + 0.4, 10) end
            if Input.IsKeyDown(Key.S) then self.forward = math.max(self.forward - 0.4, 0) end
            local x,y,z = (args.axis == 'x' and r or 0), (args.axis == 'y' and r or 0), (args.axis == 'z' and r or 0)
            self:rotate(x,y,z)
            self:moveForward()
            self.forward = math.max(self.forward - 0.1, 0) 
        end,
        rotate = function(self, x,y,z)
            self.t:RotateDeg(x,y,z)
        end,
        moveForward = function(self)
            --node.Transform.Translation = node.Transform.Translation + Vector3(1,1,0)
            self.t.Translation = self.t.Translation + (-self.t.Up * self.forward) 
        end 
    }
end
