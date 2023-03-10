using Para_inventario.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Para_inventario.Clases
{
    class ElementoDibujo
    {
        public int nro { get; set; }
        public string nombre { get; set; }
        public int cantidadDisponible { get; set; }
        public int cantidadComprada { get; set; }

        public ElementoDibujo() { }

        public ElementoDibujo(int nro, string nombre, int cantidadDisponible, int cantidadComprada)
        {
            this.nro = nro;
            this.nombre = nombre;
            this.cantidadDisponible = cantidadDisponible;
            this.cantidadComprada = cantidadComprada;
        }

        public void agregar(ElementoDibujo elementoDibujo)
        {
            ServicioElementoDibujo e = new ServicioElementoDibujo();
            e.agregar(elementoDibujo);  
        }

        public void mostrar(DataGridView elementosDibujo)
        {
            ServicioElementoDibujo elementoDibujo = new ServicioElementoDibujo();
            elementosDibujo.DataSource = elementoDibujo.mostrar();
        }

        public void eliminar(DataGridView elemento)
        {
            int nro = int.Parse(elemento.CurrentRow.Cells["nro"].Value.ToString());
            DialogResult dialogo = MessageBox.Show("¿Desea eliminar el elemento de dibujo con nro de inventario " + nro.ToString() + "?",
                "Información", MessageBoxButtons.YesNo);
            if (dialogo == DialogResult.Yes) 
            {
                ServicioElementoDibujo ele = new ServicioElementoDibujo();
                ele.eliminar(nro);
                MessageBox.Show("Elemento de dibujo eliminado exitosamente");
                elemento.Rows.Remove(elemento.CurrentRow);
                elemento.Refresh();
            }
        }

        public void consultarNroInventario(TextBox numero, DataGridView elemento)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = elemento.DataSource;
            bool bandera = false;
            if (numero.Text != null)
            {
                bandera = true;
            }
            while (bandera)
            {
                try
                {
                    if (bs.DataSource != null)
                    {
                        int nro = int.Parse(numero.Text);
                        bs.Filter = "nro = '" + nro + "'";
                        elemento.DataSource = bs.DataSource;
                        if (elemento.RowCount == 0)
                        {
                            MessageBox.Show("No se encontró ningún elemento de dibujo con el nro de inventario ingresado");
                        }
                    }
                }
                catch (Exception)
                {
                    bandera = false;
                    elemento.Refresh();  
                }
                finally
                {
                    bandera = false;
                }
            }
        }

        public void consultarNombre(TextBox nombre, DataGridView elemento) 
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = elemento.DataSource;    
            bool bandera = false;   
            if (nombre.Text != null) 
            {
                bandera = true;
            }
            while (bandera)
            {
                try
                {
                    if (bs.DataSource != null)
                    {
                        bs.Filter = "nombre like '%"+nombre.Text+"%'";
                        elemento.DataSource = bs.DataSource;    
                        if (elemento.RowCount == 0)
                        {
                            MessageBox.Show("No se encontró ningún elemento de dibujo con el nombre ingresado");
                        }
                    }
                }
                catch
                {
                    bandera = false;
                    elemento.Refresh();
                }
                finally
                {
                    bandera = false;
                }
            }
        }

        public void mostrarNombreED(ComboBox ED)
        {
            ServicioElementoDibujo servicio = new ServicioElementoDibujo();
            ED.DataSource = servicio.mostrar();
            ED.ValueMember = "nro";
            ED.DisplayMember = "nombre";
            ED.SelectedIndex = -1;
        }

        public void verificarCantidadED(int nro, DataGridView ed, DataGridView prestamos, string nombre, int cant)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = ed.DataSource;
            try
            {
                bs.Filter = "nro = '"+nro+"'";
                ed.DataSource = bs.DataSource;
                int cantidad = int.Parse(ed.CurrentRow.Cells["cantidadDisponible"].Value.ToString());
                if (cant <= cantidad)
                {
                    prestamos.Rows.Add(nro.ToString(), ed.CurrentRow.Cells["nombre"].Value.ToString(),
                        cant.ToString(), DateTime.Now.ToString(), null, nombre);
                }
                else
                {
                    MessageBox.Show("Ingrese una cantidad menor o igual a la cantidad disponible del elemento de dibujo seleccionado que es: "
                        +cantidad.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception) 
            {
                MessageBox.Show("Error");
            }
        }
    }
}
