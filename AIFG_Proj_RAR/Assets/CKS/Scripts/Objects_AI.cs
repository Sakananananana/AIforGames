using UnityEngine;

public class Objects_AI
{
    public static float CalculateDistance(Vector3 firstPos, Vector3 secondPos)
    {
        float Distance = (firstPos - secondPos).sqrMagnitude;
        return Distance;
    }
    public static Vector3 CalculateDir(Vector3 targetPos, Vector3 currentPos)
    {
        ///Calcute direction
        Vector3 direction = targetPos - currentPos;
        direction.Normalize();
        return direction;
    }

    public static Vector3 CalRotateAmount(Vector3 direction, Vector3 rightAxis)
    {
        Vector3 rotateAmount = Vector3.Cross(-direction, rightAxis);
        return rotateAmount;
    }

    public static Vector3 AimTarget(Rigidbody p1_Rb, Vector3 rotateAmount, float rotateSpeed)
    {
        p1_Rb.angularVelocity = new Vector3(0, rotateAmount.y * rotateSpeed, 0);
        return p1_Rb.angularVelocity;
    }
}
