using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EstructurasEstaticas
{
    public partial class FormPrincipal : Form
    {
        private PilaEstatica<int> pila = new(6);
        private ColaSimple<int> colaSimple = new(6);
        private ColaCircular<int> colaCircular = new(6);

        public FormPrincipal()
        {
            Text = "Tarea 3.1 - Pilas y colas con memoria estática";
            Size = new Size(1000, 620);
            MinimumSize = new Size(800, 520);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            var pestanas = new TabControl { Dock = DockStyle.Fill };
            pestanas.TabPages.Add(CrearPestanaPila());
            pestanas.TabPages.Add(CrearPestanaCola("Cola simple", circular: false));
            pestanas.TabPages.Add(CrearPestanaCola("Cola circular", circular: true));
            Controls.Add(pestanas);
        }

        // ---------------------------------------------------------------- Pila
        private TabPage CrearPestanaPila()
        {
            var (pagina, nudCapacidad, nudValor, barra, vista, bitacora) = CrearEstructuraBase("Pila (LIFO)");
            void Refrescar() => vista.Mostrar(pila.VistaInterna(), pila.IsEmpty() ? new() : new() { [pila.Tope] = $"top = {pila.Tope}" });

            AgregarBoton(barra, "Crear", () =>
            {
                pila = new PilaEstatica<int>((int)nudCapacidad.Value);
                return $"Se creó una pila de capacidad {pila.Capacidad}. top = -1";
            }, Refrescar, bitacora);
            AgregarBoton(barra, "Push", () =>
            {
                int v = (int)nudValor.Value;
                pila.Push(v);
                Avanzar(nudValor);
                return $"Push({v}) -> top = {pila.Tope}";
            }, Refrescar, bitacora);
            AgregarBoton(barra, "Pop", () => { int v = pila.Pop(); return $"Pop() = {v} -> top = {pila.Tope}"; }, Refrescar, bitacora);
            AgregarBoton(barra, "Peek", () => $"Peek() = {pila.Peek()}", Refrescar, bitacora);
            AgregarBoton(barra, "IsEmpty", () => $"IsEmpty() = {pila.IsEmpty()}", Refrescar, bitacora);
            AgregarBoton(barra, "IsFull", () => $"IsFull() = {pila.IsFull()}", Refrescar, bitacora);
            Refrescar();
            return pagina;
        }

        // ---------------------------------------------------------------- Colas
        private TabPage CrearPestanaCola(string titulo, bool circular)
        {
            var (pagina, nudCapacidad, nudValor, barra, vista, bitacora) = CrearEstructuraBase(titulo + " (FIFO)");

            void Refrescar()
            {
                var marcas = new Dictionary<int, string>();
                bool vacia = circular ? colaCircular.IsEmpty() : colaSimple.IsEmpty();
                int frente = circular ? colaCircular.Frente : colaSimple.Frente;
                int final = circular ? colaCircular.Final : colaSimple.Final;
                if (!vacia)
                {
                    marcas[frente] = "front";
                    marcas[final] = marcas.ContainsKey(final) ? "front/rear" : "rear";
                }
                vista.Mostrar(circular ? colaCircular.VistaInterna() : colaSimple.VistaInterna(), marcas);
            }

            string Punteros() => circular
                ? $"front = {colaCircular.Frente}, rear = {colaCircular.Final}, cantidad = {colaCircular.Cantidad}"
                : $"front = {colaSimple.Frente}, rear = {colaSimple.Final}";

            AgregarBoton(barra, "Crear", () =>
            {
                int c = (int)nudCapacidad.Value;
                if (circular) colaCircular = new ColaCircular<int>(c); else colaSimple = new ColaSimple<int>(c);
                return $"Se creó una {titulo.ToLower()} de capacidad {c}. {Punteros()}";
            }, Refrescar, bitacora);
            AgregarBoton(barra, "Enqueue", () =>
            {
                int v = (int)nudValor.Value;
                if (circular) colaCircular.Enqueue(v); else colaSimple.Enqueue(v);
                Avanzar(nudValor);
                return $"Enqueue({v}) -> {Punteros()}";
            }, Refrescar, bitacora);
            AgregarBoton(barra, "Dequeue", () =>
            {
                int v = circular ? colaCircular.Dequeue() : colaSimple.Dequeue();
                return $"Dequeue() = {v} -> {Punteros()}";
            }, Refrescar, bitacora);
            AgregarBoton(barra, "Peek", () => $"Peek() = {(circular ? colaCircular.Peek() : colaSimple.Peek())}", Refrescar, bitacora);
            AgregarBoton(barra, "IsEmpty", () => $"IsEmpty() = {(circular ? colaCircular.IsEmpty() : colaSimple.IsEmpty())}", Refrescar, bitacora);
            AgregarBoton(barra, "IsFull", () => $"IsFull() = {(circular ? colaCircular.IsFull() : colaSimple.IsFull())}", Refrescar, bitacora);
            Refrescar();
            return pagina;
        }

        // ---------------------------------------------------------------- Auxiliares
        /// <summary>Crea los controles comunes a las tres pestañas.</summary>
        private static (TabPage, NumericUpDown, NumericUpDown, FlowLayoutPanel, VistaArreglo, TextBox) CrearEstructuraBase(string titulo)
        {
            var nudCapacidad = new NumericUpDown { Minimum = 1, Maximum = 12, Value = 6, Width = 60 };
            var nudValor = new NumericUpDown { Minimum = 0, Maximum = 9999, Value = 10, Width = 70 };
            var barra = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            barra.Controls.AddRange(new Control[] { Etiqueta("Capacidad:"), nudCapacidad, Etiqueta("Valor:"), nudValor });

            var vista = new VistaArreglo { Dock = DockStyle.Fill };
            var bitacora = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10F), BackColor = Color.White };

            var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 170));
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tabla.Controls.Add(barra, 0, 0);
            tabla.Controls.Add(vista, 0, 1);
            tabla.Controls.Add(bitacora, 0, 2);

            var pagina = new TabPage(titulo) { Padding = new Padding(6) };
            pagina.Controls.Add(tabla);
            return (pagina, nudCapacidad, nudValor, barra, vista, bitacora);
        }

        /// <summary>Agrega un botón que ejecuta una operación, la registra y refresca la vista; los errores se muestran en la bitácora.</summary>
        private static void AgregarBoton(FlowLayoutPanel barra, string texto, Func<string> operacion, Action refrescar, TextBox bitacora)
        {
            var boton = new Button { Text = texto, AutoSize = true };
            boton.Click += (s, e) =>
            {
                try { bitacora.AppendText(operacion() + Environment.NewLine); }
                catch (InvalidOperationException ex) { bitacora.AppendText("ERROR: " + ex.Message + Environment.NewLine); }
                refrescar();
            };
            barra.Controls.Add(boton);
        }

        /// <summary>Sugiere el siguiente valor para insertar.</summary>
        private static void Avanzar(NumericUpDown n)
        {
            if (n.Value < n.Maximum) n.Value++;
        }

        private static Label Etiqueta(string texto) => new() { Text = texto, AutoSize = true, Margin = new Padding(3, 9, 3, 3) };
    }
}
