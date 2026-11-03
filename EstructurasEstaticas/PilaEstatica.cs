namespace EstructurasEstaticas;

/// <summary>
/// Pila (LIFO) implementada desde cero sobre un arreglo de tamaño fijo.
/// El índice "tope" apunta al último elemento insertado; vale -1 cuando la pila está vacía.
/// </summary>
public class PilaEstatica<T>
{
    private readonly T[] elementos;
    private int tope = -1;

    public PilaEstatica(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que cero.");
        elementos = new T[capacidad];
    }

    public int Capacidad => elementos.Length;
    public int Cantidad => tope + 1;
    public int Tope => tope;

    public bool IsEmpty() => tope == -1;
    public bool IsFull() => tope == elementos.Length - 1;

    /// <summary>Inserta en la cima. Precondición: la pila no está llena.</summary>
    public void Push(T dato)
    {
        if (IsFull()) throw new InvalidOperationException("Desbordamiento: la pila está llena.");
        tope++;
        elementos[tope] = dato;
    }

    /// <summary>Retira y devuelve la cima. Precondición: la pila no está vacía.</summary>
    public T Pop()
    {
        if (IsEmpty()) throw new InvalidOperationException("Subdesbordamiento: la pila está vacía.");
        T dato = elementos[tope];
        elementos[tope] = default!;   // se libera la referencia
        tope--;
        return dato;
    }

    /// <summary>Devuelve la cima sin retirarla.</summary>
    public T Peek()
    {
        if (IsEmpty()) throw new InvalidOperationException("La pila está vacía.");
        return elementos[tope];
    }

    /// <summary>Copia del contenido por posición para mostrarlo ("" = posición libre).</summary>
    public string[] VistaInterna()
    {
        var vista = new string[elementos.Length];
        for (int i = 0; i < vista.Length; i++) vista[i] = i <= tope ? elementos[i]?.ToString() ?? "" : "";
        return vista;
    }
}