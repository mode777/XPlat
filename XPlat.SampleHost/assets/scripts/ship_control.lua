return function(node, args)

    local scene = node.Scene
    local laser = scene.Templates["laser"]

    return {
        init = function(self)
            self.forward = 0
            self.t = node.Transform
            node.Tag = "player"
            self.t:SetRotationDeg(0,0,0)
            self.t:RotateDeg(0,0,180)
            self.cooldown = 0
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.D) then r = 8 end
            if Input.IsKeyDown(Key.A) then r = -8 end
            if Input.IsKeyDown(Key.W) then self.forward = math.min(self.forward + 0.4, 10) end
            if Input.IsKeyDown(Key.S) then self.forward = math.max(self.forward - 0.4, 0) end
            if Input.IsKeyDown(Key.SPACE) then self:shootLaser() end 
            self:rotate(r)
            self:moveForward()
            self.forward = math.max(self.forward - 0.1, 0) 
            self.cooldown = math.max(0, self.cooldown-1)
        end,
        rotate = function(self, z)
            self.t:RotateDeg(0,0,z)
        end,
        moveForward = function(self)
            self.t:MoveUp(-self.forward) 
        end,
        onCollision = function(self, info)
            if info.Other.Tag == "meteor" then
                self.forward = 0
            end
        end,
        shootLaser = function(self)
            if self.cooldown == 0 then
                local l = scene:Instantiate(laser, scene.RootNode, node.Transform)
                l.Transform:MoveUp(-100)
                self.cooldown = 10
            end
        end
    }
end
