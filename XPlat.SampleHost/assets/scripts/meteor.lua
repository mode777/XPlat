return function(node, args)
    
    local t = node.Transform

    return {
        velocity = 0,
        direction = Vector2(0,0),
        rot = 0,
        init = function(self)
            node.Tag = "meteor"
            self.collider = node:GetComponent("Collider2dComponent")
        end,
        update = function(self)
            t.Translation = t.Translation + Vector3((self.direction * self.velocity), 0)
            t:RotateDeg(0,0,self.rot)
            self.velocity = math.max(self.velocity - 0.1, 0)
            self.rot = self.rot < 0 and math.min(self.rot + 0.1, 0) or math.max(self.rot - 0.1, 0)
            self.collider.Weight = (self.velocity / 10) * node.Transform.Scale.X
        end,
        onCollision = function(self, info)
            if(info.Other.Tag == "player" or info.Other.Tag == "meteor") then
                self.direction = info.Normal
                self.velocity = 5
                local sign = math.floor(info.Distance) % 2 == 0 and 1 or -1 
                self.rot = 5 * sign
            end
        end
    }
end
