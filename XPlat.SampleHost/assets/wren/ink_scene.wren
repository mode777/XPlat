import "XPlat.Engine" for Component
import "XPlat.Core" for Window, Input, Key, Time, MouseButton
import "ui" for TextBlock, Button, ButtonGroup

class MessageBroker {
    construct new(node){
        _node = node
        _handlers = {}
        _wait = {}
    }

    on(name, fn){
        _handlers[name] = fn
    }

    waitFor(name){
        _wait[name] = false
        while(_wait[name] == false){
            Fiber.yield()
        }
        var ret = _wait[name]
        _wait[name] = false
        return ret.count == 1 ? ret[0] : ret
    }

    send(name, args){
        _node.sendMessage([name]+args)
    }

    send(name){
        _node.sendMessage([name])
    }

    onMessage(msg){
        var handler = _handlers[msg[0]]
        if(handler) {
            handler.call(msg[1..-1])   
        }
        if(_wait[msg[0]] == false){
            _wait[msg[0]] = msg[1..-1]
        }
    }
}

class InkDriver is Component {
    
    construct new(node){
        super(node)
        _broker = MessageBroker.new(node)
    }
    init(){
        _story = scene.resources.getValue("test2")
    }
    update(){
        while(_story.canContinue){
            var txt = _story.Continue()
            if(txt == ""){
                continue
            }
            _broker.send("story",[txt])
            _broker.waitFor("continue")
        }
        var choices = _story.currentChoices
        _broker.send("choices", choices)
        var selection = _broker.waitFor("select")
        _story.chooseChoiceIndex(selection)
        if(!_story.canContinue){
            _broker.send("story", ["THE END"])
            while(true){
                waitFrames(1)
            }
        }
    }
    onMessage(msg){
        _broker.onMessage(msg)
    }
}

class Interface is Component {
    construct new(node){
        super(node)
        _broker = MessageBroker.new(node)
    }
    init(){
        var c = node.getComponentByName("canvas")
        c.registerWrenComponent(this)
        _button = Button.new(10,200,Window.width-20,40,"Continue")
        _button.hide()
        _text = TextBlock.new("")
        _options = ButtonGroup.new(10,10,Window.width-20,40)
        _options.hideAll()
        _broker.on("story"){|args| paragraph(args[0]) }
        _broker.on("choices"){|args| choice(args) }
    }
    update(){
        _button.update()
        _options.update()
        //System.print("%(Input.mouseX),%(Input.mouseY),%(Input.isMouseDown(MouseButton.LEFT))")
    }
    paragraph(p){
        _text = TextBlock.new(p)
        _text.fadeIn(30)
        _button.show()
        _button.waitForClick()
        _button.hide()
        _text.fadeOut(30)
        _broker.send("continue")
    }
    choice(choices){
        for(c in choices){
            _options.label(c.index, c.text)
            _options.show(c.index)
        }
        var choice = _options.waitForChoice()
        _options.hideAll()
        _broker.send("select", [choice])
    }
    draw(vg){
        _options.draw(vg)
        _button.draw(vg)  
        
        if(_text){
            _text.draw(vg)
        }
    }
    onMessage(msg){
        _broker.onMessage(msg)
    }
}

