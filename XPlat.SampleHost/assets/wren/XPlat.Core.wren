foreign class Transform3d {
    construct new() {}
    foreign setRotationDeg(x,y,z)
    foreign rotateDeg(x,y,z)
    foreign moveUp(amnt)
    foreign translate(x,y,z)
    foreign x
    foreign x=(v)
    foreign y
    foreign y=(v)
    foreign z
    foreign z=(v)
}

foreign class Window {
    foreign static width
    foreign static height
}

foreign class Input {
    foreign static isKeyDown(key)
    foreign static mouseX
    foreign static mouseY
    foreign static isMouseDown(button)
}

foreign class Time {
    foreign static runningTime
}

class MouseButton {
    static LEFT { 1 }
    static MIDDLE { 2 }
    static RIGHT { 3 }
}

class Key {
    static BACKSPACE { 8 }
    static TAB { 9 }
    static RETURN { 13 }
    static ESCAPE { 27 }
    static SPACE { 32 }
    static EXCLAIM { 33 }
    static QUOTEDBL { 34 }
    static HASH { 35 }
    static DOLLAR { 36 }
    static PERCENT { 37 }
    static AMPERSAND { 38 }
    static QUOTE { 39 }
    static LEFTPAREN { 40 }
    static RIGHTPAREN { 41 }
    static ASTERISK { 42 }
    static PLUS { 43 }
    static COMMA { 44 }
    static MINUS { 45 }
    static PERIOD { 46 }
    static SLASH { 47 }
    static NUM0 { 48 }
    static NUM1 { 49 }
    static NUM2 { 50 }
    static NUM3 { 51 }
    static NUM4 { 52 }
    static NUM5 { 53 }
    static NUM6 { 54 }
    static NUM7 { 55 }
    static NUM8 { 56 }
    static NUM9 { 57 }
    static COLON { 58 }
    static SEMICOLON { 59 }
    static LESS { 60 }
    static EQUALS { 61 }
    static GREATER { 62 }
    static QUESTION { 63 }
    static AT { 64 }
    static LEFTBRACKET { 91 }
    static BACKSLASH { 92 }
    static RIGHTBRACKET { 93 }
    static CARET { 94 }
    static UNDERSCORE { 95 }
    static BACKQUOTE { 96 }
    static A { 97 }
    static B { 98 }
    static C { 99 }
    static D { 100 }
    static E { 101 }
    static F { 102 }
    static G { 103 }
    static H { 104 }
    static I { 105 }
    static J { 106 }
    static K { 107 }
    static L { 108 }
    static M { 109 }
    static N { 110 }
    static O { 111 }
    static P { 112 }
    static Q { 113 }
    static R { 114 }
    static S { 115 }
    static T { 116 }
    static U { 117 }
    static V { 118 }
    static W { 119 }
    static X { 120 }
    static Y { 121 }
    static Z { 122 }
    static DELETE { 127 }
    static CAPSLOCK { 1073741881 }
    static F1 { 1073741882 }
    static F2 { 1073741883 }
    static F3 { 1073741884 }
    static F4 { 1073741885 }
    static F5 { 1073741886 }
    static F6 { 1073741887 }
    static F7 { 1073741888 }
    static F8 { 1073741889 }
    static F9 { 1073741890 }
    static F10 { 1073741891 }
    static F11 { 1073741892 }
    static F12 { 1073741893 }
    static PRINTSCREEN { 1073741894 }
    static SCROLLLOCK { 1073741895 }
    static PAUSE { 1073741896 }
    static INSERT { 1073741897 }
    static HOME { 1073741898 }
    static PAGEUP { 1073741899 }
    static END { 1073741901 }
    static PAGEDOWN { 1073741902 }
    static RIGHT { 1073741903 }
    static LEFT { 1073741904 }
    static DOWN { 1073741905 }
    static UP { 1073741906 }
    static NUMLOCKCLEAR { 1073741907 }
    static KP_DIVIDE { 1073741908 }
    static KP_MULTIPLY { 1073741909 }
    static KP_MINUS { 1073741910 }
    static KP_PLUS { 1073741911 }
    static KP_ENTER { 1073741912 }
    static KP_1 { 1073741913 }
    static KP_2 { 1073741914 }
    static KP_3 { 1073741915 }
    static KP_4 { 1073741916 }
    static KP_5 { 1073741917 }
    static KP_6 { 1073741918 }
    static KP_7 { 1073741919 }
    static KP_8 { 1073741920 }
    static KP_9 { 1073741921 }
    static KP_0 { 1073741922 }
    static KP_PERIOD { 1073741923 }
    static APPLICATION { 1073741925 }
    static POWER { 1073741926 }
    static KP_EQUALS { 1073741927 }
    static F13 { 1073741928 }
    static F14 { 1073741929 }
    static F15 { 1073741930 }
    static F16 { 1073741931 }
    static F17 { 1073741932 }
    static F18 { 1073741933 }
    static F19 { 1073741934 }
    static F20 { 1073741935 }
    static F21 { 1073741936 }
    static F22 { 1073741937 }
    static F23 { 1073741938 }
    static F24 { 1073741939 }
    static EXECUTE { 1073741940 }
    static HELP { 1073741941 }
    static MENU { 1073741942 }
    static SELECT { 1073741943 }
    static STOP { 1073741944 }
    static AGAIN { 1073741945 }
    static UNDO { 1073741946 }
    static CUT { 1073741947 }
    static COPY { 1073741948 }
    static PASTE { 1073741949 }
    static FIND { 1073741950 }
    static MUTE { 1073741951 }
    static VOLUMEUP { 1073741952 }
    static VOLUMEDOWN { 1073741953 }
    static KP_COMMA { 1073741957 }
    static KP_EQUALSAS400 { 1073741958 }
    static ALTERASE { 1073741977 }
    static SYSREQ { 1073741978 }
    static CANCEL { 1073741979 }
    static CLEAR { 1073741980 }
    static PRIOR { 1073741981 }
    static RETURN2 { 1073741982 }
    static SEPARATOR { 1073741983 }
    static OUT { 1073741984 }
    static OPER { 1073741985 }
    static CLEARAGAIN { 1073741986 }
    static CRSEL { 1073741987 }
    static EXSEL { 1073741988 }
    static KP_00 { 1073742000 }
    static KP_000 { 1073742001 }
    static THOUSANDSSEPARATOR { 1073742002 }
    static DECIMALSEPARATOR { 1073742003 }
    static CURRENCYUNIT { 1073742004 }
    static CURRENCYSUBUNIT { 1073742005 }
    static KP_LEFTPAREN { 1073742006 }
    static KP_RIGHTPAREN { 1073742007 }
    static KP_LEFTBRACE { 1073742008 }
    static KP_RIGHTBRACE { 1073742009 }
    static KP_TAB { 1073742010 }
    static KP_BACKSPACE { 1073742011 }
    static KP_A { 1073742012 }
    static KP_B { 1073742013 }
    static KP_C { 1073742014 }
    static KP_D { 1073742015 }
    static KP_E { 1073742016 }
    static KP_F { 1073742017 }
    static KP_XOR { 1073742018 }
    static KP_POWER { 1073742019 }
    static KP_PERCENT { 1073742020 }
    static KP_LESS { 1073742021 }
    static KP_GREATER { 1073742022 }
    static KP_AMPERSAND { 1073742023 }
    static KP_DBLAMPERSAND { 1073742024 }
    static KP_VERTICALBAR { 1073742025 }
    static KP_DBLVERTICALBAR { 1073742026 }
    static KP_COLON { 1073742027 }
    static KP_HASH { 1073742028 }
    static KP_SPACE { 1073742029 }
    static KP_AT { 1073742030 }
    static KP_EXCLAM { 1073742031 }
    static KP_MEMSTORE { 1073742032 }
    static KP_MEMRECALL { 1073742033 }
    static KP_MEMCLEAR { 1073742034 }
    static KP_MEMADD { 1073742035 }
    static KP_MEMSUBTRACT { 1073742036 }
    static KP_MEMMULTIPLY { 1073742037 }
    static KP_MEMDIVIDE { 1073742038 }
    static KP_PLUSMINUS { 1073742039 }
    static KP_CLEAR { 1073742040 }
    static KP_CLEARENTRY { 1073742041 }
    static KP_BINARY { 1073742042 }
    static KP_OCTAL { 1073742043 }
    static KP_DECIMAL { 1073742044 }
    static KP_HEXADECIMAL { 1073742045 }
    static LCTRL { 1073742048 }
    static LSHIFT { 1073742049 }
    static LALT { 1073742050 }
    static LGUI { 1073742051 }
    static RCTRL { 1073742052 }
    static RSHIFT { 1073742053 }
    static RALT { 1073742054 }
    static RGUI { 1073742055 }
    static MODE { 1073742081 }
    static AUDIONEXT { 1073742082 }
    static AUDIOPREV { 1073742083 }
    static AUDIOSTOP { 1073742084 }
    static AUDIOPLAY { 1073742085 }
    static AUDIOMUTE { 1073742086 }
    static MEDIASELECT { 1073742087 }
    static WWW { 1073742088 }
    static MAIL { 1073742089 }
    static CALCULATOR { 1073742090 }
    static COMPUTER { 1073742091 }
    static AC_SEARCH { 1073742092 }
    static AC_HOME { 1073742093 }
    static AC_BACK { 1073742094 }
    static AC_FORWARD { 1073742095 }
    static AC_STOP { 1073742096 }
    static AC_REFRESH { 1073742097 }
    static AC_BOOKMARKS { 1073742098 }
    static BRIGHTNESSDOWN { 1073742099 }
    static BRIGHTNESSUP { 1073742100 }
    static DISPLAYSWITCH { 1073742101 }
    static KBDILLUMTOGGLE { 1073742102 }
    static KBDILLUMDOWN { 1073742103 }
    static KBDILLUMUP { 1073742104 }
    static EJECT { 1073742105 }
    static SLEEP { 1073742106 }
    static APP1 { 1073742107 }
    static APP2 { 1073742108 }
    static AUDIOREWIND { 1073742109 }
    static AUDIOFASTFORWARD { 1073742110 }
}

class Vector2 {
    x { _x }
    y { _y }
    x=(v) { _x = v }
    y=(v) { _y = v }
    construct new(x,y){
        _x = x
        _y = y
    }
}