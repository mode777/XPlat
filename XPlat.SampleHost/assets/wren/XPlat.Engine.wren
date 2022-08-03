foreign class Node {
    foreign Transform
}

class Component {
    node { _node }
    construct new(node) { _node = node }
    update(){}
    init(){}
}