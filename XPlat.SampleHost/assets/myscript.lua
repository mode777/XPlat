return function(vg)
    local m = {}

    local x = 0
    local y = 0

    function m.update()
        y = y+1
        
        vg:BeginPath()
        vg:Circle(300,300,200)
        vg:FillColor("#0f0")
        vg:Fill()
        
        vg:BeginPath()
        vg:RoundedRect(400+x,400+y,200,200,10+y)
        vg:Fill()
    end

    return m
end