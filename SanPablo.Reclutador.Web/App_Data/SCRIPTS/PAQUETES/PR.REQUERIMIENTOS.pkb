CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is


FUNCTION FN_APROBACION_NUEVO(p_ideSede     IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE)RETURN NUMBER;
                              
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR);
                                     
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                            RETURN VARCHAR2;

FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                           RETURN VARCHAR2;
                           
FUNCTION FN_VERIFICAR_CODCARGO(p_codCargo IN SOLNUEVO_CARGO.CODCARGO%TYPE)RETURN NUMBER ;


PROCEDURE COPIA_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                             p_ideSolReqPersonal IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_usrCreacion IN SOLREQ_PERSONAL.USRCREACION%TYPE);
                             
FUNCTION SP_INSERTAR_AMPLIACION(p_ideCargo         IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                                p_ideSede          IN SEDE.IDESEDE%TYPE,
                                p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                p_ideArea          IN AREA.IDEAREA%TYPE,
                                p_numVacantes      IN SOLREQ_PERSONAL.NUMVACANTES%TYPE,
                                p_motivo           IN SOLREQ_PERSONAL.MOTIVO%TYPE,
                                p_observacion      IN SOLREQ_PERSONAL.OBSERVACION%TYPE,
                                p_ideUsuarioSuceso IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso     IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_cEtapa           IN DETALLE_GENERAL.VALOR%TYPE,
                                p_responsableSig   IN ROL.CODROL%TYPE,
                                p_tipoSolicitud    IN SOLREQ_PERSONAL.TIPSOL%TYPE,
                                p_indicArea        IN BOOLEAN)RETURN NUMBER ; 

END PR_REQUERIMIENTOS;
/
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is

