foreign class Node {
    foreign transform
    foreign scene
    foreign name
}

foreign class Scene {
    foreign templates
}

class Component {
    node { _node }
    construct new(node) { _node = node }
    update(){}
    init(){}
}

foreign class TemplateCollection {
    foreign [name]
}

