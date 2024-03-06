using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionHandleExample : MonoBehaviour
{
    private PolygonCollider2D[] colliders;
    private PolygonCollider2D polyCollider;
    private PolygonCollider2D triggerCollider;
    public float xOffset = 0.03f;
    private SpriteRenderer spriteRenderer;

    public virtual void UpdateColliderVertices()
    {
        colliders = GetComponents<PolygonCollider2D>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();

        if (colliders.Length == 1)
        {
            if (!triggerCollider) triggerCollider = colliders[0];
            triggerCollider.isTrigger = true;
        }
        else if (colliders.Length > 1)
        {
            if (!polyCollider) polyCollider = colliders[0];
            if (!triggerCollider) triggerCollider = colliders[1];

            triggerCollider.isTrigger = true;
            float spriteWidth = spriteRenderer.bounds.size.x;
            float spriteHeight = spriteRenderer.bounds.size.y;

            Vector2 bottom = new Vector2(xOffset, 0f);

            float leftY = -0.5f * (0f - bottom.x) + bottom.y;
            Vector2 left = new Vector2(0f, leftY);
            float rightY = 0.5f * (spriteWidth - bottom.x) + bottom.y;
            Vector2 right = new Vector2(spriteWidth, rightY);
            float topX = (-0.5f * right.x - 0.5f * left.x + left.y - right.y) / (-0.5f - 0.5f);
            float topY = -0.5f * (topX - right.x) + right.y;
            Vector2 top = new Vector2(topX, topY);

            // Remove any existing paths
            polyCollider.pathCount = 0;
            triggerCollider.pathCount = 0;

            // Set new paths
            polyCollider.SetPath(0, new Vector2[] { bottom, left, top, right });

            float overlapOffset = 0.05f;
            Vector2 triggerBottom = new Vector2(bottom.x, bottom.y + overlapOffset);
            Vector2 triggerTop = new Vector2(top.x, spriteHeight + overlapOffset * 2);
            float triggerLeftX = (-0.5f * triggerTop.x - 0.5f * triggerBottom.x + triggerBottom.y - triggerTop.y) / (-0.5f - 0.5f);
            float triggerLeftY = -0.5f * (triggerLeftX - triggerTop.x) + triggerTop.y;
            Vector2 triggerLeft = new Vector2(triggerLeftX, triggerLeftY);
            float triggerRightX = (0.5f * triggerTop.x - -0.5f * triggerBottom.x + triggerBottom.y - triggerTop.y) / (0.5f + 0.5f);
            float triggerRightY = 0.5f * (triggerRightX - triggerTop.x) + triggerTop.y;
            Vector2 triggerRight = new Vector2(triggerRightX, triggerRightY);
            triggerCollider.SetPath(0, new Vector2[] { triggerBottom, triggerLeft, triggerTop, triggerRight });
        }

        SpriteRenderer[] items = FindObjectsOfType<SpriteRenderer>();

        // Filter out items with the "Exempt" tag
        items = System.Array.FindAll(items, item => !item.CompareTag("Exempt") && !item.CompareTag("Furniture") && !item.CompareTag("section"));

        // Sort the remaining items based on their Y position
        System.Array.Sort(items, (item1, item2) =>
        {
            if (item1.transform.position.y < item2.transform.position.y)
                return 1;
            else if (item1.transform.position.y > item2.transform.position.y)
                return -1;
            else
                return 0;
        });
        // Set the sorting order based on their sorted order
        for (int i = 0; i < items.Length; i++)
        {
            items[i].sortingOrder = i * 2;
        }
    }
}
