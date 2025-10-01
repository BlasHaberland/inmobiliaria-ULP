-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 30-09-2025 a las 11:29:36
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id_contrato` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `id_usuario_creador` int(11) NOT NULL,
  `id_usuario_finalizador` int(11) DEFAULT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin_original` date NOT NULL,
  `fecha_fin_anticipada` date DEFAULT NULL,
  `monto_mensual` decimal(12,2) NOT NULL,
  `estado` enum('vigente','finalizado','rescindido') DEFAULT 'vigente',
  `multa` decimal(12,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id_contrato`, `id_inquilino`, `id_inmueble`, `id_usuario_creador`, `id_usuario_finalizador`, `fecha_inicio`, `fecha_fin_original`, `fecha_fin_anticipada`, `monto_mensual`, `estado`, `multa`) VALUES
(51, 1, 2, 8, 8, '2025-09-26', '2025-12-26', '2025-09-26', 150000.00, 'finalizado', 300000.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id_inmueble` int(11) NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `id_tipo` int(11) NOT NULL,
  `uso` enum('residencial','comercial') NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `cantidad_ambientes` int(11) DEFAULT NULL,
  `coordenadas` varchar(100) DEFAULT NULL,
  `precio` decimal(12,2) NOT NULL,
  `estado` enum('disponible','suspendido','ocupado') DEFAULT 'disponible',
  `activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id_inmueble`, `id_propietario`, `id_tipo`, `uso`, `direccion`, `cantidad_ambientes`, `coordenadas`, `precio`, `estado`, `activo`) VALUES
(1, 1, 1, 'residencial', 'Los Arces 636', 3, '32.79856,13.41234', 100000.00, 'disponible', 1),
(2, 7, 1, 'residencial', 'Cerca Mio 213', 2, '43.1234 , -12.34512', 150000.00, 'disponible', 1),
(4, 7, 1, 'residencial', 'Lejos Mio 543', 2, '-23.435, 12.345', 100000.00, 'disponible', 1),
(5, 10, 1, 'residencial', 'calle Creativa 1001', 2, '12.654523 , 532412', 120000.00, 'disponible', 1),
(6, 9, 3, 'residencial', 'calle Quilmes 123', 4, '12341312 , 131234', 2500000.00, 'disponible', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_inquilino` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id_inquilino`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `direccion`, `activo`) VALUES
(1, '33000000', 'Pedro', 'Blanco', '2664345983', 'pedro@gmail.com', 'calle Los castaños 231', 1),
(2, '56789023', 'Franco', 'Colapinto', '2657324123', 'franco@gmail.com', 'calle Pedro Aznar 001', 1),
(3, '32145723', 'Marco', 'Aurelio', '92615544332', 'marco@gmail.com', 'calle Arco Iris 11', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id_pago` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `numero_pago` int(11) NOT NULL,
  `fecha_vencimiento` date NOT NULL,
  `fecha_pago` date DEFAULT NULL,
  `detalle` varchar(255) DEFAULT NULL,
  `importe` decimal(12,2) NOT NULL,
  `estado` enum('pendiente','realizado','anulado') DEFAULT 'pendiente',
  `id_usuario_creador` int(11) NOT NULL,
  `id_usuario_anulador` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id_pago`, `id_contrato`, `numero_pago`, `fecha_vencimiento`, `fecha_pago`, `detalle`, `importe`, `estado`, `id_usuario_creador`, `id_usuario_anulador`) VALUES
