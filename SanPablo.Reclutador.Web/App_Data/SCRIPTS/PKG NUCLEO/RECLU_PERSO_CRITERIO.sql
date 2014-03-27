DROP TABLE RECLU_PERSO_CRITERIO CASCADE CONSTRAINTS
;
CREATE TABLE RECLU_PERSO_CRITERIO
(
  IDERECLUPERSOCRITERIO     NUMBER(8), 
  IDERECLUTAPERSONA         NUMBER(8), 
  IDECRITERIOXSUBCATEGORIA  NUMBER(8), 
  PUNTTOTAL                 NUMBER(2), 
  USRCREACION               VARCHAR2(15), 
  FECCREACION               DATE, 
  USRMODIFICA               VARCHAR2(15), 
  FECMODIFICA               DATE 
)
;

COMMENT ON TABLE RECLU_PERSO_CRITERIO IS 'Tabla que registra las respuestas dadas por la persona en el examen';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.IDERECLUPERSOCRITERIO     IS 'Identificador del puntaje obtenido por criterio en el examen rendido por el postulante';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.IDERECLUTAPERSONA         IS 'identificador de la tabla RECLUTAMIENTO_PERSONA';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.IDECRITERIOXSUBCATEGORIA  IS 'Identificador del criterio x subcategor�a';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.PUNTTOTAL                 IS 'Puntaje total obtenido en el criterio por la persona';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.USRCREACION               IS 'Usuario que cre� el registro';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.FECCREACION               IS 'Fecha de creaci�n del registro';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.USRMODIFICA               IS 'Usuario que modific� el registro';

COMMENT ON COLUMN RECLU_PERSO_CRITERIO.FECMODIFICA               IS 'Fecha de modificaci�n del registro';


ALTER TABLE RECLU_PERSO_CRITERIO ADD CONSTRAINT RECLU_PERSO_CRITERIO_PK 
  PRIMARY KEY (IDERECLUPERSOCRITERIO)  USING INDEX ;

ALTER TABLE RECLU_PERSO_CRITERIO ADD CONSTRAINT RECL_PERS_CRI_IDERECLUPERS_FK 
  FOREIGN KEY (IDERECLUTAPERSONA) REFERENCES RECLUTAMIENTO_PERSONA (IDERECLUTAPERSONA);

ALTER TABLE RECLU_PERSO_CRITERIO ADD CONSTRAINT RECLPERS_CRI_IDECRITXSUBCAT_FK 
  FOREIGN KEY (IDECRITERIOXSUBCATEGORIA) REFERENCES CRITERIO_X_SUBCATEGORIA (IDECRITERIOXSUBCATEGORIA);
 