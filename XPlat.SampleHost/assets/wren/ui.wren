import "XPlat.Core" for Window, Input, Key, Time, MouseButton
import "XPlat.NanoVg" for NVGalign

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
        vg.textAlign(NVGalign.TOP | NVGalign.LEFT)
        vg.textBox(10,10,Window.width-20,_text)
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
        _clicked = 0
        _visble = true
    }

    label=(v) { _label = v }
    label { _label }

    show(){
        _visble = true
    }

    hide(){
        _visble = false
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

    waitForClick(){
        var target = _clicked + 1
        while(_clicked < target){
            Fiber.yield()
        }
    }

    update(){
        if(!_visble){
            _state == "passive"
            return
        }
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
                _clicked = _clicked + 1
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
        if(_visble){
            vg.beginPath()
            vg.rect(_x,_y,_w,_h)
            vg.fillColor(color)
            vg.fill()
            vg.fillColor("#ff0000")
            vg.textAlign(NVGalign.TOP | NVGalign.CENTER)
            vg.fontSize(40)
            vg.textBox(_x+10,_y,_w-20,_label)
        }
    }
}

class ButtonGroup {
    construct new(x,y,w,h){
        _clicked = 0
        _options = []
        for(i in 0...10){
            var b = Button.new(x,y + ((h+10)*i),w,h,"")
            _options.add(b)
            b.onClick {
                _choice = i
                _clicked = _clicked+1
            }
        }
    }

    hideAll(){
        for(b in _options){
            b.hide()
        }  
    }
    label(i,text){
        _options[i].label = text
    }
    show(i){
        _options[i].show()
    }
    update(){
        for(b in _options){
            b.update()
        }  
    }
    draw(vg){
        for(b in _options){
            b.draw(vg)
        } 
    }

    waitForChoice(){
        var target = _clicked + 1
        while(_clicked < target){
            Fiber.yield()
        }
        return _choice
    }


}