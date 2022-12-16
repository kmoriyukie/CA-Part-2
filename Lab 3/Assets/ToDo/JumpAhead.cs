using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace PathFinding{

	
    public class JumpAhead<TNode,TConnection,TNodeConnection,TGraph,THeuristic> : A_Star<TNode,TConnection,TNodeConnection,TGraph,THeuristic>
	where TNode : Node
	where TConnection : Connection<TNode>
	where TNodeConnection : NodeConnections<TNode,TConnection>
	where TGraph : Graph<TNode,TConnection,TNodeConnection>
	where THeuristic : Heuristic<TNode>
    {
        
		public float episilon;
		
		List<TNode> closed;
		Queue open;
        public JumpAhead(int maxNodes, float maxTime, int maxDepth) : base(maxNodes, maxTime, maxDepth){
            episilon = 5;
			open = new Queue();
			closed = new List<TNode>();
        }

		public List<NodeRecord> IdentifySuccessors(TGraph graph, THeuristic heuristic, TNode start, NodeRecord current, TNode goal){
			List<NodeRecord> successors = new List<NodeRecord>();
			List<NodeRecord> neighbors = Prune(graph, heuristic, start, current, goal);
			NodeRecord n;
			for(int i = 0; i < 8; i++){
				n = Jump(graph, heuristic, current, i, goal);
				if(n != null) successors.Add(n);
			}
			return successors;
		}

		public List<NodeRecord> Prune(TGraph graph, THeuristic heuristic, TNode start, NodeRecord current, TNode goal){
			// List<TNode> lst = new List<TNode>();
			List<NodeRecord> connections = new List<NodeRecord>();
			List<TConnection> neighbors = graph.getConnections(current.node).connections;
			NodeRecord aux;
			if(current.node == start){
				foreach (var item in neighbors){
					if(item == null) {
						connections.Add(null);
						continue;	
					}
					aux = new NodeRecord(item.toNode);
					aux.connection = current;
					aux.isForced = neighbors.Contains(null);
					connections.Add(new NodeRecord(item.toNode));
				}
			}
			else{ //don't prune starting node
				double cost;

				foreach (var item in neighbors)
				{
					if(item == null){
						connections.Add(null);
						continue;	
					}
					cost = fvalue(heuristic, item);
					if(cost < current.costSoFar){
						aux = new NodeRecord(item.toNode);
						aux.connection = current;
						aux.isForced = neighbors.Contains(null);
						connections.Add(aux);
					}
					else{
						connections.Add(null);
					}
				}
			}
			return connections;
		}

		
		public NodeRecord Jump(TGraph graph, THeuristic heuristic, NodeRecord current, int direction, TNode end){
			TConnection c =  graph.getConnections(current.node).connections[direction];
			if(c == null) return null;

			NodeRecord n = new NodeRecord(c.toNode);
			n.connection = current;
			n.costSoFar = current.costSoFar + heuristic.estimateCost(n.node);

			if(n.node == null) return null;
			if(n.node == end) return n;
			
			foreach (var item in graph.getConnections(n.node).connections) //is forced?
				if(item == null || fvalue(heuristic, item) < current.costSoFar + heuristic.estimateCost(n.node)) 
					return n;
			
			if(direction % 2 == 0){ // Diagonal
				if(direction == 0) // check x and y "components" of diagonal
					if(Jump(graph, heuristic, n, 7, end) != null) return n;
				else
					if(Jump(graph, heuristic, n, direction - 1, end) != null) return n;
				if (Jump(graph, heuristic, n, direction + 1, end) != null) return n;
			}

			return n;
		}


        public float fvalue(THeuristic h, NodeRecord s)
        {
            return h.estimateCost(s.node) + s.costSoFar;
        }

        public override List<TNode> findpath(TGraph graph, TNode start, TNode end, THeuristic heuristic, ref int found)
        {

			List<TNode> path = new List<TNode>();
			
			Queue open = new Queue();
			List<TNode> closed = new List<TNode>();

			open.Add(new NodeRecord(start), heuristic.estimateCost(start));
			NodeRecord current;
			
			float cost = 0;
			while (open.getLowestCostNode() != null && open.getLowestCostNode().node != end)
			{
				current = open.getLowestCostNode();
				open.Remove(current);
				closed.Add(current.node);
				foreach (var con in IdentifySuccessors(graph, heuristic, start, current, end))
				{
					if(con == null) continue;
					cost = fvalue(heuristic, con);
					if(open.Contains(con.node) && cost < con.costSoFar){
						open.Remove(con.node);
					}
					if(!open.Contains(con.node) && !closed.Contains(con.node)){
						con.costSoFar = cost;
						open.Add(new NodeRecord(con.node), cost, current);
						currentBest = open.getLowestCostNode(); // get lowest cost element
					}
				}
			}

			while(currentBest != null){
				path.Add(currentBest.node);
				currentBest = currentBest.connection;
			}

			return path;

		}
	}

}

