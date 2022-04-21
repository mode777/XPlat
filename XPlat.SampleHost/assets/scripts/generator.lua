return function(node, args)
    
    return {
        init = function(self)
                local meteor = node.Scene.Templates["meteor"]
                for i=1,50 do
                local t = Transform3d()
                t:Translate(math.random(0,1000), math.random(0,1000), 0)
                t:SetRotationDeg(0,0,math.random(360))
                local s = math.random(2,20) / 10
                t:Scale(s)
                node.Scene:Instantiate(meteor, node.Scene.RootNode, t)
            end
        end
    }
end
