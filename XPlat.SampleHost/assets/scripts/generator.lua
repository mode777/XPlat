local rock_sprites = {
    "meteorBrown_big1",
    "meteorBrown_big2",
    "meteorBrown_big3",
    "meteorBrown_big4",
    "meteorBrown_med1",
    "meteorBrown_med3",
    "meteorBrown_small1",
    "meteorBrown_small2",
    "meteorBrown_tiny1",
    "meteorBrown_tiny2",
    "meteorGrey_big1",
    "meteorGrey_big2",
    "meteorGrey_big3",
    "meteorGrey_big4",
    "meteorGrey_med1",
    "meteorGrey_med2",
    "meteorGrey_small1",
    "meteorGrey_small2",
    "meteorGrey_tiny1",
    "meteorGrey_tiny2",
}

return function(node, args) 

    return {
        init = function(self)
            local meteor = node.Scene.Templates["meteor"]
            for i=1,20 do
                local t = Transform3d()
                t:Translate(math.random(0,1000), math.random(0,1000), 0)
                t:SetRotationDeg(0,0,math.random(360))
                local s = math.random(2,20) / 10
                --t:Scale(s)
                local m = node.Scene:Instantiate(meteor, node.Scene.RootNode, t)
                
                local s = m:GetComponentByName("sprite")
                s.Sprite = s.Resource.Atlas[rock_sprites[math.random(#rock_sprites)]]
                s.OriginX = s.Sprite.Rectangle.Width / 2
                s.OriginY = s.Sprite.Rectangle.Height / 2
                
                local collider = m:GetComponentByName("collider")
                local shape = collider.Shape
                shape.r = s.Sprite.Rectangle.Height / 2
                collider.Weight = shape.r / 10
            end
        end
    }
end
