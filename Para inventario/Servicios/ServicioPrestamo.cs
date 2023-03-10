using Para_inventario.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para_inventario.Servicios
{
    class ServicioPrestamo
    {
        private string cadenaBD = System.Configuration.ConfigurationManager.AppSettings["cadenaBD"];

        public DataTable mostrarPrestamoHerramientas()
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "mostrarPrestamoHerramienta";
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);    
            return dt;
        }

        public void registrarPrestamoHerramienta(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;    
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "INSERT INTO PrestamosHerramientas (inventarioHerramienta, fechaPrestamo, cantidad, encargado, RealizadoPor)" +
                             "VALUES (@nro, @f, @c, @e, @u)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nro", prestamo.nroInventario);
                cmd.Parameters.AddWithValue("@f", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@e", prestamo.encargado);
                cmd.Parameters.AddWithValue("@u", prestamo.usuario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE HerramientasManuales SET cantidad = cantidad - @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public void registrarDevolucionHerramienta(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "UPDATE PrestamosHerramientas SET fechaDevolucion = GETDATE() WHERE inventarioHerramienta = @n AND fechaPrestamo = @fe";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.Parameters.AddWithValue("@fe", prestamo.fechaPrestamo);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE HerramientasManuales SET cantidad = cantidad + @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                MessageBox.Show("Devolución registrada exitósamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback(); 
            }
            finally
            {
                cn.Close();
            }
        }

        public void registrarPrestamoED(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "INSERT INTO PrestamosElementosDibujo (inventarioElementosDibujo, cantidad, encargado, fechaPrestamo, RealizadoPor) " +
                    "VALUES (@n, @c, @e, @f, @u)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@e", prestamo.encargado);
                cmd.Parameters.AddWithValue("@f", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@u", prestamo.usuario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE ElementosDibujo SET cantidadDisponible = cantidadDisponible - @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable mostrarPrestamosED()
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "mostrarPrestamosED";
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cn.Open();
            ad.Fill(dt);
            cn.Close();
            return dt;
        }

        public void registrarDevED(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();  
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "UPDATE ElementosDibujo SET cantidadDisponible = cantidadDisponible + @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "registrarPrestED";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@fe", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                MessageBox.Show("Devolución registrada exitósamente");
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public void registrarPrestamoMaquina(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "INSERT INTO PrestamosMaquinas (inventarioMaquinas, cantidad, fechaPrestamo, encargado, RealizadoPor) " +
                    "VALUES (@n, @c, @f, @e, @u)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@e", prestamo.encargado);
                cmd.Parameters.AddWithValue("@f", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@u", prestamo.usuario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Maquinas SET cantidad = cantidad - @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public void registrarDevMaquina(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "UPDATE PrestamosMaquinas SET fechaDevolucion = GETDATE() WHERE inventarioMaquinas = @n AND fechaPrestamo = @f";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@f", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Maquinas SET cantidad = cantidad + @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                MessageBox.Show("Devolución registrada exitósamente");
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable mostrarPrestamosMaquinas()
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "mostrarPrestamosMaquinas";
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cn.Open();
            DataTable dt = new DataTable();
            ad.Fill(dt);
            cn.Close(); 
            return dt;
        }

        public void registrarPrestamoInformatica(Prestamo informatica)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction();
            try
            {
                cmd.CommandText = "INSERT INTO PrestamosInformatica (inventarioInformatica, fechaPrestamo, cantidad, encargado, RealizadoPor)" +
                    "VALUES (@n, @f, @c, @e, @u)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", informatica.nroInventario);
                cmd.Parameters.AddWithValue("@f", informatica.fechaPrestamo);
                cmd.Parameters.AddWithValue("@c", informatica.cantidad);
                cmd.Parameters.AddWithValue("@e", informatica.encargado);
                cmd.Parameters.AddWithValue("@u", informatica.usuario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Informatica SET cantidad = cantidad - @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@n", informatica.nroInventario);
                cmd.Parameters.AddWithValue("@c", informatica.cantidad);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable mostrarPrestInformatica()
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "mostrarPrestamoInformatica";
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cn.Open();
            ad.Fill(dt);
            cn.Close();
            return dt;
        }

        public void registrarDevInformatica(Prestamo prestamo)
        {
            SqlConnection cn = new SqlConnection(cadenaBD);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();
            cmd.Transaction = cn.BeginTransaction(); 
            try 
            {
                cmd.CommandText = "UPDATE prestamosInformatica SET fechaDevolucion = GETDATE() WHERE inventarioInformatica = @n AND " +
                    "fechaPrestamo = @f";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@f", prestamo.fechaPrestamo);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Informatica SET cantidad = cantidad + @c WHERE nro = @n";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c", prestamo.cantidad);
                cmd.Parameters.AddWithValue("@n", prestamo.nroInventario);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                MessageBox.Show("Devolución registrada exitósamente");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                cmd.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
