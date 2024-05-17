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

    //Nodelar arasýnda taþa en yakýn konumu bul
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
        //Yukarýda bulunuyor ve artýk hedefe gidebilir

    }

    //Nodelar arasýnda suya en yakýn konumu bul
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
        //Yukarýda bulunuyor ve artýk hedefe gidebilir
    }

    //Nodelar arasýnda yemeðe en yakýn konumu bul
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
        //Yukarýda bulunuyor ve artýk hedefe gidebilir
    }


    //Belli bir konuma node çek
    void GoLocation(Vector2 target)
    {

        //Orada tam hedef noktada yeni bir node oluþturulana kadar döngü devam etsin
        Vector2 startLoc = transform.position;
        getNextNode(startLoc, target);
        
    }

    //Sýradaki node'u bul
    void getNextNode(Vector2 startLoc, Vector2 targetLoc)
    {
        // Hedef konum ile baþlangýç konumu arasýndaki farký hesapla
        Vector2 difference = targetLoc - startLoc;

        // Bu farkýn büyüklüðünü hesapla
        float distance = difference.magnitude;

        if(distance <= 5)
        {
            //Next node target loca instantiate ve çýk
            

        }
        else
        {
            //Next node target loc yönünde ama 5 birim uzaklýðýndaki konuma instantiate
            getNextNode(startLoc, targetLoc);
        }

    }

    //Düþman yakýnsa savun
    void CheckNodesForEnemies()
    {

    }

    //Kaynaklarýn yeterince iyiyse saldýr
    void CheckNodesForTarget()
    {

    }

}
