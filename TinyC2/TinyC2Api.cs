using System;
using System.Drawing;
using System.Numerics;

namespace TinyC2
{
    public static class TinyC2Api
    {
        /*
	tinyc2.h - v1.01

	SUMMARY:
	tinyc2 is a single-file header that implements 2D collision detection routines
	that test for overlap, and optionally can find the collision manifold. The
	manifold contains all necessary information to prevent shapes from inter-
	penetrating, which is useful for character controllers, general physics
	simulation, and user-interface programming.

	This header implements a group of "immediate mode" functions that should be
	very easily adapted into pre-existing projects.

	Revision history:
		1.0  (02/13/2017) initial release
		1.01 (02/13/2017) crusade, minor optimizations, capsule degen
*/

        /*
			Contributors:
				Plastburk         1.01 - pointers pull request
		*/

        /*
			To create implementation (the function definitions)
				#define TINYC2_IMPL
			in *one* C/CPP file (translation unit) that includes this file
		*/

        /*
			THE IMPORTANT PARTS:
			Most of the math types in this header are for internal use. Users care about
			the shape types and the collision functions.

			SHAPE TYPES:
			* c2Circle
			* c2Capsule
			* c2AABB
			* c2Ray
			* c2Poly

			COLLISION FUNCTIONS (*** is a shape name from the above list):
			* c2***to***         - boolean YES/NO hittest
			* c2***to***Manifold - construct manifold to describe how shapes hit
			* c2GJK              - runs GJK algorithm to find closest point pair
								   between two shapes
			* c2MakePoly         - Runs convex hull algorithm and computes normals on input point-set
			* c2Collided         - generic version of c2***to*** funcs
			* c2Collide          - generic version of c2***to***Manifold funcs

			The rest of the header is more or less for internal use. Here is an example of
			making some shapes and testing for collision:

				c2Circle c;
				c.p = position;
				c.r = radius;

				c2Capsule cap;
				cap.a = first_endpoint;
				cap.b = second_endpoint;
				cap.r = radius;

				int hit = c2CircletoCapsule( c, cap );
				if ( hit )
				{
					handle collision here...
				}

			For more code examples and tests please see:
			https://github.com/RandyGaul/tinyheaders/tree/master/examples_tinygl_and_tinyc2

			Here is a past discussion thread on this header:
			https://www.reddit.com/r/gamedev/comments/5tqyey/tinyc2_2d_collision_detection_library_in_c/
		*/

        /*
			DETAILS/ADVICE:
			This header does not implement a broad-phase, and instead concerns itself with
			the narrow-phase. This means this header just checks to see if two individual
			shapes are touching, and can give information about how they are touching.

			Very common 2D broad-phases are tree and grid approaches. Quad trees are good
			for static geometry that does not move much if at all. Dynamic AABB trees are
			good for general purpose use, and can handle moving objects very well. Grids
			are great and are similar to quad trees.

			If implementing a grid it can be wise to have each collideable grid cell hold
			an integer. This integer refers to a 2D shape that can be passed into the
			various functions in this header. The shape can be transformed from "model"
			space to "world" space using c2x -- a transform struct. In this way a grid
			can be implemented that holds any kind of convex shape (that this header
			supports) while conserving memory with shape instancing.

			Please email at my address with any questions or comments at:
			author's last name followed by 1748 at gmail
		*/

        /*
			Features:
			* Circles, capsules, AABBs, rays and convex polygons are supported
			* Fast boolean only result functions (hit yes/no)
			* Slghtly slower manifold generation for collision normals + depths +points
			* GJK implementation (finds closest points for disjoint pairs of shapes)
			* Robust 2D convex hull generator
			* Lots of correctly implemented and tested 2D math routines
			* Implemented in portable C, and is readily portable to other languages
			* Generic c2Collide and c2Collided function (can pass in any shape type)
			* Extensive examples at: https://github.com/RandyGaul/tinyheaders/tree/master/examples_tinygl_and_tinyc2
		*/


        // this can be adjusted as necessary, but is highly recommended to be kept at 8.
        // higher numbers will incur quite a bit of memory overhead, and convex shapes
        // over 8 verts start to just look like spheres, which can be implicitly rep-
        // resented as a point + radius. usually tools that generate polygons should be
        // constructed so they do not output polygons with too many verts.
        // Note: polygons in tinyc2 are all *convex*.
        const int C2_MAX_POLYGON_VERTS = 8;

        // 2d vector
        //public struct Vector2
        //{
        //    public float X, Y;
        //}

        // 2d rotation composed of cos/sin pair
        public struct c2r
        {
            public float c, s;
        }

        // 2d rotation matrix
        public struct c2m
        {
            public Vector2 x, y;
        };

        // 2d transformation "x"
        // These are used especially for c2Poly when a c2Poly is passed to a function.
        // Since polygons are prime for "instancing" a c2x transform can be used to
        // transform a polygon from local space to world space. In functions that take
        // a c2x pointer (like c2PolytoPoly), these pointers can be NULL, which represents
        // an identity transformation and assumes the verts inside of c2Poly are already
        // in world space.
        public struct c2x
        {
            public static readonly c2x Identity = new c2x();
            public c2x()
            {
                p = new Vector2(0, 0);
                r = c2RotIdentity();
            }

            public Vector2 p;
            public c2r r;
        }

        // 2d halfspace (aka plane, aka line)
        public struct c2h
        {
            public Vector2 n;   // normal, normalized
            public float d; // distance to origin from plane, or ax + by = d
        }

        public abstract class c2Shape {
            public abstract Vector2 Center { get; }

            public abstract c2AABB GetBBox(ref Matrix4x4 mat);
            public c2Shape GetTransformed(ref Matrix4x4 mat){
                var clone = (c2Shape)this.MemberwiseClone();
                clone.Transform(ref mat);
                return clone;
            }
            public abstract void Transform(ref Matrix4x4 mat);
            public abstract void Transform(ref Matrix3x2 mat);

            public c2Shape Clone() => this.MemberwiseClone() as c2Shape;
        }

        public class c2Circle : c2Shape
        {
            public Vector2 p;
            public float r;

            public override Vector2 Center => p;

            public override c2AABB GetBBox(ref Matrix4x4 m)
            {
                var np = Vector2.Transform(p, m);
                var r  = this.r * new Vector3(m.M11, m.M12, m.M13).Length();
                //var nr = Vector2.Transform(new Vector2(r,r), mat).Length();

                return new c2AABB(new Vector2(np.X - r, np.Y - r), new Vector2(np.X + r, np.Y + r));
            }

            public override void Transform(ref Matrix4x4 m) {
                p = Vector2.Transform(p, m);
                r *= new Vector3(m.M11, m.M12, m.M13).Length();
                //r = Vector2.Transform(new Vector2(0,r), mat).Length();
            } 

            public override void Transform(ref Matrix3x2 mat) {
                p = Vector2.Transform(p, mat);
                //r = Vector2.Transform(new Vector2(0,r), mat).Length();
            }
        }

        public class c2AABB : c2Shape
        {
            public Vector2 min, max;

            public c2AABB(Vector2 min, Vector2 max)
            {
                this.min = min;
                this.max = max;
            }

            public c2AABB()
            {
            }

            public override Vector2 Center => min + (max - min) / 2;

            public override void Transform(ref Matrix4x4 mat)
            {
                min = Vector2.Transform(min, mat);
                max = Vector2.Transform(max, mat);
            }

            public override void Transform(ref Matrix3x2 mat)
            {
                min = Vector2.Transform(min, mat);
                max = Vector2.Transform(max, mat);
            }

            public override c2AABB GetBBox(ref Matrix4x4 mat)
            {
                var r = new c2AABB(min, max);

                r.Transform(ref mat);
                return r;
            }

            public bool IntersectsWith(c2AABB rect) => (rect.min.X < max.X) && (min.X < rect.max.X) &&
                                                        (rect.min.Y < max.Y) && (min.Y < rect.max.Y);
        }

        // a capsule is defined as a line segment (from a to b) and radius r
        public class c2Capsule : c2Shape
        {
            public Vector2 a, b;
            public float r;

            public override Vector2 Center => a + (b - a) / 2;

            public override c2AABB GetBBox(ref Matrix4x4 mat)
            {
                var a = Vector2.Transform(this.a, mat);
                var b = Vector2.Transform(this.b, mat);
                var nr = r * new Vector3(mat.M11, mat.M12, mat.M13).Length();
                var min = new Vector2(MathF.Min(a.X - nr, b.X - nr), MathF.Min(a.Y - nr, b.Y - nr));
                var max = new Vector2(MathF.Max(a.X + nr, b.X + nr), MathF.Max(a.Y + nr, b.Y + nr));
                return new c2AABB(min, max);
            }

