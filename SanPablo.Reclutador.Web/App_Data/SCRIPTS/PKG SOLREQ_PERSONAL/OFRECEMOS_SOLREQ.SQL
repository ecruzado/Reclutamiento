DROP TABLE OFRECEMOS_SOLREQ CASCADE CONSTRAINTS
;
CREATE TABLE OFRECEMOS_SOLREQ
(
  IDEOFRECEMOSSOLREQ  NUMBER(8), 
  IDESOLREQPERSONAL   NUMBER(8), 
  TIPOFRECIMIENTO     VARCHAR2(3), 
  ESTACTIVO           VARCHAR2(1),
  USRCREACION         VARCHAR2(15), 
  FECCREACION         DATE, 
  USRMODIFICA         VARCHAR2(15), 
  FECMODIFICA         DATE    
)
;

COMMENT ON TABLE OFRECEMOS_SOLREQ IS 'Tabla de ofrecimientos de la empresa por cada cargo'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.IDEOFRECEMOSSOLREQ  IS 'Indentificador de ofrecimientos por cargo'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.IDESOLREQPERSONAL   IS 'Identificador del cargo'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.TIPOFRECIMIENTO     IS 'C�digo del ofrecimiento'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.USRCREACION         IS 'Usuario que cre� el registro'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.FECCREACION         IS 'Fecha de creaci�n del registro'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.USRMODIFICA         IS 'Usuario que modific� el registro'
;
COMMENT ON COLUMN OFRECEMOS_SOLREQ.FECMODIFICA         IS 'Fecha de modificaci�n del registro'
;

ALTER TABLE OFRECEMOS_SOLREQ ADD CONSTRAINT OFRECEMOS_SOLREQ_PK PRIMARY KEY (IDEOFRECEMOSSOLREQ);

ALTER TABLE OFRECEMOS_SOLREQ ADD CONSTRAINT OFRECEMOS_SOLREQ_CARGO_FK FOREIGN KEY (IDESOLREQPERSONAL) REFERENCES SOLREQ_PERSONAL (IDESOLREQPERSONAL);

ALTER TABLE OFRECEMOS_SOLREQ MODIFY IDEOFRECEMOSSOLREQ CONSTRAINT OFREC_SOLREQ_IDEOFRECSOLREQ_NN NOT NULL;

ALTER TABLE OFRECEMOS_SOLREQ MODIFY IDESOLREQPERSONAL  CONSTRAINT OFRECEMOS_SOLREQ_IDECARGO_NN NOT NULL;

CREATE INDEX OFRECEMOS_SOLREQ_IDX_1 ON OFRECEMOS_SOLREQ(IDESOLREQPERSONAL);

