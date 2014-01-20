DROP TABLE ALTERNATIVA CASCADE CONSTRAINTS;

CREATE TABLE ALTERNATIVA
(
  IDEALTERNATIVA   NUMBER(8), 
  IDECRITERIO      NUMBER(8), 
  ALTERNATIVA      VARCHAR2(150), 
  PESO             NUMBER(2), 
  RUTAIMAGEN       VARCHAR2(50), 
  ESTACTIVO        VARCHAR2(1), 
  USRCREACION      VARCHAR2(15), 
  FECCREACION      DATE, 
  USRMODIFICACION  VARCHAR2(15), 
  FECMODIFICACION  DATE 
)
;

COMMENT ON TABLE ALTERNATIVA IS 'Tabla de alternativas';
COMMENT ON COLUMN ALTERNATIVA.IDEALTERNATIVA   IS 'Identificador de la Tabla.';
COMMENT ON COLUMN ALTERNATIVA.IDECRITERIO      IS 'Identificador foraneo de Tabla Criterio.';
COMMENT ON COLUMN ALTERNATIVA.ALTERNATIVA      IS 'Nombre de la alternativa.';
COMMENT ON COLUMN ALTERNATIVA.PESO             IS 'Peso de la alternativa.';
COMMENT ON COLUMN ALTERNATIVA.RUTAIMAGEN       IS 'Ruta de la Imagen.';
COMMENT ON COLUMN ALTERNATIVA.ESTACTIVO        IS 'Indicador de estado A(activo) y I(inactivo)';
COMMENT ON COLUMN ALTERNATIVA.USRCREACION      IS 'Usuario  de creacion del registro.';
COMMENT ON COLUMN ALTERNATIVA.FECCREACION      IS 'Fecha de creacion del registro.';
COMMENT ON COLUMN ALTERNATIVA.USRMODIFICACION  IS 'Usuario de modificacion del registro.';
COMMENT ON COLUMN ALTERNATIVA.FECMODIFICACION  IS 'Fecha de modificacion del registro.';


ALTER TABLE ALTERNATIVA ADD CONSTRAINT ALTERNATIVA_PK PRIMARY KEY (IDEALTERNATIVA);