/* ------------------------------------------------------------
    Nombre      : FN_APROBACION_NUEVO
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_APROBACION_NUEVO(p_ideSede      IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE)RETURN NUMBER IS
                             
                        
c_ideUsuarioResp USUARIO.IDUSUARIO%TYPE;
c_ideLogSolicitud LOGSOLNUEVO_CARGO.IDELOGSOLNUEVOCARGO%TYPE;
c_ideRolRespSuc ROL.IDROL%TYPE;
--c_etapaSgte DETALLE_GENERAL.VALOR%TYPE;    
c_ideRolResp ROL.IDROL%TYPE;
c_ideTipoReq CARGO.TIPREQUERIMIENTO%TYPE;

BEGIN   
  IF (p_etapa = '01') THEN-- 01 envio solicitud -ETAPA PENDIENTE SOLICITUD
   c_ideRolResp := 10;     --GERENTE AREA
   
  ELSIF (p_etapa = '02')THEN --02 aprob/rechazo 
   c_ideRolResp := 5; --GGA 
   
  ELSIF (p_etapa = '03')THEN --elaboracion perfil 
   c_ideRolResp := 6;   --jefe procesos
  
  ELSIF (p_etapa = '04')THEN   --pendiente aprobacion perfil
   c_ideRolResp := 3;  --jefe de area
  
  ELSIF (p_etapa = '05')THEN   --pendiente aprobacion perfil2  
   c_ideRolResp := 7;  --encargado seleccion
  
  ELSIF (p_etapa = '06')THEN   --pendiente publicacion 
   c_ideRolResp := 9; --destinatario recursos 
  ELSIF (p_etapa = '07')THEN   --determinar el usuario asignado 
   c_ideRolResp := -1; --destinatario recursos 
    
  END IF;
  
  IF (c_ideRolResp <> -1) THEN
   SELECT  U.IDUSUARIO
   INTO c_ideUsuarioResp
   FROM USUARIO_NIVEL U, ROL R , USUAROLSEDE UR
   WHERE U.IDUSUARIO = UR.IDUSUARIO
   AND R.IDROL = UR.IDROL
   AND R.IDROL = c_ideRolResp
   AND U.IDEAREA = p_ideArea
   AND U.IDESEDE = p_ideSede
   AND U.FLGESTADO ='A'; -- ESTACTIVO
  ELSE
   c_ideUsuarioResp :=-1;
  END IF; 
  
   --ultimo registro de log de solicitud
   IF( p_etapa <> '01')THEN      
     SELECT LG.IDELOGSOLNUEVOCARGO, LG.ROLRESPONSABLE
     INTO c_ideLogSolicitud, c_ideRolRespSuc
     FROM LOGSOLNUEVO_CARGO LG   
     WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                        FROM  LOGSOLNUEVO_CARGO SN
                        WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
     AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo;  
   ELSE
     c_ideRolRespSuc := p_ideRol;
   END IF;
    
   
   BEGIN
     --   AND ESTADO = "PENDIENTE"
      IF (p_ideRol = c_ideRolRespSuc) THEN
        IF( p_etapa <> '01')THEN
          UPDATE LOGSOLNUEVO_CARGO LS
          SET LS.TIPSUCESO = p_suceso
          WHERE LS.IDELOGSOLNUEVOCARGO = c_ideLogSolicitud;
        END IF;    
        IF(p_suceso <> 'R') THEN --DIFRENTE A RECHAZADO
          IF (p_etapa <> '07') THEN 
            INSERT INTO LOGSOLNUEVO_CARGO 
            (IDELOGSOLNUEVOCARGO, IDESOLNUEVOCARGO,TIPETAPA, TIPSUCESO,OBSERVACION,FECSUCESO,USRSUCESO,ROLSUCESO, USRESPONSABLE,ROLRESPONSABLE)
            VALUES(IDESOLNUEVOCARGO_SQ.NEXTVAL,p_ideSolCargo,p_etapa,'P',p_observacion,sysdate,p_ideUsuario,p_ideRol,c_ideUsuarioResp,c_ideRolResp);
         END IF;
       END IF;
     END IF;
   COMMIT;
     
   EXCEPTION
       WHEN OTHERS THEN  
       c_ideUsuarioResp:= -1;
       ROLLBACK;
   END;
   
   RETURN c_ideUsuarioResp;
END FN_APROBACION_NUEVO;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_ETAPA_SOLICITUD
    Proposito   : Obtener la etapa y estado actual de la solicitud de cargo.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR)IS

BEGIN

  BEGIN
  OPEN p_cRetVal FOR
  SELECT LG.TIPETAPA,LG.TIPSUCESO,LG.ROLRESPONSABLE
  FROM LOGSOLNUEVO_CARGO LG   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
  AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo; 

  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:=NULL;
  END;   
  
END SP_OBTENER_ETAPA_SOLICITUD;


/* ------------------------------------------------------------
    Nombre      : FN_RESPONSABLE_SOL
    Proposito   : Obtener la etapa y estado actual de la solicitud de cargo.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
  SELECT R.DSCROL
  INTO c_responsable
  FROM LOGSOLNUEVO_CARGO LG , ROL R   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
  AND R.IDROL = LG.ROLRESPONSABLE;

  RETURN NVL(c_responsable,'');  
  
END FN_RESPONSABLE_SOL;

/* ------------------------------------------------------------
    Nombre      : FN_NOMRESPONS_SOL
    Proposito   : Obtener el nombre del responsable del suceso
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
  SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
  INTO c_responsable
  FROM LOGSOLNUEVO_CARGO LG , USUARIO US   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
  AND US.IDUSUARIO = LG.USRESPONSABLE;

  RETURN NVL(c_responsable,'');  
  
END FN_NOMRESPONS_SOL;


/* ------------------------------------------------------------
    Nombre      : FN_VERIFICAR_CODCARGO
    Proposito   : verificar que el codigo de cargo este disponible
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      27/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_VERIFICAR_CODCARGO(p_codCargo IN SOLNUEVO_CARGO.CODCARGO%TYPE)RETURN NUMBER 

IS
cCount NUMBER;

BEGIN
  SELECT COUNT(SN.CODCARGO) INTO cCount
  FROM SOLNUEVO_CARGO SN   
  WHERE UPPER(SN.CODCARGO) = UPPER(p_codCargo);

  RETURN NVL(cCount,0);
  
END FN_VERIFICAR_CODCARGO;

/* ------------------------------------------------------------
    Nombre      : OBTENER_CONOCIMIENTOS_CARGO
    Proposito   : Obtener los conocimientos generales
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

/*PROCEDURE OBTENER_CONOCIMIENTOS_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE)IS

CURSOR c_Conocimientos IS
   SELECT *
   FROM CONOGENERAL_CARGO CG
   WHERE CG.IDECARGO = p_ideCargo;
      
BEGIN

   OPEN c_Conocimientos;
   FETCH c_Tabla INTO cValorTabla;
   IF c_Tabla%NOTFOUND THEN 
     IF p_cCodTabla IS NOT NULL THEN 
       cValorTabla := '';
     END IF;  
   END IF;
   CLOSE c_Tabla;

     RETURN cValorTabla;
END OBTENER_CONOCIMIENTOS_CARGO;*/


