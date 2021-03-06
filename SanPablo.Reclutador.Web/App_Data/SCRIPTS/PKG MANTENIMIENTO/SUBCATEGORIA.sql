DROP TABLE SUBCATEGORIA CASCADE CONSTRAINTS;

CREATE TABLE SUBCATEGORIA
(
  IDESUBCATEGORIA   NUMBER(8), 
  IDECATEGORIA      NUMBER(8), 
  ORDENIMPRESION    NUMBER(3), 
  NOMSUBCATEGORIA   VARCHAR2(25), 
  DESCSUBCATEGORIA  VARCHAR2(100), 
  INSTRUCCIONES     VARCHAR2(255),
  INDEJEMPLO        VARCHAR2(1),
  ESTACTIVO         VARCHAR2(1), 
  USRCREACION       VARCHAR2(15), 
  FECCREACION       DATE, 
  USRMODIFICACION   VARCHAR2(50), 
  FECMODIFICACION   DATE
)
;

COMMENT ON TABLE SUBCATEGORIA IS 'Tabla de subcategorías';
COMMENT ON COLUMN SUBCATEGORIA.IDESUBCATEGORIA   IS 'Identificador de la tabla.';
COMMENT ON COLUMN SUBCATEGORIA.IDECATEGORIA      IS 'Identificador Foranero de la Tabla Categoria.';
COMMENT ON COLUMN SUBCATEGORIA.ORDENIMPRESION    IS 'Orden de impresion';
COMMENT ON COLUMN SUBCATEGORIA.NOMSUBCATEGORIA   IS 'Nombre de la SubCategoria.';
COMMENT ON COLUMN SUBCATEGORIA.DESCSUBCATEGORIA  IS 'Descripcion de la subcategoria.';
COMMENT ON COLUMN SUBCATEGORIA.ESTACTIVO         IS 'Indicador de estado A(activo) y I(inactivo)';
COMMENT ON COLUMN SUBCATEGORIA.USRCREACION       IS 'Usuario de creacion.';
COMMENT ON COLUMN SUBCATEGORIA.FECCREACION       IS 'Fecha de creacion.';
COMMENT ON COLUMN SUBCATEGORIA.USRMODIFICACION   IS 'Usuario de modificacion.';
COMMENT ON COLUMN SUBCATEGORIA.FECMODIFICACION   IS 'Fecha de modificacion.';



ALTER TABLE SUBCATEGORIA ADD CONSTRAINT SUBCATEGORIA_PK PRIMARY KEY (IDESUBCATEGORIA);

ALTER TABLE SUBCATEGORIA MODIFY NOMSUBCATEGORIA CONSTRAINT SUBCATEGOR_NOMSUBCAT_NN NOT NULL ;

ALTER TABLE SUBCATEGORIA MODIFY DESCSUBCATEGORIA CONSTRAINT SUBCATEGOR_DESCSUBCAT_NN NOT NULL ;

ALTER TABLE SUBCATEGORIA MODIFY INSTRUCCIONES CONSTRAINT SUBCATEGOR_INSTRUCCIONES_NN NOT NULL ;

ALTER TABLE SUBCATEGORIA MODIFY ESTACTIVO CONSTRAINT SUBCATEGOR_ESTACTIVO_NN NOT NULL ;
