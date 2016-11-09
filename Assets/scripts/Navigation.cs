using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class Navigation
    {
        public static RaycastHit2D[] arcTrace(Vector2 start, Vector2 velocity, Vector2 acceleration, float time, int steps, String layer)
        {
            RaycastHit2D[] results = new RaycastHit2D[steps];
            int layerMask = ~LayerMask.NameToLayer(layer);

            Vector2 lastPos = start;
            float stepSize = time / steps;
            for (int i = 1; i <= steps; i++)
            {
                float t = stepSize * i;
                Vector2 nextPos = start + velocity * t + acceleration * t * t / 2;
                Debug.DrawLine(lastPos, nextPos, Color.white, 10, false);
                RaycastHit2D hit = Physics2D.Linecast(lastPos, nextPos, layerMask);
                results[i - 1] = hit;
                lastPos = nextPos;
            }
            return results;
        }
    }
}