(149, 51, 1, '2025-09-26', '2025-09-26', 'anulado por rescisión de contrato', 150000.00, 'anulado', 8, 8),
(150, 51, 2, '2025-10-26', '2025-09-26', 'anulado por rescisión de contrato', 150000.00, 'anulado', 8, 8),
(151, 51, 3, '2025-11-26', '2025-09-26', 'anulado por rescisión de contrato', 150000.00, 'anulado', 8, 8),
(152, 51, 4, '2025-12-26', '2025-09-26', 'anulado por rescisión de contrato', 150000.00, 'anulado', 8, 8),
(153, 51, 5, '2025-09-26', '2025-09-26', 'pago de multa', 300000.00, 'realizado', 8, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_propietario` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_propietario`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `direccion`, `activo`) VALUES
(1, '40000000', 'Federico', 'Gonzalez', '2664123456', 'fede@gmail.com', 'calle Por ahi 206', 1),
(5, '40031230', 'Facundo', 'Bacaicoa', '2651829102', 'facu@gmail.com', 'calle Falsa 123', 1),
(6, '32145765', 'Fabian', 'Gomez', '1664010010', 'Fabian@gmail.com', 'calle Pinchicha 2000', 1),
(7, '5098713', 'Luis', 'Mercado', '2664909090', 'Luis@gmail.com', 'calle P Sherman Wallaby 42 Sidney ', 1),
(9, '1000000', 'Gian', 'Rossi', '111111111', 'gian@gmail.com', 'calle Siempre Viva 123', 1),
(10, '66342123', 'Luis', 'Borges', '15 34567890', 'luis@gmail.com', 'calle Elm Street 123', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipos_inmueble`
--

CREATE TABLE `tipos_inmueble` (
  `id_tipo` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `descripcion` text DEFAULT NULL,
  `activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipos_inmueble`
--

INSERT INTO `tipos_inmueble` (`id_tipo`, `nombre`, `descripcion`, `activo`) VALUES
(1, 'Local', 'edificio de uso comercial para variedad de rubros', 1),
(2, 'Deposito', 'edificio para uso de almacenamiento ', 1),
(3, 'Casa', 'estructura de solo una piso para uso domestico', 1),
(4, 'Departamento', 'departamento para uso demestico', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `contrasena` varchar(255) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `rol` enum('administrador','empleado') NOT NULL DEFAULT 'empleado',
  `avatar` varchar(255) DEFAULT NULL,
  `fecha_creacion` timestamp NOT NULL DEFAULT current_timestamp(),
  `fecha_actualizacion` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `email`, `contrasena`, `nombre`, `apellido`, `rol`, `avatar`, `fecha_creacion`, `fecha_actualizacion`, `activo`) VALUES
(3, 'jose@gmail.com', 'AQAAAAIAAYagAAAAECgr+genEdMV9nLKfhUS44u8sro7TGBzPqhvfGjw/p/NQl30+vRzqHo+Zl3HSVKd6g==', 'Jose', 'Hernandez', 'administrador', '', '2025-09-05 22:46:41', '2025-09-10 17:42:01', 1),
(6, 'empleado1@gmail.com', 'AQAAAAIAAYagAAAAENgGcb9443GLxWddVGl1HHjkBAwIEykz50kPB1A8vCMfSIkRaVDTXhEAqt5P5eF2xA==', 'Pedro', 'Sanchez', 'empleado', '', '2025-09-10 17:58:13', '2025-09-10 18:00:39', 1),
(8, 'administrador@gmail.com', 'AQAAAAIAAYagAAAAEKbhr03+VlLbSaVpuhAvpYeFF2YM+6TH+BVsPxPtJKdwX6tIjb5Xt2Ie7lZy0oWdQA==', 'Pablo', 'Poder', 'administrador', '78bf3e62-ea7f-461c-ba20-fa5cfa37ad6e.png', '2025-09-10 18:08:25', '2025-09-10 18:08:25', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inquilino` (`id_inquilino`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_usuario_creador` (`id_usuario_creador`),
  ADD KEY `id_usuario_finalizador` (`id_usuario_finalizador`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD KEY `id_propietario` (`id_propietario`),
  ADD KEY `id_tipo` (`id_tipo`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_inquilino`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id_pago`),
  ADD KEY `id_contrato` (`id_contrato`),
  ADD KEY `id_usuario_creador` (`id_usuario_creador`),
  ADD KEY `id_usuario_anulador` (`id_usuario_anulador`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tipos_inmueble`
--
ALTER TABLE `tipos_inmueble`
  ADD PRIMARY KEY (`id_tipo`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=154;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `tipos_inmueble`
--
ALTER TABLE `tipos_inmueble`
  MODIFY `id_tipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilinos` (`id_inquilino`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`id_inmueble`) REFERENCES `inmuebles` (`id_inmueble`),
  ADD CONSTRAINT `contratos_ibfk_3` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuarios` (`id_usuario`),
  ADD CONSTRAINT `contratos_ibfk_4` FOREIGN KEY (`id_usuario_finalizador`) REFERENCES `usuarios` (`id_usuario`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietarios` (`id_propietario`),
  ADD CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`id_tipo`) REFERENCES `tipos_inmueble` (`id_tipo`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contratos` (`id_contrato`),
  ADD CONSTRAINT `pagos_ibfk_2` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuarios` (`id_usuario`),
  ADD CONSTRAINT `pagos_ibfk_3` FOREIGN KEY (`id_usuario_anulador`) REFERENCES `usuarios` (`id_usuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