            public override void Transform(ref Matrix4x4 mat)
            {
                a = Vector2.Transform(this.a, mat);
                b = Vector2.Transform(this.b, mat);

                //r = Vector2.Transform(new Vector2(0,r), mat).Length();
            }

            public override void Transform(ref Matrix3x2 mat)
            {
                a = Vector2.Transform(this.a, mat);
                b = Vector2.Transform(this.b, mat);
                //r = Vector2.Transform(new Vector2(0,r), mat).Length();
            }
        }

        public class c2Poly : c2Shape
        {
            public int count = 0;
            public Vector2[] verts = new Vector2[C2_MAX_POLYGON_VERTS];
            public Vector2[] norms = new Vector2[C2_MAX_POLYGON_VERTS];

            public override Vector2 Center => throw new NotImplementedException();

            public override c2AABB GetBBox(ref Matrix4x4 mat)
            {
                throw new NotImplementedException();
            }

            public override void Transform(ref Matrix4x4 mat)
            {
                throw new NotImplementedException();
            }

            public override void Transform(ref Matrix3x2 mat)
            {
                throw new NotImplementedException();
            }
        }

        public struct c2Ray
        {
            public Vector2 p, d;   // position, direction (normalized)
            public float t; // distance along d from position p to find endpoint of ray
        }

        public struct c2Raycast
        {
            public float t; // time of impact
            public Vector2 n;   // normal of surface at impact (unit length)
        }

        // position of impact p = ray.p + ray.d * raycast.t
        public static Vector2 c2Impact(c2Ray ray, float t) => Vector2.Add(ray.p, Vector2.Multiply(ray.d, t));

        // contains all information necessary to resolve a collision, or in other words
        // this is the information needed to separate shapes that are colliding. Doing
        // the resolution step is *not* included in tinyc2. tinyc2 does not include
        // "feature information" that describes which topological features collided.
        // However, modifying the exist ***Manifold funcs can be done to output any
        // needed feature information. Feature info is sometimes needed for certain kinds
        // of simulations that cache information over multiple game-ticks, of which are
        // associated to the collision of specific features. An example implementation
        // is in the qu3e 3D physics engine library: https://github.com/RandyGaul/qu3e
        public class c2Manifold
        {
            public int count;
            public float depths1;
            public float depths2;
            public Vector2 contact_points1;
            public Vector2 contact_points2;
            public Vector2 normal;
        }

        // boolean collision detection
        // these versions are faster than the manifold versions, but only give a YES/NO
        // result
        //int c2CircletoCircle(c2Circle A, c2Circle B);
        //int c2CircletoAABB(c2Circle A, c2AABB B);
        //int c2CircletoCapsule(c2Circle A, c2Capsule B);
        //int c2AABBtoAABB(c2AABB A, c2AABB B);
        //int c2AABBtoCapsule(c2AABB A, c2Capsule B);
        //int c2CapsuletoCapsule(c2Capsule A, c2Capsule B);
        //int c2CircletoPoly(c2Circle A, c2Poly* B, c2x* bx);
        //int c2AABBtoPoly(c2AABB A, c2Poly* B, c2x* bx);
        //int c2CapsuletoPoly(c2Capsule A, c2Poly* B, c2x* bx);
        //int c2PolytoPoly( c2Poly* A, c2x* ax, c2Poly* B, c2x* bx);

        // ray operations
        // output is placed into the c2Raycast struct, which represents the hit location
        // of the ray. the out param contains no meaningful information if these funcs
        // return false
        //int c2RaytoCircle(c2Ray A, c2Circle B, c2Raycast* out );
        //int c2RaytoAABB(c2Ray A, c2AABB B, c2Raycast* out );
        //int c2RaytoCapsule(c2Ray A, c2Capsule B, c2Raycast* out );
        //int c2RaytoPoly(c2Ray A, c2Poly* B, c2x* bx_ptr, c2Raycast* out );

        // manifold generation
        // these functions are slower than the boolean versions, but will compute one
        // or two points that represent the plane of contact. This information is
        // is usually needed to resolve and prevent shapes from colliding. If no coll
        // ision occured the count member of the manifold struct is set to 0.
        //void c2CircletoCircleManifold(c2Circle A, c2Circle B, c2Manifold* m);
        //void c2CircletoAABBManifold(c2Circle A, c2AABB B, c2Manifold* m);
        //void c2CircletoCapsuleManifold(c2Circle A, c2Capsule B, c2Manifold* m);
        //void c2AABBtoAABBManifold(c2AABB A, c2AABB B, c2Manifold* m);
        //void c2AABBtoCapsuleManifold(c2AABB A, c2Capsule B, c2Manifold* m);
        //void c2CapsuletoCapsuleManifold(c2Capsule A, c2Capsule B, c2Manifold* m);
        //void c2CircletoPolyManifold(c2Circle A, c2Poly* B, c2x* bx, c2Manifold* m);
        //void c2AABBtoPolyManifold(c2AABB A, c2Poly* B, c2x* bx, c2Manifold* m);
        //void c2CapsuletoPolyManifold(c2Capsule A, c2Poly* B, c2x* bx, c2Manifold* m);
        //void c2PolytoPolyManifold( c2Poly* A, c2x* ax, c2Poly* B, c2x* bx, c2Manifold* m);

        public enum C2_TYPE
        {
            C2_CIRCLE,
            C2_AABB,
            C2_CAPSULE,
            C2_POLY
        }

        // Runs the GJK algorithm to find closest points, returns distance between closest points.
        // outA and outB can be NULL, in this case only distance is returned. ax_ptr and bx_ptr
        // can be NULL, and represent local to world transformations for shapes A and B respectively.
        // use_radius will apply radii for capsules and circles (if set to false, spheres are
        // treated as points and capsules are treated as line segments i.e. rays).
        //float c2GJK( object A, C2_TYPE typeA, c2x* ax_ptr, object B, C2_TYPE typeB, c2x* bx_ptr, new Vector2* outA, new Vector2* outB, int use_radius);

        // Computes 2D convex hull. Will not do anything if less than two verts supplied. If
        // more than C2_MAX_POLYGON_VERTS are supplied extras are ignored.
        //int c2Hull(new Vector2* verts, int count);
        //void c2Norms(new Vector2* verts, new Vector2* norms, int count);

        // runs c2Hull and c2Norms, assumes p.verts and p.count are both set to valid values
        //void c2MakePoly(c2Poly* p);

        // Generic collision detection routines, useful for games that want to use some poly-
        // morphism to write more generic-styled code. Internally calls various above functions.
        // For AABBs/Circles/Capsules ax and bx are ignored. For polys ax and bx can define
        // model to world transformations, or be NULL for identity transforms.
        //int c2Collided( object A, c2x* ax, C2_TYPE typeA, object B, c2x* bx, C2_TYPE typeB);
        //void c2Collide( object A, c2x* ax, C2_TYPE typeA, object B, c2x* bx, C2_TYPE typeB, c2Manifold* m);

        // adjust these primitives as seen fit
        public static float c2Sin(float radians) => MathF.Sin(radians);
        public static float c2Cos(float radians) => MathF.Cos(radians);
        public static float c2Sqrt(float a) => MathF.Sqrt(a);
        public static float c2Min(float a, float b) => ((a) < (b) ? (a) : (b));
        public static float c2Max(float a, float b) => ((a) > (b) ? (a) : (b));
        public static float c2Abs(float a) => ((a) < 0 ? -(a) : (a));
        public static float c2Clamp(float a, float lo, float hi) => c2Max(lo, c2Min(a, hi));
        public static void c2SinCos(float radians, ref float s, ref float c) { c = c2Cos(radians); s = c2Sin(radians); }
        public static float c2Sign(float a) => (a < 0 ? -1.0f : 1.0f);

        // The rest of the functions in the header-only portion are all for internal use
        // and use the author's personal naming conventions. It is recommended to use one's
        // own math library instead of the one embedded here in tinyc2, but for those
        // curious or interested in trying it out here's the details:

        // The Mul functions are used to perform multiplication. x stands for transform,
        // v stands for vector, s stands for scalar, r stands for rotation, h stands for
        // halfspace and T stands for transpose.For example c2MulxvT stands for "multiply
        // a transform with a vector, and transpose the transform".

