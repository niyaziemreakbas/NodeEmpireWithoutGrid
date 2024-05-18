using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    List<NodeAI> allyNodes;
    List<NodeAI> targetNodes;

    bool attack = false;

    public int nodeCountUntilAttack;

    public GameObject Node;

    //Kaynaklarý çekeceðimiz satýr

    enum State
    {
        SearchingStone,
        SearchingWater,
        SearchingFood,

        MovingToLocation,
        Defending,
        Attacking
    }

    Vector3 tempTarget;


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
                GoLocation(tempTarget);
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
        foreach (NodeAI node in allyNodes)
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
        foreach (NodeAI node in allyNodes)
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
        //Yukarýda bulunuyor ve artýk hedefe gidebilir
        GoLocation(currentGoal);
    }



    //Belli bir konuma node çek
    void GoLocation(Vector2 target)
    {
        if(target != Vector2.zero)
        {
            Debug.Log("Target not reachable");
        }

        //Orada tam hedef noktada yeni bir node oluþturulana kadar döngü devam etsin
        Vector2 startLoc = transform.position;
        generateNextNode(startLoc, target);
        
    }

    //Sýradaki node'u bul
    void generateNextNode(Vector2 startLoc, Vector2 targetLoc)
    {
        //Kaynak yeterliyse basacak
        //if()


        // Hedef konum ile baþlangýç konumu arasýndaki farký hesapla
        Vector2 difference = targetLoc - startLoc;

        // Bu farkýn büyüklüðünü hesapla
        float distance = difference.magnitude;

        if(distance <= 5f)
        {
            //Next node target loca instantiate ve çýk

            //Instantiate Object at targetLoc

        }
        else
        {


            // Fark vektörünü normalleþtirerek birim vektör elde et
            Vector2 direction = difference.normalized;

            // Belirli bir mesafeye (örneðin, 5 birim) çarp ve yeni noktayý hesapla
            Vector2 nextNode = startLoc + direction * 5f;

            //Instantiate Object at nextNode
            createNewNode(nextNode);

            //Next node target loc yönünde ama 5 birim uzaklýðýndaki konuma instantiate
            generateNextNode(nextNode, targetLoc);
        }

    }

    void createNewNode(Vector2 location)
    {
        //convert v2 to v3
        Vector3 vector3 = new Vector3(location.x, location.y);
        Instantiate(Node, vector3, Quaternion.identity);
    }

    //Düþman yakýnsa savun
    void CheckNodesForEnemies()
    {

    }

    //Kaynaklarýn yeterince iyiyse saldýr
    void CheckNodesForTarget()
    {

    }

    void attackControl()
    {
        if(allyNodes.Count < nodeCountUntilAttack)
        {
            attack = true;
        }
    }

}
