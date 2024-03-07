CREATE DATABASE bdcontactos;
use bdcontactos;
CREATE TABLE contactos(
	Idcontactos bigint PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Nombre varchar(50) NOT NULL,
    Telefono varchar(10) NOT NULL,
    Correo varchar(60) NOT NULL, 
	mensaje TEXT NOT NULL
	);

