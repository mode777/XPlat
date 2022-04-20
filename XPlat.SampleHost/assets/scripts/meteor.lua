return function(node, args)
    
    local t = node.Transform

    return {
        velocity = 0,
        direction = { X = 0, Y = 0},
        rot = 0,
        init = function(self)
            node.Tag = "meteor"
            self.collider = node:GetComponent("Collider2dComponent")
        end,
        update = function(self)
            t:Translate(self.direction.X * self.velocity, self.direction.Y * self.velocity, 0)
            t:RotateDeg(0,0,self.rot)
            self.velocity = math.max(self.velocity - 0.1, 0)
            self.rot = self.rot < 0 and math.min(self.rot + 0.1, 0) or math.max(self.rot - 0.1, 0)
            local x = node.Transform.ScaleX
            self.collider.Weight = (self.velocity / 10) * x
        end,
        onCollision = function(self, info)
            if(info.Other.Tag == "player" or info.Other.Tag == "meteor") then
                self.direction.X = info.NormalX
                self.direction.Y = info.NormalY
                self.velocity = 5
                local sign = math.floor(info.Distance) % 2 == 0 and 1 or -1 
                self.rot = 5 * sign
            end
        end
    }
end
