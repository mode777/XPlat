return function(node, args)
    
    local t = node.Transform

    return {
        init = function(self)
            self.player = node.Scene:FindNode("player")
        end,
        update = function(self)
            if(Input.IsKeyDown(Key.RIGHT)) then t:Translate(3,0,0) end
            if(Input.IsKeyDown(Key.LEFT)) then t:Translate(-3,0,0) end
            node.Transform.X = self.player.Transform.X - (Window.Width / 2)
            node.Transform.Y = self.player.Transform.Y - (Window.Height / 2)
        end
    }
end
