//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Globalization;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] public float speed = 5f;
    [SerializeField] public float jumpForce = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] float distanciaparedizquierda = 7.5f;
    [SerializeField] float playerwidth = 1f;
    [SerializeField] float distanciaparedderecha = 23.5f;
    public LayerMask ground;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Rigidbody2D rb;
    private bool isGrounded;
    private GameObject child;
    private Collider2D childCollider;    
    private float jumpTimeCounter;
    private float jumpTime;
    private bool isJumping;
 
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).gameObject; // Obtiene el primer hijo directamente
        childCollider = child.GetComponent<Collider2D>(); // Obtiene su Collider2D
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (childCollider == null) return;
        if (InputManager.Instance == null) return;

        //Obtener el vector de movimiento desde el InputManager.
        Vector2 moveInput = InputManager.Instance.MovementVector;
        Vector3 move = new Vector3(moveInput.x, 0, 0) * speed *Time.deltaTime;
        transform.position += move;

        if (gameObject.transform.position.x < -distanciaparedizquierda + playerwidth)
        {
            transform.position = new Vector3(-distanciaparedizquierda + playerwidth, transform.position.y, transform.position.z);
        }
        else if (gameObject.transform.position.x > distanciaparedderecha - playerwidth)
        {
            transform.position = new Vector3(distanciaparedderecha - playerwidth, transform.position.y, transform.position.z);
        }
        //Voltear el sprite
        float horizontalMovement = moveInput.x;
        if (horizontalMovement != 0)
        {
            spriteRenderer.flipX = horizontalMovement < 0;
        }
        //Detecta si el hijo está colisionando con algo
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(childCollider.bounds.center, childCollider.bounds.size , 0);

        isGrounded = hitColliders.Length > 1 ;

        if (isGrounded == true && InputManager.Instance.JumpWasPressedThisFrame())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        /*Vector2 currentPosition = transform.position;
        Collider2D wallCollider = Physics2D.OverlapBox(currentPosition, childCollider.bounds.size, 0, ground);
        if (wallCollider != null)
        {
            float distanceToWall = Vector2.Distance(currentPosition, wallCollider.ClosestPoint(currentPosition));
            if (distanceToWall < wallDistanceThreshold)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        */
        // if(Input.GetKey(KeyCode.Space) && isJumping == true)
        // {
        //     if (jumpTimeCounter > 0)
        //     {
        //          rb.velocity = Vector2.up * jumpForce;
        //          jumpTimeCounter +=Time.deltaTime;
        //     } 
        //     else
        //     {
        //         isJumping = false;
        //     }

        // }

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     isJumping = false;
        // }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    //como hipoteticamente podría hacer que el jugador pueda volver a saltar del suelo con un collider como hijo
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if (context.performed && isGrounded) // Solo salta si está en el suelo
        {

            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }
    }
    // public Vector 2 MovementVector {get; private set;)
    /*
     private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    /*
    void Update()
    {
      dashing = GetComponent<PlayerDash>().dash();
    
    MoveDirection = (Vector3)InputManager.Instance.MovementVector;

    if ((cD.GetCollisions()[0] && MoveDirection.y > 0) || cD.GetCollisions()[1] && MoveDirection.y < 0) MoveDirection.y = 0;

    MoveDirection = MoveDirection.Normalized

    if (MoveDirection != Vector3.zero)
    {
        LastDirection = MoveDirection;
    }
    if (!dashing) 
    rb.velocity = MoveDirection * MoveSpeed * Time.fixedDeltaTime; 



    }
    */
     

    #endregion   

} // class PlayerMovement 
// namespace
