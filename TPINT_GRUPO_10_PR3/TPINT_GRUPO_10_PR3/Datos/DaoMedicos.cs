using System;
using System.Collections.Generic;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class DaoMedicos
    {
        private readonly AccesoDatos datos = new AccesoDatos();

        public void Agregar(Medico nuevo)
        {
            try
            {
                datos.IniciarTransaccion();

                // Insertamos en Users (IdRol = 1 es Medico)
                datos.SetearConsulta("INSERT INTO Users (IdRol, NombreUser, Passwords, Estado) OUTPUT INSERTED.IdUser " +
                                     "VALUES (@IdRol, @NombreUser, @Passwords, @Estado)");
                datos.SetearParametro("@IdRol", 1);
                datos.SetearParametro("@NombreUser", nuevo.Usuario.NombreUsuario);
                datos.SetearParametro("@Passwords", nuevo.Usuario.Password);
                datos.SetearParametro("@Estado", true);

                int idUser = datos.EjecutarScalar();

                // Insertamos en Persona
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

                // Insertamos en Medico
                datos.SetearConsulta("INSERT INTO Medico (IdMedico, IdEspecialidad, IdUser, Legajo, EstadoMedico) " +
                                     "VALUES (@IdMedico, @IdEspecialidad, @IdUser, @Legajo, @EstadoMedico)");
                datos.SetearParametro("@IdMedico", idPersona);
                datos.SetearParametro("@IdEspecialidad", nuevo.Especialidad.IdEspecialidad);
                datos.SetearParametro("@IdUser", idUser);
                datos.SetearParametro("@Legajo", nuevo.Legajo);
                datos.SetearParametro("@EstadoMedico", true);

                datos.EjecutarAccion();

                // Insertamos los Horarios activos
                foreach (HorarioMedico horario in nuevo.Horarios)
                {
                    datos.SetearConsulta("INSERT INTO HorarioMedico (IdMedico, Dia, HoraInicio, HoraFin, EstadoHorario) " +
                                         "VALUES (@IdMedico, @Dia, @HoraInicio, @HoraFin, 1)");
                    datos.SetearParametro("@IdMedico", idPersona);
                    datos.SetearParametro("@Dia", (int)horario.Dia);
                    datos.SetearParametro("@HoraInicio", horario.HoraInicio);
                    datos.SetearParametro("@HoraFin", horario.HoraFin);

                    datos.EjecutarAccion();
                }

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

        public void Modificar(Medico medico)
        {
            try
            {
                datos.IniciarTransaccion();

                // Actualizamos Persona
                datos.SetearConsulta("UPDATE Persona SET IdLocalidad = @IdLocalidad, Nombre = @Nombre, Apellido = @Apellido, " +
                                     "Sexo = @Sexo, Nacionalidad = @Nacionalidad, FechaDeNacimiento = @FechaDeNacimiento, " +
                                     "Direccion = @Direccion, CorreoElectronico = @CorreoElectronico, Telefono = @Telefono " +
                                     "WHERE IdPersona = @IdPersona");
                datos.SetearParametro("@IdLocalidad", medico.IdLocalidad);
                datos.SetearParametro("@Nombre", medico.Nombre);
                datos.SetearParametro("@Apellido", medico.Apellido);
                datos.SetearParametro("@Sexo", medico.Sexo);
                datos.SetearParametro("@Nacionalidad", medico.Nacionalidad);
                datos.SetearParametro("@FechaDeNacimiento", medico.FechaNacimiento);
                datos.SetearParametro("@Direccion", medico.Direccion);
                datos.SetearParametro("@CorreoElectronico", medico.CorreoElectronico);
                datos.SetearParametro("@Telefono", medico.Telefono);
                datos.SetearParametro("@IdPersona", medico.IdPersona);

                datos.EjecutarAccion();

                // Actualizamos Especialidad en Medico
                datos.SetearConsulta("UPDATE Medico SET IdEspecialidad = @IdEspecialidad WHERE IdMedico = @IdMedico");
                datos.SetearParametro("@IdEspecialidad", medico.Especialidad.IdEspecialidad);
                datos.SetearParametro("@IdMedico", medico.IdPersona);

                datos.EjecutarAccion();

                // Si ingreso contraseña nueva, actualizamos Users
                if (!string.IsNullOrWhiteSpace(medico.Usuario.Password))
                {
                    datos.SetearConsulta("UPDATE Users SET Passwords = @Passwords " +
                                         "WHERE IdUser = (SELECT IdUser FROM Medico WHERE IdMedico = @IdMedico)");
                    datos.SetearParametro("@Passwords", medico.Usuario.Password);
                    datos.SetearParametro("@IdMedico", medico.IdPersona);

                    datos.EjecutarAccion();
                }

                // Desactivamos horarios anteriores e insertamos los nuevos
                datos.SetearConsulta("UPDATE HorarioMedico SET EstadoHorario = 0 WHERE IdMedico = @IdMedico AND EstadoHorario = 1");
                datos.SetearParametro("@IdMedico", medico.IdPersona);
                datos.EjecutarAccion();

                foreach (HorarioMedico horario in medico.Horarios)
                {
                    datos.SetearConsulta("INSERT INTO HorarioMedico (IdMedico, Dia, HoraInicio, HoraFin, EstadoHorario) " +
                                         "VALUES (@IdMedico, @Dia, @HoraInicio, @HoraFin, 1)");
                    datos.SetearParametro("@IdMedico", medico.IdPersona);
                    datos.SetearParametro("@Dia", (int)horario.Dia);
                    datos.SetearParametro("@HoraInicio", horario.HoraInicio);
                    datos.SetearParametro("@HoraFin", horario.HoraFin);

                    datos.EjecutarAccion();
                }

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

        public void Eliminar(int idMedico)
        {
            try
            {
                datos.IniciarTransaccion();

                datos.SetearConsulta("UPDATE Medico SET EstadoMedico = 0 WHERE IdMedico = @IdMedico");
                datos.SetearParametro("@IdMedico", idMedico);
                datos.EjecutarAccion();

                datos.SetearConsulta("UPDATE HorarioMedico SET EstadoHorario = 0 WHERE IdMedico = @IdMedico");
                datos.SetearParametro("@IdMedico", idMedico);
                datos.EjecutarAccion();

                datos.SetearConsulta("UPDATE Users SET Estado = 0 WHERE IdUser = (SELECT IdUser FROM Medico WHERE IdMedico = @IdMedico)");
                datos.SetearParametro("@IdMedico", idMedico);
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

        public List<Medico> Listar(string filtro = "", int idEspecialidad = 0, int dia = -1)
        {
            List<Medico> lista = new List<Medico>();

            try
            {
                string consulta = "SELECT m.IdMedico, m.Legajo, m.IdEspecialidad, e.NombreEspecialidad AS Especialidad, " +
                                  "p.Dni, p.Nombre, p.Apellido, p.CorreoElectronico, p.Telefono " +
                                  "FROM Medico m " +
                                  "INNER JOIN Persona p ON m.IdMedico = p.IdPersona " +
                                  "INNER JOIN Especialidad e ON m.IdEspecialidad = e.IdEspecialidad " +
                                  "WHERE m.EstadoMedico = 1 AND p.Estado = 1";

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += " AND (p.Apellido LIKE @Filtro OR p.Nombre LIKE @Filtro OR m.Legajo LIKE @Filtro)";
                }

                if (idEspecialidad > 0)
                {
                    consulta += " AND m.IdEspecialidad = @IdEspecialidad";
                }

                if (dia >= 0 && dia <= 6)
                {
                    consulta += " AND EXISTS (SELECT 1 FROM HorarioMedico hm WHERE hm.IdMedico = m.IdMedico AND hm.EstadoHorario = 1 AND hm.Dia = @Dia)";
                }

                datos.SetearConsulta(consulta);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    datos.SetearParametro("@Filtro", "%" + filtro.Trim() + "%");
                }

                if (idEspecialidad > 0)
                {
                    datos.SetearParametro("@IdEspecialidad", idEspecialidad);
                }

                if (dia >= 0 && dia <= 6)
                {
                    datos.SetearParametro("@Dia", dia);
                }

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico med = new Medico();
                    med.IdPersona = (int)datos.Lector["IdMedico"];
                    med.Legajo = datos.Lector["Legajo"].ToString();
                    med.Especialidad.IdEspecialidad = (int)datos.Lector["IdEspecialidad"];
                    med.Especialidad.NombreEspecialidad = datos.Lector["Especialidad"].ToString();
                    med.Dni = datos.Lector["Dni"].ToString();
                    med.Nombre = datos.Lector["Nombre"].ToString();
                    med.Apellido = datos.Lector["Apellido"].ToString();
                    med.CorreoElectronico = datos.Lector["CorreoElectronico"].ToString();
                    med.Telefono = datos.Lector["Telefono"].ToString();

                    lista.Add(med);
                }
            }
            finally
            {
                datos.CerrarConexion();
            }

            foreach (Medico med in lista)
            {
                med.Horarios = ListarHorariosPorMedico(med.IdPersona);
            }

            return lista;
        }

        private List<HorarioMedico> ListarHorariosPorMedico(int idMedico)
        {
            List<HorarioMedico> horarios = new List<HorarioMedico>();

            try
            {
                datos.SetearConsulta("SELECT IdHorarioMedico, Dia, HoraInicio, HoraFin " +
                                     "FROM HorarioMedico " +
                                     "WHERE IdMedico = @IdMedico AND EstadoHorario = 1 " +
                                     "ORDER BY Dia ASC");
                datos.SetearParametro("@IdMedico", idMedico);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    HorarioMedico h = new HorarioMedico();
                    h.IdHorarioMedico = (int)datos.Lector["IdHorarioMedico"];
                    h.Dia = (DayOfWeek)Convert.ToInt32(datos.Lector["Dia"]);
                    h.HoraInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    h.HoraFin = (TimeSpan)datos.Lector["HoraFin"];
                    h.EstadoHorario = true;

                    horarios.Add(h);
                }
            }
            finally
            {
                datos.CerrarConexion();
            }

            return horarios;
        }

        public Medico ObtenerPorId(int idMedico)
        {
            Medico med = null;

            try
            {
                datos.SetearConsulta("SELECT m.IdMedico, m.Legajo, m.IdEspecialidad, " +
                                     "p.Dni, p.Nombre, p.Apellido, p.Sexo, p.Nacionalidad, p.FechaDeNacimiento, " +
                                     "p.Direccion, p.IdLocalidad, p.CorreoElectronico, p.Telefono, " +
                                     "u.IdUser, u.NombreUser " +
                                     "FROM Medico m " +
                                     "INNER JOIN Persona p ON m.IdMedico = p.IdPersona " +
                                     "INNER JOIN Users u ON m.IdUser = u.IdUser " +
                                     "WHERE m.IdMedico = @IdMedico AND m.EstadoMedico = 1");
                datos.SetearParametro("@IdMedico", idMedico);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    med = new Medico();
                    med.IdPersona = (int)datos.Lector["IdMedico"];
                    med.Legajo = datos.Lector["Legajo"].ToString();
                    med.Especialidad.IdEspecialidad = (int)datos.Lector["IdEspecialidad"];
                    med.Dni = datos.Lector["Dni"].ToString();
                    med.Nombre = datos.Lector["Nombre"].ToString();
                    med.Apellido = datos.Lector["Apellido"].ToString();
                    med.Sexo = Convert.ToChar(datos.Lector["Sexo"]);
                    med.Nacionalidad = datos.Lector["Nacionalidad"].ToString();
                    med.FechaNacimiento = (DateTime)datos.Lector["FechaDeNacimiento"];
                    med.Direccion = datos.Lector["Direccion"].ToString();
                    med.IdLocalidad = (int)datos.Lector["IdLocalidad"];
                    med.CorreoElectronico = datos.Lector["CorreoElectronico"].ToString();
                    med.Telefono = datos.Lector["Telefono"].ToString();
                    med.Usuario.IdUsuario = (int)datos.Lector["IdUser"];
                    med.Usuario.NombreUsuario = datos.Lector["NombreUser"].ToString();
                }
            }
            finally
            {
                datos.CerrarConexion();
            }

            if (med != null)
            {
                med.Horarios = ListarHorariosPorMedico(med.IdPersona);
            }

            return med;
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

        public bool ExisteLegajo(string legajo)
        {
            try
            {
                datos.SetearConsulta("SELECT COUNT(1) FROM Medico WHERE Legajo = @Legajo");
                datos.SetearParametro("@Legajo", legajo.Trim());
                return datos.EjecutarScalar() > 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteNombreUsuario(string nombreUser)
        {
            try
            {
                datos.SetearConsulta("SELECT COUNT(1) FROM Users WHERE NombreUser = @NombreUser");
                datos.SetearParametro("@NombreUser", nombreUser.Trim());
                return datos.EjecutarScalar() > 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Especialidad> ListarEspecialidades()
        {
            List<Especialidad> lista = new List<Especialidad>();

            try
            {
                datos.SetearConsulta("SELECT IdEspecialidad, NombreEspecialidad FROM Especialidad ORDER BY NombreEspecialidad ASC");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad esp = new Especialidad();
                    esp.IdEspecialidad = (int)datos.Lector["IdEspecialidad"];
                    esp.NombreEspecialidad = datos.Lector["NombreEspecialidad"].ToString();
                    lista.Add(esp);
                }

                return lista;
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

        public int ObtenerProvinciaPorLocalidad(int idLocalidad)
        {
            try
            {
                datos.SetearConsulta("SELECT IdProvincia FROM Localidad WHERE IdLocalidad = @IdLocalidad");
                datos.SetearParametro("@IdLocalidad", idLocalidad);
                return datos.EjecutarScalar();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}