DROP TABLE EXPERIENCIA_CARGO CASCADE CONSTRAINTS;

CREATE TABLE EXPERIENCIA_CARGO
(
  IDEEXPCARGO      NUMBER(8), 
  IDECARGO         NUMBER(8), 
  TIPEXPLABORAL    VARCHAR2(3), 
  CANTANHOEXP      NUMBER(2), 
  CANTMESESEXP     NUMBER(2),
  PUNTEXPERIENCIA  NUMBER(2), 
  ESTACTIVO        VARCHAR2(1),
  USRCREACION      VARCHAR2(15), 
  FECCREACION      DATE, 
  USRMODIFICA      VARCHAR2(15), 
  FECMODIFICA      DATE 
)
;

COMMENT ON TABLE EXPERIENCIA_CARGO IS 'Tabla de experiencia laboral por cargo';
COMMENT ON COLUMN EXPERIENCIA_CARGO.IDEEXPCARGO      IS 'Identificador de experiencia por cargo';
COMMENT ON COLUMN EXPERIENCIA_CARGO.IDECARGO         IS 'Identificador del cargo';
COMMENT ON COLUMN EXPERIENCIA_CARGO.TIPEXPLABORAL    IS 'C�digo experiencia laboral';
COMMENT ON COLUMN EXPERIENCIA_CARGO.CANTANHOEXP      IS 'Cantidad de a�os de experiencia';
COMMENT ON COLUMN EXPERIENCIA_CARGO.PUNTEXPERIENCIA  IS 'Puntaje por experiencia';
COMMENT ON COLUMN EXPERIENCIA_CARGO.USRCREACION      IS 'Usuario que cre� el registro';
COMMENT ON COLUMN EXPERIENCIA_CARGO.FECCREACION      IS 'Fecha de creaci�n del registro';
COMMENT ON COLUMN EXPERIENCIA_CARGO.USRMODIFICA      IS 'Usuario que modific� el registro';
COMMENT ON COLUMN EXPERIENCIA_CARGO.FECMODIFICA      IS 'Fecha de modificaci�n del registro';

ALTER TABLE EXPERIENCIA_CARGO ADD CONSTRAINT EXPERIENCIA_CARGO_PK PRIMARY KEY (IDEEXPCARGO);

ALTER TABLE EXPERIENCIA_CARGO ADD CONSTRAINT EXPERIENCIA_CARGO_CARGO_FK FOREIGN KEY (IDECARGO) REFERENCES CARGO (IDECARGO);
 
ALTER TABLE EXPERIENCIA_CARGO MODIFY IDEEXPCARGO CONSTRAINT EXPERIENCIA_IDEEXPCARGO_NN NOT NULL;

ALTER TABLE EXPERIENCIA_CARGO MODIFY IDECARGO  CONSTRAINT EXPERIENCIA_IDECARGO_NN NOT NULL;

CREATE INDEX EXPERIENCIA_CARGO_IDX_1 ON EXPERIENCIA_CARGO(IDECARGO);