        // vector ops
        static Vector2 c2V(float x, float y) { Vector2 a; a.X = x; a.Y = y; return a; }
        static Vector2 c2Add(Vector2 a, Vector2 b) { a.X += b.X; a.Y += b.Y; return a; }
        static Vector2 c2Sub(Vector2 a, Vector2 b) { a.X -= b.X; a.Y -= b.Y; return a; }
        static float c2Dot(Vector2 a, Vector2 b) { return a.X * b.X + a.Y * b.Y; }
        static Vector2 c2Mulvs(Vector2 a, float b) { a.X *= b; a.Y *= b; return a; }
        static Vector2 c2Mulvv(Vector2 a, Vector2 b) { a.X *= b.X; a.Y *= b.Y; return a; }
        static Vector2 c2Div(Vector2 a, float b) { return Vector2.Multiply(a, 1.0f / b); }
        static Vector2 c2Skew(Vector2 a) { Vector2 b; b.X = -a.Y; b.Y = a.X; return b; }
        static Vector2 c2CCW90(Vector2 a) { Vector2 b; b.X = a.Y; b.Y = -a.X; return b; }
        static float c2Det2(Vector2 a, Vector2 b) { return a.X * b.Y - a.Y * b.X; }
        static Vector2 c2Minv(Vector2 a, Vector2 b) { return new Vector2(c2Min(a.X, b.X), c2Min(a.Y, b.Y)); }
        static Vector2 c2Maxv(Vector2 a, Vector2 b) { return new Vector2(c2Max(a.X, b.X), c2Max(a.Y, b.Y)); }
        static Vector2 c2Clampv(Vector2 a, Vector2 lo, Vector2 hi) { return c2Maxv(lo, c2Minv(a, hi)); }
        static Vector2 c2Absv(Vector2 a) { return new Vector2(c2Abs(a.X), c2Abs(a.Y)); }
        static float c2Hmin(Vector2 a) { return c2Min(a.X, a.Y); }
        static float c2Hmax(Vector2 a) { return c2Max(a.X, a.Y); }
        static float c2Len(Vector2 a) { return MathF.Sqrt(Vector2.Dot(a, a)); }
        static Vector2 c2Norm(Vector2 a) { return c2Div(a, c2Len(a)); }
        static Vector2 c2Neg(Vector2 a) { return new Vector2(-a.X, -a.Y); }
        static Vector2 c2Lerp(Vector2 a, Vector2 b, float t) { return Vector2.Add(a, Vector2.Multiply(b -  a, t)); }
        static bool c2Parallel(Vector2 a, Vector2 b, float kTol)
        {
            float k = c2Len(a) / c2Len(b);
            b = Vector2.Multiply(b, k);
            if (c2Abs(a.X - b.X) < kTol && c2Abs(a.Y - b.Y) < kTol) return true;
            return false;
        }

        // rotation ops
        static c2r c2Rot(float radians) { c2r r = new c2r(); c2SinCos(radians, ref r.s, ref r.c); return r; }
        static c2r c2RotIdentity() { c2r r; r.c = 1.0f; r.s = 0; return r; }
        static Vector2 c2RotX(c2r r) { return new Vector2(r.c, r.s); }
        static Vector2 c2RotY(c2r r) { return new Vector2(-r.s, r.c); }
        static Vector2 c2Mulrv(c2r a, Vector2 b) { return new Vector2(a.c * b.X - a.s * b.Y, a.s * b.X + a.c * b.Y); }
        static Vector2 c2MulrvT(c2r a, Vector2 b) { return new Vector2(a.c * b.X + a.s * b.Y, -a.s * b.X + a.c * b.Y); }
        static c2r c2Mulrr(c2r a, c2r b) { c2r c; c.c = a.c * b.c - a.s * b.s; c.s = a.s * b.c + a.c * b.s; return c; }
        static c2r c2MulrrT(c2r a, c2r b) { c2r c; c.c = a.c * b.c + a.s * b.s; c.s = a.c * b.s - a.s * b.c; return c; }

        static Vector2 c2Mulmv(c2m a, Vector2 b) { Vector2 c; c.X = a.x.X * b.X + a.y.X * b.Y; c.Y = a.x.Y * b.X + a.y.Y * b.Y; return c; }
        static Vector2 c2MulmvT(c2m a, Vector2 b) { Vector2 c; c.X = a.x.X * b.X + a.x.Y * b.Y; c.Y = a.y.X * b.X + a.y.Y * b.Y; return c; }
        static c2m c2Mulmm(c2m a, c2m b) { c2m c; c.x = c2Mulmv(a, b.x); c.y = c2Mulmv(a, b.y); return c; }
        static c2m c2MulmmT(c2m a, c2m b) { c2m c; c.x = c2MulmvT(a, b.x); c.y = c2MulmvT(a, b.y); return c; }

        // transform ops
        static c2x c2xIdentity() { c2x x; x.p = new Vector2(0, 0); x.r = c2RotIdentity(); return x; }
        static Vector2 c2Mulxv(c2x a, Vector2 b) { return c2Mulrv(a.r, b) + a.p; }
        static Vector2 c2MulxvT(c2x a, Vector2 b) { return c2MulrvT(a.r, b - a.p); }
        static c2x c2Mulxx(c2x a, c2x b) { c2x c; c.r = c2Mulrr(a.r, b.r); c.p = c2Mulrv(a.r, b.p) + a.p; return c; }
        static c2x c2MulxxT(c2x a, c2x b) { c2x c; c.r = c2MulrrT(a.r, b.r); c.p = c2MulrvT(a.r, b.p - a.p); return c; }
        static c2x c2Transform(Vector2 p, float radians) { c2x x; x.r = c2Rot(radians); x.p = p; return x; }

        // halfspace ops
        static Vector2 c2Origin(c2h h) { return Vector2.Multiply(h.n, h.d); }
        static float c2Dist(c2h h, Vector2 p) { return Vector2.Dot(h.n, p) - h.d; }
        static Vector2 c2Project(c2h h, Vector2 p) { return Vector2.Subtract(p, Vector2.Multiply(h.n, c2Dist(h, p))); }
        static c2h c2Mulxh(c2x a, c2h b) { c2h c; c.n = c2Mulrv(a.r, b.n); c.d = Vector2.Dot(c2Mulxv(a, c2Origin(b)), c.n); return c; }
        static c2h c2MulxhT(c2x a, c2h b) { c2h c; c.n = c2MulrvT(a.r, b.n); c.d = Vector2.Dot(c2MulxvT(a, c2Origin(b)), c.n); return c; }
        static Vector2 c2Intersect(Vector2 a, Vector2 b, float da, float db) { return Vector2.Add(a, Vector2.Multiply(b -  a, (da / (da - db)))); }

        static void c2BBVerts(Vector2[] @out, c2AABB bb)
        {
            @out[0] = bb.min;
            @out[1] = new Vector2(bb.max.X, bb.min.Y);
            @out[2] = bb.max;
            @out[3] = new Vector2(bb.min.X, bb.max.Y);
        }

        public static bool c2Collided(c2Shape A, c2x ax, c2Shape B, c2x bx)
        {
            switch (A)
            {
                case c2Circle ca:
                    switch (B)
                    {
                        case c2Circle cb: return c2CircletoCircle(ca, cb);
                        case c2AABB ab: return c2CircletoAABB(ca, ab);
                        case c2Capsule cab: return c2CircletoCapsule(ca, cab);
                        case c2Poly pb: return c2CircletoPoly(ca, pb, ref bx);
                        default: return false;
                    }
                    break;
                case c2AABB aa:
                    switch (B)
                    {
                        case c2Circle cb: return c2CircletoAABB(cb, aa);
                        case c2AABB ab: return c2AABBtoAABB(aa, ab);
                        case c2Capsule cab: return c2AABBtoCapsule(aa, cab);
                        case c2Poly pb: return c2AABBtoPoly(aa, pb, ref bx);
                        default: return false;
                    }
                    break;
                case c2Capsule caa:
                    switch (B)
                    {
                        case c2Circle cb: return c2CircletoCapsule(cb, caa);
                        case c2AABB ab: return c2AABBtoCapsule(ab, caa);
                        case c2Capsule cab: return c2CapsuletoCapsule(caa, cab);
                        case c2Poly pb: return c2CapsuletoPoly(caa, pb, ref bx);
                        default: return false;
                    }
                    break;
                case c2Poly pa:
                    switch (B)
                    {
                        case c2Circle cb: return c2CircletoPoly(cb, pa, ref ax);
                        case c2AABB ab: return c2AABBtoPoly(ab, pa, ref ax);
                        case c2Capsule cab: return c2CapsuletoPoly(cab, pa, ref ax);
                        case c2Poly pb: return c2PolytoPoly(pa, ref ax, pb, ref bx);
                        default: return false;
                    }
                    break;
                default: return false;
            }
        }

