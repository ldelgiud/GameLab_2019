using Microsoft.Xna.Framework;
using System.Collections;

public struct Line
{
    const float verticalLineGradient = 1e5f;

    float gradient;
    float y_intercept;
    Vector2 pointOnLine_1, pointOnLine_2;
    float gradientPerpendicular;
    bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 perpendicularToLine)
    {
        float dx = pointOnLine.X - perpendicularToLine.X;
        float dy = pointOnLine.Y - perpendicularToLine.Y;
        if (dx == 0) this.gradientPerpendicular = verticalLineGradient;
        else this.gradientPerpendicular = dy / dx;

        if (this.gradientPerpendicular == 0) gradient = verticalLineGradient;
        else this.gradient = -1 / this.gradientPerpendicular;

        this.y_intercept = pointOnLine.Y - gradient * pointOnLine.X;

        this.pointOnLine_1 = pointOnLine;
        this.pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

        this.approachSide = false;
        this.approachSide = this.GetSide(perpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        return ((p.X - this.pointOnLine_1.X) * (this.pointOnLine_2.Y - this.pointOnLine_1.Y))
             > ((p.Y - this.pointOnLine_1.Y) * (this.pointOnLine_2.X - this.pointOnLine_1.X)); 

    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }


}