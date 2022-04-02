using System;

namespace XPlat.Graphics {
    public class Atlas {

        public class FONSatlasNode
        {
            public short x, y, width;
        }

	    public int width, height;
		public FONSatlasNode[] nodes;
		public int nnodes;
		public uint cnodes;

        public Atlas(int w, int h, uint nnodes)
        {
            width = w;
			height = h;

			// Allocate space for skyline nodes
			nodes = new FONSatlasNode[nnodes];
			for (int cont = 0; cont < nnodes; cont++)
				nodes[cont] = new FONSatlasNode();

			nnodes = 0;
			cnodes = nnodes;

			// Init root node.
			nodes[0].x = 0;
			nodes[0].y = 0;
			nodes[0].width = (short)w;
			nnodes++;
        }

        void Reset(int w, int h)
		{
			width = w;
			height = h;
			nnodes = 0;

			// Init root node.
			nodes[0].x = 0;
			nodes[0].y = 0;
			nodes[0].width = (short)w;
			nnodes++;
		}

        void Expand(int w, int h)
		{
			// Insert node for empty space
			if (w > width)
				InsertNode(nnodes, width, 0, w - width);
			width = w;
			height = h;
		}

        public bool AddRect(int rw, int rh, ref int rx, ref int ry)
		{
			int besth = height, bestw = width, besti = -1;
			int bestx = -1, besty = -1, i;

			// Bottom left fit heuristic.
			for (i = 0; i < nnodes; i++)
			{
				int y = RectFits(i, rw, rh);
				if (y != -1)
				{
					short nw = nodes[i].width;
					if (y + rh < besth || (y + rh == besth && nw < bestw))
					{
						besti = i;
						bestw = nodes[i].width;
						besth = y + rh;
						bestx = nodes[i].x;
						besty = y;
					}
				}
			}

			if (besti == -1)
				return false;

			// Perform the actual packing.
			if (AddSkylineLevel(besti, bestx, besty, rw, rh) == 0)
				return false;

			rx = bestx;
			ry = besty;

			return true;
		}

        int AddSkylineLevel(int idx, int x, int y, int w, int h)
		{
			int i;

			// Insert new node
			if (InsertNode(idx, x, y + h, w) == 0)
				return 0;

			// Delete skyline segments that fall under the shaodw of the new segment.
			for (i = idx + 1; i < nnodes; i++)
			{
				if (nodes[i].x < nodes[i - 1].x + nodes[i - 1].width)
				{
					int shrink = nodes[i - 1].x + nodes[i - 1].width - nodes[i].x;
					nodes[i].x += (short)shrink;
					nodes[i].width -= (short)shrink;
					if (nodes[i].width <= 0)
					{
						RemoveNode(i);
						i--;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			// Merge same height skyline segments that are next to each other.
			for (i = 0; i < nnodes - 1; i++)
			{
				if (nodes[i].y == nodes[i + 1].y)
				{
					nodes[i].width += nodes[i + 1].width;
					RemoveNode(i + 1);
					i--;
				}
			}

			return 1;
		}

        int InsertNode(int idx, int x, int y, int w)
		{
			int i;
			// Insert node
			if (nnodes + 1 > cnodes)
			{
				cnodes = cnodes == 0 ? 8 : cnodes * 2;
				Array.Resize(ref nodes, (int)cnodes);
			}
			for (i = nnodes; i > idx; i--)
			{
				nodes[i].x = nodes[i - 1].x;
				nodes[i].y = nodes[i - 1].y;
				nodes[i].width = nodes[i - 1].width;
			}
			nodes[idx].x = (short)x;
			nodes[idx].y = (short)y;
			nodes[idx].width = (short)w;
			nnodes++;

			return 1;
		}

        void RemoveNode(int idx)
		{
			int i;
			if (nnodes == 0)
				return;
			for (i = idx; i < nnodes - 1; i++)
			{
				//nodes[i] = nodes[i + 1];

				nodes[i].x = nodes[i + 1].x;
				nodes[i].y = nodes[i + 1].y;
				nodes[i].width = nodes[i + 1].width;
			}
			nnodes--;
		}

        int RectFits(int i, int w, int h)
		{
			// Checks if there is enough space at the location of skyline span 'i',
			// and return the max height of all skyline spans under that at that location,
			// (think tetris block being dropped at that position). Or -1 if no space found.
			int x = nodes[i].x;
			int y = nodes[i].y;
			int spaceLeft;
			if (x + w > width)
				return -1;
			spaceLeft = w;
			while (spaceLeft > 0)
			{
				if (i == nnodes)
					return -1;
				y = Math.Max(y, nodes[i].y);
				if (y + h > height)
					return -1;
				spaceLeft -= nodes[i].width;
				++i;
			}
			return y;
		}

    }

}