using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    Node[] allyNodes;
    Node[] targetNodes;

    enum State
    {
        SearchingStone,
        SearchingWater,
        SearchingFood,

        MovingToLocation,
        Defending,
        Attacking
    }

    Vector3 target;


    State currentState;

    void updateState()
    {
        switch (currentState)
        {
            case State.SearchingStone:
                CheckNodesForStone();
                break;
            case State.SearchingWater:
                CheckNodesForWater();
                break;
            case State.SearchingFood:
                CheckNodesForWater();
                break;
            case State.MovingToLocation:
                GoLocation(target);
                break;
            case State.Defending:
                CheckNodesForEnemies();
                break;
            case State.Attacking:
                CheckNodesForTarget();
                break;
        }
    }

    //Nodelar aras�nda ta�a en yak�n konumu bul
    void CheckNodesForStone()
    {
        Vector2 currentGoal;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (Node node in allyNodes)
        {

            if(currentClosestGoalDistance > node.closestStoneDistance)
            {
                currentClosestGoalDistance = node.closestStoneDistance;
                currentGoal = node.closestStone;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir

    }

    //Nodelar aras�nda suya en yak�n konumu bul
    void CheckNodesForWater()
    {
        Vector2 currentGoal;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (Node node in allyNodes)
        {

            if (currentClosestGoalDistance > node.closestWaterDistance)
            {
                currentClosestGoalDistance = node.closestWaterDistance;
                currentGoal = node.closestWater;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
    }

    //Nodelar aras�nda yeme�e en yak�n konumu bul
    void CheckNodesForFood()
    {
        Vector2 currentGoal;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (Node node in allyNodes)
        {

            if (currentClosestGoalDistance > node.closestFoodDistance)
            {
                currentClosestGoalDistance = node.closestFoodDistance;
                currentGoal = node.closestFood;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
    }


    //Belli bir konuma node �ek
    void GoLocation(Vector2 target)
    {

        //Orada tam hedef noktada yeni bir node olu�turulana kadar d�ng� devam etsin
        Vector2 startLoc = transform.position;
        getNextNode(startLoc, target);
        
    }

    //S�radaki node'u bul
    void getNextNode(Vector2 startLoc, Vector2 targetLoc)
    {
        // Hedef konum ile ba�lang�� konumu aras�ndaki fark� hesapla
        Vector2 difference = targetLoc - startLoc;

        // Bu fark�n b�y�kl���n� hesapla
        float distance = difference.magnitude;

        if(distance <= 5)
        {
            //Next node target loca instantiate ve ��k
            

        }
        else
        {
            //Next node target loc y�n�nde ama 5 birim uzakl���ndaki konuma instantiate
            getNextNode(startLoc, targetLoc);
        }

    }

    //D��man yak�nsa savun
    void CheckNodesForEnemies()
    {

    }

    //Kaynaklar�n yeterince iyiyse sald�r
    void CheckNodesForTarget()
    {

    }

}
