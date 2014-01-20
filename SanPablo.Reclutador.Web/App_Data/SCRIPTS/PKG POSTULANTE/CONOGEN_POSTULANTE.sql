DROP TABLE CONOGEN_POSTULANTE CASCADE CONSTRAINTS;

CREATE TABLE CONOGEN_POSTULANTE
(
  IDECONOGENPOSTULANTE  NUMBER(8), 
  IDEPOSTULANTE         NUMBER(8), 
  TIPCONOFIMATICA       VARCHAR2(3), 
  TIPNOMOFIMATICA       VARCHAR2(3), 
  TIPIDIOMA             VARCHAR2(3),
  TIPCONOCIDIOMA        VARCHAR2(3),
  TIPCONOCGENERALES     NVARCHAR2(3),
  TIPNOMCONOCGRALES     VARCHAR2(3),
  NOMCONOCGRALES        VARCHAR2(50),
  TIPNIVELCONOCIMIENTO  VARCHAR2(3),
  INDCERTIFICACION      VARCHAR2(1),
  ESTACTIVO             VARCHAR2(1), 
  USRCREACION           VARCHAR2(15), 
  FECCREACION           DATE, 
  USRMODIFICACION       VARCHAR2(15),
  FECMODIFICACION       DATE 
)
;

COMMENT ON TABLE CONOGEN_POSTULANTE IS 'Tabla de conocimientos generales de la persona';
COMMENT ON COLUMN CONOGEN_POSTULANTE.IDECONOGENPOSTULANTE  IS 'Identificador de conocimientos generales de la persona';
COMMENT ON COLUMN CONOGEN_POSTULANTE.IDEPOSTULANTE         IS 'Identificador de la persona(Postulante)';
COMMENT ON COLUMN CONOGEN_POSTULANTE.TIPCONOFIMATICA       IS 'C�digo del tipo de conocimiento';
COMMENT ON COLUMN CONOGEN_POSTULANTE.TIPNOMOFIMATICA       IS 'C�digo del conocimiento general';
COMMENT ON COLUMN CONOGEN_POSTULANTE.ESTACTIVO             IS 'Indicador de estado A(activo) y I(inactivo)';
COMMENT ON COLUMN CONOGEN_POSTULANTE.USRCREACION           IS 'Usuario que cre� el registro';
COMMENT ON COLUMN CONOGEN_POSTULANTE.FECCREACION           IS 'Fecha de creaci�n del registro';
COMMENT ON COLUMN CONOGEN_POSTULANTE.FECMODIFICACION       IS 'Fecha de modificaci�n del registro';


ALTER TABLE CONOGEN_POSTULANTE ADD CONSTRAINT CONOGEN_POSTULANTE_PK PRIMARY KEY (IDECONOGENPOSTULANTE);

ALTER TABLE CONOGEN_POSTULANTE ADD CONSTRAINT CONOGEN_POSTULAN_POSTULANTE_FK FOREIGN KEY (IDEPOSTULANTE) REFERENCES POSTULANTE (IDEPOSTULANTE);


ALTER TABLE CONOGEN_POSTULANTE MODIFY ESTACTIVO CONSTRAINT CONOGEN_POSTE_ESTACTIVO_NN NOT NULL;
