return function(node)
    
    return {
        init = function(self)
            self.targets = {
                node.Scene:FindNode("Suzanne"):GetLuaComponent("rotate"),
                node.Scene:FindNode("Suzanne.001"):GetLuaComponent("rotate"),
                node.Scene:FindNode("Suzanne.002"):GetLuaComponent("rotate"),
            }
        end,
        update = function(self)
            if(Input.IsKeyDown(Key.NUM1)) then
                self.targets[1]:rotate(0,1,0)
            end
            if(Input.IsKeyDown(Key.NUM2)) then
                self.targets[2]:rotate(0,1,0)
            end
            if(Input.IsKeyDown(Key.NUM3)) then
                self.targets[3]:rotate(0,1,0)
            end
            --self.other:rotate(0,1,0)
        end
    }
end
