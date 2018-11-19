using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    Vector3 position;
    int arrayX;
    int arrayY;
    GameObject tileObject;

    public bool isWalkable;
    public bool isChecked;
    public bool isSelected;
    public bool isEnemy;
    public bool isPlayer;
    public bool isObstacle;
    public bool isAttackable;
    public static Color tileColor;
    public static bool movable;
    public Sprite borderFull;
    public Sprite borderEmpty;
    public Sprite borderFullBattle;

    private void OnMouseOver()
    {
        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && GameManager.currentState == GameManager.States.MOVE
            && !isEnemy && !isObstacle && isSelected && !isAttackable)
        {
            SetImageSprite(borderFull);
        }

        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && ((isSelected && isEnemy) || (isSelected && isAttackable))
            && GameManager.currentState == GameManager.States.FIGHT)
        {
            SetImageSprite(borderFullBattle);
        }
    }

    private void OnMouseExit()
    {
        
        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && GameManager.currentState == GameManager.States.MOVE
            && !isEnemy && !isObstacle && isSelected && !isAttackable)
        {
            SetImageSprite(borderEmpty);
        }
        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && ((isSelected && isEnemy) || (isSelected && isAttackable))
            && GameManager.currentState == GameManager.States.FIGHT)
        {
            SetImageSprite(borderEmpty);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().sprite = borderEmpty;
        if (!EnemyController.isMovable && (!TurnManager.currentObjectTurn || (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy" && TurnManager.currentObjectTurn.name == "Dragon"))
            && collision.tag == "Tile" && !collision.GetComponent<Tile>().isChecked && !collision.GetComponent<Tile>().isAttackable)
        {
            collision.GetComponent<Tile>().isAttackable = true;
            EnemyController.tilesAttackable.Add(collision.gameObject);
        }
        else
        {
            if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy" && collision.tag == "Tile" && !collision.GetComponent<Tile>().isObstacle
            && !collision.GetComponent<Tile>().isChecked && (!collision.GetComponent<Tile>().isEnemy || (collision.GetComponent<Tile>().isWalkable && collision.GetComponent<Tile>().isPlayer)))
            {
                TileManager.tilesSelectable.Add(collision.gameObject);
                collision.gameObject.layer = LayerMask.NameToLayer("GridBattle");
                collision.GetComponent<Tile>().isSelected = true;
                StartCoroutine(WaitColor(collision));
            }
            else if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && collision.tag == "Tile" && !collision.GetComponent<Tile>().isObstacle
                && (!collision.GetComponent<Tile>().isChecked && (!collision.GetComponent<Tile>().isPlayer || (collision.GetComponent<Tile>().isWalkable && !collision.GetComponent<Tile>().isEnemy))
                || ((collision.GetComponent<Tile>().isEnemy && collision.GetComponent<Tile>().isAttackable))))
            {
                TileManager.tilesSelectable.Add(collision.gameObject);
                collision.gameObject.layer = LayerMask.NameToLayer("GridBattle");
                collision.GetComponent<Tile>().isSelected = true;
                StartCoroutine(WaitColor(collision));
            }
        }
    }

    public void ResetSpriteImage()
    {
        GetComponent<SpriteRenderer>().sprite = borderEmpty;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.4f);
    }

    public void SetImageSprite(Sprite image)
    {
        GetComponent<SpriteRenderer>().sprite = image;
        if(image.name == borderFull.name)
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.4f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.4f);
        }
    }

    IEnumerator WaitColor(Collider2D collision)
    {
        yield return new WaitForSeconds(1f);
        if(GameManager.currentState == GameManager.States.ABILITY)
        {
            collision.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 140f / 255f, 0f / 255f, 0.4f);
        } else
        {
            collision.GetComponent<SpriteRenderer>().color = new Color(tileColor.r, tileColor.g, tileColor.b, 0.4f);
        }
        
    }

    public int ArrayX
    {
        get
        {
            return arrayX;
        }

        set
        {
            arrayX = value;
        }
    }

    public int ArrayY
    {
        get
        {
            return arrayY;
        }

        set
        {
            arrayY = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public GameObject TileObject
    {
        get
        {
            return tileObject;
        }

        set
        {
            tileObject = value;
        }
    }
}
