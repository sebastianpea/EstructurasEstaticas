namespace EstructurasEstaticas;

/// <summary>Dibuja un arreglo como fila de celdas con su índice y marca los punteros (top, front, rear).</summary>
public class VistaArreglo : Control
{
    private string[] celdas = Array.Empty<string>();
    private Dictionary<int, string> punteros = new();

    public VistaArreglo()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.WhiteSmoke;
        Height = 170;
    }

    public void Mostrar(string[] contenido, Dictionary<int, string> marcas)
    {
        celdas = contenido;
        punteros = marcas;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (celdas.Length == 0) return;

        var g = e.Graphics;
        int ancho = Math.Min(70, (ClientSize.Width - 20) / celdas.Length), alto = 50, y = 45;
        using var fuente = new Font("Segoe UI", 10F, FontStyle.Bold);
        using var chica = new Font("Segoe UI", 8.5F);
        using var ocupada = new SolidBrush(Color.FromArgb(180, 215, 255));
        using var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        for (int i = 0; i < celdas.Length; i++)
        {
            var r = new Rectangle(10 + i * ancho, y, ancho - 4, alto);
            g.FillRectangle(celdas[i] == "" ? Brushes.White : ocupada, r);
            g.DrawRectangle(Pens.DimGray, r);
            g.DrawString(celdas[i], fuente, Brushes.Black, r, formato);
            g.DrawString($"[{i}]", chica, Brushes.Gray, new Rectangle(r.X, r.Bottom + 2, r.Width, 16), formato);

            if (punteros.TryGetValue(i, out string? marca))
                g.DrawString("▼ " + marca, chica, Brushes.Crimson, new Rectangle(r.X - 20, 5, r.Width + 40, 36), formato);
        }
    }
}