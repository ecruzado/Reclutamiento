DROP TABLE UBIGEO CASCADE CONSTRAINTS
;
CREATE TABLE UBIGEO
(
	IDEUBIGEO        NUMBER(8) NOT NULL,    -- identificador de registro 
	IDEUBIGEOPADRE   NUMBER(8) NULL,
	NOMBRE           VARCHAR2(50) NOT NULL,    -- nombre del ubigeo 
	CODIGO           VARCHAR2(50) NOT NULL,    -- codigo del ubigeo 
	USRCREACION      VARCHAR2(15) NULL,    -- Usuario Creacion del registro. 
	FECCREACION      DATE NULL,    -- Fecha de creacion del registro. 
	USRMODIFICACION  VARCHAR2(15) NULL,    -- Usuario de modificacion del registro. 
	FECMODIFICACION  DATE NULL    -- Fecha de modificacion. 
)
;

COMMENT ON TABLE UBIGEO IS 'Tabla de ubicaciones geográficas'
;
COMMENT ON COLUMN UBIGEO.IDEUBIGEO        IS 'identificador de registro'
;
COMMENT ON COLUMN UBIGEO.NOMBRE           IS 'nombre del ubigeo'
;
COMMENT ON COLUMN UBIGEO.CODIGO           IS 'codigo del ubigeo'
;
COMMENT ON COLUMN UBIGEO.USRCREACION      IS 'Usuario Creacion del registro.'
;
COMMENT ON COLUMN UBIGEO.FECCREACION      IS 'Fecha de creacion del registro.'
;
COMMENT ON COLUMN UBIGEO.USRMODIFICACION  IS 'Usuario de modificacion del registro.'
;
COMMENT ON COLUMN UBIGEO.FECMODIFICACION  IS 'Fecha de modificacion.'
;

ALTER TABLE UBIGEO ADD CONSTRAINT PK_UBIGEO 
	PRIMARY KEY (IDEUBIGEO) 
 USING INDEX 
;

ALTER TABLE UBIGEO ADD CONSTRAINT FK_UBIGEO_UBIGEO 
	FOREIGN KEY (IDEUBIGEOPADRE) REFERENCES UBIGEO (IDEUBIGEO)
;

