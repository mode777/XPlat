return function(node, args)

    return {
      
        init = function(self)

        end,
        update = function(self)
            --local t = {}
            --for i=0,1000 do
            --    t[i] = Vector3(1,1,1)
            --end
           node.Transform:Translate(1,1,1)
        end,
        onCollision = function(self, info)
          
        end
    }
end
