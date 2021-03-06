
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is

/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_NUEVO
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_NUEVO(p_ideSede           IN SEDE.IDESEDE%TYPE,
                            p_ideArea           IN AREA.IDEAREA%TYPE,
                            p_codCargo          IN SOLNUEVO_CARGO.CODCARGO%TYPE,
                            p_nomCargo          IN SOLNUEVO_CARGO.NOMBRE%TYPE,
                            p_descCargo         IN SOLNUEVO_CARGO.DESCRIPCION%TYPE,
                            p_numPosiciones     IN SOLNUEVO_CARGO.NUMPOSICIONES%TYPE,
                            p_tipRangoSalario   IN SOLNUEVO_CARGO.TIPRANSALARIO%TYPE,
                            p_ideDependencia    IN SOLNUEVO_CARGO.IDEDEPENDENCIA%TYPE,
                            p_ideDepartamento   IN SOLNUEVO_CARGO.IDEDEPARTAMENTO%TYPE,
                            p_estudios          IN SOLNUEVO_CARGO.ESTUDIOS%TYPE,
                            p_funciones         IN SOLNUEVO_CARGO.FUNCIONES%TYPE,
                            p_competencias      IN SOLNUEVO_CARGO.COMPETENCIAS%TYPE,
                            p_observacion       IN SOLNUEVO_CARGO.OBSERVACIONES%TYPE,                            
                            p_ideUsuarioSuceso  IN USUARIO.IDUSUARIO%TYPE,
                            p_ideRolSuceso      IN ROL.IDROL%TYPE,
                            p_ideRolResponsable IN ROL.IDROL%TYPE,
                            p_indArea           IN VARCHAR2,
                            p_usuarioCreacion   IN SOLNUEVO_CARGO.USRCREACION%TYPE,
                            p_etapa             IN DETALLE_GENERAL.VALOR%TYPE,
                            p_ideUsuarioResp    OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE)IS
                             
                        
c_idesolCargo SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE;
qWhere VARCHAR2(100);
qQuery VARCHAR2(1000);