        public static void c2Collide(c2Shape A, c2x ax, c2Shape B, c2x bx, c2Manifold m)
        {
            m.count = 0;

            switch (A)
            {
                case c2Circle cA:
                    switch (B)
                    {
                        case c2Circle cB:
                            c2CircletoCircleManifold(cA, cB, m);
                            break;
                        case c2AABB aB:
                            c2CircletoAABBManifold(cA, aB, m);
                            break;
                        case c2Capsule caB:
                            c2CircletoCapsuleManifold(cA, caB, m);
                            break;
                        case c2Poly pB:
                            c2CircletoPolyManifold(cA, pB, ref bx, m);
                            break;
                    }
                    break;

                case c2AABB aA:
                    switch (B)
                    {
                        case c2Circle cB:
                            c2CircletoAABBManifold(cB, aA, m);
                            break;
                        case c2AABB aB:
                            c2AABBtoAABBManifold(aA, aB, m);
                            break;
                        case c2Capsule caB:
                            c2AABBtoCapsuleManifold(aA, caB, m);
                            break;
                        case c2Poly pB:
                            c2AABBtoPolyManifold(aA, pB, ref bx, m);
                            break;
                    }
                    break;

                case c2Capsule caA:
                    switch (B)
                    {
                        case c2Circle cB:
                            c2CircletoCapsuleManifold(cB, caA, m);
                            break;
                        case c2AABB aB:
                            c2AABBtoCapsuleManifold(aB, caA, m);
                            break;
                        case c2Capsule caB:
                            c2CapsuletoCapsuleManifold(caA, caB, m);
                            break;
                        case c2Poly pB:
                            c2CapsuletoPolyManifold(caA, pB, ref bx, m);
                            break;
                    }
                    break;

                case c2Poly pA:
                    switch (B)
                    {
                        case c2Circle cB:
                            c2CircletoPolyManifold(cB, pA, ref ax, m);
                            break;
                        case c2AABB aB:
                            c2AABBtoPolyManifold(aB, pA, ref ax, m);
                            break;
                        case c2Capsule caB:
                            c2CapsuletoPolyManifold(caB, pA, ref ax, m);
                            break;
                        case c2Poly pB:
                            c2PolytoPolyManifold(pA, ref ax, pB, ref bx, m);
                            break;
                    }
                    break;
            }
        }

        const int C2_GJK_ITERS = 20;

        public class c2Proxy
        {
            public float radius;
            public int count;
            public Vector2[] verts = new Vector2[C2_MAX_POLYGON_VERTS];
        }

        public struct c2sv
        {
            public Vector2 sA;
            public Vector2 sB;
            public Vector2 p;
            public float u;
            public int iA;
            public int iB;
        }

        public class c2Simplex
        {
            public c2sv[] verts = new c2sv[4];
            public float div;
            public int count;
        }

        static void c2MakeProxy(object shape, C2_TYPE type, c2Proxy p)
        {
            switch (type)
            {
                case C2_TYPE.C2_CIRCLE:
                    {
                        c2Circle c = (c2Circle)shape;
                        p.radius = c.r;
                        p.count = 1;
                        p.verts[0] = c.p;
                    }
                    break;

                case C2_TYPE.C2_AABB:
                    {
                        c2AABB bb = (c2AABB)shape;
                        p.radius = 0;
                        p.count = 4;
                        c2BBVerts(p.verts, bb);
                    }
                    break;

                case C2_TYPE.C2_CAPSULE:
                    {
                        c2Capsule c = (c2Capsule)shape;
                        p.radius = c.r;
                        p.count = 2;
                        p.verts[0] = c.a;
                        p.verts[1] = c.b;
                    }
                    break;

                case C2_TYPE.C2_POLY:
                    {
                        c2Poly poly = (c2Poly)shape;
                        p.radius = 0;
                        p.count = poly.count;
                        for (int i = 0; i < p.count; ++i) p.verts[i] = poly.verts[i];
                    }
                    break;
            }
        }

        static int c2Support(Vector2[] verts, int count, Vector2 d)
        {
            int imax = 0;
            float dmax = Vector2.Dot(verts[0], d);

            for (int i = 1; i < count; ++i)
            {
                float dot = Vector2.Dot(verts[i], d);
                if (dot > dmax)
                {
                    imax = i;
                    dmax = dot;
                }
            }

            return imax;
        }

        //#define Vector2.Multiply( s. n. x , (den * s. n.u) ) Vector2.Multiply( s.n.x, (den * s.n.u) )
        //#define  Vector2.Multiply( s. a.  x   +  (den * s. a.u) , Vector2.Multiply( s. b.  x  , (den * s. b.u) ) )  Vector2.Multiply( s. a. x  +  (den * s. a.u) , Vector2.Multiply( s. b. x , (den * s. b.u) ) )
        //#define  Vector2.Add( Vector2.Multiply( s. a.  x   +  (den * s. a.u) , Vector2.Multiply( s. b.  x  , (den * s. b.u) ) ), Vector2.Multiply( s. c.  x  , (den * s. c.u) ) )  Vector2.Add( Vector2.Multiply( s. a. x  +  (den * s. a.u) , Vector2.Multiply( s. b. x , (den * s. b.u) ) ), Vector2.Multiply( s. c. x , (den * s. c.u) ) )

        static Vector2 c2L(c2Simplex s)
        {
            float den = 1.0f / s.div;
            switch (s.count)
            {
                case 1: return s.verts[0].p;
                case 2: return Vector2.Multiply(s.verts[0].p, (den * s.verts[0].u)) + Vector2.Multiply(s.verts[1].p, (den * s.verts[1].u));
                case 3: return (Vector2.Multiply(s.verts[0].p, (den * s.verts[0].u)) + Vector2.Multiply(s.verts[1].p, (den * s.verts[1].u))) + 
                                Vector2.Multiply(s.verts[2].p, (den * s.verts[2].u));
                default: return new Vector2(0, 0);
            }
        }

        static void c2Witness(c2Simplex s, ref Vector2 a, ref Vector2 b)
        {
            float den = 1.0f / s.div;
            switch (s.count)
            {
                case 1: a = s.verts[0].sA; b = s.verts[0].sB; break;
                case 2: a = Vector2.Multiply(s.verts[0].sA, (den * s.verts[0].u)) + Vector2.Multiply(s.verts[1].sA, (den * s.verts[1].u)); 
                        b = Vector2.Multiply(s.verts[0].sB, (den * s.verts[0].u)) + Vector2.Multiply(s.verts[1].sB, (den * s.verts[1].u)); 
                        break;
                case 3: a = (Vector2.Multiply(s.verts[0].sA, (den * s.verts[0].u))+Vector2.Multiply(s.verts[1].sA, (den * s.verts[1].u))) + Vector2.Multiply(s.verts[2].sA, (den * s.verts[2].u)); 
                        b = (Vector2.Multiply(s.verts[0].sB, (den * s.verts[0].u)) + Vector2.Multiply(s.verts[1].sB, (den * s.verts[1].u))) + Vector2.Multiply(s.verts[2].sB, (den * s.verts[2].u)); 
                        break;
                default: a = new Vector2(0, 0); b = new Vector2(0, 0); break;
            }
        }

        static Vector2 c2D(c2Simplex s)
        {
            switch (s.count)
            {
                case 1: return c2Neg(s.verts[0].p);
                case 2:
                    {
                        Vector2 ab = s.verts[1].p -  s.verts[0].p;
                        if (c2Det2(ab, c2Neg(s.verts[0].p)) > 0) return c2Skew(ab);
                        return c2CCW90(ab);
                    }
                case 3:
                default: return new Vector2(0, 0);
            }
        }

        static void c22(c2Simplex s)
        {
            Vector2 a = s.verts[0].p;
            Vector2 b = s.verts[1].p;
            float u = Vector2.Dot(b, c2Norm(b -  a));
            float v = Vector2.Dot(a, c2Norm(a -  b));

            if (v <= 0)
            {
                s.verts[0].u = 1.0f;
                s.div = 1.0f;
                s.count = 1;
            }

            else if (u <= 0)
            {
                s.verts[0] = s.verts[1];
                s.verts[0].u = 1.0f;
                s.div = 1.0f;
                s.count = 1;
            }

            else
            {
                s.verts[0].u = u;
                s.verts[1].u = v;
                s.div = u + v;
                s.count = 2;
            }
        }

