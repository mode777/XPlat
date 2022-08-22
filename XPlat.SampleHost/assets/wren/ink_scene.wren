import "XPlat.Engine" for Component
import "XPlat.Core" for Window, Input, Key, Time, MouseButton

class TextBlock {
    construct new(text){
        _text = text
        _opacity = 255
        _color = [255,255,255]
        _face = "merriweather"
        _size = 20
    }

    appendLine(txt){
        _text = _text + (_text == "" ? "" : "\n") + txt
    }

    lerp(a,b,k){
        return ((b - a) * k) + a
    }

    fadeIn(frames){
        for (f in 0..frames) {
            _opacity = lerp(0,255,f/frames)
            Fiber.yield()
        }
    }
    fadeOut(frames){
        for (f in 0..frames) {
            _opacity = 255 - lerp(0,255,f/frames)
            Fiber.yield()
        }
    }

    draw(vg){
        vg.fillColor(_color[0],_color[1],_color[2],_opacity)
        vg.fontFace(_face)
        vg.fontSize(_size)
        vg.textBox(10,30,Window.width-20,_text)
    }
}

class InkDriver is Component {
    construct new(node){
        super(node)
    }
    init(){
        var c = node.getComponentByName("canvas")
        c.registerWrenComponent(this)
        _story = scene.resources.getValue("test2")
        _text = null
    }
    update(){
        while(_story.canContinue){
            var txt = _story.Continue()
            if(txt == ""){
                continue
            }
            _text = TextBlock.new(txt)
            _text.fadeIn(30)
            waitContinue()
            _text.fadeOut(30)
            _text = null
        }
        var choices = _story.currentChoices
        if(choices.count > 0){
            _text = TextBlock.new("")
            for(c in choices){
                _text.appendLine("Choice %(c.index+1): %(c.text)")
            }
            var opt = waitChoice(choices.count)
            _story.chooseChoiceIndex(opt)
            _text = null
        }
        if(!_story.canContinue){
            _text = TextBlock.new("THE END")
        }
    }
    waitContinue(){
        while(true){
            if(Input.isKeyDown(Key.SPACE)){
                while(true){
                    if(!Input.isKeyDown(Key.SPACE)){
                        return
                    }
                    waitFrames(1)
                }
            }
            waitFrames(1)
        }
    }
    waitChoice(options){
        while(true){
            for (k in Key.NUM1..Key.NUM1+options) {
                if(Input.isKeyDown(k)){
                    return k - Key.NUM1
                }
            }
            waitFrames(1)
        }
    }
    draw(vg){
        if(_text){
            _text.draw(vg)
        }
    }
}

class Button {
    construct new(x,y,w,h,label){
        _x = x
        _y = y
        _w = w
        _h = h
        _label = label
        _state = "passive"
    }

    pos(x,y){
        _x = x
        _y = y
    }

    size(w,h){
        
    }

    onClick(fn){
        _handler = fn
    }

    update(){
        if(_state == "passive"){
            if(hasMouseOver){
                _state = "hover"
            }
        } else if(_state == "hover"){
            if(!hasMouseOver){
                _state = "passive"
            } else if(Input.isMouseDown(MouseButton.LEFT)){
                _state = "active"
            }
        } else if(_state == "active"){
            if(!hasMouseOver){
                _state = "passive"
            } else if(!Input.isMouseDown(MouseButton.LEFT)){
                _state = "hover"
                if(_handler){
                    _handler.call()
                }
            }
        }
    }

    hasMouseOver {
        var mx = Input.mouseX
        var my = Input.mouseY
        return mx > _x && mx <= _x+_w && my > _y && my < _y+_h
    }

    color {
        if(_state == "passive"){
            return "#ffffff88"
        } else if(_state == "hover"){
            return "#ffffffaa"
        } else if(_state == "active"){
            return "#ffffffff"
        }
    }

    draw(vg){
        vg.beginPath()
        vg.rect(_x,_y,_w,_h)
        vg.fillColor(color)
        vg.fill()
    }
}

class UI is Component {
    construct new(mode){
        super(node)
        _choices = []
        _choiceCallback = null
        _text = []
    }

    onChoice(fn){
        _choiceCallback = fn
    }

    dispatchChoice(num){
        if(_choiceCallback != null){
            _choiceCallback.call(num)
        }
    }

    addChoice(){

    }
}


class Interface is Component {
    construct new(node){
        super(node)
    }
    init(){
        var c = node.getComponentByName("canvas")
        c.registerWrenComponent(this)
        _button = Button.new(100,100,200,50,"Test")
    }
    update(){
        _button.update()
        //System.print("%(Input.mouseX),%(Input.mouseY),%(Input.isMouseDown(MouseButton.LEFT))")
    }
    draw(vg){
        _button.draw(vg)
    }
}