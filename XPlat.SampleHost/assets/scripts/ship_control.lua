return function(node, args)
    
    return {
        init = function(self)
            self.forward = 0
            self.t = node.Transform
            node.Tag = "player"
            self.t:SetRotationDeg(0,0,0)
            self.t:RotateDeg(0,0,180)
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.D) then r = 4 end
            if Input.IsKeyDown(Key.A) then r = -4 end
            if Input.IsKeyDown(Key.W) then self.forward = math.min(self.forward + 0.4, 10) end
            if Input.IsKeyDown(Key.S) then self.forward = math.max(self.forward - 0.4, 0) end
            self:rotate(r)
            self:moveForward()
            self.forward = math.max(self.forward - 0.1, 0) 
        end,
        rotate = function(self, z)
            self.t:RotateDeg(0,0,z)
        end,
        moveForward = function(self)
            --node.Transform.Translation = node.Transform.Translation + Vector3(1,1,0)
            self.t.Translation = self.t.Translation + (-self.t.Up * self.forward) 
        end,
        onCollision = function(self, info)
            self.forward = 0
        end
    }
end
