foreign class Node {
    foreign tag=(v)
    foreign tag
    foreign transform
    foreign scene
    foreign name
    foreign getWrenComponent(name)
    foreign getWrenComponent()
    foreign getComponentByName(name)
    foreign sendMessage(msg)
    waitForInit(){
        Fiber.yield()
        Fiber.yield()
    }
}

foreign class Scene {
    foreign templates
    foreign rootNode
    foreign instantiate(template, parentNode, transform)
    foreign delete(node)
    foreign findNode(name)
    foreign resources
}

foreign class ResourceManager {
    foreign getValue(id)
}

class Component {
    node { _node }
    transform { _transform }
    scene { _scene }
    construct new(node) { 
        _node = node 
        _fibers = []
        _transform = node.transform
        _scene = node.scene
    }
    update(){}
    updateFibers(){
        var remove = []
        for(fiber in _fibers){
            if(fiber.isDone){
                remove.add(fiber)
            } else {
                fiber.call()
            }
        }
        for (fiber in remove) {
            _fibers.remove(fiber)
        }
    }
    runFiber(fn){
        var fb = Fiber.new(fn)
        _fibers.add(fb)
        return fb
    }
    runLoop(fn){
        runFiber {
            while(true){
                fn.call()
                Fiber.yield()
            }
        }
    }
    waitFrames(frames){
        for (i in 0..frames) {
            Fiber.yield()
        }
    }
    init(){}
    onMessage(obj){}
    initInternal(){
        runFiber {
            this.init()
        }
        runFiber {
            while(true){
                this.update()
                Fiber.yield()
            }
        }
    }
    onMessageInternal(msg){
        runFiber {
            this.onMessage(msg)
        }
    }
    onCollision(info){}
    onCollisionInternal(info){
        runFiber {
            this.onCollision(info)
        }
    }
}



foreign class TemplateCollection {
    foreign [name]
}

foreign class SpriteAtlasResource {
    foreign atlas
}

foreign class CollisionInfo {
    foreign other
    foreign normalX
    foreign normalY
    foreign pointX
    foreign pointY
    foreign distance
}

