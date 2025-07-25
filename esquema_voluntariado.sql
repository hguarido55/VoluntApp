
-- Tabla principal: Usuario
CREATE TABLE Usuario (
    DNI INT PRIMARY KEY,
    Horas_Registradas INT,
    Telefono INT,
    Nombre VARCHAR(100),
    Apellidos VARCHAR(100),
    Email VARCHAR(100),
    Hash_Contrasena VARCHAR(255),
    Disponibilidad BOOLEAN
);

-- Tabla Voluntariado
CREATE TABLE Voluntariado (
    ID_Voluntariado INT PRIMARY KEY,
    Plazas INT,
    Descripcion TEXT,
    Tipo VARCHAR(50),
    Ubicacion VARCHAR(100),
    Fecha_Inicio DATETIME,
    Fecha_Fin DATETIME,
    Estado VARCHAR(50)
);

-- Tabla ONG
CREATE TABLE ONG (
    ID_ONG INT PRIMARY KEY,
    Nombre VARCHAR(100),
    Telefono INT
);

-- Tabla Reporte
CREATE TABLE Reporte (
    ID_Reporte INT PRIMARY KEY,
    Duracion INT,
    Fecha DATETIME,
    Descripcion TEXT
);

-- Relación N:M entre Usuario y Voluntariado con atributos: Estado y Descripcion
CREATE TABLE Inscribe (
    DNI INT,
    ID_Voluntariado INT,
    Estado VARCHAR(50),
    Descripcion TEXT,
    PRIMARY KEY (DNI, ID_Voluntariado),
    FOREIGN KEY (DNI) REFERENCES Usuario(DNI),
    FOREIGN KEY (ID_Voluntariado) REFERENCES Voluntariado(ID_Voluntariado)
);

-- Relación 1:N de ONG publica Voluntariado
CREATE TABLE Publica (
    ID_ONG INT,
    ID_Voluntariado INT PRIMARY KEY,
    FOREIGN KEY (ID_ONG) REFERENCES ONG(ID_ONG),
    FOREIGN KEY (ID_Voluntariado) REFERENCES Voluntariado(ID_Voluntariado)
);

-- Relación 1:N de ONG genera Reporte
CREATE TABLE Genera (
    ID_ONG INT,
    ID_Reporte INT PRIMARY KEY,
    FOREIGN KEY (ID_ONG) REFERENCES ONG(ID_ONG),
    FOREIGN KEY (ID_Reporte) REFERENCES Reporte(ID_Reporte)
);