namespace EstructurasEstaticas;

/// <summary>
/// Cola (FIFO) circular sobre un arreglo fijo. Los índices "frente" y "final" avanzan con
/// el operador módulo, de modo que los espacios liberados al inicio se reutilizan.
/// Un contador distingue la cola vacía de la cola llena.
/// </summary>
public class ColaCircular<T>
{
    private readonly T[] elementos;
    private int frente = 0;
    private int final = -1;
    private int cantidad = 0;

    public ColaCircular(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que cero.");
        elementos = new T[capacidad];
    }

    public int Capacidad => elementos.Length;
    public int Cantidad => cantidad;
    public int Frente => frente;
    public int Final => final;

    public bool IsEmpty() => cantidad == 0;
    public bool IsFull() => cantidad == elementos.Length;

    public void Enqueue(T dato)
    {
        if (IsFull()) throw new InvalidOperationException("La cola circular está llena.");
        final = (final + 1) % elementos.Length;   // avance circular
        elementos[final] = dato;
        cantidad++;
    }

    public T Dequeue()
    {
        if (IsEmpty()) throw new InvalidOperationException("La cola circular está vacía.");
        T dato = elementos[frente];
        elementos[frente] = default!;
        frente = (frente + 1) % elementos.Length;  // avance circular
        cantidad--;
        return dato;
    }

    public T Peek()
    {
        if (IsEmpty()) throw new InvalidOperationException("La cola circular está vacía.");
        return elementos[frente];
    }

    public string[] VistaInterna()
    {
        var vista = new string[elementos.Length];
        for (int i = 0; i < vista.Length; i++) vista[i] = "";
        for (int k = 0, i = frente; k < cantidad; k++, i = (i + 1) % elementos.Length)
            vista[i] = elementos[i]?.ToString() ?? "";
        return vista;
    }
}