return function(node, args)
    
    local t = node.Transform

    return {
        speed = -5,
        init = function(self)
            node.Tag = "laser"
            self.start = Time.RunningTime
        end,
        update = function(self)
            t:MoveUp(self.speed)
            if (Time.RunningTime - self.start) > 3 then
                node.Scene:Delete(node)
            end
        end
    }
end
