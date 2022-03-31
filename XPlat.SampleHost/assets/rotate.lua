return function(node){
    
    local v = nil
    
    return {
        init = function()
            v = node.Transform.RotationDeg
        end,
        update = function()
            v.Y = Time.RunningTime * 50
            node.Transform:RotateDeg(v)
        end
    }
}