BEGIN   
  
  IF (p_indArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
  END IF;
    
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
                     
  EXECUTE IMMEDIATE qQuery INTO p_ideUsuarioResp;
  
  SELECT IDESOLNUEVOCARGO_SQ.NEXTVAL
  INTO c_idesolCargo
  FROM DUAL; 
  
  BEGIN
    INSERT INTO SOLNUEVO_CARGO
    (IDESOLNUEVOCARGO,IDESEDE,CODCARGO,NOMBRE,DESCRIPCION,NUMPOSICIONES,TIPRANSALARIO,IDEDEPENDENCIA,IDEDEPARTAMENTO,IDEAREA,ESTUDIOS,FUNCIONES,COMPETENCIAS,OBSERVACIONES,ESTACTIVO,USRCREACION,FECCREACION,TIPETAPA)
    VALUES(c_idesolCargo,p_ideSede,p_codCargo,p_nomCargo,p_descCargo,p_numPosiciones,p_tipRangoSalario,p_ideDependencia,p_ideDepartamento,p_ideArea,p_estudios,p_funciones,p_competencias,p_observacion,'A',p_usuarioCreacion,SYSDATE,p_etapa);
    
    --INSERTAR EL LOG DE SOLICITUD
    INSERT INTO LOGSOLNUEVO_CARGO
    (IDELOGSOLNUEVOCARGO,IDESOLNUEVOCARGO,TIPETAPA,ROLRESPONSABLE, USRESPONSABLE,FECSUCESO,USRSUCESO,ROLSUCESO)
    VALUES (IDELOGSOLREQ_PERSONAL_SQ.NEXTVAL,c_idesolCargo,p_etapa,p_ideRolResponsable,p_ideUsuarioResp,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso);
   
    COMMIT;   
     
  EXCEPTION
    WHEN OTHERS THEN  
    p_ideUsuarioResp:= -1;
    ROLLBACK;
  END;
   
END SP_INSERTAR_NUEVO;

/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_LOG
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_LOG(p_ideSolicitudNuevo  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                          p_ideSede            IN SEDE.IDESEDE%TYPE,
                          p_ideArea            IN AREA.IDEAREA%TYPE,
                          p_ideUsuarioSuceso   IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                          p_ideRolSuceso       IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                          p_ideRolResponsable  IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                          p_indArea            IN VARCHAR2,
                          p_etapa              IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE, 
                          p_ideUsuarioResp     OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE)IS
                             
                        
c_idesolCargo SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE;
qWhere VARCHAR2(100);
qQuery VARCHAR2(1000);

BEGIN   
  
  IF (p_ideRolResponsable != 0) THEN
  
  IF (p_indArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
  END IF;
    
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
       
                
  EXECUTE IMMEDIATE qQuery INTO p_ideUsuarioResp;
  
  ELSE
    p_ideUsuarioResp:=0;
  
  END IF;
  
  BEGIN
    
    INSERT INTO LOGSOLNUEVO_CARGO
    (IDELOGSOLNUEVOCARGO,IDESOLNUEVOCARGO,TIPETAPA,ROLRESPONSABLE, USRESPONSABLE,FECSUCESO,USRSUCESO,ROLSUCESO)
    VALUES (IDELOGSOLREQ_PERSONAL_SQ.NEXTVAL,p_ideSolicitudNuevo,p_etapa,p_ideRolResponsable,p_ideUsuarioResp,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso);
   
    UPDATE SOLNUEVO_CARGO SN
    SET SN.TIPETAPA = p_etapa
    WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitudNuevo;
    COMMIT;   
     
  EXCEPTION
    WHEN OTHERS THEN  
    p_ideUsuarioResp:= -1;
    ROLLBACK;
  END;
   
END SP_INSERTAR_LOG;


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
  SELECT LG.TIPETAPA,LG.ROLRESPONSABLE
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
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud    IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
    
    SELECT R.DSCROL
    INTO c_responsable
    FROM LOGSOLNUEVO_CARGO LG , ROL R   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLNUEVO_CARGO SN
                       WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
    AND R.IDROL = LG.ROLRESPONSABLE
    AND LG.IDESOLNUEVOCARGO = p_ideSolicitud;
    
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
FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                           p_tipoSolicitud   IN VARCHAR2)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN

  IF p_tipoSolicitud = 'N' THEN
  
    SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
    INTO c_responsable
    FROM LOGSOLNUEVO_CARGO LG , USUARIO US   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLNUEVO_CARGO SN
                       WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
    AND US.IDUSUARIO = LG.USRESPONSABLE
    AND LG.IDESOLNUEVOCARGO = p_ideSolicitud;
  ELSE
    
    SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
    INTO c_responsable
    FROM LOGSOLREQ_PERSONAL LG , USUARIO US   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLREQ_PERSONAL SN
                       WHERE SN.IDESOLREQPERSONAL = p_ideSolicitud)
    AND US.IDUSUARIO = LG.USRESPONSABLE
    AND LG.IDESOLREQPERSONAL = p_ideSolicitud;
    
  END IF;
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
         S.PUNTMINUBIGEO,S.PUNTTOTEXAMEN,S.PUNTMINEXAMEN,S.TIPREQUERIMIENTO)=(
         SELECT C.IDECARGO,C.NOMCARGO,C.DESCARGO ,C.SEXO,C.INDSEXO,C.EDADINICIO,C.EDADFIN,C.PUNTEDAD,C.INDEDAD,C.TIPRANGOSALARIO,C.PUNTSALARIO,C.OBJETIVOSCARGO,C.FUNCIONESCARGO,C.OBSERVACIONCARGO,
         C.PUNTTOTPOSTUINTE,C.PUNTMINPOSTUINTE,C.PUNTTOTEDAD,C.PUNTMINEDAD,C.PUNTTOTSEXO,C.PUNTMINSEXO,C.PUNTTOTSALARIO,C.PUNTMINSALARIO,C.PUNTTOTNIVELEST,C.PUNTMINNIVELEST,C.PUNTTOTCENTROEST,C.PUNTTOTEXPLABORAL,
         C.PUNTMINEXPLABORAL,C.PUNTTOTOFIMATI,C.PUNTMINOFIMATI,C.PUNTMINIDIOMA,C.PUNTMINIDIOMA,C.PUNTTOTCONOGEN,C.PUNTMINCONOGEN,C.PUNTTOTDISCAPA,C.PUNTMINDISCAPA,C.PUNTTOTHORARIO,C.PUNTMINHORARIO,C.PUNTTOTUBIGEO,
         C.PUNTMINUBIGEO,C.PUNTTOTEXAMEN,C.PUNTMINEXAMEN, C.TIPREQUERIMIENTO
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
PROCEDURE SP_INSERTAR_AMPLIACION(p_ideCargo         IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                                p_ideSede          IN SEDE.IDESEDE%TYPE,
                                p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                p_ideArea          IN AREA.IDEAREA%TYPE,
                                p_numVacantes      IN SOLREQ_PERSONAL.NUMVACANTES%TYPE,
                                p_motivo           IN SOLREQ_PERSONAL.MOTIVO%TYPE,
                                p_tipoPuesto       IN SOLREQ_PERSONAL.TIPPUESTO%TYPE, 
                                p_observacion      IN SOLREQ_PERSONAL.OBSERVACION%TYPE,
                                p_ideUsuarioSuceso IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso     IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_cEtapa           IN DETALLE_GENERAL.VALOR%TYPE,
                                p_responsableSig   IN ROL.IDROL%TYPE,
                                p_tipoSolicitud    IN SOLREQ_PERSONAL.TIPSOL%TYPE,
                                p_indicArea        IN VARCHAR2,
                                p_cRetVal          OUT NUMBER) 

IS
c_ideLogSequency  LOGSOLREQ_PERSONAL.IDELOGSOLREQ_PERSONAL%TYPE;
c_codAmpliacion   SOLREQ_PERSONAL.CODSOLREQPERSONAL%TYPE;
c_ideUsuarioResp  USUARIO.IDUSUARIO%TYPE;
c_descUsuario     USUARIO.CODUSUARIO%TYPE;
c_idRolResp       ROL.IDROL%TYPE;
qWhere            VARCHAR2(100);
qQuery            VARCHAR2(500);
BEGIN
  
    
  
    SELECT US.CODUSUARIO
    INTO c_descUsuario
    FROM USUARIO US
    WHERE US.IDUSUARIO = p_ideUsuarioSuceso;
    
    IF (p_indicArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
    END IF;
    
    qQuery := 'SELECT UR.IDUSUARIO , UR.IDROL '||
              'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
              'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
              'AND UR.IDROL = R.IDROL '||
              'AND UR.IDESEDE = '||p_ideSede||' '|| 
              'AND R.IDROL = '||p_responsableSig||' '||
               qWhere ||' '||
              'AND UN.FLGESTADO = ''A'' '||
              'AND ROWNUM <= 1';
              
        
   EXECUTE IMMEDIATE qQuery INTO c_ideUsuarioResp, c_idRolResp ;
   
   SELECT SOLREQ_PERSONAL_SQ.NEXTVAL
    INTO c_ideLogSequency
    FROM DUAL; 
    
    SELECT IDESOLAMPLIACION_SQ.NEXTVAL
    INTO c_codAmpliacion 
    FROM DUAL;
    
   p_cRetVal := c_ideUsuarioResp;
  BEGIN 
    INSERT INTO SOLREQ_PERSONAL 
    (IDESOLREQPERSONAL,CODSOLREQPERSONAL,IDESEDE,IDEDEPENDENCIA,IDEDEPARTAMENTO,IDEAREA,NUMVACANTES,MOTIVO,OBSERVACION,ESTACTIVO,TIPSOL,USRCREACION,FECCREACION,TIPETAPA,TIPPUESTO)
    VALUES(c_ideLogSequency, c_codAmpliacion,p_ideSede, p_ideDependencia,p_ideDepartamento,p_ideArea,p_numVacantes,p_motivo,p_observacion,'A',p_tipoSolicitud,c_descUsuario,SYSDATE,p_cEtapa,p_tipoPuesto);
    --realizar la copia de cargo a la tabla ampliacion 
    PR_REQUERIMIENTOS.COPIA_CARGO(p_ideCargo,c_ideLogSequency,c_descUsuario);
    --insertar el log de solicitud
    PR_INTRANET_ED.SP_INSERT_LOG_SOLREQPERSONAL( c_ideLogSequency,p_cEtapa,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso, c_idRolResp,c_ideUsuarioResp, NULL); 
    COMMIT; 
  EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
    p_cRetVal := -1;
  END;
    
END SP_INSERTAR_AMPLIACION;

/* ------------------------------------------------------------
    Nombre      : SP_RESPONSABLE_PUBLICACION
    Proposito   : determinar el responsable de la publicación de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      06/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_RESPONSABLE_PUBLICACION(p_idSolicitudNuevo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_idSede            IN SEDE.IDESEDE%TYPE,
                                     p_idUsuarioResp     OUT USUARIO.IDUSUARIO%TYPE,
                                     p_idRolResp         OUT ROL.IDROL%TYPE)

IS

c_tipoRequerimiento CARGO.TIPREQUERIMIENTO%TYPE;
BEGIN
  SELECT C.TIPREQUERIMIENTO
  INTO c_tipoRequerimiento
  FROM CARGO C , SOLNUEVO_CARGO SN
  WHERE C.CODCARGO = SN.CODCARGO
  AND SN.IDESOLNUEVOCARGO = p_idSolicitudNuevo;
  
  BEGIN
    SELECT UN.IDUSUARIO,UN.IDROL
    INTO p_idUsuarioResp,p_idRolResp
    FROM USUARIOREQ UQ, USUAROLSEDE UN
    WHERE UQ.IDUSUARIO = UN.IDUSUARIO
    AND UN.IDESEDE =1
    AND UQ.TIPREQ = '01'
    AND UN.IDROL IN (8,9)
    AND ROWNUM < 2
    ORDER BY UN.IDUSUARIO,UN.IDROL;
  EXCEPTION
    WHEN OTHERS THEN
    p_idUsuarioResp:= -1;
    p_idRolResp:=-1;
  END;
  
END SP_RESPONSABLE_PUBLICACION;

/* ------------------------------------------------------------
    Nombre      : FN_DETERMINAR_RESPONSABLE
    Proposito   : determinar el responsable de la publicación de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      06/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_DETERMINAR_RESPONSABLE(p_idCargo       IN CARGO.IDECARGO%TYPE,
                                    p_idSede        IN SEDE.IDESEDE%TYPE,
                                    p_idUsuarioResp OUT USUARIO.IDUSUARIO%TYPE)

IS

c_tipoRequerimiento CARGO.TIPREQUERIMIENTO%TYPE;
BEGIN
  SELECT C.TIPREQUERIMIENTO
  INTO c_tipoRequerimiento
  FROM CARGO C
  WHERE C.IDECARGO = p_idCargo;
  
  BEGIN
    SELECT UN.IDUSUARIO
    INTO p_idUsuarioResp
    FROM USUARIOREQ UQ, USUARIO_NIVEL UN
    WHERE UQ.IDUSUARIO = UN.IDUSUARIO
    AND UN.IDESEDE = p_idSede
    AND UQ.TIPREQ = c_tipoRequerimiento
    AND ROWNUM <= 1;
  EXCEPTION
    WHEN OTHERS THEN
    p_idUsuarioResp:= -1;
  END;
  
END SP_DETERMINAR_RESPONSABLE;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_ETAPA_SOLREQ
    Proposito   : Obtener la etapa y estado actual de la solicitud de requerimiento.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      07/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_OBTENER_ETAPA_SOLREQ(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetVal OUT SYS_REFCURSOR)IS

BEGIN

  BEGIN
  OPEN p_cRetVal FOR
  SELECT LG.TIPETAPA, LG.ROLRESPONSABLE
  FROM LOGSOLREQ_PERSONAL LG   
  WHERE FECSUCESO = (SELECT MAX (SR.FECSUCESO)
                     FROM  LOGSOLREQ_PERSONAL SR
                     WHERE SR.IDESOLREQPERSONAL = p_ideSolRequerimiento)
  AND LG.IDESOLREQPERSONAL =  p_ideSolRequerimiento; 

  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:=NULL;
  END;   
  
END SP_OBTENER_ETAPA_SOLREQ;
  
/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_APROB_AMP
    Proposito   : Obtener la etapa y estado actual de la solicitud de requerimiento.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      07/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_APROB_AMP(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_ideRolResponsable   IN ROL.IDROL%TYPE,
                                p_ideSede             IN SEDE.IDESEDE%TYPE,
                                p_ideArea             IN AREA.IDEAREA%TYPE,
                                p_indicArea           IN VARCHAR2,
                                p_tipoEtapa           IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                p_usuarioSuceso       IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso        IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_observacion         IN LOGSOLREQ_PERSONAL.OBSERVACION%TYPE,
                                c_ideUsuarioResp      IN OUT USUARIO.IDUSUARIO%TYPE)IS

qWhere  VARCHAR2(100);
qQuery  VARCHAR2(1000);

BEGIN

  IF (c_ideUsuarioResp IS NULL)THEN
  
  IF (p_indicArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
  ELSE
    qWhere :='';
  END IF;
   
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
                  
  EXECUTE IMMEDIATE qQuery INTO c_ideUsuarioResp ;
  END IF;
  BEGIN
 
   PR_INTRANET_ED.SP_INSERT_LOG_SOLREQPERSONAL(p_ideSolRequerimiento,p_tipoEtapa,SYSDATE,p_usuarioSuceso,p_ideRolSuceso,p_ideRolResponsable,c_ideUsuarioResp,p_observacion);
   UPDATE SOLREQ_PERSONAL SP
   SET SP.TIPETAPA = p_tipoEtapa
   WHERE SP.IDESOLREQPERSONAL = p_ideSolRequerimiento;
  COMMIT; 
  EXCEPTION
  WHEN OTHERS THEN
    c_ideUsuarioResp:=NULL;
    ROLLBACK;
  END;   
  
END SP_INSERTAR_APROB_AMP;

/* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de todas las solicitudes
                 (Nuevo - Ampliacion - Reemplazo)
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_LISTA_SOLGRAL(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                           p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                           p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                           p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                           p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                           p_cTipResp        in NUMBER,   
                           p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                           p_cFecIni         IN VARCHAR2,
                           p_cFeFin          IN VARCHAR2,
                           p_cTipoSolicitud  IN VARCHAR2,
                           p_cCodSolicitud   IN VARCHAR2,
                           p_cRetVal         OUT SYS_REFCURSOR)IS
          
c_Consulta1 VARCHAR2(2000):=NULL;
c_Consulta2 VARCHAR2(2000):=NULL;
c_Where VARCHAR2(1000):=NULL;
c_Query VARCHAR2(4000):=NULL;
          
BEGIN
  
  DELETE T_RECLTEMP;   

  INSERT INTO T_RECLTEMP
  SELECT SN.IDESOLNUEVOCARGO, PR_INTRANET.FN_ESTADO_SOLICITUD(SN.IDESOLNUEVOCARGO,'N') ESTADO, SN.CODCARGO,NVL(SN.IDECARGO,0), SN.NOMBRE,
       DE.IDEDEPENDENCIA,DE.NOMDEPENDENCIA,DP.IDEDEPARTAMENTO,DP.NOMDEPARTAMENTO,AR.IDEAREA,AR.NOMAREA, SN.NUMPOSICIONES,0 POSTULANTE, 0 PRESELECCIONADOS, 0 EVALUADOS, 0 SELECCIONADOS, SN.FECCREACION,SN.FECCREACION CIERRE,NVL(R.IDROL,0), NVL(R.DSCROL,'') ,
       (PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SN.IDESOLNUEVOCARGO,'N')) NOMRESPONSABLE,SN.FECPUBLICACION,SN.FECEXPIRACION,DECODE(SN.FECPUBLICACION,NULL,'NO','SI') PUBLICADO, LS.TIPETAPA,(PR_INTRANET.FN_VALOR_GENERAL('ETAPA',LS.TIPETAPA)) TETAPA,'01' TIPSOL ,'NUEVO' TIPOSOLICITUD  
  FROM SOLNUEVO_CARGO SN, AREA AR, DEPARTAMENTO DP, DEPENDENCIA DE , LOGSOLNUEVO_CARGO LS LEFT JOIN ROL R
  ON (LS.ROLRESPONSABLE = R.IDROL)
  WHERE SN.IDEAREA = AR.IDEAREA
  AND AR.IDEDEPARTAMENTO = DP.IDEDEPARTAMENTO
  AND DP.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
  AND LS.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
  AND LS.FECSUCESO = (SELECT MAX (FECSUCESO)
                           FROM  LOGSOLNUEVO_CARGO LN
                           WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO);
                           
  INSERT INTO T_RECLTEMP
  SELECT SQ.IDESOLREQPERSONAL, PR_INTRANET.FN_ESTADO_SOLICITUD(SQ.IDESOLREQPERSONAL ,'O') ESTADO,TO_CHAR(SQ.CODSOLREQPERSONAL) CODSOLICITUD, NVL(SQ.IDECARGO,0), SQ.NOMCARGO,
       DE.IDEDEPENDENCIA, DE.NOMDEPENDENCIA,DP.IDEDEPARTAMENTO, DP.NOMDEPARTAMENTO,AR.IDEAREA, AR.NOMAREA, SQ.NUMVACANTES,0 POSTULANTE, 0 PRESELECCIONADOS, 0 EVALUADOS, 0 SELECCIONADOS, SQ.FECCREACION, SQ.FECCREACION CIERRE ,NVL(R.IDROL,0), NVL(R.DSCROL,'') ,
       (PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SQ.IDESOLREQPERSONAL,'O')) NOMRESPONSABLE,SQ.FECPUBLICACION,SQ.FECEXPIRACACION,DECODE(SQ.FECPUBLICACION,NULL,'NO','SI') PUBLICADO, LQ.TIPETAPA,(PR_INTRANET.FN_VALOR_GENERAL('ETAPA',LQ.TIPETAPA)) TETAPA,SQ.TIPSOL,(PR_INTRANET.FN_VALOR_GENERAL('TIPSOL',SQ.TIPSOL)) TIPOSOLICITUD
  FROM SOLREQ_PERSONAL SQ, AREA AR, DEPARTAMENTO DP, DEPENDENCIA DE , LOGSOLREQ_PERSONAL LQ LEFT JOIN ROL R 
  ON (LQ.ROLRESPONSABLE = R.IDROL)
  WHERE SQ.IDEAREA = AR.IDEAREA
  AND LQ.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL
  AND AR.IDEDEPARTAMENTO = DP.IDEDEPARTAMENTO
  AND DP.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
  AND LQ.FECSUCESO = (SELECT MAX (FECSUCESO)
                           FROM  LOGSOLREQ_PERSONAL LSQ
                           WHERE LSQ.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL);  
  
  
   IF p_nIdCargo>0 THEN
    
      c_Where := c_Where || ' AND  S.IDECARGO = '||p_nIdCargo;
    
    END IF;
   
   IF p_cCodSolicitud IS NOT NULL THEN
        
      c_Where := c_Where || '  AND S.CODIGO = ''' ||p_cCodSolicitud||''' ' ;
        
    END IF;
   
    IF p_nIdDependencia>0 THEN
        
      c_Where := c_Where || '  AND S.IDEDEPENDENCIA = '||p_nIdDependencia ;
        
    END IF;
    
    IF p_nIdDepartamento>0 THEN
        
      c_Where := c_Where || '  AND S.IDEDEPARTAMENTO = '||p_nIdDepartamento ;
        
    END IF;

    IF p_nIdArea>0 THEN
        
      c_Where := c_Where || ' AND S.IDEAREA = '||p_nIdArea ;
        
    END IF;
 
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'' THEN
        
      c_Where := c_Where || ' AND S.TIPETAPA = '||p_cTipEtapa ;
                            
    END IF;

    IF p_cTipResp IS NOT NULL AND p_cTipResp<> 0 THEN
        
      c_Where := c_Where || ' AND '''||p_cTipResp||''' = S.IDRESPONSABLE ';
        
    END IF;
    
    IF p_cEstado IS NOT NULL AND p_cEstado<>'' THEN
      
      IF p_cEstado = 'I' THEN
        c_Where := c_Where || ' AND '''||p_cEstado||''' = S.ESTADO ';
      ELSE
        c_Where := c_Where || ' AND S.ESTADO != ''I'' '; 
      END IF;   
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni))>0 AND LENGTH(rtrim(p_cFeFin))>0 THEN
       
      c_Where := c_Where || ' AND S.INICIO >= to_date('''||p_cFecIni||''',''DD/MM/YYYY'')'
                         || ' AND S.CIERRE < to_date('''||p_cFeFin||''',''DD/MM/YYYY'')+1';
        
    END IF;
    
    IF p_cTipoSolicitud IS NOT NULL THEN
        
      c_Where := c_Where || ' AND '''||p_cTipoSolicitud||''' = S.TIPSOL ';
        
    END IF;
    
   c_Where := c_Where || ' ORDER BY S.IDESOLICITUD ';
  
   c_Consulta1:= ' SELECT * FROM T_RECLTEMP  S WHERE 1=1 ';
   
   c_Query := c_Consulta1 || c_Where;
   
   INSERT INTO LOG_MENSAJE 
   VALUES(c_Query);
   
   OPEN p_cRetVal FOR 
   c_Query;
   
   COMMIT;
   
END SP_LISTA_SOLGRAL; 

END PR_REQUERIMIENTOS;
/
