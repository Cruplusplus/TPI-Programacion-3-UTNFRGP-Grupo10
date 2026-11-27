using System;
using System.Collections.Generic;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class PacienteDAL
    {
        private readonly AccesoDatos datos = new AccesoDatos();

        public void Agregar(Paciente nuevo)
        {
            try
            {
                datos.IniciarTransaccion();

                // 1. Insertamos en Persona y recuperamos el IdPersona generado
                datos.SetearConsulta("INSERT INTO Persona (IdLocalidad, Dni, Nombre, Apellido, Sexo, Nacionalidad, FechaDeNacimiento, Direccion, CorreoElectronico, Telefono, Estado) " +
                                     "OUTPUT INSERTED.IdPersona VALUES (@IdLocalidad, @Dni, @Nombre, @Apellido, @Sexo, @Nacionalidad, @FechaDeNacimiento, @Direccion, @CorreoElectronico, @Telefono, @Estado)");
                datos.SetearParametro("@IdLocalidad", nuevo.IdLocalidad);
                datos.SetearParametro("@Dni", nuevo.Dni);
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Sexo", nuevo.Sexo);
                datos.SetearParametro("@Nacionalidad", nuevo.Nacionalidad);
                datos.SetearParametro("@FechaDeNacimiento", nuevo.FechaNacimiento);
                datos.SetearParametro("@Direccion", nuevo.Direccion);
                datos.SetearParametro("@CorreoElectronico", nuevo.CorreoElectronico);
                datos.SetearParametro("@Telefono", nuevo.Telefono);
                datos.SetearParametro("@Estado", true);

                int idPersona = datos.EjecutarScalar();

                // 2. Insertamos en Paciente (IdPaciente es IDENTITY, pasamos IdPersona y EstadoPaciente)
                datos.SetearConsulta("INSERT INTO Paciente (IdPersona, EstadoPaciente) VALUES (@IdPersona, @EstadoPaciente)");
                datos.SetearParametro("@IdPersona", idPersona);
                datos.SetearParametro("@EstadoPaciente", true);

                datos.EjecutarAccion();
                datos.ConfirmarTransaccion();
            }
            catch
            {
                datos.CancelarTransaccion();
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Paciente paciente)
        {
            try
            {
                datos.IniciarTransaccion();

                datos.SetearConsulta("UPDATE Persona SET IdLocalidad = @IdLocalidad, Nombre = @Nombre, Apellido = @Apellido, " +
                                     "Sexo = @Sexo, Nacionalidad = @Nacionalidad, FechaDeNacimiento = @FechaDeNacimiento, " +
                                     "Direccion = @Direccion, CorreoElectronico = @CorreoElectronico, Telefono = @Telefono " +
                                     "WHERE IdPersona = @IdPersona");
                datos.SetearParametro("@IdLocalidad", paciente.IdLocalidad);
                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Sexo", paciente.Sexo);
                datos.SetearParametro("@Nacionalidad", paciente.Nacionalidad);
                datos.SetearParametro("@FechaDeNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Direccion", paciente.Direccion);
                datos.SetearParametro("@CorreoElectronico", paciente.CorreoElectronico);
                datos.SetearParametro("@Telefono", paciente.Telefono);
                datos.SetearParametro("@IdPersona", paciente.IdPersona);

                datos.EjecutarAccion();
                datos.ConfirmarTransaccion();
            }
            catch
            {
                datos.CancelarTransaccion();
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarPorDni(string dni)
        {
            try
            {
                datos.IniciarTransaccion();

                // Baja lógica en Paciente (EstadoPaciente = 0) y en Persona (Estado = 0)
                datos.SetearConsulta("UPDATE Paciente SET EstadoPaciente = 0 " +
                                     "WHERE IdPersona = (SELECT IdPersona FROM Persona WHERE Dni = @Dni)");
                datos.SetearParametro("@Dni", dni.Trim());
                datos.EjecutarAccion();

                datos.SetearConsulta("UPDATE Persona SET Estado = 0 WHERE Dni = @Dni");
                datos.SetearParametro("@Dni", dni.Trim());
                datos.EjecutarAccion();

                datos.ConfirmarTransaccion();
            }
            catch
            {
                datos.CancelarTransaccion();
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Paciente> Listar(string filtroTexto = "", int idProvincia = 0, string sexo = "")
        {
            List<Paciente> lista = new List<Paciente>();

            try
            {
                string consulta = "SELECT pac.IdPaciente, pac.IdPersona, per.Dni, per.Nombre, per.Apellido, per.Sexo, " +
                                  "per.Nacionalidad, per.FechaDeNacimiento, per.Direccion, per.CorreoElectronico, per.Telefono, " +
                                  "loc.IdLocalidad, loc.NombreLocalidad, prov.IdProvincia, prov.NombreProvincia " +
                                  "FROM Paciente pac " +
                                  "INNER JOIN Persona per ON pac.IdPersona = per.IdPersona " +
                                  "INNER JOIN Localidad loc ON per.IdLocalidad = loc.IdLocalidad " +
                                  "INNER JOIN Provincia prov ON loc.IdProvincia = prov.IdProvincia " +
                                  "WHERE pac.EstadoPaciente = 1 AND per.Estado = 1";

                if (!string.IsNullOrWhiteSpace(filtroTexto))
                {
                    consulta += " AND (per.Dni LIKE @Filtro OR per.Nombre LIKE @Filtro OR per.Apellido LIKE @Filtro)";
                }

                if (idProvincia > 0)
                {
                    consulta += " AND prov.IdProvincia = @IdProvincia";
                }

                if (!string.IsNullOrWhiteSpace(sexo))
                {
                    consulta += " AND per.Sexo = @Sexo";
                }

                consulta += " ORDER BY per.Apellido ASC, per.Nombre ASC";

                datos.SetearConsulta(consulta);

                if (!string.IsNullOrWhiteSpace(filtroTexto))
                {
                    datos.SetearParametro("@Filtro", "%" + filtroTexto.Trim() + "%");
                }

                if (idProvincia > 0)
                {
                    datos.SetearParametro("@IdProvincia", idProvincia);
                }

                if (!string.IsNullOrWhiteSpace(sexo))
                {
                    datos.SetearParametro("@Sexo", sexo.Trim());
                }

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente pac = new Paciente();
                    pac.IdPersona = (int)datos.Lector["IdPersona"];
                    pac.Dni = datos.Lector["Dni"].ToString();
                    pac.Nombre = datos.Lector["Nombre"].ToString();
                    pac.Apellido = datos.Lector["Apellido"].ToString();
                    pac.Sexo = Convert.ToChar(datos.Lector["Sexo"]);
                    pac.Nacionalidad = datos.Lector["Nacionalidad"].ToString();
                    pac.FechaNacimiento = (DateTime)datos.Lector["FechaDeNacimiento"];
                    pac.Direccion = datos.Lector["Direccion"].ToString();
                    pac.CorreoElectronico = datos.Lector["CorreoElectronico"].ToString();
                    pac.Telefono = datos.Lector["Telefono"].ToString();

                    pac.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    pac.LocalidadObj.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    pac.LocalidadObj.IdProvincia = (int)datos.Lector["IdProvincia"];
                    pac.LocalidadObj.NombreLocalidad = datos.Lector["NombreLocalidad"].ToString();

                    pac.ProvinciaObj.IdProvincia = (int)datos.Lector["IdProvincia"];
                    pac.ProvinciaObj.NombreProvincia = datos.Lector["NombreProvincia"].ToString();
                    pac.EstadoPaciente = true;

                    lista.Add(pac);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Paciente ObtenerPorDni(string dni)
        {
            Paciente pac = null;

            try
            {
                string consulta = "SELECT pac.IdPaciente, pac.IdPersona, per.Dni, per.Nombre, per.Apellido, per.Sexo, " +
                                  "per.Nacionalidad, per.FechaDeNacimiento, per.Direccion, per.CorreoElectronico, per.Telefono, " +
                                  "loc.IdLocalidad, loc.NombreLocalidad, prov.IdProvincia, prov.NombreProvincia " +
                                  "FROM Paciente pac " +
                                  "INNER JOIN Persona per ON pac.IdPersona = per.IdPersona " +
                                  "INNER JOIN Localidad loc ON per.IdLocalidad = loc.IdLocalidad " +
                                  "INNER JOIN Provincia prov ON loc.IdProvincia = prov.IdProvincia " +
                                  "WHERE per.Dni = @Dni AND pac.EstadoPaciente = 1 AND per.Estado = 1";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Dni", dni.Trim());
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    pac = new Paciente();
                    pac.IdPersona = (int)datos.Lector["IdPersona"];
                    pac.Dni = datos.Lector["Dni"].ToString();
                    pac.Nombre = datos.Lector["Nombre"].ToString();
                    pac.Apellido = datos.Lector["Apellido"].ToString();
                    pac.Sexo = Convert.ToChar(datos.Lector["Sexo"]);
                    pac.Nacionalidad = datos.Lector["Nacionalidad"].ToString();
                    pac.FechaNacimiento = (DateTime)datos.Lector["FechaDeNacimiento"];
                    pac.Direccion = datos.Lector["Direccion"].ToString();
                    pac.CorreoElectronico = datos.Lector["CorreoElectronico"].ToString();
                    pac.Telefono = datos.Lector["Telefono"].ToString();

                    pac.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    pac.LocalidadObj.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    pac.LocalidadObj.IdProvincia = (int)datos.Lector["IdProvincia"];
                    pac.LocalidadObj.NombreLocalidad = datos.Lector["NombreLocalidad"].ToString();

                    pac.ProvinciaObj.IdProvincia = (int)datos.Lector["IdProvincia"];
                    pac.ProvinciaObj.NombreProvincia = datos.Lector["NombreProvincia"].ToString();
                    pac.EstadoPaciente = true;
                }

                return pac;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteDni(string dni)
        {
            try
            {
                datos.SetearConsulta("SELECT COUNT(1) FROM Persona WHERE Dni = @Dni");
                datos.SetearParametro("@Dni", dni.Trim());
                return datos.EjecutarScalar() > 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Provincia> ListarProvincias()
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                datos.SetearConsulta("SELECT IdProvincia, NombreProvincia FROM Provincia ORDER BY NombreProvincia ASC");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Provincia prov = new Provincia();
                    prov.IdProvincia = (int)datos.Lector["IdProvincia"];
                    prov.NombreProvincia = datos.Lector["NombreProvincia"].ToString();
                    lista.Add(prov);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Localidad> ListarLocalidades(int idProvincia)
        {
            List<Localidad> lista = new List<Localidad>();

            try
            {
                datos.SetearConsulta("SELECT IdLocalidad, IdProvincia, NombreLocalidad FROM Localidad WHERE IdProvincia = @IdProvincia ORDER BY NombreLocalidad ASC");
                datos.SetearParametro("@IdProvincia", idProvincia);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Localidad loc = new Localidad();
                    loc.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    loc.IdProvincia = (int)datos.Lector["IdProvincia"];
                    loc.NombreLocalidad = datos.Lector["NombreLocalidad"].ToString();
                    lista.Add(loc);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}