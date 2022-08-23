foreign class NVGcontext {
    foreign fillColor(color)
    foreign fillColor(r,g,b,a)
    foreign fontFace(name)
    foreign fontSize(size)
    foreign text(x,y,text)
    foreign textBox(x,y,width,text)
    foreign beginPath()
    foreign rect(x,y,w,h)
    foreign roundedRect(x,y,w,h,r)
    foreign fill()
    foreign textAlign(align)
}

class NVGalign {
    // Horizontal align
    static LEFT { 1 << 0 }
    // Default, align text horizontally to left.
    static CENTER { 1 << 1 }
    // Align text horizontally to center.
    static RIGHT { 1 << 2 }
    // Align text horizontally to right.
    // Vertical align
    static TOP { 1 << 3 }
    // Align text vertically to top.
    static MIDDLE { 1 << 4 }
    // Align text vertically to middle.
    static BOTTOM { 1 << 5 }
    // Align text vertically to bottom.
    static BASELINE { 1 << 6 }
    // Default, align text vertically to baseline.
}