using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
	MY FIRST ATTEMPT AT IMPLEMENTING AN ALTERNATIVE PATHFINDING ALGORITHM, IT DOES NOT WORK
*/
namespace PathFinding{

	
    public class ARA<TNode,TConnection,TNodeConnection,TGraph,THeuristic> : A_Star<TNode,TConnection,TNodeConnection,TGraph,THeuristic>
	where TNode : Node
	where TConnection : Connection<TNode>
	where TNodeConnection : NodeConnections<TNode,TConnection>
	where TGraph : Graph<TNode,TConnection,TNodeConnection>
	where THeuristic : Heuristic<TNode>
    {
        
		public float episilon;
		
		List<TNode> closed;
		visitedNodeRecord visitedRecord;
		Queue open, incons;
        public ARA(int maxNodes, float maxTime, int maxDepth) : base(maxNodes, maxTime, maxDepth){
            episilon = 5;
			open = new Queue();
			closed = new List<TNode>();
			visitedRecord = new visitedNodeRecord();
			incons = new Queue();
        }

		public float fvalue(THeuristic h, TNode s){
			Debug.Log(episilon);
			if(visitedRecord.Contains(s))
				return visitedRecord.getCost(s) + h.estimateCost(s) * episilon;
			return float.NaN;
		}

		float min(float a, float b){
			if(a > b) return b;
			return a;
		}

		float max(float a, float b){
			if(a < b) return b;
			return a;
		}
		public class visitedNodeRecord{
			List<NodeRecord> record;
			public visitedNodeRecord(){
				record = new List<NodeRecord>();
			}

			public void Add(NodeRecord nr){
				if(Contains(nr)) return;
				record.Add(nr);
			}
			public void Add(TNode n, float cost = 0){
				if(Contains(n)) return;
				NodeRecord nr = new NodeRecord();
				nr.costSoFar = cost;
				record.Add(nr);
			}

			public bool Contains(TNode n){
				foreach (var item in record)
				{
					if (item.node == n) return true;
				}
				return false;
			}
			public float getCost(TNode n){
				foreach (var item in record)
				{
					if (item.node == n) return item.costSoFar;
				}
				return -1;
			}

			public bool Contains(NodeRecord n){
				foreach (var item in record)
				{
					if (item == n) return true;
				}
				return false;
			}

			public void setCost(NodeRecord n, float cost){
				foreach (var item in record)
				{
					if (item == n){
						item.costSoFar = cost;
						return;
					}
				}
			}

			public void setCost(TNode n, float cost){
				foreach (var item in record)
				{
					if (item.node == n){item.costSoFar = cost; return;}
				}
			}
		}
		public void ImprovePath(TGraph graph, TNode end, THeuristic heuristic){

			NodeRecord current;
			

			while (open.getLowestCostNode()!= null && incons.getLowestCostNode() != null && fvalue(heuristic, end) > min(open.getLowestCostNode().costSoFar, incons.getLowestCostNode().costSoFar))
			{
				current = open.getLowestCostNode();
				open.Remove(current);
				closed.Add(current.node);

				if(!visitedRecord.Contains(current)){
					visitedRecord.Add(current);
					visitedNodes.Add(current.node);
				}

				foreach (var item in graph.getConnections(current.node).connections)
				{
					if(item == null) continue;
					if(!visitedRecord.Contains(item.toNode)){
						visitedRecord.Add(item.toNode, float.NaN);
					}
					else{
						visitedRecord.setCost(item.toNode, current.costSoFar + item.cost);

						if(!closed.Contains(item.toNode)) open.Add(item.toNode, fvalue(heuristic, item.toNode));
						else incons.Add(item.toNode);
					}
				}
			}


		}

		public List<TNode> getpath(){
			List<TNode> path = new List<TNode>();
			while(currentBest != null){
				path.Add(currentBest.node);
				currentBest = currentBest.connection;
			}
			return path;
		}
		
        public override List<TNode> findpath(TGraph graph, TNode start, TNode end, THeuristic heuristic, ref int found)
        {
			visitedRecord.Add(start);
			visitedRecord.Add(end);
			
			visitedRecord.setCost(start, 0);
			visitedRecord.setCost(end, float.MaxValue);
			
			List<TNode> path;

			open.Clear();
			closed.Clear();
			incons.Clear();

			ImprovePath(graph, end, heuristic);
			
			float m1, m2;

			if(open.Count() == 0) m1 = float.MaxValue;
			else m1 = open.getLowestCostNode().costSoFar + heuristic.estimateCost(open.getLowestCostNode().node);
			if(incons.Count() == 0) m2 = float.MaxValue;
			else m2 = incons.getLowestCostNode().costSoFar + heuristic.estimateCost(incons.getLowestCostNode().node);
			if(open.Count() > 0 && incons.Count() > 0){
				episilon = max(episilon, visitedRecord.getCost(end)/min(m1, m2));
			}
			Debug.Log(episilon);
			path = getpath();
			// for (int i = 0; i < path.Count(); i++)
			// {
			// 	Debug.Log(path[i].id);
			// }
			while (episilon > 1)
			{	
				episilon-=0.5f;
				for(int i = 0; i < incons.Count(); i++){
					open.Add(incons.getItemAt(i));
					incons.Remove(incons.getItemAt(i).node);
				}


				closed.Clear();
				ImprovePath(graph, end, heuristic);
				
				if(open.Count() == 0) m1 = float.MaxValue;
				else m1 = open.getLowestCostNode().costSoFar + heuristic.estimateCost(open.getLowestCostNode().node);
				if(incons.Count() == 0) m2 = float.MaxValue;
				else m2 = incons.getLowestCostNode().costSoFar + heuristic.estimateCost(incons.getLowestCostNode().node);
				if(open.Count() > 0 && incons.Count() > 0){
					episilon = max(episilon, visitedRecord.getCost(end)/min(m1, m2));
				}
				path = getpath();
			}


			// List<TNode> aux = new List<TNode>();
			return path;	
        }

    }


}