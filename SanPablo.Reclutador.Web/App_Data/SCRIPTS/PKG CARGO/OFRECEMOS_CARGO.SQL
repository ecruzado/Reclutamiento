DROP TABLE OFRECEMOS_CARGO CASCADE CONSTRAINTS
;
CREATE TABLE OFRECEMOS_CARGO
(
  IDEOFRECEMOSCARGO  NUMBER(8), 
  IDECARGO           NUMBER(8), 
  TIPOFRECIMIENTO    VARCHAR2(3), 
  ESTACTIVO          VARCHAR2(1),
  USRCREACION        VARCHAR2(15), 
  FECCREACION        DATE, 
  USRMODIFICA        VARCHAR2(15), 
  FECMODIFICA        DATE    
)
;

COMMENT ON TABLE OFRECEMOS_CARGO IS 'Tabla de ofrecimientos de la empresa por cada cargo'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.IDEOFRECEMOSCARGO  IS 'Indentificador de ofrecimientos por cargo'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.IDECARGO           IS 'Identificador del cargo'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.TIPOFRECIMIENTO    IS 'C�digo del ofrecimiento'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.USRCREACION        IS 'Usuario que cre� el registro'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.FECCREACION        IS 'Fecha de creaci�n del registro'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.USRMODIFICA        IS 'Usuario que modific� el registro'
;
COMMENT ON COLUMN OFRECEMOS_CARGO.FECMODIFICA        IS 'Fecha de modificaci�n del registro'
;

ALTER TABLE OFRECEMOS_CARGO ADD CONSTRAINT OFRECEMOS_CARGO_PK PRIMARY KEY (IDEOFRECEMOSCARGO);

ALTER TABLE OFRECEMOS_CARGO ADD CONSTRAINT OFRECEMOS_CARGO_CARGO_FK FOREIGN KEY (IDECARGO) REFERENCES CARGO (IDECARGO);

ALTER TABLE OFRECEMOS_CARGO MODIFY IDEOFRECEMOSCARGO CONSTRAINT OFREC_CARGO_IDEOFRECCARGO_NN NOT NULL;

ALTER TABLE OFRECEMOS_CARGO MODIFY IDECARGO  CONSTRAINT OFRECEMOS_CARGO_IDECARGO_NN NOT NULL;

CREATE INDEX OFRECEMOS_CARGO_IDX_1 ON OFRECEMOS_CARGO(IDECARGO);

