use master
go

create database Clinica_TPI
go

use Clinica_TPI
go

-- DROP TABLE IF EXISTS Turno;
-- DROP TABLE IF EXISTS HorarioMedico;
-- DROP TABLE IF EXISTS Medico;
-- DROP TABLE IF EXISTS Paciente;
-- DROP TABLE IF EXISTS Especialidad;
-- DROP TABLE IF EXISTS Persona;
-- DROP TABLE IF EXISTS Localidad;
-- DROP TABLE IF EXISTS Provincia;
-- DROP TABLE IF EXISTS Users;
-- DROP TABLE IF EXISTS Rol;
-- go

create table Rol
(
	IdRol int not null,
	Nombre varchar(50) not null,
	constraint PK_Rol primary key (IdRol)
)
go

create table Users
(
	IdUser int identity(1,1) not null,
	IdRol int not null,
	NombreUser varchar(50) not null,
	Passwords varchar(50) not null,
	Estado bit default 1 not null,
	constraint PK_Users primary key (IdUser),
	constraint UQ_Users_NombreUser unique (NombreUser),
	constraint FK_Users_Rol foreign key (IdRol) references Rol (IdRol)
)
go

create table Provincia
(
	IdProvincia int identity(1,1) not null,
	NombreProvincia varchar(50) not null,
	constraint PK_Provincias_Id primary key (IdProvincia)
)
go

create table Localidad
(
	IdLocalidad int identity(1,1) not null,
	IdProvincia int not null,
	NombreLocalidad varchar(50) not null,
	constraint PK_Localidades_Id primary key (IdLocalidad),
	constraint FK_Localidades_Provincia foreign key (IdProvincia) references Provincia (IdProvincia)
)
go

create table Persona
(
	IdPersona int identity(1,1) not null,
	IdLocalidad int not null,
	Dni varchar(8) not null,
	Nombre varchar(50) not null,
	Apellido varchar(50) not null,
	Sexo char(1) not null,
	Nacionalidad varchar(50) not null,
	FechaDeNacimiento date not null,
	Direccion varchar(50) not null,
	CorreoElectronico varchar(50) not null,
	Telefono varchar(15) not null,
	Estado bit default 1 not null,
	constraint PK_Personas_Id primary key (IdPersona),
	constraint UQ_Personas_Dni unique (Dni),
	constraint CK_Personas_Sexo check (Sexo in ('M', 'F', 'X')),
	constraint FK_Personas_Localidad foreign key (IdLocalidad) references Localidad (IdLocalidad)
)
go

create table Paciente
(
	IdPaciente int identity(1,1) not null,
	IdPersona int not null,
	EstadoPaciente bit default 1 not null,
	constraint PK_Pacientes_Id primary key (IdPaciente),
	constraint UQ_Pacientes_Persona unique (IdPersona),
	constraint FK_Pacientes_Persona foreign key (IdPersona) references Persona (IdPersona)
)
go

create table Especialidad
(
	IdEspecialidad int identity(1,1) not null,
	NombreEspecialidad varchar(50) not null,
	constraint PK_Especialidad_Id primary key (IdEspecialidad)
)
go

create table Medico
(
	IdMedico int not null,
	IdEspecialidad int not null,
	IdUser int not null,
	Legajo varchar(15) not null,
	EstadoMedico bit default 1 not null,
	constraint PK_Medico_Id primary key (IdMedico),
	constraint UQ_Medico_Legajo unique (Legajo),
	constraint UQ_Medico_User unique (IdUser),
	constraint FK_Medico_Persona foreign key (IdMedico) references Persona (IdPersona),
	constraint FK_Medico_Especialidad foreign key (IdEspecialidad) references Especialidad (IdEspecialidad),
	constraint FK_Medico_User foreign key (IdUser) references Users (IdUser)
)
go

create table HorarioMedico
(
	IdHorarioMedico int identity(1,1) not null,
	IdMedico int not null,
	Dia tinyint not null,
	HoraInicio time(0) not null,
	HoraFin time(0) not null,
	EstadoHorario bit default 1 not null,
	constraint PK_HorarioMedico_Id primary key (IdHorarioMedico),
	constraint CK_HorarioMedico_Dia check (Dia between 0 and 6),
	constraint CK_HorarioMedico_Horas check (HoraFin > HoraInicio),
	constraint FK_HorarioMedico_Medico foreign key (IdMedico) references Medico (IdMedico)
)
go

create table Turno
(
	IdTurno int identity(1,1) not null,
	IdMedico int not null,
	IdPaciente int not null,
	FechaTurno date not null,
	HoraTurno time(0) not null,
	Asistencia bit null,
	Observacion varchar(100) null,
	EstadoTurno bit default 1 not null,
	constraint PK_Turno_Id primary key (IdTurno),
	constraint FK_Turno_Medico foreign key (IdMedico) references Medico (IdMedico),
	constraint FK_Turno_Paciente foreign key (IdPaciente) references Paciente (IdPaciente)
)
go

-- ========== DATOS INICIALES ==========
INSERT INTO Rol (IdRol, Nombre) VALUES (0, 'Admin'), (1, 'Medico'), (2, 'Paciente');
go

INSERT INTO Users (IdRol, NombreUser, Passwords) VALUES (0, 'admin', 'admin123');
go

INSERT INTO Especialidad (NombreEspecialidad) VALUES
('Clínica Médica'),
('Cardiología'),
('Dermatología'),
('Pediatría'),
('Traumatología'),
('Neurología');
go

