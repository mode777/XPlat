return function(node, args)
    
    local t = node.Transform
    local buffer, cam, atlas, player

    return {
        init = function(self)
            cam = node.Scene:FindNode("camera")
            player = node.Scene:FindNode("player")


            local comp = node:GetComponentByName("buffer")
            buffer = comp.Buffer
            atlas = comp.Atlas
            local r = math.random
            local sprites = {
                atlas["star1"],
                atlas["star2"],
                atlas["star3"]
            }

            for i=0,buffer.Size-1 do
                buffer:Set(i,sprites[r(#sprites-1)+1], r(Window.Width), r(Window.Height), r() * math.pi * 2, 0.1+r()*0.5, 0.1+r()*0.5, sprites[1].Width/2, sprites[1].Height/2)
                buffer:SetColor(i, 255,255,255, 127+r(128))
                
            end
            -- buffer:Add(atlas["star1"], 10, 10, r() * math.pi * 2)
            -- buffer:Set(0)
        end,
        update = function(self)
            local pxmin, pymin = player.Transform.X-Window.Width/2, player.Transform.Y-Window.Height/2
            local pxmax, pymax = player.Transform.X+Window.Width/2, player.Transform.Y+Window.Height/2
            for i=0,buffer.Size-1 do
                local x,y = buffer:GetX(i), buffer:GetY(i)
                if x > pxmax then buffer:Move(i, -Window.Width, 0) end
                if x < pxmin then buffer:Move(i, Window.Width, 0) end
                if y > pymax then buffer:Move(i, 0, -Window.Height) end
                if y < pymin then buffer:Move(i, 0, Window.Height) end
                --self.buffer:Rotate(i,0.01)
            end
        end
    }
end
