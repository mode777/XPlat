using TinyC2;
using XPlat.Engine.Components;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class CollisionInfo {
        
        public CollisionInfo(Node other, c2Manifold manifold)
        {
            Other = other;
            Manifold = manifold;
        }

        public Node Other;
        public c2Manifold Manifold;
    }

    public class CollisionSubsystem2d : IUpdateSubSystem
    {
        private struct CollisionHash {
            private int A;
            private int B;

            public CollisionHash(int hashA, int hashB){
                if(hashA < hashB){
                    this.A = hashA;
                    this.B = hashB; 
                } else {
                    this.B = hashA;
                    this.A = hashB;
                }
            }
        }

        private SpatialGrid<Collider2dComponent> _world;
        private HashSet<CollisionHash> _checks; 
        //private c2Manifold _manifold;

        public CollisionSubsystem2d()
        {
            _world = new SpatialGrid<Collider2dComponent>(256);
        }
        public void AfterUpdate()
        {
            foreach (var bucket in _world)
            {
                for (int i = 0; i < bucket.Count-1; i++)
                {
                    var a = bucket[i];
                    for (int j = i+1; j < bucket.Count; j++){
                        var b = bucket[j];
                        Collide(a,b);
                    }
                }
            }
        }

        private void Collide(Collider2dComponent a, Collider2dComponent b){
            // need at least on active participant for collisions to occur
            if(a.Mode != ColliderMode.Active && b.Mode != ColliderMode.Active) return;

            // first check if pair was tested before
            var h = new CollisionHash(a.GetHashCode(), b.GetHashCode());
            if(_checks.Contains(h)) return;
            
            // check if bboxes intersect
            if(!a.BoundingBox.IntersectsWith(b.BoundingBox)) return;

            // do actual collision detection
            var actualA = a.Shape.GetTransformed(ref a.Node._globalMatrix);
            var actualB = b.Shape.GetTransformed(ref b.Node._globalMatrix);
            var man = new c2Manifold();
            c2Collide(actualA, c2x.Identity, actualB, c2x.Identity, man);
            if(man.count > 0){
                a.Node.AddCollision(new CollisionInfo(b.Node, man));
                b.Node.AddCollision(new CollisionInfo(a.Node, man));
            }
        }

        public void BeforeUpdate()
        {
            _checks.Clear();
        }

        public void OnUpdate(Node n)
        {
            n.ResetCollisions();
            var c = n.GetComponent<Collider2dComponent>();
            if(c != null){
                c.UpdateBoundingBox(ref n._globalMatrix);
                _world.Insert(c, c.BoundingBox);
            }
        }
    }
}