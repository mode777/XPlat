return function(node)

    local target
    
    return {
        __component = "mycomponent"
        init = function()
            target = node.Scene:FindNode("Suzanne") 
            print(target)
        end,
        update = function()
        end
    }
end
