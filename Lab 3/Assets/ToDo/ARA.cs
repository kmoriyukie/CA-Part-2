using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


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
		List<NodeRecord> visitedNodeRecord;
		Queue open, incons;
        public ARA(int maxNodes, float maxTime, int maxDepth) : base(maxNodes, maxTime, maxDepth){
            episilon = 5;
			open = new Queue();
			closed = new List<TNode>();
			visitedNodeRecord = new List<NodeRecord>();
			incons = new Queue();
        }

		public float fvalue(THeuristic h, TNode s){
			Debug.Log(episilon);
			
			return h.costFromStart(s) + h.estimateCost(s) * episilon;
		}

		float min(float a, float b){
			if(a > b) return b;
			return a;
		}

		public void ImprovePath(TGraph graph, TNode end, THeuristic heuristic){

			NodeRecord current;
			

			while (open.getLowestCostNode() != null && open.getLowestCostNode().node != end)
			{
				current = open.getLowestCostNode();
				open.Remove(current);
				closed.Add(current.node);


				foreach (var item in graph.getConnections(current.node).connections)
				{
					if(closed.Contains(item.toNode)){}
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
			NodeRecord nr = new NodeRecord(start);
			nr.costSoFar = 0;
			NodeRecord nrEnd = new NodeRecord(end);
			nrEnd.costSoFar = float.NaN;

			List<TNode> path;
			visitedNodes.Add(start);
			visitedNodeRecord.Add(nr);

			open.Add(nr, fvalue(heuristic, start));
			ImprovePath(graph, end, heuristic);
			
			float m1, m2;

			if(open.Count() == 0) m1 = 10000000;
			else m1 = open.getLowestCostNode().costSoFar + heuristic.estimateCost(open.getLowestCostNode().node);
			if(incons.Count() == 0) m2 = 10000000;
			else m2 = incons.getLowestCostNode().costSoFar + heuristic.estimateCost(incons.getLowestCostNode().node);
			if(open.Count() > 0 && incons.Count() > 0){
				episilon = min(episilon, nrEnd.costSoFar/min(m1, m2));
			}
			path = getpath();				
			 while (episilon > 1)
			{	
				episilon-=0.5f;
				for(int i = 0; i < incons.Count(); i++){
					open.Add(incons.getItemAt(i), fvalue(heuristic, incons.getItemAt(i).node));
					incons.Remove(incons.getItemAt(i).node);
				}
				
				closed.Clear();
				ImprovePath(graph, end, heuristic);
				
				if(open.Count() == 0) m1 = 1000;
				else m1 = open.getLowestCostNode().costSoFar + heuristic.estimateCost(open.getLowestCostNode().node);
				if(incons.Count() == 0) m2 = 1000;
				else m2 = incons.getLowestCostNode().costSoFar + heuristic.estimateCost(incons.getLowestCostNode().node);
				if(open.Count() > 0 && incons.Count() > 0){
					episilon = min(episilon, nrEnd.costSoFar/min(m1, m2));
				}
				path = getpath();
			}

			for (int i = 0; i < path.Count(); i++)
			{
				Debug.Log(path[i].id);
			}
			// List<TNode> aux = new List<TNode>();
			return path;	
        }

    }


}