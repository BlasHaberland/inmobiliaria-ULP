CREATE DATABASE IF NOT EXISTS inmobiliaria
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

USE inmobiliaria;

CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) UNIQUE NOT NULL,
    contrasena VARCHAR(255) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    rol ENUM('administrador','empleado') NOT NULL DEFAULT 'empleado',
    avatar VARCHAR(255) NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE propietarios (
    id_propietario INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(50),
    email VARCHAR(100),
    direccion VARCHAR(255),
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE tipos_inmueble (
    id_tipo INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE inmuebles (
    id_inmueble INT AUTO_INCREMENT PRIMARY KEY,
    id_propietario INT NOT NULL,
    id_tipo INT NOT NULL,
    uso ENUM('residencial','comercial') NOT NULL,
    direccion VARCHAR(255) NOT NULL,
    cantidad_ambientes INT,
    coordenadas VARCHAR(100),
    precio DECIMAL(12,2) NOT NULL,
    estado ENUM('disponible','suspendido','ocupado') DEFAULT 'disponible',
    activo BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (id_propietario) REFERENCES propietarios(id_propietario),
    FOREIGN KEY (id_tipo) REFERENCES tipos_inmueble(id_tipo)
);

CREATE TABLE inquilinos (
    id_inquilino INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(50),
    email VARCHAR(100),
    direccion VARCHAR(255),
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE contratos (
    id_contrato INT AUTO_INCREMENT PRIMARY KEY,
    id_inquilino INT NOT NULL,
    id_inmueble INT NOT NULL,
    id_usuario_creador INT NOT NULL,
    id_usuario_finalizador INT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin_original DATE NOT NULL,
    fecha_fin_anticipada DATE NULL,
    monto_mensual DECIMAL(12,2) NOT NULL,
    estado ENUM('vigente','finalizado','rescindido') DEFAULT 'vigente',
    multa DECIMAL(12,2) NULL,
    FOREIGN KEY (id_inquilino) REFERENCES inquilinos(id_inquilino),
    FOREIGN KEY (id_inmueble) REFERENCES inmuebles(id_inmueble),
    FOREIGN KEY (id_usuario_creador) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_usuario_finalizador) REFERENCES usuarios(id_usuario)
);

CREATE TABLE pagos (
    id_pago INT AUTO_INCREMENT PRIMARY KEY,
    id_contrato INT NOT NULL,
    numero_pago INT NOT NULL,
    fecha_pago DATE NOT NULL,
    detalle VARCHAR(255),
    importe DECIMAL(12,2) NOT NULL,
    estado ENUM('activo','anulado') DEFAULT 'activo',
    id_usuario_creador INT NOT NULL,
    id_usuario_anulador INT NULL,
    FOREIGN KEY (id_contrato) REFERENCES contratos(id_contrato),
    FOREIGN KEY (id_usuario_creador) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_usuario_anulador) REFERENCES usuarios(id_usuario)
);