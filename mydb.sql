-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 06-10-2025 a las 00:53:26
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `mydb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `idContratos` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `idUsuario_crea` int(11) NOT NULL,
  `idUsuario_termina` int(11) NOT NULL,
  `monto_mensual` decimal(10,2) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `fecha_fin_original` date NOT NULL,
  `fecha_termina_anticipada` date DEFAULT NULL,
  `multa` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`idContratos`, `idInquilino`, `idInmueble`, `idUsuario_crea`, `idUsuario_termina`, `monto_mensual`, `fecha_inicio`, `fecha_fin`, `fecha_fin_original`, `fecha_termina_anticipada`, `multa`) VALUES
(1, 2, 1, 1, 1, 50000.00, '2025-09-01', '2026-09-08', '2026-09-08', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `idInmuebles` int(11) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `uso` varchar(45) NOT NULL,
  `tipo` int(11) NOT NULL,
  `ambientes` int(11) NOT NULL,
  `direccion` varchar(200) NOT NULL,
  `coordenadas` varchar(50) DEFAULT NULL,
  `precio` decimal(10,2) DEFAULT NULL,
  `estado` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`idInmuebles`, `idPropietario`, `uso`, `tipo`, `ambientes`, `direccion`, `coordenadas`, `precio`, `estado`) VALUES
(1, 1, 'Alquiler', 1, 3, 'Calle Falsa 123, Ciudad', '40.7128,-74.0060', 50000.00, 'Disponible');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `idInquilinos` int(11) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`idInquilinos`, `dni`, `nombre`, `apellido`, `telefono`) VALUES
(2, '87654321', 'María', 'Gómez', '987654321');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `idPagos` int(11) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `id_usuario_crea` int(11) NOT NULL,
  `id_usuario_anula` int(11) DEFAULT NULL,
  `numero_pago` int(11) NOT NULL,
  `fecha_pago` date NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `estado` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`idPagos`, `idContrato`, `id_usuario_crea`, `id_usuario_anula`, `numero_pago`, `fecha_pago`, `importe`, `estado`) VALUES
(1, 1, 1, 1, 1, '2025-09-05', 50000.00, 'pagado');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `idPropietario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `direccion` varchar(100) DEFAULT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `dni` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`idPropietario`, `Nombre`, `Apellido`, `direccion`, `telefono`, `dni`) VALUES
(1, 'Juan', 'Pérez', 'Calle Falsa 123', '123456789', '12345678');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tiposinmueble`
--

CREATE TABLE `tiposinmueble` (
  `idTipo` int(11) NOT NULL,
  `descripcion` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `tiposinmueble`
--

INSERT INTO `tiposinmueble` (`idTipo`, `descripcion`) VALUES
(1, 'Departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `idUsuarios` int(11) NOT NULL,
  `email` varchar(45) NOT NULL,
  `contrasena` varchar(100) NOT NULL,
  `estado` varchar(45) DEFAULT NULL,
  `rol` varchar(20) DEFAULT NULL,
  `avatar` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`idUsuarios`, `email`, `contrasena`, `estado`, `rol`, `avatar`) VALUES
(1, 'admin@inmobiliaria.com', '1234', 'Activo', 'Administrador', '/images/avatars/avatar_1_638952894974978605.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`idContratos`),
  ADD KEY `fk_id_inquilino_idx` (`idInquilino`),
  ADD KEY `fk_id_inmueble_idx` (`idInmueble`),
  ADD KEY `fk_id_usuario_crea_idx` (`idUsuario_crea`),
  ADD KEY `fk_id_usuario_termina_idx` (`idUsuario_termina`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`idInmuebles`),
  ADD KEY `fk_inmuebles_propietario_idx` (`idPropietario`),
  ADD KEY `fk_tipo_inmuebles_idx` (`tipo`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`idInquilinos`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`idPagos`),
  ADD KEY `fk_id_contrato_idx` (`idContrato`),
  ADD KEY `fk_id_usuario_crea_idx` (`id_usuario_crea`),
  ADD KEY `fk_id_usuario_anula_idx` (`id_usuario_anula`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`idPropietario`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tiposinmueble`
--
ALTER TABLE `tiposinmueble`
  ADD PRIMARY KEY (`idTipo`),
  ADD UNIQUE KEY `descripcion_UNIQUE` (`descripcion`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`idUsuarios`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `idInmuebles` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `idInquilinos` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `idPagos` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tiposinmueble`
--
ALTER TABLE `tiposinmueble`
  MODIFY `idTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `idUsuarios` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `fk_id_inmueble` FOREIGN KEY (`idInmueble`) REFERENCES `inmuebles` (`idInmuebles`),
  ADD CONSTRAINT `fk_id_inquilino` FOREIGN KEY (`idInquilino`) REFERENCES `inquilinos` (`idInquilinos`),
  ADD CONSTRAINT `fk_id_usuario_crea` FOREIGN KEY (`idUsuario_crea`) REFERENCES `usuarios` (`idUsuarios`),
  ADD CONSTRAINT `fk_id_usuario_termina` FOREIGN KEY (`idUsuario_termina`) REFERENCES `usuarios` (`idUsuarios`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `fk_inmuebles_propietario` FOREIGN KEY (`idPropietario`) REFERENCES `propietarios` (`idPropietario`),
  ADD CONSTRAINT `fk_tipo_inmuebles` FOREIGN KEY (`tipo`) REFERENCES `tiposinmueble` (`idTipo`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `fk_id_contrato` FOREIGN KEY (`idContrato`) REFERENCES `contratos` (`idContratos`),
  ADD CONSTRAINT `fk_pagos_usuario_anula` FOREIGN KEY (`id_usuario_anula`) REFERENCES `usuarios` (`idUsuarios`),
  ADD CONSTRAINT `fk_pagos_usuario_crea` FOREIGN KEY (`id_usuario_crea`) REFERENCES `usuarios` (`idUsuarios`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
