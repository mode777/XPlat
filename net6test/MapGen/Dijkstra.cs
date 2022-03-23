using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net6test.MapGenerator
{
    internal class Algo
    {
// 1  function Dijkstra(Graph, source):
// 2
// 3      create vertex set Q
// 4
// 5      for each vertex v in Graph:            
// 6          dist[v] ← INFINITY                 
// 7          prev[v] ← UNDEFINED                
// 8          add v to Q                     
// 9      dist[source] ← 0                       
//10     
//11      while Q is not empty:
//12          u ← vertex in Q with min dist[u]   
//13                                             
//14          remove u from Q
//15         
//16          for each neighbor v of u still in Q:
//17              alt ← dist[u] + length(u, v)
//18              if alt<dist[v]:              
//19                  dist[v] ← alt
//20                  prev[v] ← u
//21
//22      return dist[], prev[]

//1  S ← empty sequence
//2  u ← target
//3  if prev[u] is defined or u = source:          // Do something only if the vertex is reachable
//4      while u is defined:                       // Construct the shortest path with a stack S
//5          insert u at the beginning of S        // Push the vertex onto the stack
//6          u ← prev[u]                           // Traverse from target to source
        
        public static List<Node> Dijkstra(List<Node> graph, Node source, Node Target)
        {
            var Q = new HashSet<Node>();
            var dist = new Dictionary<Node, int>();
            var prev = new Dictionary<Node, Node>();

            foreach (var v in graph)
            {
                dist[v] = int.MaxValue;
                prev[v] = null;
                Q.Add(v);
            }
            dist[source] = 0;

            while(Q.Count > 0)
            {
                var u = dist.Where(x => Q.Contains(x.Key)).OrderBy(x => x.Value).First().Key;
                Q.Remove(u);

                if(u == Target)
                {
                    var s = new List<Node>();
                    if(prev.ContainsKey(u) || u == source)
                    {
                        while(u != null)
                        {
                            s.Insert(0, u);
                            prev.TryGetValue(u, out var outval);
                            u = outval;
                        }
                        return s;
                    }
                }

                foreach (var v in u.Neighbours)
                {
                    var alt = dist[u] + 1;
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }
            // path not found
            return null;
        }
    }
}
