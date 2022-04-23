return function(node, args)

    local scene = node.Scene
    local laser = scene.Templates["laser"]

    return {
        turnSpeed = 4,
        velocity = 0,
        init = function(self)
            self.t = node.Transform
            node.Tag = "player"
            self.t:SetRotationDeg(0,0,0)
            self.t:RotateDeg(0,0,180)
            self.cooldown = 0
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.RIGHT) then r = self.turnSpeed end
            if Input.IsKeyDown(Key.LEFT) then r = -self.turnSpeed end
            if Input.IsKeyDown(Key.UP) then self.velocity = math.min(self.velocity + 0.4, 10) end
            if Input.IsKeyDown(Key.DOWN) then self.velocity = math.max(self.velocity - 0.4, 0) end
            if Input.IsKeyDown(Key.SPACE) then self:shootLaser() end 
            self:rotate(r)
            self:moveForward()
            self.velocity = math.max(self.velocity - 0.1, 0) 
            self.cooldown = math.max(0, self.cooldown-1)
        end,
        rotate = function(self, z)
            self.t:RotateDeg(0,0,z)
        end,
        moveForward = function(self)
            self.t:MoveUp(-self.velocity) 
        end,
        onCollision = function(self, info)
            if info.Other.Tag == "meteor" then
                --self.velocity = 0
            end
        end,
        shootLaser = function(self)
            if self.cooldown == 0 then
                local l = scene:Instantiate(laser, scene.RootNode, node.Transform)
                l.Transform:MoveUp(-100)
                local tab = l:GetLuaComponent("script")
                tab.speed = tab.velocity + self.velocity
                self.cooldown = 10
            end
        end
    }
end
