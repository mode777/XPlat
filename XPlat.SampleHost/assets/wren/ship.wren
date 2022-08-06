import "XPlat.Engine" for Component
import "XPlat.Core" for Input, Key

class ShipControl is Component {
    construct new(node) { 
        super(node)
        _scene = node.scene
        _laser = _scene.templates["laser"]
        _turnSpeed = 4
        _velocity = 0
        _t = node.transform
        _cooldown = 0
    }
    init(){
        _t.setRotationDeg(0,0,0)
        _t.rotateDeg(0,0,180)
    }
    update(){
        var r = 0
        if(Input.isKeyDown(Key.LEFT)){ 
            r = _turnSpeed 
        }
        if(Input.isKeyDown(Key.RIGHT)){ 
            r = -_turnSpeed 
        }
        if(Input.isKeyDown(Key.UP)){ 
            //r = _turnSpeed 
        }
        if(Input.isKeyDown(Key.DOWN)){ 
            //r = -_turnSpeed 
        }
        rotate(r)
    }
    rotate(z){
        _t.rotateDeg(0,0,z)
    }
    moveForward(){
        _t.moveUp(-_velocity)
    }
}

class CameraControl is Component {
    construct new(node) { super(node) }
    init(){
        System.print("Init camera")
    }
}