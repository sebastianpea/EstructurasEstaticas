namespace EstructurasEstaticas;

/// <summary>
/// Cola (FIFO) simple sobre un arreglo fijo. "frente" señala el primer elemento y "final" el último.
/// Los índices solo avanzan: al llegar "final" al extremo del arreglo la cola se considera llena,
/// aunque haya espacios libres al inicio (falso desbordamiento).
/// </summary>
public class ColaSimple<T>
{
    private readonly T[] elementos;
    private int frente = 0;
    private int final = -1;

    public ColaSimple(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que cero.");
        elementos = new T[capacidad];
    }

    public int Capacidad => elementos.Length;
    public int Cantidad => final - frente + 1;
    public int Frente => frente;
    public int Final => final;

    public bool IsEmpty() => frente > final;
    public bool IsFull() => final == elementos.Length - 1;

    /// <summary>Inserta al final. Precondición: "final" no ha llegado al extremo del arreglo.</summary>
    public void Enqueue(T dato)
    {
        if (IsFull()) throw new InvalidOperationException("La cola está llena: 'final' llegó al extremo del arreglo.");
        final++;
        elementos[final] = dato;
    }

    /// <summary>Retira y devuelve el elemento del frente.</summary>
    public T Dequeue()
    {
        if (IsEmpty()) throw new InvalidOperationException("La cola está vacía.");
        T dato = elementos[frente];
        elementos[frente] = default!;
        frente++;
        return dato;
    }

    public T Peek()
    {
        if (IsEmpty()) throw new InvalidOperationException("La cola está vacía.");
        return elementos[frente];
    }

    public string[] VistaInterna()
    {
        var vista = new string[elementos.Length];
        for (int i = 0; i < vista.Length; i++) vista[i] = i >= frente && i <= final ? elementos[i]?.ToString() ?? "" : "";
        return vista;
    }
}