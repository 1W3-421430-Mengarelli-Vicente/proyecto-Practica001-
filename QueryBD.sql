CREATE DATABASE [PRACTICA_01_PII];
GO

USE [PRACTICA_01_PII];
GO

CREATE TABLE Productos (
	codigo VARCHAR(50),
	nombre VARCHAR(100) NOT NULL,
	marca VARCHAR(100) NOT NULL,
	especificaciones VARCHAR(200),
	precio_unitario DECIMAL(10, 2) NOT NULL DEFAULT 0,
	stock INT NOT NULL,
	fecha_baja DATE NULL
	CONSTRAINT PK_Productos PRIMARY KEY (codigo)
);
GO


CREATE TABLE Formas_Pagos (
    id_forma_pago INT,
    forma_pago VARCHAR(100) NOT NULL,
    CONSTRAINT PK_Formas_Pagos PRIMARY KEY (id_forma_pago)
);
GO

CREATE TABLE Facturas (
    nro_factura INT IDENTITY(1,1),
    fecha DATE NOT NULL DEFAULT GETDATE(),
    id_forma_pago INT NOT NULL,
    cliente VARCHAR(100),
    CONSTRAINT PK_Facturas PRIMARY KEY (nro_factura),
    CONSTRAINT FK_Facturas_Formas_Pagos FOREIGN KEY (id_forma_pago)
    REFERENCES Formas_Pagos(id_forma_pago)
);
GO

CREATE TABLE Detalles_Facturas (
    id_detalle_factura INT IDENTITY(1,1),
    codigo VARCHAR(50) NOT NULL,
    cantidad INT NOT NULL,
    nro_factura INT NOT NULL,
    CONSTRAINT PK_Detalles_Facturas PRIMARY KEY (id_detalle_factura),
    CONSTRAINT FK_Detalles_Facturas_Productos FOREIGN KEY (codigo)
    REFERENCES Productos(codigo),
    CONSTRAINT FK_Detalles_Facturas_Facturas FOREIGN KEY (nro_factura)
    REFERENCES Facturas(nro_factura)
);
GO

INSERT INTO Formas_Pagos (id_forma_pago, forma_pago)
VALUES  (1, 'Efectivo'),
		(2, 'Tarjeta de crédito'),
		(3, 'Tarjeta de débito'),
		(4, 'Transferencia bancaria'),
		(5, 'Mercado Pago');
GO

CREATE PROCEDURE SP_OBTENER_PRODUCTOS
AS
BEGIN
    SELECT * FROM Productos
END
GO

CREATE PROCEDURE SP_OBTENER_PRODUCTO_POR_CODIGO
@codigo varchar(50)
AS
BEGIN
    SELECT * FROM Productos
	WHERE codigo LIKE '%' + @codigo + '%'
END
GO

CREATE PROCEDURE SP_MODIFICAR_PRODUCTOS
@codigo varchar(50) = '',
@nombre varchar(100),
@marca varchar(100),
@especificaciones varchar(200),
@precio_unitario DECIMAL(10, 2),
@stock int
AS 
BEGIN 
	IF NOT EXISTS (SELECT 1 FROM Productos WHERE @codigo = codigo)
	BEGIN
	     INSERT INTO Productos (codigo, nombre, marca, especificaciones, precio_unitario, stock)
	     VALUES (@codigo, @nombre, @marca, @especificaciones, @precio_unitario ,@stock)
	END
	ELSE
	BEGIN	    
        UPDATE Productos
	    SET nombre = @nombre, marca = @marca, especificaciones = @especificaciones, precio_unitario = @precio_unitario ,stock = @stock
	    WHERE codigo = @codigo
    END
END
GO

CREATE PROCEDURE SP_DAR_BAJA_PRODUCTO
@codigo varchar(50)
AS
BEGIN
    UPDATE Productos 
	SET fecha_baja = GETDATE()
	WHERE codigo = @codigo
