using System;
using System.Collections.Generic;
using TPINT_GRUPO_10_PR3.Datos;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3.Negocio
{
    public class NegocioMedico
    {
        private readonly DaoMedicos dao = new DaoMedicos();

        public List<Medico> Listar(string filtro = "", int idEspecialidad = 0, int dia = -1)
        {
            return dao.Listar(filtro, idEspecialidad, dia);
        }

        public Medico ObtenerPorId(int idMedico)
        {
            if (idMedico <= 0)
                throw new Exception("El identificador del medico no es valido.");

            return dao.ObtenerPorId(idMedico);
        }

        public void Agregar(Medico nuevo)
        {
            ValidarDatosBasicos(nuevo, esAlta: true);

            if (dao.ExisteDni(nuevo.Dni))
                throw new Exception("Ya existe una persona registrada con ese DNI.");

            if (dao.ExisteLegajo(nuevo.Legajo))
                throw new Exception("Ya existe un medico registrado con ese Legajo.");

            if (dao.ExisteNombreUsuario(nuevo.Usuario.NombreUsuario))
                throw new Exception("El nombre de usuario ya esta en uso. Elegi otro.");

            dao.Agregar(nuevo);
        }

        public void Modificar(Medico medico)
        {
            if (medico.IdPersona <= 0)
                throw new Exception("No se pudo identificar el medico a modificar.");

            ValidarDatosBasicos(medico, esAlta: false);

            dao.Modificar(medico);
        }

        public void Eliminar(int idMedico)
        {
            if (idMedico <= 0)
                throw new Exception("No se pudo identificar el medico a eliminar.");

            dao.Eliminar(idMedico);
        }

        // Metodos puente para los desplegables de la vista
        public List<Especialidad> ListarEspecialidades()
        {
            return dao.ListarEspecialidades();
        }

        public List<Provincia> ListarProvincias()
        {
            return dao.ListarProvincias();
        }

        public List<Localidad> ListarLocalidades(int idProvincia)
        {
            return dao.ListarLocalidades(idProvincia);
        }

        public int ObtenerProvinciaPorLocalidad(int idLocalidad)
        {
            return dao.ObtenerProvinciaPorLocalidad(idLocalidad);
        }

        private void ValidarDatosBasicos(Medico med, bool esAlta)
        {
            if (med == null)
                throw new Exception("Los datos del medico no pueden estar vacios.");

            if (string.IsNullOrWhiteSpace(med.Legajo) ||
                string.IsNullOrWhiteSpace(med.Dni) ||
                string.IsNullOrWhiteSpace(med.Nombre) ||
                string.IsNullOrWhiteSpace(med.Apellido) ||
                string.IsNullOrWhiteSpace(med.Nacionalidad) ||
                string.IsNullOrWhiteSpace(med.Direccion) ||
                string.IsNullOrWhiteSpace(med.CorreoElectronico) ||
                string.IsNullOrWhiteSpace(med.Telefono))
            {
                throw new Exception("Todos los campos personales y de contacto son obligatorios.");
            }

            if (med.IdLocalidad <= 0)
                throw new Exception("Debe seleccionar una localidad valida.");

            if (med.Especialidad == null || med.Especialidad.IdEspecialidad <= 0)
                throw new Exception("Debe seleccionar una especialidad.");

            if (med.FechaNacimiento >= DateTime.Today.AddYears(-21))
                throw new Exception("La fecha de nacimiento no es valida para un profesional medico.");

            if (esAlta)
            {
                if (string.IsNullOrWhiteSpace(med.Usuario.NombreUsuario) || string.IsNullOrWhiteSpace(med.Usuario.Password))
                    throw new Exception("Debe completar el usuario y la contrasena de acceso.");
            }

            if (med.Horarios == null || med.Horarios.Count == 0)
                throw new Exception("Debe seleccionar al menos un dia de atencion.");

            foreach (HorarioMedico h in med.Horarios)
            {
                if (h.HoraInicio >= h.HoraFin)
                    throw new Exception("La hora de inicio debe ser anterior a la hora de fin.");
            }
        }
    }
}