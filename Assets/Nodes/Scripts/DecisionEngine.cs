using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    NodeAI[] allyNodes;
    NodeAI[] targetNodes;

    //Kaynaklar� �ekece�imiz sat�r

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
        foreach (NodeAI node in allyNodes)
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
        foreach (NodeAI node in allyNodes)
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
        Vector2 currentGoal = Vector2.zero;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (NodeAI node in allyNodes)
        {

            if (currentClosestGoalDistance > node.closestFoodDistance)
            {
                currentClosestGoalDistance = node.closestFoodDistance;
                currentGoal = node.closestFood;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
        GoLocation(currentGoal);
    }


    //Belli bir konuma node �ek
    void GoLocation(Vector2 target)
    {
        if(target != Vector2.zero)
        {
            Debug.Log("Target not reachable");
        }

        //Orada tam hedef noktada yeni bir node olu�turulana kadar d�ng� devam etsin
        Vector2 startLoc = transform.position;
        generateNextNode(startLoc, target);
        
    }

    //S�radaki node'u bul
    void generateNextNode(Vector2 startLoc, Vector2 targetLoc)
    {
        //Kaynak yeterliyse basacak
        //if()


        // Hedef konum ile ba�lang�� konumu aras�ndaki fark� hesapla
        Vector2 difference = targetLoc - startLoc;

        // Bu fark�n b�y�kl���n� hesapla
        float distance = difference.magnitude;

        if(distance <= 5f)
        {
            //Next node target loca instantiate ve ��k
            

        }
        else
        {


            // Fark vekt�r�n� normalle�tirerek birim vekt�r elde et
            Vector2 direction = difference.normalized;

            // Belirli bir mesafeye (�rne�in, 5 birim) �arp ve yeni noktay� hesapla
            Vector2 nextNode = startLoc + direction * 5f;

            //Next node target loc y�n�nde ama 5 birim uzakl���ndaki konuma instantiate
            generateNextNode(nextNode, targetLoc);
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