END	
GO

CREATE PROCEDURE SP_OBTENER_FACTURAS
AS
BEGIN 
    SELECT * FROM Facturas f
    JOIN Formas_Pagos fp ON fp.id_forma_pago = f.id_forma_pago
END
GO

CREATE PROCEDURE SP_OBTENER_FACTURA_POR_ID
@nro_factura int
AS
BEGIN
    SELECT * FROM Facturas WHERE nro_factura = @nro_factura
END
GO

CREATE PROCEDURE SP_MODIFICAR_FACTURAS
@nro_factura int = 0,
@fecha date, 
@id_forma_pago int,
@cliente varchar(100),
@new_id int OUTPUT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Facturas WHERE nro_factura = @nro_factura)
	BEGIN
	    INSERT INTO Facturas (fecha, id_forma_pago, cliente)
	    VALUES (@fecha, @id_forma_pago, @cliente)

		SET @new_id = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
	    UPDATE Facturas
		SET fecha = @fecha, id_forma_pago = @id_forma_pago, cliente = @cliente
		WHERE nro_factura = @nro_factura

		SET @new_id = @nro_factura
	END
END	
GO

CREATE PROCEDURE SP_OBTENER_TOTAL_FACTURA
@nro_factura INT
AS
BEGIN
    SELECT SUM(df.cantidad * p.precio_unitario) AS total
    FROM Detalles_Facturas df
    JOIN Productos p ON p.codigo = df.codigo
    WHERE df.nro_factura = @nro_factura
END
GO

CREATE PROCEDURE SP_AGREGAR_DETALLE
@codigo varchar(50),
@cantidad int,
@nro_factura int
AS
BEGIN 
    INSERT INTO Detalles_Facturas (codigo, cantidad, nro_factura)
	VALUES (@codigo, @cantidad, @nro_factura)
END
GO

CREATE PROCEDURE SP_ELIMINAR_FACTURA
@nro_factura int
AS
BEGIN 
    BEGIN TRY
	    BEGIN TRANSACTION;

		DELETE Detalles_Facturas
	    WHERE nro_factura = @nro_factura
	    DELETE Facturas 
	    WHERE nro_factura = @nro_factura

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
	    ROLLBACK TRANSACTION;
		THROW;
    END CATCH
END
GO

CREATE PROCEDURE SP_OBTENER_METODOS_PAGOS
AS
BEGIN
    SELECT * FROM Formas_Pagos
END
GO

CREATE PROCEDURE SP_OBTENER_DETALLES_FACTURAS
@nro_factura int
AS
BEGIN
    SELECT * FROM Detalles_Facturas df
    JOIN Productos p ON p.codigo = df.codigo
    WHERE df.nro_factura = @nro_factura
END
GO

CREATE PROCEDURE SP_OBTENER_ULTIMA_FACTURA
AS
BEGIN
    SELECT TOP 1 nro_factura FROM Facturas ORDER BY nro_factura DESC
END
GO


