DROP TABLE CATEGORIA CASCADE CONSTRAINTS;

CREATE TABLE CATEGORIA
(
  IDECATEGORIA    NUMBER(8), 
  ORDENIMPRESION  NUMBER(8),
  NOMCATEGORIA    VARCHAR2(20), 
  DESCCATEGORIA   VARCHAR2(150), 
  TIPCATEGORIA    VARCHAR2(3), 
  ESTACTIVO       VARCHAR2(1), 
  USRCREACION     VARCHAR2(15), 
  FECCREACION     DATE, 
  USRMODIFICA     VARCHAR2(15), 
  FECMODIFICA     DATE 
)
;

COMMENT ON TABLE CATEGORIA IS 'Tabla de categorķas';
COMMENT ON COLUMN CATEGORIA.IDECATEGORIA    IS 'Identificador de la Tabla.';
COMMENT ON COLUMN CATEGORIA.NOMCATEGORIA    IS 'Nombre de la Categoria.';
COMMENT ON COLUMN CATEGORIA.DESCCATEGORIA   IS 'Descripcion de la Categoria.';
COMMENT ON COLUMN CATEGORIA.TIPCATEGORIA    IS 'Tipo de la Categoria. Identificador foraneo de Tabla General.';
COMMENT ON COLUMN CATEGORIA.ESTACTIVO       IS 'Indicador de estado A(activo) y I(inactivo)';
COMMENT ON COLUMN CATEGORIA.USRCREACION     IS 'Usuario de Creacion del registro.';
COMMENT ON COLUMN CATEGORIA.FECCREACION     IS 'Fecha de creacion del registro.';
COMMENT ON COLUMN CATEGORIA.USRMODIFICA     IS 'Usuario de modificacion del registro.';
COMMENT ON COLUMN CATEGORIA.FECMODIFICA     IS 'Fecha de modificacion del registro.';


ALTER TABLE CATEGORIA ADD CONSTRAINT CATEGORIA_PK  PRIMARY KEY (IDECATEGORIA);

ALTER TABLE CATEGORIA MODIFY NOMCATEGORIA CONSTRAINT CATEGORIA_NOMCATEGORIA_NN NOT NULL ;

ALTER TABLE CATEGORIA MODIFY DESCCATEGORIA CONSTRAINT CATEGORIA_DESCCATEGORIA_NN NOT NULL ;

ALTER TABLE CATEGORIA MODIFY TIPCATEGORIA CONSTRAINT CATEGORIA_TIPCATEGORIA_NN NOT NULL ;

ALTER TABLE CATEGORIA MODIFY ESTACTIVO CONSTRAINT CATEGORIA_ESTACTIVO_NN NOT NULL ;