-- ========== PROVINCIAS (24) ==========
INSERT INTO Provincia (NombreProvincia) VALUES
('Buenos Aires'),
('Ciudad Autónoma de Buenos Aires'),
('Catamarca'),
('Chaco'),
('Chubut'),
('Córdoba'),
('Corrientes'),
('Entre Ríos'),
('Formosa'),
('Jujuy'),
('La Pampa'),
('La Rioja'),
('Mendoza'),
('Misiones'),
('Neuquén'),
('Río Negro'),
('Salta'),
('San Juan'),
('San Luis'),
('Santa Cruz'),
('Santa Fe'),
('Santiago del Estero'),
('Tierra del Fuego'),
('Tucumán');
go

-- ========== LOCALIDADES (100) ==========
INSERT INTO Localidad (IdProvincia, NombreLocalidad)
SELECT p.IdProvincia, v.Localidad
FROM (VALUES
('Buenos Aires', 'La Plata'),
('Buenos Aires', 'Mar del Plata'),
('Buenos Aires', 'Bahía Blanca'),
('Buenos Aires', 'Tigre'),
('Buenos Aires', 'General Pacheco'),
('Buenos Aires', 'San Isidro'),
('Buenos Aires', 'Tandil'),
('Buenos Aires', 'Quilmes'),
('Ciudad Autónoma de Buenos Aires', 'Palermo'),
('Ciudad Autónoma de Buenos Aires', 'Belgrano'),
('Ciudad Autónoma de Buenos Aires', 'Caballito'),
('Ciudad Autónoma de Buenos Aires', 'Villa Urquiza'),
('Catamarca', 'San Fernando del Valle de Catamarca'),
('Catamarca', 'Belén'),
('Catamarca', 'Tinogasta'),
('Catamarca', 'Andalgalá'),
('Chaco', 'Resistencia'),
('Chaco', 'Presidencia Roque Sáenz Peña'),
('Chaco', 'Villa Ángela'),
('Chaco', 'Charata'),
('Chubut', 'Rawson'),
('Chubut', 'Comodoro Rivadavia'),
('Chubut', 'Trelew'),
('Chubut', 'Puerto Madryn'),
('Chubut', 'Esquel'),
('Córdoba', 'Córdoba'),
('Córdoba', 'Villa Carlos Paz'),
('Córdoba', 'Río Cuarto'),
('Córdoba', 'Villa María'),
('Córdoba', 'Alta Gracia'),
('Corrientes', 'Corrientes'),
('Corrientes', 'Goya'),
('Corrientes', 'Paso de los Libres'),
('Corrientes', 'Esquina'),
('Entre Ríos', 'Paraná'),
('Entre Ríos', 'Concordia'),
('Entre Ríos', 'Gualeguaychú'),
('Entre Ríos', 'Concepción del Uruguay'),
('Formosa', 'Formosa'),
('Formosa', 'Clorinda'),
('Formosa', 'Pirané'),
('Jujuy', 'San Salvador de Jujuy'),
('Jujuy', 'Palpalá'),
('Jujuy', 'San Pedro de Jujuy'),
('Jujuy', 'La Quiaca'),
('La Pampa', 'Santa Rosa'),
('La Pampa', 'General Pico'),
('La Pampa', 'Toay'),
('La Rioja', 'La Rioja'),
('La Rioja', 'Chilecito'),
('La Rioja', 'Aimogasta'),
('Mendoza', 'Mendoza'),
('Mendoza', 'San Rafael'),
('Mendoza', 'Godoy Cruz'),
('Mendoza', 'Maipú'),
('Mendoza', 'Luján de Cuyo'),
('Misiones', 'Posadas'),
('Misiones', 'Oberá'),
('Misiones', 'Eldorado'),
('Misiones', 'Puerto Iguazú'),
('Neuquén', 'Neuquén'),
('Neuquén', 'San Martín de los Andes'),
('Neuquén', 'Cutral Có'),
('Neuquén', 'Plottier'),
('Neuquén', 'Centenario'),
('Río Negro', 'Viedma'),
('Río Negro', 'San Carlos de Bariloche'),
('Río Negro', 'General Roca'),
('Río Negro', 'Cipolletti'),
('Río Negro', 'El Bolsón'),
('Salta', 'Salta'),
('Salta', 'San Ramón de la Nueva Orán'),
('Salta', 'Tartagal'),
('Salta', 'Cafayate'),
('Salta', 'General Güemes'),
('San Juan', 'San Juan'),
('San Juan', 'Chimbas'),
('San Juan', 'Caucete'),
('San Luis', 'San Luis'),
('San Luis', 'Villa Mercedes'),
('San Luis', 'Merlo'),
('Santa Cruz', 'Río Gallegos'),
('Santa Cruz', 'Caleta Olivia'),
('Santa Cruz', 'El Calafate'),
('Santa Fe', 'Santa Fe'),
('Santa Fe', 'Rosario'),
('Santa Fe', 'Rafaela'),
('Santa Fe', 'Venado Tuerto'),
('Santa Fe', 'Reconquista'),
('Santiago del Estero', 'Santiago del Estero'),
('Santiago del Estero', 'La Banda'),
('Santiago del Estero', 'Termas de Río Hondo'),
('Santiago del Estero', 'Frías'),
('Tierra del Fuego', 'Ushuaia'),
('Tierra del Fuego', 'Río Grande'),
('Tierra del Fuego', 'Tolhuin'),
('Tucumán', 'San Miguel de Tucumán'),
('Tucumán', 'Yerba Buena'),
('Tucumán', 'Concepción'),
('Tucumán', 'Tafí Viejo')
) AS v(Provincia, Localidad)
JOIN Provincia p ON p.NombreProvincia = v.Provincia;
go

SELECT (SELECT COUNT(*) FROM Provincia) AS Provincias,
       (SELECT COUNT(*) FROM Localidad) AS Localidades;
go