INSERT INTO Productos (codigo, nombre, marca, especificaciones, precio_unitario, stock)
VALUES
('P001', 'Teclado Mecánico RGB', 'HyperX', 'Switch Red, cable trenzado', 8500.00, 15),
('P002', 'Mouse Gamer', 'Logitech', 'Sensor Hero, 16000 DPI', 6200.00, 25),
('P003', 'Monitor 24"', 'Samsung', '1080p, 75Hz, HDMI', 28000.00, 10),
('P004', 'Auriculares Inalámbricos', 'Sony', 'Bluetooth, Cancelación de ruido', 17000.00, 8),
('P005', 'Silla Ergonómica', 'Noblechairs', 'Ajustable, soporte lumbar', 56000.00, 5),
('P006', 'Disco SSD 1TB', 'Western Digital', 'SATA III, 560MB/s', 32000.00, 12),
('P007', 'Memoria RAM 16GB', 'Corsair', 'DDR4, 3200MHz', 18000.00, 20),
('P008', 'Procesador Ryzen 5 5600X', 'AMD', '6 núcleos, 12 hilos', 95000.00, 7),
('P009', 'Placa de Video RTX 3060', 'NVIDIA', '12GB GDDR6', 215000.00, 3),
('P010', 'Fuente 650W', 'EVGA', '80+ Bronze, modular', 25000.00, 9),
('P011', 'Gabinete ATX', 'Cooler Master', 'Vidrio templado, RGB', 30000.00, 6),
('P012', 'Disco HDD 2TB', 'Seagate', '7200 RPM, SATA III', 15000.00, 18),
('P013', 'Webcam Full HD', 'Logitech', '1080p, micrófono integrado', 11000.00, 11),
('P014', 'Micrófono USB', 'Blue Yeti', 'Cardioide, condensador', 34000.00, 5),
('P015', 'Notebook 15.6"', 'Lenovo', 'i5, 8GB RAM, 512GB SSD', 380000.00, 4),
('P016', 'Tablet 10"', 'Samsung', 'Octa-Core, 64GB, Android', 95000.00, 7),
('P017', 'Impresora Multifunción', 'HP', 'WiFi, tinta continua', 60000.00, 6),
('P018', 'Router WiFi 6', 'TP-Link', 'Dual Band, AX1800', 23000.00, 10),
('P019', 'Switch de Red 8 Puertos', 'TP-Link', 'Gigabit, metálico', 14000.00, 8),
('P020', 'Lector de Huellas USB', 'ZKTeco', 'Reconocimiento rápido', 18000.00, 3),
('P021', 'Mousepad XL', 'Redragon', '800x300mm, superficie speed', 4000.00, 30),
('P022', 'Cable HDMI 2m', 'Genérico', '4K, trenzado', 2500.00, 40),
('P023', 'Hub USB 4 Puertos', 'Ugreen', 'USB 3.0, alimentación externa', 6500.00, 20),
('P024', 'Enfriador de Notebook', 'Cooler Master', '2 ventiladores, LED azul', 9500.00, 12),
('P025', 'Teclado Inalámbrico', 'Logitech', 'Con touchpad, USB', 10000.00, 14),
('P026', 'Lámpara LED Escritorio', 'Xiaomi', 'Brillo ajustable, USB-C', 8500.00, 10),
('P027', 'Smartwatch', 'Amazfit', 'Sensor ritmo cardíaco, GPS', 42000.00, 9),
('P028', 'Cámara IP WiFi', 'Ezviz', 'Detección de movimiento, app móvil', 15500.00, 8),
('P029', 'SSD Externo 512GB', 'SanDisk', 'USB-C, resistente al agua', 45000.00, 5),
('P030', 'Kit Teclado + Mouse', 'Genius', 'Inalámbrico, USB nano receptor', 9000.00, 22);


INSERT INTO Facturas (fecha, id_forma_pago, cliente)
VALUES
(GETDATE(), 1, 'Juan Pérez'),
(GETDATE(), 2, 'Ana López'),
(GETDATE(), 3, 'Carlos Martínez'),
(GETDATE(), 4, 'Laura Gómez'),
(GETDATE(), 5, 'Diego Fernández');


INSERT INTO Detalles_Facturas (codigo, cantidad, nro_factura)
VALUES
('P001', 2, 1),  -- Juan compró 2 teclados mecánicos
('P002', 1, 1),  -- y un mouse gamer

('P003', 1, 2),  -- Ana compró un monitor
('P007', 2, 2),  -- y 2 RAMs

('P008', 1, 3),  -- Carlos compró un Ryzen 5
('P006', 1, 3),  -- y un SSD

('P004', 1, 4),  -- Laura compró auriculares
('P005', 1, 4),  -- y una silla

('P015', 1, 5),  -- Diego compró una notebook
('P021', 1, 5);  -- y un mousepad XL
