using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FightManager : MonoBehaviour
{
    public class Enemy
    {
        public string name;
        public int hp;
        public string color = "#FF5555";
        public bool isBoss = false;
    }
    public PlayerManager uiManager;

    private int roomCount = 0;
    private bool isBossRoom = false;
    private bool inCombat = false;
    private int currentEnemyHP;

    public void OnNextButtonClicked()
    {
        if (inCombat)
        {
            HandleCombat();
        }
        else
        {
            roomCount++;
            if (roomCount <= 4)
            {
                EnterNormalRoom();
            }
            else
            {
                EnterBossRoom();
            }
        }
    }

    void EnterNormalRoom()
    {
        string[] rooms = { "une cave sombre", "une forêt dense", "un couloir humide", "une vieille salle de torture" };
        string roomName = rooms[Random.Range(0, rooms.Length)];

        uiManager.AddLog($"--- Salle {roomCount}/4 ---", "#FFFF00");
        uiManager.AddLog($"Vous entrez dans {roomName}", "white");

        if (Random.value > 0.3f)
        {
            StartCombat("un Gobelin", 20);
        }
        else
        {
            uiManager.AddLog("La salle semble vide... pour l'instant.", "#AAAAAA");
        }
    }

    void EnterBossRoom()
    {
        uiManager.AddLog("!!! ZONE DE BOSS !!!", "orange");
        StartCombat("Le Chevalier Noir", 100);
        isBossRoom = true;
    }

    void StartCombat(string name, int hp)
    {
        inCombat = true;
        currentEnemyHP = hp;
        uiManager.AddLog($"<color=red>{name}</color> apparaît ! (HP: {hp})", "white");
    }

    void HandleCombat()
    {
       
        int damage = Random.Range(5, 15);
        currentEnemyHP -= damage;

        uiManager.AddLog($"Le héros frappe ! <color=orange>-{damage} HP</color>.", "white");

        if (currentEnemyHP <= 0)
        {
            uiManager.AddLog("L'ennemi est vaincu !", "#00FF00");
            inCombat = false;

            if (isBossRoom)
            {
                uiManager.AddLog("Félicitations ! Le donjon est terminé !", "yellow");
                roomCount = 0;
                isBossRoom = false;
            }
        }
        else
        {
            uiManager.AddLog($"L'ennemi perd du sang (HP restant: {currentEnemyHP})", "#FF7777");
        }
    }
}
