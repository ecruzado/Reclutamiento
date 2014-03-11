create or replace package PR_INTRANET is

  type cur_cursor is REF CURSOR;
  
  FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2,
                         p_valor     IN VARCHAR2)
  RETURN VARCHAR2;
                       
  
  FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER)RETURN NUMBER;   
                          
  PROCEDURE SP_GETREPEXAMEN(P_NIDEXAMEN IN NUMBER,
                          p_cRetVal OUT CUR_CURSOR );       
                          
                          
  FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER)RETURN NUMBER;
  
  FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER)RETURN NUMBER;
  
  FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER,
                               p_nIdOp IN NUMBER)RETURN NUMBER; 
  
                               
  PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino VARCHAR2,
                                   p_ideCargo     CARGO.IDECARGO%TYPE, 
                                   p_valor       IN NUMBER,
                                   p_valorEliminar IN NUMBER); 
  
  PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                             p_ideUsuario    IN CARGO.USRCREACION%TYPE,
                             p_cRetCursor    OUT SYS_REFCURSOR);  
  
 PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea  IN AREA.IDEAREA%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR);
                                  
 PROCEDURE SP_OBTENER_COMPETENCIAREMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                      p_cRetCursor OUT SYS_REFCURSOR);  
                                      
 PROCEDURE SP_OBTENER_OFRECIMIENTO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                        p_cRetCursor OUT SYS_REFCURSOR);
                                       
 PROCEDURE SP_OBTENER_HORARIO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor OUT SYS_REFCURSOR);

 PROCEDURE SP_OBTENER_UBIGEO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR);
                                  
 PROCEDURE SP_OBTENER_NIVELACAD_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                     p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_CENT_EST_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_CONOCIMIENTO_REMP(p_ideSolicitud     IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                        p_tipoConocimiento IN VARCHAR2,
                                        p_cRetCursor OUT SYS_REFCURSOR);
                                        
 PROCEDURE SP_OBTENER_EXPR_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_EVAL_REMP(p_ideSolicitud IN  SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_cRetCursor   OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_DISCAP_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetCursor    OUT SYS_REFCURSOR);
                           
 FUNCTION FN_GET_ROL(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2;
                    
FUNCTION FN_GET_SEDE(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2;
                    
PROCEDURE FN_GET_ROLXUSUARIO(
          p_nIdUsua IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          );                                                                         
          
PROCEDURE FN_GET_SEDEXUSUARIO(
           p_nIdUsua IN NUMBER,
           p_nIdRol IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          );   
          
PROCEDURE FN_GET_OPCIONESxROL(p_nIdRol IN NUMBER,
                              p_ctipMenu IN varchar2,            
                              p_cRetVal OUT CUR_CURSOR
          );
          
          
PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol IN NUMBER,
                                    p_ctipMenu IN varchar2,  
                                   p_cRetVal OUT CUR_CURSOR
          );          
          
             
FUNCTION FN_VALOR_GENERAL(p_tipoTabla IN GENERAL.TIPTABLA%TYPE,
                          p_valor     IN DETALLE_GENERAL.VALOR%TYPE)
                          RETURN DETALLE_GENERAL.DESCRIPCION%TYPE;  
                          
PROCEDURE FN_GET_CARGO(p_nIdCargo IN NUMBER,
                       p_cRetVal OUT CUR_CURSOR
          );
          
PROCEDURE FN_GET_LISTAREQ(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          );
          
PROCEDURE FN_GET_LISTAREQ2(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          );
          
FUNCTION FN_ESTADO_SOLICITUD(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_tipoSolicitud IN VARCHAR2)RETURN VARCHAR2;
end PR_INTRANET;
/
create or replace package body PR_INTRANET is

 /* ------------------------------------------------------------
    Nombre      : SP_LISTA_LVAL
    Proposito   : devuelve una lista de acuerdo al valor del tipo del lval
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2,
                       p_valor     IN VARCHAR2) return VARCHAR2
IS

cDato varchar2(1000);
BEGIN                                                                                                                                                                                                                                                                                          
  
    BEGIN
      SELECT G.DESCRIPCION AS DESCRIPCION
      INTO cDato
      FROM CHSPRP.DETALLE_GENERAL  G 
      WHERE G.IDEGENERAL=P_IDGENERAL
      AND G.VALOR = P_VALOR;
    EXCEPTION
    WHEN OTHERS THEN
     cDato :=null;
    END;
    
RETURN cDato;  
 
END SP_LISTA_LVAL;

 /* ------------------------------------------------------------
    Nombre      : FN_GETMAXPUNTAJE
    Proposito   : devuelve el maximo puntaje del criterio
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER)RETURN NUMBER
IS
 nMaxPuntaje NUMBER;
BEGIN

  BEGIN
   
    SELECT MAX(A.PESO) AS MAXPUNTAJE
     INTO nMaxPuntaje
    FROM ALTERNATIVA A 
    WHERE A.IDECRITERIO=P_NIDCRITERIO;
  
  EXCEPTION
  WHEN OTHERS THEN
    nMaxPuntaje:=0; 
  END;

  RETURN nMaxPuntaje;

end FN_GETMAXPUNTAJE;

 /* ------------------------------------------------------------
    Nombre      : FN_GETREPEXAMEN
    Proposito   : devuelve los datos para el reporte del PDF
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

PROCEDURE SP_GETREPEXAMEN(P_NIDEXAMEN IN NUMBER,
                          p_cRetVal OUT CUR_CURSOR )
IS
 
 
BEGIN
 
  OPEN p_cRetVal FOR
    SELECT EX1.IDEEXAMEN,EX1.IDESEDE,EX1.NOMEXAMEN,EX1.DESCEXAMEN,S1.IDESUBCATEGORIA,
         C1.IDECATEGORIA,C1.NOMCATEGORIA,
         C1.DESCCATEGORIA,C1.TIPCATEGORIA,C1.INSTRUCCIONES,C1.TIPOEJEMPLO,
         C1.IMAGENEJEMPLO,C1.TEXTOEJEMPLO,
         S1.DESCSUBCATEGORIA,S1.NOMSUBCATEGORIA,
         A1.IDECRITERIO,A1.IDEALTERNATIVA,CR1.TIPMODO AS CODMOD,
         DECODE(CR1.TIPMODO,'01','TEXTO','02','IMAGE') AS DESMODO,
         CR1.TIPCRITERIO,
         CR1.PREGUNTA,CR1.IMAGENCRIT,A1.IMAGE,A1.ALTERNATIVA,A1.ESTACTIVO,
         s1.ordenimpresion as ORDENSUB,CS1.PRIORIDAD AS ORDENCRIT,
         (SELECT NVL(SUM(S.TIEMPO),0) 
          FROM   SUBCATEGORIA S
          WHERE  S.IDECATEGORIA= C1.IDECATEGORIA) AS TIEMPOCAT,
          PR_INTRANET.FN_DURACIONEXAMEN(ex1.ideexamen) AS TIMPOEXAMEN
    FROM CHSPRP.CRITERIO CR1,ALTERNATIVA A1,CRITERIO_X_SUBCATEGORIA CS1,
         SUBCATEGORIA S1,CATEGORIA C1, EXAMEN_X_CATEGORIA EC1,Examen EX1
    WHERE
    ex1.ideexamen = EC1.Ideexamen
    and EC1.Idecategoria = c1.idecategoria
    and C1.IDECATEGORIA = S1.IDECATEGORIA
    AND C1.TIPCATEGORIA = CR1.TIPCRITERIO  
    AND CS1.IDESUBCATEGORIA=S1.IDESUBCATEGORIA
    AND CR1.IDECRITERIO=CS1.IDECRITERIO
    AND A1.IDECRITERIO=CR1.IDECRITERIO
    and c1.estactivo='A'
    AND A1.ESTACTIVO='A'
    AND CR1.ESTACTIVO='A'
    AND CS1.ESTREGISTRO='A'
    and ec1.estactivo='A'
    and EX1.Estactivo='A'
    and ex1.ideexamen=P_NIDEXAMEN
    ORDER BY c1.idecategoria,s1.ordenimpresion,
    CS1.Prioridad,A1.IDEALTERNATIVA DESC;
 
END SP_GETREPEXAMEN;

 /* ------------------------------------------------------------
    Nombre      : FN_DURACIONEXAMEN
    Proposito   : devuelve el tiempo de examen total en min
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER)RETURN NUMBER
IS

  nCont Number;
  nTiempo Number;
  CURSOR cData IS
  select c.idecategoria from examen_x_categoria c 
  where c.ideexamen = p_nIdExamen;

BEGIN
  nCont:=0;
  nTiempo:=0;
  
  FOR C1 IN cData LOOP
    BEGIN 
      SELECT NVL(SUM(S.TIEMPO),0) 
      INTO nTiempo
      FROM   SUBCATEGORIA S
      WHERE  S.IDECATEGORIA= C1.IDECATEGORIA;
    EXCEPTION
    WHEN OTHERS THEN
      nTiempo:=0;
    END;
    
    nCont := nCont+nTiempo;
    
  END LOOP;

  RETURN nvl(nCont,0);

END;

/* ------------------------------------------------------------
    Nombre      : FN_ELIMINA_ROL
    Proposito   : Elimina el rol si no depende de un usuario o sede
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER)RETURN NUMBER
IS
nDato NUMBER;
BEGIN


  delete from ROLOPCION where idrol=p_nIdRol;
  delete from rol r where r.idrol=p_nIdRol;
  commit;
  
  nDato :=1;
  
  
  RETURN nDato;

END FN_ELIMINA_ROL;

/* ------------------------------------------------------------
    Nombre      : FN_ELIMINA_ROL_OPCION
    Proposito   : Elimina la opcion asociada ala rol
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER,
                               p_nIdOp IN NUMBER)RETURN NUMBER
IS
nDato NUMBER;
BEGIN

  delete from ROLOPCION r 
  where r.idrol=p_nIdRol 
  and r.idopcion=p_nIdOp;
  commit;
  
  nDato :=1;
  
  
  RETURN nDato;

END FN_ELIMINA_ROL_OPCION;

/* ------------------------------------------------------------
    Nombre      : SP_ACTUALIZAR_PUNTAJES
    Proposito   : Actualizar los puntajes en la tabla cargo 
                  al modificar tablas asociadas
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino IN VARCHAR2,
                                 p_ideCargo     IN CARGO.IDECARGO%TYPE, 
                                 p_valor       IN NUMBER,
                                 p_valorEliminar IN NUMBER)IS

consulta01 VARCHAR2(100);
consulta02 VARCHAR2(100);                                 

BEGIN

consulta01 := 'SELECT '||p_nombreCampoDestino ||
              ' FROM Cargo FOR UPDATE';
consulta02 := 'UPDATE CARGO SET '||p_nombreCampoDestino ||' = '
                                 ||p_nombreCampoDestino||' + '
                                 ||p_valor ||' - '||p_valorEliminar;

EXECUTE IMMEDIATE consulta01;

EXECUTE IMMEDIATE consulta02;

commit;

END SP_ACTUALIZAR_PUNTAJES;   

/* ------------------------------------------------------------
    Nombre      : SP_CREAR_CARGO
    Proposito   : Verificar si el cargo esta creado  o crear el
                  registro y recuperar datos.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                           p_ideUsuario    IN CARGO.USRCREACION%TYPE,
                           p_cRetCursor    OUT SYS_REFCURSOR)IS

nroCont    NUMBER;
nCodCargo  CARGO.CODCARGO%TYPE;
nIdeCargo  CARGO.IDECARGO%TYPE;
nNomCargo  CARGO.NOMCARGO%TYPE;
nDescCargo CARGO.DESCARGO%TYPE;
nIdeArea   CARGO.IDEAREA%TYPE;
nNumPosic  SOLNUEVO_CARGO.NUMPOSICIONES%TYPE;

BEGIN

   SELECT SN.CODCARGO, SN.NOMBRE, SN.DESCRIPCION, SN.IDEAREA, SN.NUMPOSICIONES 
   INTO nCodCargo, nNomCargo, nDescCargo, nIdeArea, nNumPosic
   FROM SOLNUEVO_CARGO SN
   WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud;
   
   SELECT COUNT(*) INTO nroCont FROM CARGO WHERE CODCARGO = nCodCargo;
   IF (nroCont > 0) THEN
     SELECT C.IDECARGO INTO nIdeCargo
     FROM CARGO C
     WHERE C.CODCARGO = nCodCargo;
   END IF;
   
   IF (nIdeCargo IS NULL) THEN
   INSERT  INTO CARGO (IDECARGO,IDESEDE,CODCARGO,NOMCARGO,DESCARGO,IDEAREA,ESTACTIVO,USRCREACION,FECCREACION)  
               VALUES (IDECARGO_SQ.NEXTVAL,1,nCodCargo,nNomCargo,nDescCargo,nIdeArea,'A',p_ideUsuario,SYSDATE);
   COMMIT;
   END IF;
     
   OPEN  p_cRetCursor FOR
      SELECT   C.IDECARGO, C.CODCARGO, C.NOMCARGO, C.DESCARGO,C.NUMPOSICION, AR.NOMAREA, D.NOMDEPARTAMENTO,DE.NOMDEPENDENCIA,nNumPosic NUMPOSIC 
      FROM  CARGO C ,  AREA AR, DEPARTAMENTO D, DEPENDENCIA DE
      WHERE C.CODCARGO = nCodCargo
      AND AR.IDEDEPARTAMENTO = D.IDEDEPARTAMENTO
      AND D.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
      AND AR.IDEAREA = nIdeArea;
  
END SP_OBTENER_CARGO;  

/* ------------------------------------------------------------
    Nombre      : SP_CONSULTAR_DATOS_AREA
    Proposito   : Recuperarr los datos de area, departamento y dependencia
                  del usuario del sistema.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea  IN AREA.IDEAREA%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT AR.IDEAREA, AR.NOMAREA, DE.IDEDEPENDENCIA, DE.NOMDEPENDENCIA, DP.IDEDEPARTAMENTO, DP.NOMDEPARTAMENTO
    FROM AREA AR, DEPENDENCIA DE, DEPARTAMENTO DP
    WHERE AR.IDEDEPARTAMENTO = DP.IDEDEPARTAMENTO
    AND DP.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
    AND AR.IDEAREA = p_ideArea;
END SP_CONSULTAR_DATOS_AREA;

/* ------------------------------------------------------------
    Nombre      : SP_DATOS_COMPETENCIA_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_COMPETENCIAREMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT CS.IDECOMPETENCIASOLREQ,PR_INTRANET.FN_VALOR_GENERAL('TIPCOMPETENCIA',CS.TIPCOMPETEN) DESCRIPCION
    FROM COMPETENCIAS_SOLREQ CS
    WHERE CS.ESTACTIVO = 'A' 
    AND CS.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_COMPETENCIAREMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_OFRECIMIENTO_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_OFRECIMIENTO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                       p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT OS.IDEOFRECEMOSSOLREQ,PR_INTRANET.FN_VALOR_GENERAL('TIPOFRECIMIENTO',OS.TIPOFRECIMIENTO) DESCRIPCION
    FROM OFRECEMOS_SOLREQ OS
    WHERE OS.ESTACTIVO = 'A' 
    AND OS.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_OFRECIMIENTO_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_HORARIO_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_HORARIO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT HS.IDEHORARIOSOLREQ,PR_INTRANET.FN_VALOR_GENERAL('TIPHORARIO',HS.TIPHORARIO) DESCRIPCION,HS.PUNTHORARIO 
    FROM HORARIO_SOLREQ HS
    WHERE HS.ESTACTIVO = 'A' 
    AND HS.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_HORARIO_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_UBIGEO_REMP
    Proposito   : Recuperar los datos de ubigeo de una solicitud 
                  de requerimiento de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_UBIGEO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
   
    OPEN p_cRetCursor FOR
    SELECT US.IDEUBIGEOSOLREQ, US.IDEUBIGEO, UBI.DISTRITO,UBI.PROVINCIA, UBI.DEPARTAMENT, US.PUNTUBIGEO
    FROM UBIGEO_SOLREQ US, UBIGEODESCRIPCION UBI
    WHERE US.ESTACTIVO = 'A' 
    AND US.IDEUBIGEO = UBI.IDUBIGEO
    AND US.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_UBIGEO_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_NIVELACAD_REMP
    Proposito   : Recuperar los datos de nivel academico de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_NIVELACAD_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT  NA.IDENIVELACADESOLREQ,(PR_INTRANET.FN_VALOR_GENERAL('TIPEDUCACION',NA.TIPEDUCACION)) TIPEDUCACION, 
            (PR_INTRANET.FN_VALOR_GENERAL('TIPAREA',NA.TIPAREAESTUDIO)) AREAESTUDIO, (PR_INTRANET.FN_VALOR_GENERAL('NIVELALCANZADO',NA.TIPNIVELCANZADO)) NIVELALCANZADO, 
            NA.CICLOSEMESTRE, NA.PUNTNIVESTUDIO
    FROM NIVELACADEMICO_SOLREQ NA
    WHERE NA.ESTACTIVO = 'A' 
    AND NA.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_NIVELACAD_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_CENT_EST_REMP
    Proposito   : Recuperar los datos de centro de estudios de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_CENT_EST_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT CE.IDECENTESTSOLREQ, (PR_INTRANET.FN_VALOR_GENERAL('TIPTIPINSTIT',CE.TIPCENESTU)) TIPINST , 
          (PR_INTRANET.FN_VALOR_GENERAL('TIPTIPINSTIT',CE.TIPNOMCENESTU)) NOMBINST, CE.PUNTACENTROEST
    FROM CENTROEST_SOLREQ CE
    WHERE CE.ESTACTIVO = 'A' 
    AND CE.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_CENT_EST_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_CONOCIMIENTO_REMP
    Proposito   : Recuperar los datos de conocimientos de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_CONOCIMIENTO_REMP(p_ideSolicitud     IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                       p_tipoConocimiento IN VARCHAR2,
                                       p_cRetCursor OUT SYS_REFCURSOR)IS
                                       
c_condicion VARCHAR2(100);
c_Consulta VARCHAR2(1000);

BEGIN
    IF (p_tipoConocimiento = 'OFIMATICA')THEN
       c_condicion := 'CG.TIPCONOFIMATICA IS NOT NULL';
    ELSIF (p_tipoConocimiento = 'IDIOMA') THEN
       c_condicion := 'CG.TIPIDIOMA IS NOT NULL';
    ELSIF (p_tipoConocimiento = 'GENERAL') THEN
       c_condicion := 'CG.TIPCONOGENERAL IS NOT NULL';
    END IF;         
       
    c_Consulta:= 'SELECT CG.IDECONOGENSOLREQ,(PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOFIMATICA'',CG.TIPCONOFIMATICA)) OFIMATICA, (PR_INTRANET.FN_VALOR_GENERAL(''TIPNOMOFIMATICA'',CG.TIPNOMOFIMATICA)) DESCOFIMATICA,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOIDIOMA'',CG.TIPCONOCIDIOMA)) CONOIDIOMA,(PR_INTRANET.FN_VALOR_GENERAL(''TIPIDIOMA'',CG.TIPIDIOMA)) IDIOMA,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOGRALES'',CG.TIPCONOGENERAL)) GENERAL, (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOGRALES'',CG.TIPNOMCONOCGRALES)) DESCGENERAL,CG.PUNTCONOCIMIENTO,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPNIVELALCAN'',CG.TIPNIVELCONOCIMIENTO)) NIVELCONO
                 FROM CONOGENERAL_SOLREQ CG
                 WHERE CG.ESTACTIVO = ''A'' 
                 AND CG.IDESOLREQPERSONAL = '|| p_ideSolicitud ||
                 ' AND '||c_condicion;
                 
    OPEN p_cRetCursor FOR c_Consulta;  


END SP_OBTENER_CONOCIMIENTO_REMP;


/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_EXPR_REMP
    Proposito   : Recuperar los datos de experiencia de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_EXPR_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT ES.IDEEXPSOLREQ, PR_INTRANET.FN_VALOR_GENERAL('TIPCARGO',ES.TIPEXPLABORAL) EXPERIENCIA, ES.CANTANHOEXP, ES.CANTMESESEXP, ES.PUNTEXPERIENCIA 
    FROM EXPERIENCIA_SOLREQ ES
    WHERE ES.ESTACTIVO = 'A' 
    AND ES.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_EXPR_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_EVAL_REMP
    Proposito   : Recuperar los datos de evaluaciones de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_EVAL_REMP(p_ideSolicitud IN  SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                               p_cRetCursor   OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT EV.IDEEVALUACIONSOLREQ, EX.NOMEXAMEN, EX.DESCEXAMEN,AR.NOMAREA, EV.PUNTEXAMEN,EV.NOTAMINEXAMEN, PR_INTRANET.FN_VALOR_GENERAL('TIPOCRITERIO',EV.TIPEXAMEN) TIPOEXAMEN  
    FROM EVALUACION_SOLREQ EV, EXAMEN EX, AREA AR
    WHERE EV.ESTACTIVO = 'A' 
    AND EV.IDEEXAMEVAL = EX.IDEEXAMEN
    AND EV.TIPAREARESPON = AR.IDEAREA
    AND EV.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_EVAL_REMP;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_DISCAP_REMP
    Proposito   : Recuperar los datos de discapacidades de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  
PROCEDURE SP_OBTENER_DISCAP_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor    OUT SYS_REFCURSOR)IS
BEGIN
    OPEN p_cRetCursor FOR
    SELECT DR.IDEDISCAPASOLREQ, PR_INTRANET.FN_VALOR_GENERAL('TIPDISCAPACIDAD',DR.TIPDISCAPA) DESCDISCAP,DR.PUNTDISCAPA  
    FROM DISCAPACIDAD_SOLREQ DR
    WHERE DR.ESTACTIVO = 'A' 
    AND DR.IDESOLREQPERSONAL = p_ideSolicitud;

END SP_OBTENER_DISCAP_REMP;

/* ------------------------------------------------------------
    Nombre      : FN_GET_ROL
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_GET_ROL(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2
IS
rol varchar2(2000);
cont number;

CURSOR cData IS
    SELECT DISTINCT UR.IDROL,(SELECT R.DSCROL FROM ROL R WHERE R.IDROL=UR.IDROL) DSCROL 
    FROM CHSPRP.USUAROLSEDE UR 
    WHERE UR.IDUSUARIO=p_idUsuario;

BEGIN
  cont := 1;
  FOR C1 IN cData LOOP
      
    BEGIN 
      IF cont=1 THEN
         rol := rol || C1.DSCROL; 
      ELSE
         rol := rol ||', '||C1.DSCROL;
      END IF;    
    cont:=cont+1;
          
    EXCEPTION
    WHEN OTHERS THEN
      rol:=null;
    END;
  
  END LOOP;

  RETURN NVL(rol,'');  
  
END FN_GET_ROL;

/* ------------------------------------------------------------
    Nombre      : FN_GET_SEDE
    Proposito   : Obtiene las sedes por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

FUNCTION FN_GET_SEDE(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2
IS
cSede varchar2(3000);
cont number;

CURSOR cData IS
    SELECT UR.Idesede,(SELECT R.DESCRIPCION FROM SEDE R WHERE R.Idesede=UR.Idesede) DSCSEDE 
    FROM CHSPRP.USUAROLSEDE UR 
    WHERE UR.IDUSUARIO=p_idUsuario;

BEGIN
  cont := 1;
  FOR C1 IN cData LOOP
      
    BEGIN 
    
     IF C1.DSCSEDE IS NOT NULL THEN
        IF cont=1 THEN
           cSede := cSede || C1.DSCSEDE; 
        ELSE
           cSede := cSede ||', '||C1.DSCSEDE;
        END IF;    
        cont:=cont+1;
      END IF;
      
    EXCEPTION
    WHEN OTHERS THEN
      cSede:=null;
    END;
  
  END LOOP;

  RETURN NVL(cSede,'');  
  
END FN_GET_SEDE;


/* ------------------------------------------------------------
    Nombre      : FN_GET_ROLXUSUARIO
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

PROCEDURE FN_GET_ROLXUSUARIO(
           p_nIdUsua IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          )IS
BEGIN
  OPEN p_cRetVal FOR 
    SELECT DISTINCT U.IDROL,(SELECT TRIM(R.CODROL) 
                FROM ROL R 
                WHERE R.IDROL=U.IDROL)CODIGOROL
    FROM CHSPRP.USUAROLSEDE U 
    WHERE( p_nIdUsua = 0 OR U.IDUSUARIO = p_nIdUsua )
    
    order by U.IDROL;


END FN_GET_ROLXUSUARIO; 


/* ------------------------------------------------------------
    Nombre      : FN_GET_SEDEXUSUARIO
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

PROCEDURE FN_GET_SEDEXUSUARIO(
           p_nIdUsua IN NUMBER,
           p_nIdRol IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          )IS
BEGIN
  OPEN p_cRetVal FOR 
    SELECT E.DESCRIPCION,E.IDESEDE 
    FROM CHSPRP.USUAROLSEDE S, SEDE E
    WHERE S.IDESEDE=E.IDESEDE
    AND S.IDUSUARIO=p_nIdUsua
    AND S.IDROL = p_nIdRol
    AND S.IDESEDE<>0
    AND S.IDESEDE IS NOT NULL
    AND E.ESTREGISTRO ='A';

END FN_GET_SEDEXUSUARIO; 


/* ------------------------------------------------------------
    Nombre      : FN_GET_OPCIONESxROL
    Proposito   : Obtiene las copciones de menu por rol
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */


PROCEDURE FN_GET_OPCIONESxROL(p_nIdRol IN NUMBER,
                              p_ctipMenu IN varchar2,            
                              p_cRetVal OUT CUR_CURSOR
          )IS
BEGIN
  

     OPEN P_CRETVAL FOR 
     SELECT OP.IDOPCIONPADRE,OP.IDOPCION,OP.DESCRIPCION,OP.DSCURL,R.IDROL,OP.TIPMENU
     FROM OPCIONES OP,ROLOPCION R
     WHERE OP.FLGHABILITADO='A'
     AND R.IDOPCION = OP.IDOPCION
     AND R.IDROL = P_NIDROL
     and OP.TIPMENU = p_ctipMenu
     ORDER BY OP.IDOPCIONPADRE,OP.IDOPCION;
   
   
   

END FN_GET_OPCIONESxROL; 
/* ------------------------------------------------------------
    Nombre      : FN_GET_OPCIONESPADRExROL
    Proposito   : Obtiene las opciones padre
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol IN NUMBER,
                                    p_ctipMenu IN varchar2,  
                                   p_cRetVal OUT CUR_CURSOR
          )IS
BEGIN
  
 OPEN P_CRETVAL FOR 
     SELECT DISTINCT OP.IDOPCIONPADRE,(SELECT P.DESCRIPCION 
                                    FROM OPCIONES P 
                                    WHERE P.IDOPCIONPADRE=OP.IDOPCIONPADRE
                                    AND P.IDOPCION IS NULL
                                    AND P.TIPMENU = p_ctipMenu
                                    ) DESCRIPCION,
           OP.TIPMENU                         
     FROM OPCIONES OP,ROLOPCION R
     WHERE OP.FLGHABILITADO='A'
     AND R.IDOPCION = OP.IDOPCION
     AND R.IDROL = p_nIdRol
     and OP.TIPMENU = p_ctipMenu
     ORDER BY OP.IDOPCIONPADRE; 
   
END FN_GET_OPCIONESPADRExROL; 

/* ------------------------------------------------------------
    Nombre      : FN_VALOR_GENERAL
    Proposito   : OBTIENE LA DESCRIPCION DE LA TABLA DETALLE GENERAL
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana      Creaci?n    
  ------------------------------------------------------------ */

FUNCTION FN_VALOR_GENERAL(p_tipoTabla IN GENERAL.TIPTABLA%TYPE,
                          p_valor     IN DETALLE_GENERAL.VALOR%TYPE)
                          RETURN DETALLE_GENERAL.DESCRIPCION%TYPE IS

cDescripcion    DETALLE_GENERAL.DESCRIPCION%TYPE;
      
BEGIN
    
    BEGIN
    SELECT DG.DESCRIPCION
    INTO cDescripcion
    FROM DETALLE_GENERAL DG
    WHERE DG.IDEGENERAL = (SELECT G.IDEGENERAL 
                           FROM GENERAL G
                           WHERE G.TIPTABLA = p_tipoTabla)
    AND DG.VALOR = p_valor;
    
    
    EXCEPTION
      WHEN OTHERS THEN
      cDescripcion := '';
    END;
    
    RETURN NVL(cDescripcion,'');
END FN_VALOR_GENERAL; 


/* ------------------------------------------------------------
    Nombre      : FN_GET_CARGO
    Proposito   : Obtiene la lista de cargos o el cargo por el id de cargo
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE FN_GET_CARGO(p_nIdCargo IN NUMBER,
                       p_cRetVal OUT CUR_CURSOR
          )IS
BEGIN
  
    OPEN P_CRETVAL FOR 
    SELECT DISTINCT C.IDECARGO,C.NOMCARGO 
    FROM CARGO C
    WHERE C.ESTACTIVO='A'
    AND ( p_nIdCargo = 0 OR C.IDECARGO = p_nIdCargo )
    ORDER BY C.NOMCARGO;
   
END FN_GET_CARGO; 

/* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
/* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE FN_GET_LISTAREQ(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          )IS
          
cCONSULTA VARCHAR2(2000):=NULL;
cWhere VARCHAR2(1000):=NULL;
cQUERY VARCHAR2(4000):=NULL;
          
BEGIN
  
                 
   cCONSULTA:='SELECT DISTINCT S.IDESOLREQPERSONAL,S.CODSOLREQPERSONAL,S.IDECARGO, ' 
   || ' (SELECT C.NOMCARGO ' 
   || ' FROM CARGO C '
   || ' WHERE C.IDECARGO=S.IDECARGO '
   || ' AND C.IDESEDE=S.IDESEDE) DESCARGO, '
   ||  ' S.IDEDEPENDENCIA, '
   || ' (SELECT D.NOMDEPENDENCIA '
   || ' FROM DEPENDENCIA D '
   || ' WHERE D.IDEDEPENDENCIA = S.IDEDEPENDENCIA '
   || ' AND D.IDESEDE=S.IDESEDE) DESDEPENDENCIA, '
   || ' S.IDEDEPARTAMENTO, '
   || ' (SELECT E.NOMDEPARTAMENTO  '
   || '  FROM DEPARTAMENTO E '
   || '  WHERE E.IDEDEPARTAMENTO=S.IDEDEPARTAMENTO '
   || '  AND E.IDEDEPENDENCIA = S.IDEDEPENDENCIA) DESDEPARTAMENTO, '
   || ' S.IDEAREA, '
   || ' (SELECT A.NOMAREA '
   || '  FROM AREA A '
   || '  WHERE A.IDEAREA = S.IDEAREA '
   || '  AND A.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO) DESAREA, '
   || ' S.NUMVACANTES, '
   || ' 0 POSTULANTE, '
   || ' 0 PRESELECCIONADOS, '
   || ' 0 EVALUADOS, '
   || ' 0 SELECCIONADOS , '
   || ' L.ROLRESPONSABLE ROL , '
   || ' (select codRol from rol r where r.flgestado=''A'' and r.idrol =L.ROLRESPONSABLE ) DESROL, '
   || ' S.ESTACTIVO, '
   || ' L.TIPETAPA, '
   || ' S.FECPUBLICACION, '
   || ' S.FECCREACION, '
   || ' S.FECEXPIRACACION, '
   || ' L.USRESPONSABLE ID_USUARIO_RESP,'
   || ' (SELECT U.CODUSUARIO FROM USUARIO U WHERE U.IDUSUARIO = L.USRESPONSABLE AND ROWNUM<2) NOMPERSONREEMPLAZO,'
   || ' DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO '
   || '  FROM SOLREQ_PERSONAL S,LOGSOLREQ_PERSONAL L '
   || '  WHERE S.IDESOLREQPERSONAL = L.IDESOLREQPERSONAL '
   || '  AND L.FECSUCESO =  (SELECT MAX(P.FECSUCESO) FROM LOGSOLREQ_PERSONAL P '
   || '  WHERE P.IDESOLREQPERSONAL=L.IDESOLREQPERSONAL) ';
   
    IF p_nIdCargo>0 THEN
    
      cWhere := cWhere || ' AND  S.IDECARGO = '||p_nIdCargo;
    
    END IF;
   
    IF p_nIdDependencia>0 THEN
        
      cWhere := cWhere || '  AND S.IDEDEPENDENCIA = '||p_nIdDependencia ;
        
    END IF;
    
    IF p_nIdDepartamento>0 THEN
        
      cWhere := cWhere || '  AND S.IDEDEPARTAMENTO = '||p_nIdDepartamento ;
        
    END IF;

    IF p_nIdArea>0 THEN
        
      cWhere := cWhere || ' AND S.IDEAREA = '||p_nIdArea ;
        
    END IF;
 
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cTipEtapa||''' = L.TIPETAPA ';
                            

        
    END IF;

    IF p_cTipResp IS NOT NULL AND p_cTipResp<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cTipResp||''' = L.ROL ';
        
    END IF;
    
    IF p_cEstado IS NOT NULL AND p_cEstado<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cEstado||''' = L.ESTACTIVO ';
        
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni))>0 AND LENGTH(rtrim(p_cFeFin))>0 THEN
       
      cWhere := cWhere || ' AND s.FECCREACION >= to_date('''||p_cFecIni||''',''DD/MM/YYYY'')'
                       || ' AND s.FECCREACION < to_date('''||p_cFeFin||''',''DD/MM/YYYY'')+1';
        
    END IF;
    
    cWhere := cWhere || 'ORDER BY S.IDESOLREQPERSONAL ';
    
    cQUERY := cCONSULTA || cWhere;                                       

DELETE FROM  LOG_MENSAJE;

INSERT INTO LOG_MENSAJE 
VALUES(cQUERY);
COMMIT;

 OPEN p_cRetVal FOR cQUERY;
   
END FN_GET_LISTAREQ; 

/* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE FN_GET_LISTAREQ2(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          )IS
          
cCONSULTA VARCHAR2(2000):=NULL;
cWhere VARCHAR2(1000):=NULL;
cQUERY VARCHAR2(4000):=NULL;
          
BEGIN
  
                 
   cCONSULTA:='SELECT DISTINCT S.IDESOLREQPERSONAL,S.CODSOLREQPERSONAL,S.IDECARGO, ' 
   || ' (SELECT C.NOMCARGO ' 
   || ' FROM CARGO C '
   || ' WHERE C.IDECARGO=S.IDECARGO '
   || ' AND C.IDESEDE=S.IDESEDE) DESCARGO, '
   ||  ' S.IDEDEPENDENCIA, '
   || ' (SELECT D.NOMDEPENDENCIA '
   || ' FROM DEPENDENCIA D '
   || ' WHERE D.IDEDEPENDENCIA = S.IDEDEPENDENCIA '
   || ' AND D.IDESEDE=S.IDESEDE) DESDEPENDENCIA, '
   || ' S.IDEDEPARTAMENTO, '
   || ' (SELECT E.NOMDEPARTAMENTO  '
   || '  FROM DEPARTAMENTO E '
   || '  WHERE E.IDEDEPARTAMENTO=S.IDEDEPARTAMENTO '
   || '  AND E.IDEDEPENDENCIA = S.IDEDEPENDENCIA) DESDEPARTAMENTO, '
   || ' S.IDEAREA, '
   || ' (SELECT A.NOMAREA '
   || '  FROM AREA A '
   || '  WHERE A.IDEAREA = S.IDEAREA '
   || '  AND A.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO) DESAREA, '
   || ' S.NUMVACANTES, '
   || ' 0 POSTULANTE, '
   || ' 0 PRESELECCIONADOS, '
   || ' 0 EVALUADOS, '
   || ' 0 SELECCIONADOS , '
   || ' L.ROLRESPONSABLE ROL , '
   || ' (select codRol from rol r where r.flgestado=''A'' and r.idrol =L.ROLRESPONSABLE ) DESROL, '
   --|| ' S.ESTACTIVO, '
   || 'PR_INTRANET.FN_ESTADO_SOLICITUD(S.IDESOLREQPERSONAL,''R'') ESTADO,'
   || ' L.TIPETAPA, '
   || ' S.FECPUBLICACION, '
   || ' S.FECCREACION, '
   || ' S.FECEXPIRACACION, '
   || ' L.USRESPONSABLE ID_USUARIO_RESP,'
   || ' (SELECT U.CODUSUARIO FROM USUARIO U WHERE U.IDUSUARIO = L.USRESPONSABLE AND ROWNUM<2) NOMPERSONREEMPLAZO,'
   || ' DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO '
   || '  FROM SOLREQ_PERSONAL S,LOGSOLREQ_PERSONAL L '
   || '  WHERE S.IDESOLREQPERSONAL = L.IDESOLREQPERSONAL '
   || '  AND L.FECSUCESO =  (SELECT MAX(P.FECSUCESO) FROM LOGSOLREQ_PERSONAL P '
   || '  WHERE P.IDESOLREQPERSONAL=L.IDESOLREQPERSONAL) ';
   
    IF p_nIdCargo>0 THEN
    
      cWhere := cWhere || ' AND  S.IDECARGO = '||p_nIdCargo;
    
    END IF;
   
    IF p_nIdDependencia>0 THEN
        
      cWhere := cWhere || '  AND S.IDEDEPENDENCIA = '||p_nIdDependencia ;
        
    END IF;
    
    IF p_nIdDepartamento>0 THEN
        
      cWhere := cWhere || '  AND S.IDEDEPARTAMENTO = '||p_nIdDepartamento ;
        
    END IF;

    IF p_nIdArea>0 THEN
        
      cWhere := cWhere || ' AND S.IDEAREA = '||p_nIdArea ;
        
    END IF;
 
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cTipEtapa||''' = L.TIPETAPA ';
                            

        
    END IF;

    IF p_cTipResp IS NOT NULL AND p_cTipResp<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cTipResp||''' = L.ROL ';
        
    END IF;
    
    IF p_cEstado IS NOT NULL AND p_cEstado<>'' THEN
        
      cWhere := cWhere || ' AND '''||p_cEstado||''' = L.ESTACTIVO ';
        
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni))>0 AND LENGTH(rtrim(p_cFeFin))>0 THEN
       
      cWhere := cWhere || ' AND s.FECCREACION >= to_date('''||p_cFecIni||''',''DD/MM/YYYY'')'
                       || ' AND s.FECCREACION < to_date('''||p_cFeFin||''',''DD/MM/YYYY'')+1';
        
    END IF;
    
    cWhere := cWhere || 'ORDER BY S.IDESOLREQPERSONAL ';
    
    cQUERY := cCONSULTA || cWhere;                                       

DELETE FROM  LOG_MENSAJE;

INSERT INTO LOG_MENSAJE 
VALUES(cQUERY);
COMMIT;

 OPEN p_cRetVal FOR cQUERY;
   
END FN_GET_LISTAREQ2; 
/* ------------------------------------------------------------
    Nombre      : SP_ESTADO_SOLICITUD
    Proposito   : Obtiene el estado deacuerdo el tiempo 
                  transcurrido desde la solicitud
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      11/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_ESTADO_SOLICITUD(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_tipoSolicitud IN VARCHAR2)RETURN VARCHAR2 IS

p_retVal         VARCHAR2(1);                              
c_estado         VARCHAR2(1);
c_fechaSolicitud DATE;
c_diasTrans      NUMBER;
BEGIN
  BEGIN
  
  IF(p_tipoSolicitud = 'N') THEN
  SELECT SN.ESTACTIVO,SN.FECCREACION
    INTO c_estado,c_fechaSolicitud
    FROM SOLNUEVO_CARGO SN
    WHERE SN.IDESOLNUEVOCARGO = p_idSolicitud;
  ELSE 
    SELECT SQ.ESTACTIVO,SQ.FECCREACION
    INTO c_estado,c_fechaSolicitud
    FROM SOLREQ_PERSONAL SQ
    WHERE SQ.IDESOLREQPERSONAL = p_idSolicitud;
  END IF;
  
  IF (c_estado = 'A')THEN
  
  SELECT TO_DATE(SYSDATE) - TO_DATE(c_fechaSolicitud)
  INTO c_diasTrans
  FROM DUAL;
  
  IF (c_diasTrans <= 21) THEN
     p_retVal := 'V';
  ELSIF ((c_diasTrans > 21)AND(c_diasTrans <= 35))THEN
     p_retVal := 'M';
  ELSIF ( c_diasTrans > 35) THEN
     p_retVal := 'R';
  END IF;
  
  ELSE
      p_retVal := c_estado;
  END IF;
  EXCEPTION 
    WHEN OTHERS THEN
    p_retVal := 'I';
    
  END;
  RETURN p_retVal;
  
END FN_ESTADO_SOLICITUD; 



END PR_INTRANET;
/
