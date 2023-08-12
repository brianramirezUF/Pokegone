using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleDialogBox : MonoBehaviour
{

    [SerializeField] int lettersPerSecond;
    [SerializeField] TMP_Text dialogText;
    public string enemyName;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<TMP_Text> actionTexts;
    [SerializeField] List<TMP_Text> moveTexts;

    public TMP_Text ppText;
    public TMP_Text typeText;

    public BattleState state;
    int currentAction;

    public BattleHud player, enemy;

    public GameObject BattleCamera, MainCamera, Hitbox;
    public Text ingameHP;
    public PlayerMovement ingamePlayer;
    public PlayerHealth ingameHealth;

    public GameObject rematch;
    public GameObject won;

    public GameObject blockade;

    private float multiplier;

    public AudioSource dialogueSFX;
    public AudioSource bossMusic;

    bool ultimateUsed = false;

    public void onStart()
    {
        player.hp = ingameHealth.currentHealth;
        player.setHP();
        if(enemy.name == "Heitzmanberg")
        {
            bossMusic.Play();
        }
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        
    }


    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        SetMoveNames(player.moveNames);
        dialogueSFX.Play();
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        dialogueSFX.Stop();
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public IEnumerator SetupBattle()
    {
        yield return TypeDialog($"{enemyName} has initiated a battle!");
        yield return new WaitForSeconds(1f);
        multiplier = 1f;
        ultimateUsed = false;

        PlayerAction();
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(TypeDialog("Choose an action"));
        EnableActionSelector(true);
    }

    public void PlayerMove()
    {
        state = BattleState.PlayerMove;
        EnableActionSelector(false);
        EnableDialogText(false);
        EnableMoveSelector(true);
    }

    public void PerformPlayerMove(string moveName, int dmg)
    {
        StartCoroutine(PerformPlayerMoveEnum(moveName, dmg));
    }

    IEnumerator PerformPlayerMoveEnum(string moveName, int dmg)
    {
        state = BattleState.Busy;
        yield return TypeDialog($"{player.name} used {moveName}");
        yield return new WaitForSeconds(1f);

        bool isFainted = dealDamage(dmg);
        yield return enemy.UpdateHP();
        if(moveName == "Ahh A Wire")
        {
            player.hp += Mathf.FloorToInt(dmg*0.5f);
            if (player.hp > player.maxhp)
            {
                player.hp = player.maxhp;
            }
            yield return player.UpdateHP();
        }

        if (isFainted)
        {
            yield return TypeDialog($"{enemy.name} Fainted");
            yield return new WaitForSeconds(1f);
            if (enemy.name.Equals("Mr. White"))
            {
                blockade.SetActive(false);
                yield return TypeDialog($"Pinkman has leveled up to level 3!");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"Pinkman has acquired a new move: Yeah, Science!");
                yield return new WaitForSeconds(2f);
            }
            if (enemy.name.Equals("Saul Goodman"))
            {
                blockade.SetActive(false);
                yield return TypeDialog($"Pinkman has leveled up to level 6!");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"Pinkman has acquired a new move: Ahh A Wire");
                yield return new WaitForSeconds(2f);
            }
            if (enemy.name.Equals("Gus Fring"))
            {
                blockade.SetActive(false);
                yield return TypeDialog($"Pinkman has leveled up to level 8!");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"Pinkman has acquired a new move: Yeah B!");
                yield return new WaitForSeconds(2f);
            }
            if (enemy.name.Equals("Heitzmanberg"))
            {
                blockade.SetActive(false);
                yield return TypeDialog($"Pinkman has leveled up to level 11!");
                yield return new WaitForSeconds(1f);
            }
            BattleCamera.SetActive(false);
            MainCamera.SetActive(true);
            Hitbox.SetActive(false);
            ingameHP.text = $"{player.hp}\n---\n{player.maxhp}";
            ingamePlayer.DisablePlayer(false);
            won.SetActive(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public void SetMoveNames(List<string> moves)
    {
        for(int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i];
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

    public bool dealDamage(int dmg)
    {
        //float a = (2 * player.level + 10) / 250f;
        //float d = a * dmg + 2;

        float modifiers = Random.Range(0.85f, 1f);
        
        int damage = Mathf.FloorToInt(dmg);

        enemy.hp -= damage;
        if (enemy.hp <= 0)
        {
            enemy.hp = 0;
            return true;
        }

        return false;
    }

    public bool takeDamage(int dmg)
    {
        //float a = (2 * player.level + 10) / 250f;
        //float d = a * dmg + 2;

        float modifiers = Random.Range(0.85f, 1f);
        
        int damage = Mathf.FloorToInt(dmg);

        player.hp -= damage;
        if (player.hp <= 0)
        {
            player.hp = 0;
            return true;
        }

        return false;
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        int r = Random.Range(0, 4);
        int rH = Random.Range(1, 4);
        bool isFainted = false;
        bool selfFainted = false;
        if(enemy.name != "Heitzmanberg")
        {
            yield return TypeDialog($"{enemy.name} used {enemy.moveNames[r]}");
            yield return new WaitForSeconds(1f);
        }

        if(enemy.name.Equals("Mr. White"))
        {

            if (r == 0)
            {
                yield return TypeDialog($"{enemy.name} gives a mean glance to Pinkman and insults his intelligence.. its not very effective");
                isFainted = takeDamage(Mathf.FloorToInt(8 * multiplier));
            }
            else if (r == 1)
            {
                yield return TypeDialog($"{enemy.name} exclaims the need to cook, his attack stat is raised");
                multiplier += 0.25f;
            }
            else if(r == 2)
            {
                yield return TypeDialog($"{enemy.name} coughs harshly, hurting himself.. Pinkman takes pity damage");
                selfFainted = dealDamage(Mathf.FloorToInt(10*multiplier));
                isFainted = takeDamage(Mathf.FloorToInt(15 * multiplier));
                yield return enemy.UpdateHP();
            }
            else if(r == 3)
            {
                yield return TypeDialog($"{enemy.name} reveals that his Cancer has returned.. its super effective");
                isFainted = takeDamage(Mathf.FloorToInt(25 * multiplier));
            }
            yield return new WaitForSeconds(1f);
        }

        if (enemy.name.Equals("Saul Goodman"))
        {

            if (r == 0)
            {
                yield return TypeDialog($"{enemy.name} pulls off the ol' switchero, tricking Pinkman into taking serious legal damage..");
                isFainted = takeDamage(Mathf.FloorToInt(15 * multiplier));
            }
            else if (r == 1)
            {
                yield return TypeDialog($"{enemy.name} says his name in a punny way: Its all good man! Healing back a portion of his health");
                enemy.hp += Mathf.FloorToInt(enemy.maxhp * 0.25f);
                if(enemy.hp > enemy.maxhp)
                {
                    enemy.hp = enemy.maxhp;
                }
                yield return enemy.UpdateHP();
            }
            else if (r == 2)
            {
                yield return TypeDialog($"{enemy.name} hands you his card, Better Call Saul! Pinkman's wallet takes a fat hit..");
                isFainted = takeDamage(Mathf.FloorToInt(25 * multiplier));
            }
            else if (r == 3)
            {
                yield return TypeDialog($"{enemy.name} winks at Pinkman, referencing the five knuckle shuffle.. its not very effective");
                isFainted = takeDamage(Mathf.FloorToInt(8 * multiplier));
            }
            yield return new WaitForSeconds(1f);
        }

        if (enemy.name.Equals("Gus Fring"))
        {

            if (r == 0)
            {
                yield return TypeDialog($"{enemy.name} simply glances at Pinkman while the air fills with silence..");
                isFainted = takeDamage(Mathf.FloorToInt(25 * multiplier));
            }
            else if (r == 1)
            {
                yield return TypeDialog($"{enemy.name} shows his true colors, his attacks stat is raised");
                multiplier += 0.25f;
            }
            else if (r == 2)
            {
                yield return TypeDialog($"{enemy.name} cooks Pinkman a seemingly harmless family dinner, its not very effective");
                isFainted = takeDamage(Mathf.FloorToInt(18 * multiplier));
            }
            else if (r == 3)
            {
                yield return TypeDialog($"{enemy.name} fixes his collar and straightens his tie, healing himself and damaging Pinkman");
                enemy.hp += Mathf.FloorToInt(enemy.maxhp * 0.10f);
                if (enemy.hp > enemy.maxhp)
                {
                    enemy.hp = enemy.maxhp;
                }
                yield return enemy.UpdateHP();
                isFainted = takeDamage(Mathf.FloorToInt(10 * multiplier));
            }
            yield return new WaitForSeconds(1f);
        }

        if (enemy.name.Equals("Heitzmanberg"))
        {
            //r = Random.Range(1, 4);

            if (enemy.hp <= 100 && player.hp >= 50 && !ultimateUsed)
            {
                ultimateUsed = true;
                yield return TypeDialog($"{enemy.name} hacked into the Unity files to swap health");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"DO YOU THINK IM BOUND BY THE CONSTRAINTS OF YOUR CODE??");
                int temp = player.hp;
                player.hp = Mathf.FloorToInt((enemy.hp * 1f / enemy.maxhp * 1f)*player.maxhp);
                enemy.hp = Mathf.FloorToInt((temp * 1f / player.maxhp * 1f)*enemy.maxhp);
                yield return player.UpdateHP();
                yield return enemy.UpdateHP();
            }
            else if (rH == 1)
            {
                yield return TypeDialog($"{enemy.name} used {enemy.moveNames[rH]}");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"{enemy.name} takes out a soda and drinks it, healing himself");
                enemy.hp += Mathf.FloorToInt(enemy.maxhp * 0.2f);
                if (enemy.hp > enemy.maxhp)
                {
                    enemy.hp = enemy.maxhp;
                }
                yield return enemy.UpdateHP();
            }
            else if (rH == 2)
            {
                yield return TypeDialog($"{enemy.name} used {enemy.moveNames[rH]}");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"{enemy.name} looks for free assets and imports them for a damage buff");
                multiplier += 0.25f;
            }
            else if (rH == 3)
            {
                yield return TypeDialog($"{enemy.name} used {enemy.moveNames[rH]}");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"{enemy.name} raises the late penalty, lowering all your grades");
                isFainted = takeDamage(Mathf.FloorToInt(30 * multiplier));
            }
            yield return new WaitForSeconds(1f);
        }

        //bool isFainted = takeDamage(10);
        yield return player.UpdateHP();
        if (isFainted)
        {
            yield return TypeDialog($"{player.name} Fainted");
            yield return new WaitForSeconds(1f);
            BattleCamera.SetActive(false);
            MainCamera.SetActive(true);
            ingameHP.text = $"{player.hp}\n---\n{player.maxhp}";
            ingameHealth.currentHealth = player.hp;
            enemy.hp = enemy.maxhp;
            ingamePlayer.DisablePlayer(false);
            rematch.SetActive(true);
            enemy.setHP();
            if(enemy.name == "Heitzmanberg")
            {
                bossMusic.Stop();
            }
        }
        else if (selfFainted)
        {
            yield return TypeDialog($"{enemy.name} Fainted");
            yield return new WaitForSeconds(1f);
            if (enemy.name.Equals("Mr. White"))
            {
                blockade.SetActive(false);
                yield return TypeDialog($"Pinkman has leveled up to level 3!");
                yield return new WaitForSeconds(1f);
                yield return TypeDialog($"Pinkman has acquired a new move: Yeah, Science!");
                yield return new WaitForSeconds(2f);
            }
            BattleCamera.SetActive(false);
            MainCamera.SetActive(true);
            Hitbox.SetActive(false);
            ingameHP.text = $"{player.hp}\n---\n{player.maxhp}";
            ingameHealth.currentHealth = player.hp;
            ingamePlayer.DisablePlayer(false);
            won.SetActive(true);
        }
        else
        {
            PlayerAction();
        }
    }

    public void Run()
    {
        StartCoroutine(RunAway());
    }

    IEnumerator RunAway()
    {
        yield return TypeDialog($"{player.name} Ran Away!");
        yield return new WaitForSeconds(1f);
        BattleCamera.SetActive(false);
        MainCamera.SetActive(true);
        ingameHP.text = $"{player.hp}\n---\n{player.maxhp}";
        ingameHealth.currentHealth = player.hp;
        enemy.hp = enemy.maxhp;
        ingamePlayer.DisablePlayer(false);
        rematch.SetActive(true);
        enemy.setHP();
    }
}
