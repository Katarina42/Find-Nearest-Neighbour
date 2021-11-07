using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FindNearestNeighbour : MonoBehaviour
{
    #region Vars
    private Vector3[] linePositions = new Vector3[2];
    private Neighbour closest;
    private LineRenderer line;
    #endregion

    #region Unity
    private void OnEnable()
    {
        AddToTree(transform);
        line = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        UpdatePositions();
        closest = FindClosest(transform.position);
        if (closest == null)
            return;

        linePositions[0] = transform.position;
        linePositions[1] = closest.transform.position;
        line.SetPositions(linePositions);
    }
    #endregion

    #region 3D Tree
    private static Neighbour root;
    private static Neighbour last;
    private static int count;
    private static Neighbour[] open;

    private static void UpdatePositions()
    {
        var current = root;
        while (current != null)
        {
            current.oldRef = current.next;
            current = current.next;
        }

        current = root;

        root = null;
        last = null;
        count = 0;

        while (current != null)
        {
            if (!current.transform.gameObject.activeInHierarchy)
            {
                current = current.oldRef;
                continue;
            }   

            AddToTree(current.transform);
            current = current.oldRef;
        }
    }

    private static float Distance(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
    }

    private static float GetSplitValue(int level, Vector3 position)
    {
        return (level % 3 == 0) ? position.x : (level % 3 == 1) ? position.y : position.z;
    }

    private static void AddToTree(Transform transform)
    {
        Neighbour neighbour = new Neighbour();
        neighbour.transform = transform;
        neighbour.left = null;
        neighbour.right = null;
        neighbour.level = 0;
        count++;
        var parent = FindParent(neighbour.transform.position);

        if (last != null)
            last.next = neighbour;
        last = neighbour;

        if (parent == null)
        {
            root = neighbour;
            return;
        }

        var splitParent = GetSplitValue(parent.level,parent.transform.position);
        var splitNew = GetSplitValue(parent.level, neighbour.transform.position);

        neighbour.level = parent.level + 1;

        if (splitNew < splitParent)
            parent.left = neighbour; //go left
        else
            parent.right = neighbour; //go right
    }

    private static Neighbour FindParent(Vector3 position)
    {
        //travers from root to bottom and check every node
        var current = root;
        var parent = root;
        while (current != null)
        {
            var splitCurrent = GetSplitValue(current.level,current.transform.position);
            var splitSearch = GetSplitValue(current.level, position);

            parent = current;
            if (splitSearch < splitCurrent)
                current = current.left; //go left
            else
                current = current.right; //go right

        }
        return parent;
    }

    private static Neighbour FindClosest(Vector3 position)
    {
        if (root == null)
            return null;

        var nearestDist = float.MaxValue;
        Neighbour nearest = null;

        if (open == null || open.Length < count)
            open = new Neighbour[count];
        for (int i = 0; i < open.Length; i++)
            open[i] = null;

        var openAdd = 0;
        var openCur = 0;

        if (root != null)
            open[openAdd++] = root;

        while (openCur < open.Length && open[openCur] != null)
        {
            var current = open[openCur++];

            var nodeDist = Distance(position, current.transform.position);
            if (nodeDist < nearestDist && nodeDist != 0)
            {
                nearestDist = nodeDist;
                nearest = current;
            }

            var splitCurrent = GetSplitValue(current.level,current.transform.position);
            var splitSearch = GetSplitValue(current.level, position);

            if (splitSearch < splitCurrent)
            {
                if (current.left != null)
                    open[openAdd++] = current.left; //go left
                if (Mathf.Abs(splitCurrent - splitSearch) * Mathf.Abs(splitCurrent - splitSearch) < nearestDist && current.right != null)
                    open[openAdd++] = current.right; //go right
            }
            else
            {
                if (current.right != null)
                    open[openAdd++] = current.right; //go right
                if (Mathf.Abs(splitCurrent - splitSearch) * Mathf.Abs(splitCurrent - splitSearch) < nearestDist && current.left != null)
                    open[openAdd++] = current.left; //go left
            }
        }

        return nearest;
    }

    private class Neighbour
    {
        internal Transform transform;
        internal int level;
        internal Neighbour left;
        internal Neighbour right;
        internal Neighbour next;
        internal Neighbour oldRef;
    }
    #endregion
}

