return function(node, args)
    
    local t = node.Transform

    return {
        velocity = 0,
        direction = { X = 0, Y = 0},
        rot = 0,
        wait = 0,
        weight = 0,
        init = function(self)
            node.Tag = "meteor"
            self.weight = node:GetComponent("Collider2dComponent").Weight / 5
        end,
        update = function(self)
            self.wait = math.max(self.wait-1,0)
            --print(node.Tag)
            t:Translate(self.direction.X * self.velocity, self.direction.Y * self.velocity, 0)
            t:RotateDeg(0,0,self.rot)
            self.velocity = math.max(self.velocity - 0.1, 0)
            self.rot = self.rot < 0 and math.min(self.rot + 0.1, 0) or math.max(self.rot - 0.1, 0)
        end,
        onCollision = function(self, info)
            if self.wait > 0 then return end
            if(info.Other.Tag == "player" or info.Other.Tag == "meteor" or info.Other.Tag == "laser") then
                local l = info.Other:GetLuaComponent()

                self.direction.X = info.NormalX
                self.direction.Y = info.NormalY
                
                self.velocity = info.Other.Tag == "laser" and 2 or l.velocity 
                self.wait = 6
                --l.velocity = 0
                local sign = math.floor(info.Distance) % 2 == 0 and 1 or -1 
                self.rot = self.velocity * sign
            end
        end
    }
end
