foreign class SpriteComponent {
    foreign originX
    foreign originX=(v)
    foreign originY
    foreign originY=(v)
    foreign width
    foreign height
    foreign center()
    foreign setSprite(name)
}

foreign class Collider2dComponent {
    foreign radius=(v)
    foreign radius
    foreign weight=(v)
    foreign weight
}

foreign class CanvasComponent {
    foreign registerWrenComponent(obj)
}

