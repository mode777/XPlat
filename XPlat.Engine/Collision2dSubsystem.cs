using System.Numerics;
using TinyC2;
using XPlat.Engine.Components;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class CollisionInfo {
        
        public CollisionInfo(Node other)
        {
            Other = other;
        }

        public Node Other;
        public Vector2 Normal;
        public Vector2 Point;
        public float Distance;
    }

    public class Collision2dSubsystem : IUpdateSubSystem
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

        private SpatialGrid<Collider2dComponent> _world = new(256);
        private HashSet<CollisionHash> _checks = new();
        private int numBuckets;
        private int numChecks;
        private int numRects;
        private int numColls;

        //private c2Manifold _manifold;

        public void AfterUpdate()
        {
            this.numBuckets = 0;
            this.numChecks = 0;
            this.numRects = 0;
            this.numColls = 0;
            foreach (var bucket in _world)
            {
                for (int i = 0; i < bucket.Count-1; i++)
                {
                    var a = bucket[i];
                    for (int j = i+1; j < bucket.Count; j++){
                        var b = bucket[j];
                        Collide(a,b);
                        numChecks++;
                    }
                }
                numBuckets++;
                bucket.Clear();
            }
            //System.Console.WriteLine(GetDebugInfo());
        }

        private string GetDebugInfo() => $"Buckets: {numBuckets}, Checks: {numChecks}, Rect Checks: {numRects}, Coll Checks: {numColls}";

        private void Collide(Collider2dComponent a, Collider2dComponent b){
            // need at least on active participant for collisions to occur
            if(a.Mode != ColliderMode.Active && b.Mode != ColliderMode.Active) return;

            // first check if pair was tested before
            var h = new CollisionHash(a.GetHashCode(), b.GetHashCode());
            if(_checks.Contains(h)) return;

            _checks.Add(h);
            
            numRects++;
            // check if bboxes intersect
            if(!a.BoundingBox.IntersectsWith(b.BoundingBox)) return;

            numColls++;
            // do actual collision detection
            a.UpdateTransformedShape(ref a.Node._globalMatrix);
            b.UpdateTransformedShape(ref b.Node._globalMatrix);
            var man = new c2Manifold();
            c2Collide(a.ShapeTransformed, c2x.Identity, b.ShapeTransformed, c2x.Identity, man);
            if(man.count > 0){
                var active = a.Mode == ColliderMode.Active ? a : b;
                var other = active == a ? b : a;
                var activeInfo = new CollisionInfo(other.Node);
                var otherInfo = new CollisionInfo(active.Node);

                var n = otherInfo.Normal = activeInfo.Normal = man.normal;
                var d = otherInfo.Distance = activeInfo.Distance = man.depths1;
                otherInfo.Point = activeInfo.Point = man.contact_points1;
                var origin = other.ShapeTransformed.Center - active.ShapeTransformed.Center;
                var signActive = Vector2.Dot(n, origin) > 0 ? -1 : 1;
                var signOther = signActive == 1 ? -1 : 1;
                otherInfo.Normal *= signOther;
                activeInfo.Normal *= signActive;

                //movable.p += (sign * n * d);
                // Solving the collision
                switch(other.Mode){
                    case ColliderMode.Ghost:
                        // nothing to solve
                        break;
                    case ColliderMode.Passive:
                        var amnt = activeInfo.Normal * activeInfo.Distance;
                        active.Node.Transform.Translation += new Vector3(amnt, 0);
                        break;
                    case ColliderMode.Active:
                        var totalWeight = active.Weight + other.Weight;
                        float activeWeight, otherWeight;
                        if(totalWeight != 0){
                            activeWeight = active.Weight / totalWeight;
                            otherWeight = other.Weight / totalWeight;
                        } else {
                            activeWeight = 0.5f;
                            otherWeight = 0.5f;
                        }
                        active.Node.Transform.Translation += new Vector3(activeInfo.Normal * activeInfo.Distance * activeWeight, 0);
                        other.Node.Transform.Translation += new Vector3(otherInfo.Normal * otherInfo.Distance * otherWeight, 0);
                        break;
                }
                active.Node.AddCollision(activeInfo);
                other.Node.AddCollision(otherInfo);
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