using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum DIRECCION { Arriba, Derecha, Izquierda, Abajo }

public class Terreno :MonoBehaviour {

    public GameObject manzana;
    public GameObject prefab;
    public GameObject padre;

    public LineRenderer serpiente;

    public int x = 10, y = 10;

    public bool continuado = false;

    int puntuacion = 0;

    DIRECCION proximaDireccion = DIRECCION.Arriba;

    List<Vector3> direccion = new List<Vector3>() { new Vector3(0, 0), new Vector3(0, 1), new Vector3(0, 1) };
    Vector3 posActual;

    public float actualizacion = 0.5f;

    float tiempoActual = 0f;

    bool gameOver = false;

    SnakeIA snake;

    void Start() {
        for(int _x = 0; _x < x; _x++) {
            for(int _y = 0; _y < y; _y++) {
                GameObject _obj = Instantiate(prefab);
                _obj.transform.position = new Vector3(_x, _y, 2);

                _obj.transform.parent = padre.transform;
            }
        }

        snake = FindObjectOfType<SnakeIA>();
        if(snake == null) {
            Debug.Log("IAno encontrada");
            gameOver = true;
        }

        PonerManzana();

        posActual = direccion[1];
    }

    void Update() {
        if(gameOver)
            return;

        DIRECCION lastDirection = proximaDireccion;

        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            proximaDireccion = DIRECCION.Arriba;
        } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
            proximaDireccion = DIRECCION.Abajo;
        } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            proximaDireccion = DIRECCION.Derecha;
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            proximaDireccion = DIRECCION.Izquierda;
        }

        if(lastDirection != proximaDireccion) {
            if(posActual + Posicion(proximaDireccion) == direccion[direccion.Count - 3]) {
                proximaDireccion = lastDirection;
            }
        }


        tiempoActual += Time.deltaTime;
        if(tiempoActual > actualizacion) {
            tiempoActual = 0;

            Actualizacion();
        } else if(continuado) {
            serpiente.SetPosition(0, Vector3.Lerp(direccion[0], direccion[1], tiempoActual / actualizacion));
            serpiente.SetPosition(direccion.Count - 1, Vector3.Lerp(direccion[direccion.Count - 1], direccion[direccion.Count - 1] + Posicion(proximaDireccion), tiempoActual / actualizacion));
        }
    }

     DIRECCION nextDirection() {
        Vector3 _pos = posActual;
        _pos.x++;
        _pos.y++;
        double[] inputs = new double[24] {
            Pos(y-_pos.y),
            Pos(x-_pos.x),
            Pos(y),
            Pos(x),

        };
    }

    double Pos(double pos) {
        return pos / (x - 1);
    }

    double GetAppleNearX(double pos) {

    }
    

    void Actualizacion() {
        if(gameOver) {
            return;
        }

        posActual += Posicion(proximaDireccion);

        if(posActual == manzana.transform.position) {
            serpiente.positionCount = direccion.Count+1;
            puntuacion++;
            PonerManzana();
        } else {
            direccion.RemoveAt(0);
        }

        if (direccion.Contains (posActual) || posActual.x < 0 || posActual.x >= x || posActual.y < 0 || posActual.y >= y) {
            Debug.Log("Has perdido. Puntuacion: "+puntuacion);
            gameOver = true;

            serpiente.startColor = new Color(0.3f, 0.3f, 0.3f);
            serpiente.endColor = new Color (0.6f, 0.6f, 0.6f);
        }
        
        direccion.Add(posActual);
        direccion[direccion.Count - 2] = posActual;

        serpiente.SetPositions(direccion.ToArray());

        posActual = direccion[direccion.Count - 1];
    }

    void PonerManzana () {
        Vector3 pos;
        bool sePuede = false;
        int ite = 0;

        while (!sePuede) {
            pos = new Vector3(Random.Range(0, x), Random.Range(0, y));

            if (!direccion.Contains(pos)) {
                sePuede = true;

                manzana.transform.position = pos;
            }

            ite++;
            if (ite > 100) {
                Debug.LogWarning("Se ha sobrepasado la cantidad de posibilidades para poner la manzana.");
                break;
            }
        } 

    }

    Vector3 Posicion (DIRECCION _direccion) {
        switch(_direccion) {
            case DIRECCION.Arriba:
                return Vector3.up;
            case DIRECCION.Abajo:
                return Vector3.down;
            case DIRECCION.Derecha:
                return Vector3.right;
            case DIRECCION.Izquierda:
                return Vector3.left;
        }

        return Vector3.zero;
    }

    public void Reiniciar () {
        SceneManager.LoadScene("Scene");
    }
}
