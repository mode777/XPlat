return function(node, args)
    
    local t = node.Transform
    local buffer, cam, atlas, player
    local sw, sh = 0,0

    return {
        init = function(self)
            cam = node.Scene:FindNode("camera")
            player = node.Scene:FindNode("player")
            local comp = node:GetComponentByName("buffer")
            buffer = comp.Buffer
            atlas = comp.Atlas
            self:checkScreen()
            self:resetStarfield()
        end,
        checkScreen = function(self)
            if sw ~= Window.Width or sh ~= Window.Height then
                sw = Window.Width
                sh = Window.Height
                return true
            end
            return false
        end,
        resetStarfield = function(self)
            local r = math.random
            local sprites = {
                atlas["star1"],
                atlas["star2"],
                atlas["star3"]
            }
            for i=0,buffer.Size-1 do
                buffer:Set(i,sprites[r(#sprites-1)+1], r(sw), r(sh), r() * math.pi * 2, 0.1+r()*0.5, 0.1+r()*0.5, sprites[1].Width/2, sprites[1].Height/2)
                buffer:SetColor(i, 255,255,255, 127+r(128))
            end
        end,
        update = function(self)
            if self:checkScreen() then self:resetStarfield() end 
            local pxmin, pymin = player.Transform.X-sw/2, player.Transform.Y-sh/2
            local pxmax, pymax = player.Transform.X+sw/2, player.Transform.Y+sh/2
            for i=0,buffer.Size-1 do
                local x,y = buffer:GetX(i), buffer:GetY(i)
                if x > pxmax then buffer:Move(i, -Window.Width, 0) end
                if x < pxmin then buffer:Move(i, Window.Width, 0) end
                if y > pymax then buffer:Move(i, 0, -Window.Height) end
                if y < pymin then buffer:Move(i, 0, Window.Height) end
            end
        end
    }
end