        static void c23(c2Simplex s)
        {
            Vector2 a = s.verts[0].p;
            Vector2 b = s.verts[1].p;
            Vector2 c = s.verts[2].p;

            float uAB = Vector2.Dot(b, c2Norm(b -  a));
            float vAB = Vector2.Dot(a, c2Norm(a -  b));
            float uBC = Vector2.Dot(c, c2Norm(c -  b));
            float vBC = Vector2.Dot(b, c2Norm(b -  c));
            float uCA = Vector2.Dot(a, c2Norm(a -  c));
            float vCA = Vector2.Dot(c, c2Norm(c -  a));
            float area = c2Det2(c2Norm(b -  a), c2Norm(c -  a));
            float uABC = c2Det2(b, c) * area;
            float vABC = c2Det2(c, a) * area;
            float wABC = c2Det2(a, b) * area;

            if (vAB <= 0 && uCA <= 0)
            {
                s.verts[0].u = 1.0f;
                s.div = 1.0f;
                s.count = 1;
            }

            else if (uAB <= 0 && vBC <= 0)
            {
                s.verts[0] = s.verts[1];
                s.verts[0].u = 1.0f;
                s.div = 1.0f;
                s.count = 1;
            }

            else if (uBC <= 0 && vCA <= 0)
            {
                s.verts[0] = s.verts[2];
                s.verts[0].u = 1.0f;
                s.div = 1.0f;
                s.count = 1;
            }

            else if (uAB > 0 && vAB > 0 && wABC <= 0)
            {
                s.verts[0].u = uAB;
                s.verts[1].u = vAB;
                s.div = uAB + vAB;
                s.count = 2;
            }

            else if (uBC > 0 && vBC > 0 && uABC <= 0)
            {
                s.verts[0] = s.verts[1];
                s.verts[1] = s.verts[2];
                s.verts[0].u = uBC;
                s.verts[1].u = vBC;
                s.div = uBC + vBC;
                s.count = 2;
            }

            else if (uCA > 0 && vCA > 0 && vABC <= 0)
            {
                s.verts[1] = s.verts[0];
                s.verts[0] = s.verts[2];
                s.verts[0].u = uCA;
                s.verts[1].u = vCA;
                s.div = uCA + vCA;
                s.count = 2;
            }

            else
            {
                s.verts[0].u = uABC;
                s.verts[1].u = vABC;
                s.verts[2].u = wABC;
                s.div = uABC + vABC + wABC;
                s.count = 3;
            }
        }

        // Please see http://box2d.org/downloads/ under GDC 2010 for Erin's demo code
        // and PDF slides for documentation on the GJK algorithm.
        static float c2GJK(object A, C2_TYPE typeA, ref c2x ax_ptr, object B, C2_TYPE typeB, ref c2x bx_ptr, ref Vector2 outA, ref Vector2 outB, bool use_radius)
        {
            c2x ax;
            c2x bx;
            if (typeA != C2_TYPE.C2_POLY/* || !ax_ptr*/) ax = c2xIdentity();
            else ax = ax_ptr;
            if (typeB != C2_TYPE.C2_POLY/* || !bx_ptr*/) bx = c2xIdentity();
            else bx = bx_ptr;

            c2Proxy pA = new c2Proxy();
            c2Proxy pB = new c2Proxy();
            c2MakeProxy(A, typeA, pA);
            c2MakeProxy(B, typeB, pB);

            c2Simplex s = new c2Simplex();
            s.verts[0].iA = 0;
            s.verts[0].iB = 0;
            s.verts[0].sA = c2Mulxv(ax, pA.verts[0]);
            s.verts[0].sB = c2Mulxv(bx, pB.verts[0]);
            s.verts[0].p = s.verts[0].sB -  s.verts[0].sA;
            s.verts[0].u = 1.0f;
            s.count = 1;

            c2sv[] verts = s.verts;
            int[] saveA = new int[3], saveB = new int[3];
            int save_count = 0;
            float d0 = float.MaxValue;
            float d1 = float.MaxValue;
            int iter = 0;
            bool hit = false;
            while (iter < C2_GJK_ITERS)
            {
                save_count = s.count;
                for (int i = 0; i < save_count; ++i)
                {
                    saveA[i] = verts[i].iA;
                    saveB[i] = verts[i].iB;
                }

                switch (s.count)
                {
                    case 1: break;
                    case 2: c22(s); break;
                    case 3: c23(s); break;
                }

                if (s.count == 3)
                {
                    hit = true;
                    break;
                }

                Vector2 p = c2L(s);
                d1 = Vector2.Dot(p, p);

                if (d1 > d0) break;
                d0 = d1;

                Vector2 d = c2D(s);
                if (Vector2.Dot(d, d) < float.Epsilon * float.Epsilon) break;

                int iA = c2Support(pA.verts, pA.count, c2MulrvT(ax.r, c2Neg(d)));
                Vector2 sA = c2Mulxv(ax, pA.verts[iA]);
                int iB = c2Support(pB.verts, pB.count, c2MulrvT(bx.r, d));
                Vector2 sB = c2Mulxv(bx, pB.verts[iB]);

                ++iter;

                bool dup = false;
                for (int i = 0; i < save_count; ++i)
                {
                    if (iA == saveA[i] && iB == saveB[i])
                    {
                        dup = true;
                        break;
                    }
                }
                if (dup) break;

                verts[s.count].iA = iA;
                verts[s.count].sA = sA;
                verts[s.count].iB = iB;
                verts[s.count].sB = sB;
                verts[s.count].p = verts[s.count].sB -  verts[s.count].sA;
                ++s.count;
            }

            Vector2 a = default(Vector2), b = default(Vector2);
            c2Witness(s, ref a, ref b);
            float dist = c2Len(a -  b);

            if (hit)
            {
                a = b;
                dist = 0;
            }

            else if (use_radius)
            {
                float rA = pA.radius;
                float rB = pB.radius;

                if (dist > rA + rB && dist > float.Epsilon)
                {
                    dist -= rA + rB;
                    Vector2 n = c2Norm(b -  a);
                    a = Vector2.Add(a, Vector2.Multiply(n, rA));
                    b = Vector2.Subtract(b, Vector2.Multiply(n, rB));
                }

                else
                {
                    Vector2 p = Vector2.Multiply(a +  b, 0.5f);
                    a = p;
                    b = p;
                    dist = 0;
                }
            }

            outA = a;
            outB = b;
            return dist;
        }

        static int c2Hull(Vector2[] verts, int count)
        {
            if (count <= 2) return 0;
            count = (int)c2Min(C2_MAX_POLYGON_VERTS, count);

            int right = 0;
            float xmax = verts[0].X;
            for (int i = 1; i < count; ++i)
            {
                float x = verts[i].X;
                if (x > xmax)
                {
                    xmax = x;
                    right = i;
                }

                else if (x == xmax)
                    if (verts[i].Y < verts[right].Y) right = i;
            }

            int[] hull = new int[C2_MAX_POLYGON_VERTS];
            int out_count = 0;
            int index = right;

            while (true)
            {
                hull[out_count] = index;
                int next = 0;

                for (int i = 1; i < count; ++i)
                {
                    if (next == index)
                    {
                        next = i;
                        continue;
                    }

                    Vector2 e1 = verts[next] -  verts[hull[out_count]];
                    Vector2 e2 = verts[i] -  verts[hull[out_count]];
                    float c = c2Det2(e1, e2);
                    if (c < 0) next = i;
                    if (c == 0 && Vector2.Dot(e2, e2) > Vector2.Dot(e1, e1)) next = i;
                }

                ++out_count;
                index = next;
                if (next == right) break;
            }

            Vector2[] hull_verts = new Vector2[C2_MAX_POLYGON_VERTS];
            for (int i = 0; i < out_count; ++i) hull_verts[i] = verts[hull[i]];
            Array.Copy(verts, hull_verts, out_count);
            //memcpy(verts, hull_verts, sizeof(new Vector2) * out_count);
            return out_count;
        }

        static void c2Norms(Vector2[] verts, Vector2[] norms, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                int a = i;
                int b = i + 1 < count ? i + 1 : 0;
                Vector2 e = verts[b] -  verts[a];
                norms[i] = c2Norm(c2CCW90(e));
            }
        }

        static void c2MakePoly(c2Poly p)
        {
            p.count = c2Hull(p.verts, p.count);
            c2Norms(p.verts, p.norms, p.count);
        }

        static bool c2CircletoCircle(c2Circle A, c2Circle B)
        {
            Vector2 c = B.p -  A.p;
            float d2 = Vector2.Dot(c, c);
            float r2 = A.r + B.r;
            r2 = r2 * r2;
            return d2 < r2;
        }

