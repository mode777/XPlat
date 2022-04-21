return function(node, args)
    
    local t = node.Transform

    return {
        init = function(self)
            self.start = Time.RunningTime
        end,
        update = function(self)
            t:MoveUp(-5)
            if (Time.RunningTime - self.start) > 3 then
                node.Scene:Delete(node)
            end
        end
    }
end
