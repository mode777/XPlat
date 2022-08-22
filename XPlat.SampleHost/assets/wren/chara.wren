import "XPlat.Engine" for Component
import "XPlat.Core" for Input, Key, Time, Window, Transform3d, Vector2
import "random" for Random

class Character is Component {
    construct new(node){
        super(node)
    }

    update(){
        if(Input.isKeyDown(Key.RIGHT)){
            transform.rotateDeg(0,0,1)
        }
        if(Input.isKeyDown(Key.LEFT)){
            transform.rotateDeg(0,0,-1)
        }
    }
}