/* ------------------------------------------------------------
    Nombre      : COPIA_CARGO
    Proposito   : Hacer una copia del cargo a las tablas de 
                  ampliacion de cargo
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      28/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE COPIA_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                             p_ideSolReqPersonal IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_usrCreacion IN SOLREQ_PERSONAL.USRCREACION%TYPE) IS

c_ideCodigo OFRECEMOS_SOLREQ.TIPOFRECIMIENTO%TYPE;
c_ideSolReqPersonal SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE;
CURSOR c_Ofrecemos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT O.TIPOFRECIMIENTO 
  FROM OFRECEMOS_CARGO O 
  WHERE O.IDECARGO =  p_ideCargo;
  
CURSOR c_competencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT C.TIPCOMPETEN 
  FROM COMPETENCIAS_CARGO C 
  WHERE C.IDECARGO =  p_ideCargo;

CURSOR c_horarios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT H.TIPHORARIO, H.PUNTHORARIO 
  FROM HORARIO_CARGO H 
  WHERE H.IDECARGO =  p_ideCargo;

CURSOR c_ubigeos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT UB.IDEUBIGEO, UB.PUNTUBIGEO 
  FROM UBIGEO_CARGO UB 
  WHERE UB.IDECARGO =  p_ideCargo;
  
CURSOR c_centrosEstudios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT CE.TIPCENESTU, CE.TIPNOMCENESTU, CE.PUNTACENTROEST  
  FROM CENTROEST_CARGO CE 
  WHERE CE.IDECARGO =  p_ideCargo;

CURSOR c_nivelesAlcan(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT NA.TIPEDUCACION, NA.TIPAREAESTUDIO, NA.TIPNIVELCANZADO , NA.PUNTNIVESTUDIO, NA.CICLOSEMESTRE  
  FROM NIVELACADEMICO_CARGO NA 
  WHERE NA.IDECARGO =  p_ideCargo;

CURSOR c_conocimientos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM CONOGENERAL_CARGO CG 
  WHERE CG.IDECARGO =  p_ideCargo;
  
CURSOR c_experiencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM EXPERIENCIA_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;

CURSOR c_discapacidades(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT DC.TIPDISCAPA, DC.DESDISCAPA, DC.PUNTDISCAPA  
  FROM DISCAPACIDAD_CARGO DC 
  WHERE DC.IDECARGO =  p_ideCargo;
  
CURSOR c_evaluaciones(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT EC.IDEEXAMEVAL, EC.TIPEXAMEN, EC.NOTAMINEXAMEN, EC.TIPAREARESPON, EC.PUNTEXAMEN  
  FROM EVALUACION_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;
  
BEGIN
 c_ideSolReqPersonal:=p_ideSolReqPersonal;
 BEGIN
  UPDATE SOLREQ_PERSONAL S SET (S.IDECARGO,S.NOMCARGO,S.DESCARGO, S.SEXO,S.INDSEXO,S.EDADINICIO,S.EDADFIN,S.PUNTEDAD,S.INDEDAD,S.TIPRANGOSALARIO,S.PUNTSALARIO,S.OBJETIVOCARGO,S.FUNCIONESCARGO,S.OBSERVACIONCARGO,
         S.PUNTTOTPOSTUINTE,S.PUNTMINPOSTUINTE,S.PUNTTOTEDAD,S.PUNTMINEDAD,S.PUNTTOTSEXO,S.PUNTMINSEXO,S.PUNTTOTSALARIO,S.PUNTMINSALARIO,S.PUNTTOTNIVELEST,S.PUNTMINNIVELEST,S.PUNTTOTCENTROEST,S.PUNTTOTEXPLABORAL,
         S.PUNTMINEXPLABORAL,S.PUNTTOTOFIMATI,S.PUNTMINOFIMATI,S.PUNTTOTCONOIDIOMA,S.PUNTMINCONOIDIOMA,S.PUNTTOTCONOGEN,S.PUNTMINCONOGEN,S.PUNTTOTDISCAPA,S.PUNTMINDISCAPA,S.PUNTTOTHORARIO,S.PUNTMINHORARIO,S.PUNTTOTUBIGEO,
         S.PUNTMINUBIGEO,S.PUNTTOTEXAMEN,S.PUNTMINEXAMEN)=(
         SELECT C.IDECARGO,C.NOMCARGO,C.DESCARGO ,C.SEXO,C.INDSEXO,C.EDADINICIO,C.EDADFIN,C.PUNTEDAD,C.INDEDAD,C.TIPRANGOSALARIO,C.PUNTSALARIO,C.OBJETIVOSCARGO,C.FUNCIONESCARGO,C.OBSERVACIONCARGO,
         C.PUNTTOTPOSTUINTE,C.PUNTMINPOSTUINTE,C.PUNTTOTEDAD,C.PUNTMINEDAD,C.PUNTTOTSEXO,C.PUNTMINSEXO,C.PUNTTOTSALARIO,C.PUNTMINSALARIO,C.PUNTTOTNIVELEST,C.PUNTMINNIVELEST,C.PUNTTOTCENTROEST,C.PUNTTOTEXPLABORAL,
         C.PUNTMINEXPLABORAL,C.PUNTTOTOFIMATI,C.PUNTMINOFIMATI,C.PUNTMINIDIOMA,C.PUNTMINIDIOMA,C.PUNTTOTCONOGEN,C.PUNTMINCONOGEN,C.PUNTTOTDISCAPA,C.PUNTMINDISCAPA,C.PUNTTOTHORARIO,C.PUNTMINHORARIO,C.PUNTTOTUBIGEO,
         C.PUNTMINUBIGEO,C.PUNTTOTEXAMEN,C.PUNTMINEXAMEN
         FROM CARGO C WHERE C.IDECARGO = p_ideCargo)
  WHERE S.IDESOLREQPERSONAL = p_ideSolReqPersonal;
 
  FOR REG_OFRECEMOS IN c_Ofrecemos(p_ideCargo) LOOP        
     INSERT INTO OFRECEMOS_SOLREQ 
     (IDEOFRECEMOSSOLREQ,IDESOLREQPERSONAL,TIPOFRECIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEOFRECEMOSSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_OFRECEMOS.TIPOFRECIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_COMPETENCIAS IN c_competencias(p_ideCargo) LOOP        
     INSERT INTO COMPETENCIAS_SOLREQ 
     (IDECOMPETENCIASOLREQ,IDESOLREQPERSONAL,TIPCOMPETEN,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEOFRECEMOSSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_COMPETENCIAS.TIPCOMPETEN,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_HORARIOS IN c_horarios(p_ideCargo) LOOP        
     INSERT INTO HORARIO_SOLREQ
     (IDEHORARIOSOLREQ,IDESOLREQPERSONAL,TIPHORARIO,PUNTHORARIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEHORARIOSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_HORARIOS.TIPHORARIO, REG_HORARIOS.PUNTHORARIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_UBIGEOS IN c_ubigeos(p_ideCargo) LOOP        
     INSERT INTO UBIGEO_SOLREQ
     (IDEUBIGEOSOLREQ,IDESOLREQPERSONAL,IDEUBIGEO,PUNTUBIGEO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEUBIGEOSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_UBIGEOS.IDEUBIGEO, REG_UBIGEOS.PUNTUBIGEO,'A',p_usrCreacion,SYSDATE );
  END LOOP;  
  
  FOR REG_CENTROEST IN c_centrosEstudios(p_ideCargo) LOOP        
     INSERT INTO CENTROEST_SOLREQ
     (IDECENTESTSOLREQ,IDESOLREQPERSONAL,TIPCENESTU,TIPNOMCENESTU,PUNTACENTROEST,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECENTESTSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_CENTROEST.TIPCENESTU, REG_CENTROEST.TIPNOMCENESTU,REG_CENTROEST.PUNTACENTROEST ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_NIVELESALC IN c_nivelesAlcan(p_ideCargo) LOOP        
     INSERT INTO NIVELACADEMICO_SOLREQ
     (IDENIVELACADESOLREQ,IDESOLREQPERSONAL,TIPEDUCACION,TIPAREAESTUDIO,TIPNIVELCANZADO,CICLOSEMESTRE,PUNTNIVESTUDIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDENIVELACADESOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_NIVELESALC.TIPEDUCACION, REG_NIVELESALC.TIPAREAESTUDIO,REG_NIVELESALC.TIPNIVELCANZADO,REG_NIVELESALC.CICLOSEMESTRE, REG_NIVELESALC.PUNTNIVESTUDIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_CONOCIMIENTOS IN c_conocimientos(p_ideCargo) LOOP        
     INSERT INTO 
     CONOGENERAL_SOLREQ(IDECONOGENSOLREQ,IDESOLREQPERSONAL,TIPCONOFIMATICA,TIPNOMOFIMATICA,TIPIDIOMA,TIPCONOCIDIOMA,TIPCONOGENERAL,TIPNOMCONOCGRALES,NOMCONOCGRALES,TIPNIVELCONOCIMIENTO,INDCERTIFICACION,PUNTCONOCIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECONOGENSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_CONOCIMIENTOS.TIPCONOFIMATICA,REG_CONOCIMIENTOS.TIPNOMOFIMATICA,REG_CONOCIMIENTOS.TIPIDIOMA,REG_CONOCIMIENTOS.TIPCONOCIDIOMA,REG_CONOCIMIENTOS.TIPCONOGENERAL, REG_CONOCIMIENTOS.TIPNOMCONOCGRALES, REG_CONOCIMIENTOS.NOMCONOCGRALES, REG_CONOCIMIENTOS.TIPNIVELCONOCIMIENTO, REG_CONOCIMIENTOS.INDCERTIFICACION, REG_CONOCIMIENTOS.PUNTCONOCIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EXPERIENCIAS IN c_experiencias(p_ideCargo) LOOP        
     INSERT INTO EXPERIENCIA_SOLREQ
     (IDEEXPSOLREQ,IDESOLREQPERSONAL,TIPEXPLABORAL,CANTANHOEXP,CANTMESESEXP,PUNTEXPERIENCIA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEXPSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_EXPERIENCIAS.TIPEXPLABORAL,REG_EXPERIENCIAS.CANTANHOEXP,REG_EXPERIENCIAS.CANTMESESEXP,REG_EXPERIENCIAS.PUNTEXPERIENCIA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_DISCAPACIDAD IN c_discapacidades(p_ideCargo) LOOP        
     INSERT INTO DISCAPACIDAD_SOLREQ
     (IDEDISCAPASOLREQ,IDESOLREQPERSONAL,TIPDISCAPA,DESDISCAPA,PUNTDISCAPA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEDISCAPASOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_DISCAPACIDAD.TIPDISCAPA, REG_DISCAPACIDAD.DESDISCAPA, REG_DISCAPACIDAD.PUNTDISCAPA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EVALUACION IN c_evaluaciones(p_ideCargo) LOOP        
     INSERT INTO EVALUACION_SOLREQ
     (IDEEVALUACIONSOLREQ,IDESOLREQPERSONAL,IDEEXAMEVAL,TIPEXAMEN,NOTAMINEXAMEN,TIPAREARESPON,PUNTEXAMEN,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEVALUACIONSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_EVALUACION.IDEEXAMEVAL, REG_EVALUACION.TIPEXAMEN, REG_EVALUACION.NOTAMINEXAMEN,REG_EVALUACION.TIPAREARESPON,REG_EVALUACION.PUNTEXAMEN,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  COMMIT;
  EXCEPTION 
     WHEN OTHERS THEN
     ROLLBACK;
  END;

END COPIA_CARGO;  


/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_AMPLIACION
    Proposito   : realiza el insert de una solicitud clona los datos 
                  y registra en el log de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      05/05/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION SP_INSERTAR_AMPLIACION(p_ideCargo         IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                                p_ideSede          IN SEDE.IDESEDE%TYPE,
                                p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                p_ideArea          IN AREA.IDEAREA%TYPE,
                                p_numVacantes      IN SOLREQ_PERSONAL.NUMVACANTES%TYPE,
                                p_motivo           IN SOLREQ_PERSONAL.MOTIVO%TYPE,
                                p_observacion      IN SOLREQ_PERSONAL.OBSERVACION%TYPE,
                                p_ideUsuarioSuceso IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso     IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_cEtapa           IN DETALLE_GENERAL.VALOR%TYPE,
                                p_responsableSig   IN ROL.CODROL%TYPE,
                                p_tipoSolicitud    IN SOLREQ_PERSONAL.TIPSOL%TYPE,
                                p_indicArea        IN BOOLEAN)RETURN NUMBER 

IS
c_ideLogSequency  LOGSOLREQ_PERSONAL.IDELOGSOLREQ_PERSONAL%TYPE;
c_codAmpliacion   SOLREQ_PERSONAL.CODSOLREQPERSONAL%TYPE;
c_ideUsuarioResp  USUARIO.IDUSUARIO%TYPE;
c_descUsuario     USUARIO.CODUSUARIO%TYPE;
c_idRolResp       ROL.IDROL%TYPE;
c_idUsuarioResp   USUARIO.IDUSUARIO%TYPE;
qWhere            VARCHAR2(100);
qQuery            VARCHAR2(500);
BEGIN
  
    SELECT SOLREQ_PERSONAL_SQ.NEXTVAL
    INTO c_ideLogSequency
    FROM DUAL; 
    
    SELECT IDESOLAMPLIACION_SQ.NEXTVAL
    INTO c_codAmpliacion 
    FROM DUAL;
  
    SELECT US.CODUSUARIO
    INTO c_descUsuario
    FROM USUARIO US
    WHERE US.IDUSUARIO = p_ideUsuarioSuceso;
    
    IF (p_indicArea = TRUE)THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
    END IF;
    
    qQuery := 'SELECT US.IDUSUARIO , R.IDROL '||
              'INTO c_idUsuarioResp, c_idRolResp '||
              'FROM ROL R, USUARIO US, USUAROLSEDE UR, USUARIO_NIVEL UN '||
              'WHERE R.IDROL = UR.IDROL '||
              'AND US.IDUSUARIO = UR.IDUSUARIO '||
              'AND UR.IDESEDE = '||p_ideSede|| 
              ' AND R.CODROL = ' ||p_responsableSig ||' '|| qWhere;
        
   EXECUTE IMMEDIATE qQuery;
   
  BEGIN 
    INSERT INTO SOLREQ_PERSONAL 
    (IDESOLREQPERSONAL,CODSOLREQPERSONAL,IDEDEPENDENCIA,IDEDEPARTAMENTO,IDEAREA,NUMVACANTES,MOTIVO,OBSERVACION,ESTACTIVO,TIPSOL,USRCREACION,FECCREACION)
    VALUES(c_ideLogSequency, c_codAmpliacion, p_ideDependencia,p_ideDepartamento,p_ideArea,p_numVacantes,p_motivo,p_observacion,'A',p_tipoSolicitud,c_descUsuario,SYSDATE);
    --realizar la copia de cargo a la tabla ampliacion 
    PR_REQUERIMIENTOS.COPIA_CARGO(p_ideCargo,c_ideLogSequency,c_descUsuario);
    --insertar el log de solicitud
    PR_INTRANET_ED.SP_INSERT_LOG_SOLREQPERSONAL( c_ideLogSequency,p_cEtapa,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso, c_idRolResp,c_idUsuarioResp, NULL); 
    COMMIT; 
  EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
    c_idUsuarioResp := -1;
  END;
  
  RETURN c_idUsuarioResp;
  
END SP_INSERTAR_AMPLIACION;
  
END PR_REQUERIMIENTOS;
/
