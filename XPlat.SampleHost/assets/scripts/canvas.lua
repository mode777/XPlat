return function(node, args) 

    return {
        init = function(self)
            local c = node:GetComponentByName("canvas")
            c.OnDraw:Add(self.draw)
        end,
        draw = function(obj, vg) 
            vg:FillColor('#aef')
            vg:FontFace('merriweather')
            vg:FontSize(40);
            vg:Text(100, 100, 'A long time ago in a galaxy far away...')
        end
    }
end
