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
        public int maxHP;
        public int damage;
        public string color = "#FF5555";
        public bool isBoss = false;
    }

    [Header("References")]
    public PlayerManager playerManager;
    public PlayerStatsManager statsManager;
    public LootManager lootManager;

    [Header("UI")]
    public GameObject gameOverScreen;
    public Button useConsumableButton;

    private int roomCount = 0;
    private bool isBossRoom = false;
    private bool inCombat = false;

    private Enemy currentEnemy;

    void Start()
    {
        if (playerManager == null)
        {
            playerManager = FindFirstObjectByType<PlayerManager>();
        }

        if (statsManager == null)
        {
            statsManager = FindFirstObjectByType<PlayerStatsManager>();
        }

        // Configurer le bouton de consommable
        if (useConsumableButton != null)
        {
            useConsumableButton.onClick.AddListener(OnUseConsumableClicked);
        }
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

    public void OnUseConsumableClicked()
    {
        if (statsManager != null)
        {
            statsManager.UseConsumable();
        }
    }

    void EnterNormalRoom()
    {
        string[] rooms = { "une cave sombre", "une forêt dense", "un couloir humide", "une vieille salle de torture" };
        string roomName = rooms[Random.Range(0, rooms.Length)];

        playerManager.AddLog($"--- Salle {roomCount}/4 ---", "#FFFF00");
        playerManager.AddLog($"Vous entrez dans {roomName}", "white");

        if (Random.value > 0.3f)
        {
            // Ennemis plus forts au fil des salles
            int enemyHP = 15 + (roomCount * 5);
            int enemyDamage = 3 + (roomCount * 2);

            string[] enemyNames = { "Gobelin", "Squelette", "Loup Sauvage", "Bandit" };
            string enemyName = enemyNames[Random.Range(0, enemyNames.Length)];

            Enemy enemy = new Enemy
            {
                name = enemyName,
                hp = enemyHP,
                maxHP = enemyHP,
                damage = enemyDamage,
                color = "#FF5555",
                isBoss = false
            };
            StartCombat(enemy);
        }
        else
        {
            playerManager.AddLog("La salle semble vide... pour l'instant.", "#6c6060ff");
        }
    }

    void EnterBossRoom()
    {
        playerManager.AddLog("!!! ZONE DE BOSS !!!", "orange");

        Enemy chevalierNoir = new Enemy
        {
            name = "Chevalier Noir",
            hp = 80,
            maxHP = 80,
            damage = 15,
            color = "#FF0000",
            isBoss = true
        };

        StartCombat(chevalierNoir);
        isBossRoom = true;
    }

    void StartCombat(Enemy enemy)
    {
        inCombat = true;
        currentEnemy = enemy;
        playerManager.AddLog($"<color=red>{enemy.name}</color> apparaît ! (HP: {enemy.hp}/{enemy.maxHP})", "white");
    }

    void HandleCombat()
    {
        if (statsManager == null || playerManager == null) return;

        // Tour du joueur
        PlayerAttack();

        // Vérifier si l'ennemi est mort
        if (currentEnemy.hp <= 0)
        {
            playerManager.AddLog($"Le {currentEnemy.name} est vaincu !", "#00FF00");
            inCombat = false;

            if (isBossRoom)
            {
                playerManager.AddLog("Félicitations ! Le Boss est vaincu !", "yellow");
                roomCount = 0;
                isBossRoom = false;
            }

            // Drop de loot
            if (lootManager != null)
            {
                int lootCount = currentEnemy.isBoss ? Random.Range(2, 4) : 1;
                for (int i = 0; i < lootCount; i++)
                {
                    lootManager.GenerateLoot();
                }
            }
            return;
        }

        // Tour de l'ennemi
        EnemyAttack();

        // Vérifier si le joueur est mort
        if (playerManager.player.hp <= 0)
        {
            playerManager.AddLog("Notre héros est tombé au combat...", "#0950b3ff");
            inCombat = false;
            playerManager.player.isDead = true;

            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }
        }
    }

    void PlayerAttack()
    {
        // Calcul de la précision
        float accuracy = statsManager.GetTotalAccuracy();
        bool hitSuccess = Random.Range(0f, 100f) <= accuracy;

        if (!hitSuccess)
        {
            playerManager.AddLog("Le héros <color=yellow>rate</color> son attaque !", "white");
            return;
        }

        // Calcul des dégâts
        int baseDamage = statsManager.GetTotalDamage();

        // Vérifier coup critique
        float critChance = statsManager.GetTotalCritChance();
        bool isCrit = Random.Range(0f, 100f) <= critChance;

        int finalDamage = isCrit ? Mathf.RoundToInt(baseDamage * 1.5f) : baseDamage;

        currentEnemy.hp -= finalDamage;

        string critText = isCrit ? " <color=yellow>CRITIQUE!</color>" : "";
        playerManager.AddLog(
            $"Le héros frappe !{critText} <color=orange>-{finalDamage} HP</color> " +
            $"({currentEnemy.name}: {Mathf.Max(0, currentEnemy.hp)}/{currentEnemy.maxHP} HP)",
            "white"
        );

        // Réduire la durabilité de l'arme
        statsManager.DamageWeapon(1);
    }

    void EnemyAttack()
    {
        int rawDamage = currentEnemy.damage;

        // Réduction par l'armure
        int reducedDamage = statsManager.CalculateDamageReduction(rawDamage);

        playerManager.player.hp -= reducedDamage;

        string armorText = reducedDamage < rawDamage ?
            $" (Armure: <color=blue>-{rawDamage - reducedDamage}</color>)" : "";

        playerManager.AddLog(
            $"{currentEnemy.name} attaque ! <color=red>-{reducedDamage} HP</color>{armorText} " +
            $"(Héros: {Mathf.Max(0, playerManager.player.hp)}/{playerManager.player.maxHP} HP)",
            "white"
        );

        // Réduire la durabilité de l'armure
        statsManager.DamageArmor(1);

        // Dégâts d'épines (thorns)
        int thorns = statsManager.GetTotalThorns();
        if (thorns > 0)
        {
            currentEnemy.hp -= thorns;
            playerManager.AddLog(
                $"Les épines infligent <color=cyan>{thorns}</color> dégâts à {currentEnemy.name} !",
                "white"
            );
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}