using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PathFinding{

	public class A_Star<TNode,TConnection,TNodeConnection,TGraph,THeuristic> : PathFinder<TNode,TConnection,TNodeConnection,TGraph,THeuristic>
	where TNode : Node
	where TConnection : Connection<TNode>
	where TNodeConnection : NodeConnections<TNode,TConnection>
	where TGraph : Graph<TNode,TConnection,TNodeConnection>
	where THeuristic : Heuristic<TNode>
	{
	// Class that implements the A* pathfinding algorithm
	// You have to implement the findpath function.
	// You can add whatever you need.
				
		protected List<TNode> visitedNodes; // list of visited nodes 
		
		// protected List<NodeRecord> visitedNodes;
		protected NodeRecord currentBest; // current best node found
		
		protected enum NodeRecordCategory{ OPEN, CLOSED, UNVISITED };
				
		protected class NodeRecord{	
		// You can use (or not) this structure to keep track of the information that we need for each node
			
			public NodeRecord(){}
			public NodeRecord(TNode n){node = n;}
			
			public TNode node; 
			public NodeRecord connection;	// connection traversed to reach this node 
			public float costSoFar; // cost accumulated to reach this node
			public float estimatedTotalCost; // estimated total cost to reach the goal from this node
			public NodeRecordCategory category; // category of the node: open, closed or unvisited
			public int depth; // depth in the search graph

		};

		protected class Queue{

			protected class QueueItem{
				public QueueItem(){}
				public QueueItem(NodeRecord node, float p){noderecord = node; priority = p;}
				public QueueItem(NodeRecord node, float p, NodeRecord parent){
					noderecord = node; 
					noderecord.connection = parent;
					priority = p;
				}

				public NodeRecord noderecord;
				public float priority;
			};

			protected List<QueueItem> NodeQueue;
			
			public Queue(){ NodeQueue = new List<QueueItem>();}

			public void Add(NodeRecord nr, float r = 0, NodeRecord parent = null){
				QueueItem Q = new QueueItem(nr, r, parent);
				float currentPriority = Q.priority;
				for (int i = 0; i < NodeQueue.Count; i++)
				{
					if(NodeQueue[i].priority < currentPriority){
						NodeQueue.Insert(i, Q);
						return;
					}
				}
				NodeQueue.Add(Q);
			}
			public void Add(TNode n, float r = 0){
				Add(new NodeRecord(n), r);
				// rank.Add(r);
			}
			public int Count(){
				return NodeQueue.Count();
			}
			public NodeRecord getLowestCostNode(){
				if(NodeQueue.Count > 0)
					return NodeQueue[NodeQueue.Count -1].noderecord;
				else
					return null;
			}
			
			public bool Contains(TNode n){
				foreach (QueueItem item in NodeQueue)
				{
					if(item.noderecord.node == n) return true;
				}
				return false;
			}

			public void Remove(TNode n){
				foreach (QueueItem item in NodeQueue)
				{
					if(item.noderecord.node == n) {
						NodeQueue.Remove(item);
						return;
					}
				}
			}
			public void Remove(NodeRecord n){
				foreach (QueueItem item in NodeQueue)
				{
					if(item.noderecord == n) {
						NodeQueue.Remove(item);
						return;
					}
				}
			}

			public NodeRecord GetNodeRecordAt(int index){
				return NodeQueue[index].noderecord;
			}
		};


		public	A_Star(int maxNodes, float maxTime, int maxDepth):base(){ 
			
			visitedNodes = new List<TNode> ();
			
		}

		public virtual List<TNode> getVisitedNodes(){
			return visitedNodes;
		}
		
		public override List<TNode> findpath(TGraph graph, TNode start, TNode end, THeuristic heuristic, ref int found)
		{
			List<TNode> path = new List<TNode>();
			
			// TO IMPLEMENT
			Queue open = new Queue();
			List<TNode> closed = new List<TNode>();

			open.Add(new NodeRecord(start));
			NodeRecord current;
			
			float cost = 0;

			while (open.getLowestCostNode() != null && open.getLowestCostNode().node != end)
			{
				current = open.getLowestCostNode();
				open.Remove(current);
				closed.Add(current.node);

				visitedNodes.Add(current.node);
				foreach (var con in graph.getConnections(current.node).connections)
				{
					cost = con.cost + heuristic.estimateCost(con.toNode);
					if(open.Contains(con.toNode) && cost < con.cost){
						open.Remove(con.toNode);
					}
					if(!open.Contains(con.toNode) && !closed.Contains(con.toNode)){
						con.setCost(cost);
						open.Add(new NodeRecord(con.toNode), cost, current);
						currentBest = open.getLowestCostNode(); // get lowest cost element
						// Debug.Log(currentBest.node.id);
					}
				}
			}


			while(currentBest != null){
				path.Add(currentBest.node);
				currentBest = currentBest.connection;
			}

			return path;
		}


	};

}