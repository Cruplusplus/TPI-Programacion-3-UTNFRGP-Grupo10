using System;
using System.Collections.Generic;
using System.Linq;
using TPINT_GRUPO_10_PR3.Datos;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3.Negocio
{
    public class PacienteBLL
    {
        private readonly PacienteDAL pacienteDal = new PacienteDAL();

        public void Agregar(Paciente nuevo)
        {
            ValidarDatosPaciente(nuevo, esAlta: true);

            if (pacienteDal.ExisteDni(nuevo.Dni))
            {
                throw new Exception("Ya existe una persona registrada con el DNI " + nuevo.Dni + ".");
            }

            pacienteDal.Agregar(nuevo);
        }

        public void Modificar(Paciente paciente)
        {
            if (paciente.IdPersona <= 0)
            {
                throw new Exception("No se pudo identificar el paciente a modificar.");
            }

            ValidarDatosPaciente(paciente, esAlta: false);
            pacienteDal.Modificar(paciente);
        }

        public void EliminarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new Exception("El DNI del paciente a eliminar no es válido.");
            }

            pacienteDal.EliminarPorDni(dni);
        }

        public List<Paciente> Listar(string filtroTexto = "", int idProvincia = 0, string sexo = "")
        {
            return pacienteDal.Listar(filtroTexto, idProvincia, sexo);
        }

        public Paciente ObtenerPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni)) return null;
            return pacienteDal.ObtenerPorDni(dni);
        }

        public bool ExisteDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni)) return false;
            return pacienteDal.ExisteDni(dni);
        }

        public List<Provincia> ListarProvincias()
        {
            return pacienteDal.ListarProvincias();
        }

        public List<Localidad> ListarLocalidades(int idProvincia)
        {
            return pacienteDal.ListarLocalidades(idProvincia);
        }

        private void ValidarDatosPaciente(Paciente p, bool esAlta)
        {
            if (p == null)
                throw new Exception("Los datos del paciente son inválidos.");

            if (esAlta)
            {
                if (string.IsNullOrWhiteSpace(p.Dni) || p.Dni.Length < 7 || p.Dni.Length > 8 || !p.Dni.All(char.IsDigit))
                    throw new Exception("El DNI debe contener entre 7 y 8 dígitos numéricos.");
            }

            if (string.IsNullOrWhiteSpace(p.Nombre))
                throw new Exception("Debe ingresar el nombre del paciente.");

            if (string.IsNullOrWhiteSpace(p.Apellido))
                throw new Exception("Debe ingresar el apellido del paciente.");

            if (p.Sexo != 'M' && p.Sexo != 'F' && p.Sexo != 'X')
                throw new Exception("Debe seleccionar un sexo válido.");

            if (string.IsNullOrWhiteSpace(p.Nacionalidad))
                throw new Exception("Debe ingresar la nacionalidad.");

            if (p.FechaNacimiento == DateTime.MinValue || p.FechaNacimiento > DateTime.Today)
                throw new Exception("La fecha de nacimiento no es válida.");

            if (string.IsNullOrWhiteSpace(p.Direccion))
                throw new Exception("Debe ingresar la dirección.");

            if (p.IdLocalidad <= 0)
                throw new Exception("Debe seleccionar una provincia y una localidad.");

            if (string.IsNullOrWhiteSpace(p.CorreoElectronico) || !p.CorreoElectronico.Contains("@"))
                throw new Exception("Debe ingresar un correo electrónico válido.");

            if (string.IsNullOrWhiteSpace(p.Telefono))
                throw new Exception("Debe ingresar un teléfono de contacto.");
        }
    }
}