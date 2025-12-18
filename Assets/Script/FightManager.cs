using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    public class Enemy
    {
        public string name;
        public int hp;
        public int damage;
        public string color = "#FF5555";
        public bool isBoss = false;
    }

    public PlayerManager uiManager;

    public GameObject gameOverScreen;

    public LootManager lootManager;

    private int roomCount = 0;
    private bool isBossRoom = false;
    private bool inCombat = false;

    private Enemy currentEnemy;
    private PlayerManager.Player player;

    void Start()
    {
        player = new PlayerManager.Player();
    }

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
            Enemy gobelin = new Enemy { name = "Gobelin", hp = 20, damage = 5, color = "#FF5555", isBoss = false };
            StartCombat(gobelin);
        }
        else
        {
            uiManager.AddLog("La salle semble vide... pour l'instant.", "#6c6060ff");
        }
    }

    void EnterBossRoom()
    {
        uiManager.AddLog("!!! ZONE DE BOSS !!!", "orange");

        Enemy chevalierNoir = new Enemy { name = "Chevalier Noir", hp = 50, damage = 10, color = "#FF5555", isBoss = true };

        StartCombat(chevalierNoir);
        isBossRoom = true;
    }

    void StartCombat(Enemy enemy)
    {
        inCombat = true;
        currentEnemy = enemy;
        uiManager.AddLog($"<color=red>{enemy.name}</color> apparaît ! (HP: {enemy.hp})", "white");
    }

    void HandleCombat()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            currentEnemy.hp -= player.damage;
            uiManager.AddLog($"Le héros frappe ! <color=orange>-{player.damage} HP</color> (Ennemi: {currentEnemy.hp} HP).", "white");
        }
        else
        {
            player.hp -= currentEnemy.damage;
            uiManager.AddLog($"{currentEnemy.name} attaque ! <color=red>-{currentEnemy.damage} HP</color> (Héros: {player.hp} HP).", "white");
        }

        if (currentEnemy.hp <= 0)
        {
            uiManager.AddLog($"Le {currentEnemy.name} est vaincu !", "#00FF00");
            inCombat = false;
            if (isBossRoom)
            {
                uiManager.AddLog("Félicitations ! Le Boss est vaincu !", "yellow");
                roomCount = 0; isBossRoom = false;
            }

            if (lootManager != null)
            {
                lootManager.GenerateLoot();
            }
        }
        else if (player.hp <= 0)
        {
            uiManager.AddLog("Notre héros est tombé au combat...", "#0950b3ff");
            inCombat = false;

            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