        static bool c2CircletoAABB(c2Circle A, c2AABB B)
        {
            Vector2 L = c2Clampv(A.p, B.min, B.max);
            Vector2 ab = A.p -  L;
            float d2 = Vector2.Dot(ab, ab);
            float r2 = A.r * A.r;
            return d2 < r2;
        }

        static bool c2AABBtoAABB(c2AABB A, c2AABB B)
        {
            bool d0 = B.max.X < A.min.X;
            bool d1 = A.max.X < B.min.X;
            bool d2 = B.max.Y < A.max.Y;
            bool d3 = A.max.Y < B.max.Y;
            return !(d0 | d1 | d2 | d3);
        }

        // see: http://www.randygaul.net/2014/07/23/distance-point-to-line-segment/
        public static bool c2CircletoCapsule(c2Circle A, c2Capsule B)
        {
            Vector2 n = B.b -  B.a;
            Vector2 ap = A.p -  B.a;
            float da = Vector2.Dot(ap, n);
            float d2;

            if (da < 0) d2 = Vector2.Dot(ap, ap);
            else
            {
                float db = Vector2.Dot(A.p -  B.b, n);
                if (db < 0)
                {
                    Vector2 e = Vector2.Subtract(ap, Vector2.Multiply(n, (da / Vector2.Dot(n, n))));
                    d2 = Vector2.Dot(e, e);
                }
                else
                {
                    Vector2 bp = A.p -  B.b;
                    d2 = Vector2.Dot(bp, bp);
                }
            }

            float r = A.r + B.r;
            return d2 < r * r;
        }

