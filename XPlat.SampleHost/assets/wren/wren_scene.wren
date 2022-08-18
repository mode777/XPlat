import "XPlat.Engine" for Component
import "XPlat.Core" for Input, Key, Time, Window, Transform3d, Vector2
import "random" for Random

class Ship is Component {

    velocity { _velocity }

    construct new(node) { 
        super(node)
        _scene = node.scene
        _laser = _scene.templates["laser"]
        _turnSpeed = 4
        _velocity = 0
        _t = node.transform
        node.tag = "player"
    }
    init(){
        _t.setRotationDeg(0,0,0)
        _t.rotateDeg(0,0,180)
        runLoop{ shootLaser() }
    }
    update(){
        var r = 0
        if(Input.isKeyDown(Key.LEFT)){ 
            r = -_turnSpeed 
        }
        if(Input.isKeyDown(Key.RIGHT)){ 
            r = _turnSpeed 
        }
        if(Input.isKeyDown(Key.UP)){ 
            _velocity = (_velocity+0.4).min(10)
        }
        if(Input.isKeyDown(Key.DOWN)){ 
            _velocity = (_velocity-0.4).max(0)
        }
        rotate(r)
        moveForward()
        _velocity = (_velocity-0.1).max(0)
    }
    rotate(z){
        _t.rotateDeg(0,0,z)
    }
    moveForward(){
        _t.moveUp(-_velocity)
    }
    shootLaser(){
        if(Input.isKeyDown(Key.SPACE)){
            var l = _scene.instantiate(_laser, _scene.rootNode, _t)
            l.transform.moveUp(-100)
            l.sendMessage({ "velocity": _velocity })
            waitFrames(9)
        }
    }
}

class Camera is Component {
    construct new(node) { 
        super(node) 
    }
    init(){
        node.tag = "camera"
        _player = scene.findNode("player")
    }
    update(){
        transform.x = _player.transform.x - (Window.width / 2) 
        transform.y = _player.transform.y - (Window.height / 2) 
    }
}

class Laser is Component {
    construct new(node) { 
        super(node)
        _t = node.transform 
    }

    init(){
        _velocity = 20
        _weight = 0.05
        node.tag = "laser"
        _start = Time.runningTime
    }
    update(){
        _t.moveUp(-_velocity)
        var delta = Time.runningTime - _start
        if(delta > 1){
            scene.delete(node)
        }
    }
    onMessage(msg){
        _speed = _velocity + msg["velocity"]
    }
}

class Meteor is Component {

    velocity { _velocity }

    construct new(node){ 
        super(node) 
        _velocity = 0
        _direction = Vector2.new(0,0)
        _rot = 0
        _weight = 0
    }  
    init(){
        node.tag = "meteor"
        _weight = node.getComponentByName("collider").weight / 5
    } 
    update(){
        transform.translate(_direction.x * _velocity, _direction.y * _velocity, 0)
        transform.rotateDeg(0,0,_rot)
        _velocity = (_velocity-0.1).max(0)
        _rot = _rot < 0 ? (_rot+0.1).min(0) : (_rot-0.1).max(0)
    }
    onCollision(info){
        //System.print("%(info.other.tag) vs %(node.tag)")
        if(!_wait){
            _wait = true
            if(info.other.tag == "player" || info.other.tag == "meteor" || info.other.tag == "laser"){
                var l = info.other.getWrenComponent()
                
                _direction.x = info.normalX
                _direction.y = info.normalY

                _velocity = info.other.tag == "laser" ? 2 : l.velocity
                var sign = info.distance.floor % 2 == 0 ? 1 : -1
                _rot = _velocity * sign

                waitFrames(6)
            }
            _wait = false
        }
    }
}

var RockSprites = [
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
]

class Generator is Component {

    construct new(node){
        super(node)
        _r = Random.new()
    }
    init(){
        var meteor = scene.templates["meteor"]
        for(i in (0...20)){
            var t = Transform3d.new()
            t.translate(_r.int(1000),_r.int(1000),0)
            t.setRotationDeg(0,0,_r.int(360))
            var m = scene.instantiate(meteor, scene.rootNode, t)
            
            var s = m.getComponentByName("sprite")
            var sprite = RockSprites[_r.int(RockSprites.count)]
            s.setSprite(sprite)
            s.center()

            var collider = m.getComponentByName("collider")
            collider.radius = s.height/2
            collider.weight = collider.radius / 10
        }
    }
}

class Canvas is Component {
    construct new(node){
        super(node)
    }

    init(){
        var c = node.getComponentByName("canvas")
        c.registerWrenComponent(this)
    }
    draw(vg){
        vg.fillColor("#aef")
        vg.fontFace("merriweather")
        vg.fontSize(40)
        vg.text(100,100, "A long time ago in a galaxy far away...")
    }
}