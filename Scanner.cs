///using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class Scanner : MonoBehaviour
//{
//    public float scanRange;
//    public LayerMask targetLayer;
//    public RaycastHit2D[] tarGets;
//    public Transform nearestTarget;

//    void FixedUpdate()
//    {
//        tarGets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0);
//        nearestTarget = Getnearest();
//    }

//    Transform Getnearest()
//    {
//        Transform result = null;
//        float diff = 100;

//        foreach (RaycastHit2D target in tarGets)
//        {
//            Vector3 myPos = transform.position;
//            Vector3 targetpos = target.transform.position;
//            float curdiff = Vector3.Distance(myPos, targetpos);

//            if (curdiff < diff)
//            {
//                diff = curdiff;
//                result = target.transform;
//            }
//        }
//        return result;
//    }
//}