        static bool c2AABBtoCapsule(c2AABB A, c2Capsule B)
        {
            c2x axp = new c2x(), bxp = new c2x();
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_AABB, ref axp, B, C2_TYPE.C2_CAPSULE, ref bxp, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2CapsuletoCapsule(c2Capsule A, c2Capsule B)
        {
            c2x axp = new c2x(), bxp = new c2x();
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_CAPSULE, ref axp, B, C2_TYPE.C2_CAPSULE, ref bxp, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2CircletoPoly(c2Circle A, c2Poly B, ref c2x bx)
        {
            c2x axp = new c2x();
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_CIRCLE, ref axp, B, C2_TYPE.C2_POLY, ref bx, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2AABBtoPoly(c2AABB A, c2Poly B, ref c2x bx)
        {
            c2x axp = new c2x();
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_AABB, ref axp, B, C2_TYPE.C2_POLY, ref bx, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2CapsuletoPoly(c2Capsule A, c2Poly B, ref c2x bx)
        {
            c2x axp = new c2x();
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_CAPSULE, ref axp, B, C2_TYPE.C2_POLY, ref bx, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2PolytoPoly(c2Poly A, ref c2x ax, c2Poly B, ref c2x bx)
        {
            Vector2 oa = new Vector2(), ob = new Vector2();
            if (c2GJK(A, C2_TYPE.C2_POLY, ref ax, B, C2_TYPE.C2_POLY, ref bx, ref oa, ref ob, true) > 0) return false;
            return true;
        }

        static bool c2RaytoCircle(c2Ray A, c2Circle B, c2Raycast @out)
        {
            Vector2 p = B.p;
            Vector2 m = A.p -  p;
            float c = Vector2.Dot(m, m) - B.r * B.r;
            float b = Vector2.Dot(m, A.d);
            float disc = b * b - c;
            if (disc < 0) return false;

            float t = -b - MathF.Sqrt(disc);
            if (t >= 0 && t <= A.t)
            {
                @out.t = t;
                Vector2 impact = c2Impact(A, t);
                @out.n = c2Norm(impact -  p);
                return true;
            }
            return false;
        }

        static bool c2RaytoAABB(c2Ray A, c2AABB B, ref c2Raycast @out)
        {
            Vector2 inv = new Vector2(1.0f / A.d.X, 1.0f / A.d.Y);
            Vector2 d0 = c2Mulvv(B.min -  A.p, inv);
            Vector2 d1 = c2Mulvv(B.max -  A.p, inv);
            Vector2 v0 = c2Minv(d0, d1);
            Vector2 v1 = c2Maxv(d0, d1);
            float lo = c2Hmax(v0);
            float hi = c2Hmin(v1);

            if (hi >= 0 && hi >= lo && lo <= A.t)
            {
                Vector2 c = Vector2.Multiply(B.min +  B.max, 0.5f);
                c = c2Impact(A, lo) - c;
                Vector2 abs_c = c2Absv(c);
                if (abs_c.X > abs_c.Y) @out.n = new Vector2(c2Sign(c.X), 0);
                else @out.n = new Vector2(0, c2Sign(c.Y));
                @out.t = lo;
                return true;
            }
            return false;
        }

        static bool c2RaytoCapsule(c2Ray A, c2Capsule B, ref c2Raycast @out)
        {
            c2m M;
            M.y = c2Norm(B.b -  B.a);
            M.x = c2CCW90(M.y);

            // rotate capsule to origin, along Y axis
            // rotate the ray same way
            Vector2 yBb = c2MulmvT(M, B.b -  B.a);
            Vector2 yAp = c2MulmvT(M, A.p -  B.a);
            Vector2 yAd = c2MulmvT(M, A.d);
            Vector2 yAe = Vector2.Add(yAp, Vector2.Multiply(yAd, A.t));

            if (yAe.X * yAp.X < 0 || c2Min(c2Abs(yAe.X), c2Abs(yAp.X)) < B.r)
            {
                float c = yAp.X > 0 ? B.r : -B.r;
                float d = (yAe.X - yAp.X);
                float t = (c - yAp.X) / d;
                float y = yAp.Y + (yAe.Y - yAp.Y) * t;

                // hit bottom half-circle
                if (y < 0)
                {
                    c2Circle ci = new c2Circle();
                    ci.p = B.a;
                    ci.r = B.r;
                    return c2RaytoCircle(A, ci, @out);
                }

                // hit top-half circle
                else if (y > yBb.Y)
                {
                    c2Circle ci = new c2Circle();
                    ci.p = B.b;
                    ci.r = B.r;
                    return c2RaytoCircle(A, ci, @out);
                }

                // hit the middle of capsule
                else
                {
                    @out.n = c > 0 ? M.x : c2Skew(M.y);
                    @out.t = t * A.t;
                    return true;
                }
            }

            return false;
        }

        static bool c2RaytoPoly(c2Ray A, c2Poly B, ref c2x bx_ptr, ref c2Raycast @out)
        {
            c2x bx = bx_ptr;
            Vector2 p = c2MulxvT(bx, A.p);
            Vector2 d = c2MulrvT(bx.r, A.d);
            float lo = 0;
            float hi = A.t;
            int index = ~0;

            // test ray to each plane, tracking lo/hi times of intersection
            for (int i = 0; i < B.count; ++i)
            {
                float num = Vector2.Dot(B.norms[i], B.verts[i] -  p);
                float den = Vector2.Dot(B.norms[i], d);
                if (num == 0 && den < 0) return false;
                else
                {
                    if (den < 0 && num < lo * den)
                    {
                        lo = num / den;
                        index = i;
                    }
                    else if (den > 0 && num < hi * den) hi = num / den;
                }
                if (hi < lo) return false;
            }

            if (index != ~0)
            {
                @out.t = lo;
                @out.n = c2Mulrv(bx.r, B.norms[index]);
                return true;
            }

            return false;
        }

        static void c2CircletoCircleManifold(c2Circle A, c2Circle B, c2Manifold m)
        {
            m.count = 0;
            var d = B.p - A.p;
            // var dist = distV.Length();
            // var minDist = B.r + A.r;
            // if(dist < minDist){
            //     m.depths1 = minDist - dist;
            //     m.normal = Vector2.Normalize(distV);
            //     m.contact_points1 = (A.p + B.p) * 0.5f;
            //     m.count = 1;
            // }
            
            float d2 = Vector2.Dot(d, d);
            float r = A.r + B.r;
            if (d2 < r * r)
            {
                float l = MathF.Sqrt(d2);
                m.count = 1;
                //m.depths1 = r - d.Length();
                m.depths1 = r - l;
                m.contact_points1 = (A.p + B.p) * 0.5f;
                //m.normal = Vector2.Normalize(d);
                m.normal = l != 0 ? d * (1.0f / l) : new Vector2(0, 1.0f);
            }
        }

        static void c2CircletoAABBManifold(c2Circle A, c2AABB B, c2Manifold m)
        {
            m.count = 0;
            Vector2 L = c2Clampv(A.p, B.min, B.max);
            Vector2 ab = L -  A.p;
            float d2 = Vector2.Dot(ab, ab);
            float r2 = A.r * A.r;
            if (d2 < r2)
            {
                // shallow (center of circle not inside of AABB)
                if (d2 != 0)
                {
                    float d = MathF.Sqrt(d2);
                    Vector2 n = c2Norm(c2Neg(ab));
                    m.count = 1;
                    m.depths1 = A.r - d;
                    m.contact_points1 = Vector2.Add(A.p, Vector2.Multiply(n, -d));
                    m.normal = n;
                }

                // deep (center of circle inside of AABB)
                // clamp circle's center to edge of AABB, then form the manifold
                else
                {
                    Vector2 mid = Vector2.Multiply(B.min +  B.max, 0.5f);
                    Vector2 e = Vector2.Multiply(B.max -  B.min, 0.5f);
                    Vector2 d = A.p -  mid;
                    Vector2 abs_d = c2Absv(d);
                    Vector2 n;
                    Vector2 p = A.p;
                    float depth;
                    if (abs_d.X > abs_d.Y)
                    {
                        if (d.X < 0)
                        {
                            n = new Vector2(-1.0f, 0);
                            p.X = mid.X - e.X;
                        }

                        else
                        {
                            n = new Vector2(1.0f, 0);
                            p.X = mid.X + e.X;
                        }
                        depth = e.X - abs_d.X;
                    }
                    else
                    {
                        if (d.Y < 0)
                        {
                            n = new Vector2(0, -1.0f);
                            p.Y = mid.Y - e.Y;
                        }

                        else
                        {
                            n = new Vector2(0, 1.0f);
                            p.Y = mid.Y + e.Y;
                        }
                        depth = e.Y - abs_d.Y;
                    }
                    m.count = 1;
                    m.depths1 = depth;
                    m.contact_points1 = p;
                    m.normal = n;
                }
            }
        }

        static void c2CircletoCapsuleManifold(c2Circle A, c2Capsule B, c2Manifold m)
        {
            m.count = 0;
            Vector2 a = new Vector2(), b = new Vector2();
            float r = A.r + B.r;
            c2x axp = new c2x(), bxp = new c2x();
            float d = c2GJK(A, C2_TYPE.C2_CIRCLE, ref axp, B, C2_TYPE.C2_CAPSULE, ref bxp, ref a, ref b, false);
            if (d < r)
            {
                m.count = 1;
                m.depths1 = r - d;
                m.contact_points1 = Vector2.Multiply(a +  b, 0.5f);
                if (d == 0) m.normal = c2Skew(c2Norm(B.b -  B.a));
                else m.normal = c2Norm(b -  a);
            }
        }

        static void c2AABBtoAABBManifold(c2AABB A, c2AABB B, c2Manifold m)
        {
            m.count = 0;
            Vector2 mid_a = Vector2.Multiply(A.min +  A.max, 0.5f);
            Vector2 mid_b = Vector2.Multiply(B.min +  B.max, 0.5f);
            Vector2 eA = c2Absv(Vector2.Multiply(A.max -  A.min, 0.5f));
            Vector2 eB = c2Absv(Vector2.Multiply(B.max -  B.min, 0.5f));
            Vector2 d = mid_b -  mid_a;

            // calc overlap on x and y axes
            float dx = eA.X + eB.X - c2Abs(d.X);
            if (dx < 0) return;
            float dy = eA.Y + eB.Y - c2Abs(d.Y);
            if (dy < 0) return;

            Vector2 n;
            float depth;
            Vector2 p;

            // x axis overlap is smaller
            if (dx < dy)
            {
                depth = dx;
                if (d.X < 0)
                {
                    n = new Vector2(-1.0f, 0);
                    p = Vector2.Subtract(mid_a, new Vector2(eA.X, 0));
                }
                else
                {
                    n = new Vector2(1.0f, 0);
                    p = Vector2.Add(mid_a, new Vector2(eA.X, 0));
                }
            }

            // y axis overlap is smaller
            else
            {
                depth = dy;
                if (d.Y < 0)
                {
                    n = new Vector2(0, -1.0f);
                    p = Vector2.Subtract(mid_a, new Vector2(0, eA.Y));
                }
                else
                {
                    n = new Vector2(0, 1.0f);
                    p = Vector2.Add(mid_a, new Vector2(0, eA.Y));
                }
            }

            m.count = 1;
            m.contact_points1 = p;
            m.depths1 = depth;
            m.normal = n;
        }

        static void c2AABBtoCapsuleManifold(c2AABB A, c2Capsule B, c2Manifold m)
        {
            m.count = 0;
            c2Poly p = new c2Poly();
            c2BBVerts(p.verts, A);
            p.count = 4;
            c2Norms(p.verts, p.norms, 4);
            c2x bxp = new c2x();
            c2CapsuletoPolyManifold(B, p, ref bxp, m);
        }

        static void c2CapsuletoCapsuleManifold(c2Capsule A, c2Capsule B, c2Manifold m)
        {
            m.count = 0;
            Vector2 a = new Vector2(), b = new Vector2();
            c2x axp = new c2x(), bxp = new c2x();
            float d = c2GJK(A, C2_TYPE.C2_CAPSULE, ref axp, B, C2_TYPE.C2_CAPSULE, ref bxp, ref a, ref b, false);
            if (d < A.r + B.r)
            {
                m.count = 1;
                m.contact_points1 = Vector2.Multiply(a +  b, 0.5f);
                m.depths1 = d;
                m.normal = d != 0 ? c2Norm(b -  a) : c2Norm(c2Skew(A.b -  A.a));
            }
        }

        // Circle center is inside the polygon
        // find the face closest to circle center to form manifold
        static c2h C2_PLANE_AT(c2Poly p, int i) => new c2h { n = (p).norms[i], d = Vector2.Dot((p).norms[i], (p).verts[i]) };

        static void c2CircletoPolyManifold(c2Circle A, c2Poly B, ref c2x bx_tr, c2Manifold m)
        {
            m.count = 0;
            Vector2 a = new Vector2(), b = new Vector2();
            c2x axp = new c2x();
            float d = c2GJK(A, C2_TYPE.C2_CIRCLE, ref axp, B, C2_TYPE.C2_POLY, ref bx_tr, ref a, ref b, false);

            // shallow, the circle center did not hit the polygon
            // just use a and b from GJK to define the collision
            if (d != 0)
            {
                Vector2 n = b -  a;
                float l = Vector2.Dot(n, n);
                if (l < A.r * A.r)
                {
                    l = MathF.Sqrt(l);
                    m.count = 1;
                    m.contact_points1 = b;
                    m.depths1 = l;
                    m.normal = Vector2.Multiply(n, 1.0f / l);
                }
            }

            else
            {
                c2x bx = bx_tr;
                float sep = -float.MaxValue;
                int index = ~0;
                Vector2 local = c2MulxvT(bx, A.p);

                for (int i = 0; i < B.count; ++i)
                {
                    c2h h1 = C2_PLANE_AT(B, i);
                    float d1 = c2Dist(h1, local);
                    if (d1 > A.r) return;
                    if (d1 > sep)
                    {
                        sep = d1;
                        index = i;
                    }
                }

                c2h h = C2_PLANE_AT(B, index);
                Vector2 p = c2Project(h, local);
                m.count = 1;
                m.contact_points1 = c2Mulxv(bx, p);
                m.depths1 = sep;
                m.normal = c2Mulrv(bx.r, B.norms[index]);
            }
        }

        // Forms a c2Poly and uses c2PolytoPolyManifold
        static void c2AABBtoPolyManifold(c2AABB A, c2Poly B, ref c2x bx, c2Manifold m)
        {
            m.count = 0;
            c2Poly p = new c2Poly();
            c2BBVerts(p.verts, A);
            p.count = 4;
            c2Norms(p.verts, p.norms, 4);
            c2x axp = new c2x();
            c2PolytoPolyManifold(p, ref axp, B, ref bx, m);
        }

        // clip a segment to a plane
        static int c2Clip(Vector2[] seg, c2h h)
        {
            Vector2[] @out = new Vector2[2];
            int sp = 0;
            float d0, d1;
            if ((d0 = c2Dist(h, seg[0])) < 0) @out[sp++] = seg[0];
            if ((d1 = c2Dist(h, seg[1])) < 0) @out[sp++] = seg[1];
            if (d0 * d1 < 0) @out[sp++] = c2Lerp(seg[0], seg[1], d0 / (d0 - d1));
            seg[0] = @out[0]; seg[1] = @out[1];
            return sp;
        }

        // clip a segment to the "side planes" of another segment.
        // side planes are planes orthogonal to a segment and attached to the
        // endpoints of the segment
        static bool c2SidePlanes(Vector2[] seg, c2x x, c2Poly p, int e, ref c2h h)
        {
            Vector2 ra = c2Mulxv(x, p.verts[e]);
            Vector2 rb = c2Mulxv(x, p.verts[e + 1 == p.count ? 0 : e + 1]);
            Vector2 @in = c2Norm(rb -  ra);
            c2h left = new c2h { n = c2Neg(@in), d = Vector2.Dot(c2Neg(@in), ra) };
            c2h right = new c2h { n = @in, d = Vector2.Dot(@in, rb) };
            if (c2Clip(seg, left) < 2) return false;
            if (c2Clip(seg, right) < 2) return false;
            h = new c2h { n = c2CCW90(@in), d = Vector2.Dot(c2CCW90(@in), ra) };
            return true;
        }

        static void c2KeepDeep(Vector2[] seg, c2h h, c2Manifold m)
        {
            int cp = 0;
            for (int i = 0; i < 2; ++i)
            {
                Vector2 p = seg[i];
                float d = c2Dist(h, p);
                if (d < 0)
                {
                    if (cp == 0) m.contact_points1 = p;
                    if (cp == 1) m.contact_points2 = p;
                    if (cp == 0) m.depths1 = -d;
                    if (cp == 1) m.depths2 = -d;
                    ++cp;
                }
            }
            m.count = cp;
            m.normal = h.n;
        }

        static Vector2 c2CapsuleSupport(c2Capsule A, Vector2 dir)
        {
            float da = Vector2.Dot(A.a, dir);
            float db = Vector2.Dot(A.b, dir);
            if (da > db) return Vector2.Add(A.a, Vector2.Multiply(dir, A.r));
            else return Vector2.Add(A.b, Vector2.Multiply(dir, A.r));
        }

        static void c2AntinormalFace(c2Capsule cap, c2Poly p, c2x x, ref int face_out, ref Vector2 n_out)
        {
            float sep = -float.MaxValue;
            int index = ~0;
            Vector2 n = default(Vector2);
            for (int i = 0; i < p.count; ++i)
            {
                c2h h = c2Mulxh(x, C2_PLANE_AT(p, i));
                Vector2 n0 = c2Neg(h.n);
                Vector2 s = c2CapsuleSupport(cap, n0);
                float d = c2Dist(h, s);
                if (d > sep)
                {
                    sep = d;
                    index = i;
                    n = n0;
                }
            }
            face_out = index;
            n_out = n;
        }

        static void c2CapsuletoPolyManifold(c2Capsule A, c2Poly B, ref c2x bx_ptr, c2Manifold m)
        {
            m.count = 0;
            Vector2 a = new Vector2(), b = new Vector2();
            c2x axp = new c2x();
            float d = c2GJK(A, C2_TYPE.C2_CAPSULE, ref axp, B, C2_TYPE.C2_POLY, ref bx_ptr, ref a, ref b, false);

            // deep, treat as segment to poly collision
            if (d == 0)
            {
                c2x bx = bx_ptr;
                Vector2 n = new Vector2();
                int index = 0;
                c2AntinormalFace(A, B, bx, ref index, ref n);
                Vector2[] seg = new Vector2[] { Vector2.Add(A.a, Vector2.Multiply(n, A.r)), Vector2.Add(A.b, Vector2.Multiply(n, A.r)) };
                c2h h = new c2h();
                if (!c2SidePlanes(seg, bx, B, index, ref h)) return;
                c2KeepDeep(seg, h, m);
            }

            // shallow, use GJK results a and b to define manifold
            else if (d < A.r)
            {
                c2x bx = bx_ptr;
                Vector2 ab = b -  a;
                bool face_case = false;

                for (int i = 0; i < B.count; ++i)
                {
                    Vector2 n = c2Mulrv(bx.r, B.norms[i]);
                    if (c2Parallel(ab, n, 5.0e-3f))
                    {
                        face_case = true;
                        break;
                    }
                }

                // 1 contact
                if (!face_case)
                {
                    m.count = 1;
                    m.contact_points1 = b;
                    m.depths1 = A.r - d;
                    m.normal = Vector2.Multiply(ab, 1.0f / d);
                }

                // 2 contacts if laying on a polygon face nicely
                else
                {
                    Vector2 n = new Vector2();
                    int index = 0;
                    c2AntinormalFace(A, B, bx, ref index, ref n);
                    Vector2[] seg = new Vector2[] { Vector2.Add(A.a, Vector2.Multiply(n, A.r)), Vector2.Add(A.b, Vector2.Multiply(n, A.r)) };
                    c2h h = new c2h();
                    if (!c2SidePlanes(seg, bx, B, index, ref h)) return;
                    c2KeepDeep(seg, h, m);
                }
            }
        }

        static float c2CheckFaces(c2Poly A, c2x ax, c2Poly B, c2x bx, ref int face_index)
        {
            c2x b_in_a = c2MulxxT(ax, bx);
            c2x a_in_b = c2MulxxT(bx, ax);
            float sep = -float.MaxValue;
            int index = ~0;

            for (int i = 0; i < A.count; ++i)
            {
                c2h h = C2_PLANE_AT(A, i);
                int idx = c2Support(B.verts, B.count, c2Mulrv(a_in_b.r, c2Neg(h.n)));
                Vector2 p = c2Mulxv(b_in_a, B.verts[idx]);
                float d = c2Dist(h, p);
                if (d > sep)
                {
                    sep = d;
                    index = i;
                }
            }

            face_index = index;
            return sep;
        }

        static void c2Incident(Vector2[] incident, c2Poly ip, c2x ix, c2Poly rp, c2x rx, int re)
        {
            Vector2 n = c2MulrvT(ix.r, c2Mulrv(rx.r, rp.norms[re]));
            int index = ~0;
            float min_dot = float.MaxValue;
            for (int i = 0; i < ip.count; ++i)
            {
                float dot = Vector2.Dot(n, ip.norms[i]);
                if (dot < min_dot)
                {
                    min_dot = dot;
                    index = i;
                }
            }
            incident[0] = c2Mulxv(ix, ip.verts[index]);
            incident[1] = c2Mulxv(ix, ip.verts[index + 1 == ip.count ? 0 : index + 1]);
        }

        // Please see Dirk Gregorius's 2013 GDC lecture on the Separating Axis Theorem
        // for a full-algorithm overview. The short description is:
        // Test A against faces of B, test B against faces of A
        // Define the reference and incident shapes (denoted by r and i respectively)
        // Define the reference face as the axis of minimum penetration
        // Find the incident face, which is most anti-normal face
        // Clip incident face to reference face side planes
        // Keep all points behind the reference face
        static void c2PolytoPolyManifold(c2Poly A, ref c2x ax_ptr, c2Poly B, ref c2x bx_ptr, c2Manifold m)
        {
            m.count = 0;
            c2x ax = ax_ptr;
            c2x bx = bx_ptr;
            int ea = 0, eb = 0;
            float sa = 0, sb = 0;
            if ((sa = c2CheckFaces(A, ax, B, bx, ref ea)) >= 0) return;
            if ((sb = c2CheckFaces(B, bx, A, ax, ref eb)) >= 0) return;

            c2Poly rp = null, ip = null;
            c2x rx, ix;
            int re;
            float kRelTol = 0.95f, kAbsTol = 0.01f;
            if (sa * kRelTol > sb + kAbsTol)
            {
                rp = A; rx = ax;
                ip = B; ix = bx;
                re = ea;
            }
            else
            {
                rp = B; rx = bx;
                ip = A; ix = ax;
                re = eb;
            }

            Vector2[] incident = new Vector2[2];
            c2Incident(incident, ip, ix, rp, rx, re);
            c2h rh = new c2h();
            if (!c2SidePlanes(incident, rx, rp, re, ref rh)) return;
            c2KeepDeep(incident, rh, m);
        }

        /*
            zlib license:

            Copyright (c) 2017 Randy Gaul http://www.randygaul.net
            This software is provided 'as-is', without any express or implied warranty.
            In no event will the authors be held liable for any damages arising from
            the use of this software.
            Permission is granted to anyone to use this software for any purpose,
            including commercial applications, and to alter it and redistribute it
            freely, subject to the following restrictions:
              1. The origin of this software must not be misrepresented; you must not
                 claim that you wrote the original software. If you use this software
                 in a product, an acknowledgment in the product documentation would be
                 appreciated but is not required.
              2. Altered source versions must be plainly marked as such, and must not
                 be misrepresented as being the original software.
              3. This notice may not be removed or altered from any source distribution.
        */
    }
}