return function(vg)
    local m = {}

    local tilesize = 64

    function drawGrid()
        vg:StrokeColor("#000")
        vg:StrokeWidth(1)
        for x=0,16 do
            vg:BeginPath()
            vg:MoveTo(x * tilesize, 0)
            vg:LineTo(x * tilesize, tilesize*tilesize)
            vg:Stroke()
        end
        for y=0,16 do
            vg:BeginPath()
            vg:MoveTo(0, y * tilesize)
            vg:LineTo(tilesize*tilesize, y * tilesize)
            vg:Stroke()
        end
    end

    function drawRect(x,y,w,h)
        vg:FillColor("#000")
        vg:BeginPath()
        vg:Rect(x*tilesize,y*tilesize,tilesize*w,tilesize*h)
        vg:Fill()
    end

    function m.update()
       drawGrid()
       drawRect(0,0,1,1)
       drawRect(1,0,0.5,1)
       drawRect(2.5,0,0.5,1)
    end

    return m
end