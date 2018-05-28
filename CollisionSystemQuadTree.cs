﻿using UnityEngine;

namespace Peril.Physics
{
    public class CollisionSystemQuadTree : CollisionSystem
    {

        ///// Constructor /////

        public CollisionSystemQuadTree(QuadTree tree)
        {
            _quadTree = tree;
        }

        ///// Fields /////

        private QuadTree _quadTree;

        ///// Methods /////

        public override void DetectBodyVsBody()
        {
            for(int i = 0; i < bodyList.Count; i++)
            {
                if (bodyList[i].Sleeping)
                    continue;

                // todo: something better maybe?
                var maxDist = bodyList[i].CollisionShape.Extents.x;
                maxDist = Mathf.Max(maxDist, bodyList[i].CollisionShape.Extents.y);
                maxDist = Mathf.Max(maxDist, bodyList[i].CollisionShape.Extents.z);

                var ents = _quadTree.GetBodies(bodyList[i].CollisionShape.Center, maxDist);
                for(int j = 0; j < ents.Count; j++)
                {
                    if (bodyList[i].Sleeping
                        || !(ents[j] is ICollisionBody)
                        || ReferenceEquals(bodyList[i], ents[j]))
                    {
                        continue;
                    }

                    Test(bodyList[i], (ICollisionBody)ents[j]);
                }
            }
        }

        public override bool LineOfSight(Vector3 start, Vector3 end)
        {
            for (var i = 0; i < bodyList.Count; i++)
            {
                if (CollisionTest.SegmentIntersects(bodyList[i].CollisionShape, start, end))
                    return false;
            }
            return true;
        }

    }
}
