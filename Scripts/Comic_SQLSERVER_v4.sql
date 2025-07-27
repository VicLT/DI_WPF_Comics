CREATE TABLE Cliente_VLT (
    ClienteID int IDENTITY (1, 1) PRIMARY KEY,
    Nombre varchar(60),
    Apellido varchar(100),
    NIF varchar(10) NOT NULL,
    Direccion varchar(255)
);

SET IDENTITY_INSERT Cliente_VLT ON;  
INSERT INTO Cliente_VLT (ClienteID, Nombre, Apellido, NIF, Direccion) VALUES
(1, 'Laura', 'Morales', '12345678L', 'Calle de Alcalá, 120'),
(2, 'Carlos', 'Ruiz', '87654321M', 'Avenida de la Constitución, 45'),
(3, 'Marta', 'López', '23456789P', 'Paseo de Gracia, 55'),
(4, 'David', 'García', '34567890Q', 'Calle Gran Vía, 30'),
(5, 'Ana', 'Pérez', '45678901R', 'Calle Larios, 10');
SET IDENTITY_INSERT Cliente_VLT OFF;

CREATE TABLE Cliente_Detalle_Operacion_VLT (
    ClienteID int NOT NULL,
    Detalle_OperacionID int NOT NULL,
    PRIMARY KEY (ClienteID, Detalle_OperacionID),
    FOREIGN KEY (ClienteID) REFERENCES Cliente_VLT(ClienteID),
    FOREIGN KEY (Detalle_OperacionID) REFERENCES Detalle_Operacion(Detalle_OperacionID)
);

INSERT INTO Cliente_Detalle_Operacion_VLT (ClienteID, Detalle_OperacionID) VALUES
(1,3),
(2,2),
(3,5),
(4,1),
(5,4);