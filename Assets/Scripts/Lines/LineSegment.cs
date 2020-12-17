using UnityEngine;

//inspired by https://martin-thoma.com/how-to-check-if-two-line-segments-intersect/ and adapted to unity c# by myself
public struct LineSegment
{
    public Vector2 first;
    public Vector2 second;

    public float Length
    {
        get => Vector2.Distance(first, second);
    }

    public LineSegment(Vector2 first, Vector2 second)
    {
        this.first = first;
        this.second = second;
    }

    public bool DoesIntersectWith(LineSegment other)
    {
        Vector2[] box1 = GetBoundingBox();
        Vector2[] box2 = other.GetBoundingBox();
        return DoBoundingBoxesIntersect(box1, box2)
                && IsTouchedBy(other)
                && other.IsTouchedBy(this);
    }

    private bool DoBoundingBoxesIntersect(Vector2[] a, Vector2[] b)
    {
        return a[0].x <= b[1].x
            && a[1].x >= b[0].x
            && a[0].y <= b[1].y
            && a[1].y >= b[0].y;
    }

    private Vector2[] GetBoundingBox()
    {
        Vector2 xx = first.x < second.x ? new Vector2(first.x, second.x) : new Vector2(second.x, first.x);
        Vector2 yy = first.y < second.y ? new Vector2(first.y, second.y) : new Vector2(second.y, first.y);
        return new Vector2[] { new Vector2(xx.x, yy.x), new Vector2(xx.y, yy.y) };
    }

    private bool IsTouchedBy(LineSegment other)
    {
        return PointIsOn(other.first)
                || PointIsOn(other.second)
                || (PointIsRightOf(other.first) ^ PointIsRightOf(other.second));
    }

    private bool PointIsOn(Vector2 point)
    {
        // Move the image, so that a.first is on (0|0)
        LineSegment aTmp = new LineSegment(new Vector2(0, 0), new Vector2(second.x - first.x, second.y - first.y));
        Vector2 bTmp = new Vector2(point.x - first.x, point.y - first.y);
        float r = CrossProduct(aTmp.second, bTmp);
        return Mathf.Abs(r) < float.Epsilon;
    }

    private bool PointIsRightOf(Vector2 point)
    {
        // Move the image, so that a.first is on (0|0)
        LineSegment aTmp = new LineSegment(new Vector2(0, 0), new Vector2(
                second.x - first.x, second.y - first.y));
        Vector2 bTmp = new Vector2(point.x - first.x, point.y - first.y);
        return CrossProduct(aTmp.second, bTmp) < 0;
    }

    private float CrossProduct(Vector2 v1, Vector2 v2)
    {
        return (v1.x * v2.y) - (v1.y * v2.x);
    }
}
