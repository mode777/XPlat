import "XPlat.Engine" for Component

class ShipControl is Component {
    construct new(node) { 
        super(node)
        _transform = node.Transform
    }
    init(){
        System.print("Init component")
        System.print(node.Transform == _transform)
    }
    // update(){
    //     System.print("Update component")
    // }
}

class CameraControl is Component {
    construct new(node) { super(node) }
    init(){
        System.print("Init component")
    }
    update(){
        System.print("Update component")
    }
}