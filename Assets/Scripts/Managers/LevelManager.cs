//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject levelPlayerPos;
    [SerializeField] 
    private GrayZone _grayZone;
    [SerializeField]
    private float ChangeTimeTrasluz = 3.5f;//el tiempo que tarda en poner una imagen translúcida del siguiente estado
    // Variables del contador de tiempo de la sala
    public float RoomMaxTime = 10f;
    public float RoomTimeRemaining;
    // Variables del contador de tiempo del estado
    public float StateMaxTime = 4f;
    public float StateTime=0f;
    // Variables de cambios de estado (0-Estado Neutral, 1-Estado 1, 2-Estado 2
    public int State = 0;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    private PlatformMovement[] _platformMovement;
    private CambioEstado[] estados;//llama a los prefabs que pueden cambiar de estado


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }
    private void Start()
    {
        _platformMovement = FindObjectsByType<PlatformMovement>(FindObjectsSortMode.None);
        estados = FindObjectsOfType<CambioEstado>();//llama a todos los prefabs que contienen este script
    }

    
    private void Update()
    {
        if (!_grayZone.IsTimeStopped())
        {
            if (RoomTimeRemaining > 0)
            {
                RoomTimeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("TiempoFinalizado");
                ResetPlayer();
                RoomTimeRemaining = RoomMaxTime;
            }

        }
       
            StateTime += Time.deltaTime;
        
        if (StateTime > StateMaxTime)
        {
            for (int i = 0; i < estados.Length; i++)
            {
                estados[i].CambiaEstado();
            }
            StateTime = 0f;
        }

        else if (StateTime > ChangeTimeTrasluz && StateTime < StateMaxTime)
        {
            for (int i = 0; i < estados.Length; i++)
            {
                estados[i].CambiaEstadoTrasLuz();
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    public void ResetPlayer()
    {
        player.transform.position = levelPlayerPos.transform.position;
        
        for (int i = 0;i<_platformMovement.Length;i++)
        {
            _platformMovement[i].ResetPlatform();
        }

    }
    public void ChangeState(int state)
    {
        if (state == 0)
        {
            State = 2;
        } else if (state == 2)
        {
            State = 0;
        }
        //if (state == 2)
        //{
        //    State = 0;
        //}
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }


    #endregion
} // class LevelManager 
// namespace