return function(node, args)

    local scene = node.Scene
    local meteor = scene.Templates["meteor"]

    return {
        init = function(self)
            self.forward = 0
            self.t = node.Transform
            node.Tag = "player"
            self.t:SetRotationDeg(0,0,0)
            self.t:RotateDeg(0,0,180)
            self:spawnRocks()
        end,
        update = function(self)
            local r = 0
            if Input.IsKeyDown(Key.D) then r = 8 end
            if Input.IsKeyDown(Key.A) then r = -8 end
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
            self.t:MoveUp(-self.forward) 
        end,
        onCollision = function(self, info)
            self.forward = 0
        end,
        spawnRocks = function(self)
            for i=1,500 do
                local t = Transform3d()
                t:Translate(math.random(0,1000), math.random(0,1000), 0)
                t:SetRotationDeg(0,0,math.random(360))
                local s = math.random(2,20) / 10
                t:Scale(s)
                scene:Instantiate(meteor, scene.RootNode, t)
            end
        end
    }
end
