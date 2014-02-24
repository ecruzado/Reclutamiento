CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is



PROCEDURE SP_APROBACION_NUEVO(p_ideSede     IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE,
                              c_ideUsuarioResp OUT USUARIO.IDUSUARIO%TYPE);

END PR_REQUERIMIENTOS;
/
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is


PROCEDURE SP_APROBACION_NUEVO(p_ideSede     IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE,
                              c_ideUsuarioResp OUT USUARIO.IDUSUARIO%TYPE)IS
                        

c_ideLogSolicitud LOGSOLNUEVO_CARGO.IDELOGSOLNUEVOCARGO%TYPE;
c_ideRolRespSuc ROL.IDROL%TYPE;
c_etapaSgte DETALLE_GENERAL.VALOR%TYPE;    
c_ideRolResp ROL.IDROL%TYPE;
c_ideTipoReq CARGO.TIPREQUERIMIENTO%TYPE;

BEGIN   
  IF (p_etapa = '01') THEN-- 01 envio solicitud -ETAPA PENDIENTE SOLICITUD
   c_ideRolResp := 10;     --GERENTE AREA
   c_etapaSgte :='02';
  ELSIF (p_etapa = '02')THEN --02 aprob/rechazo 
   c_ideRolResp := 5; --GGA 
   c_etapaSgte :='03';
  ELSIF (p_etapa = '03')THEN --elaboracion perfil 
   c_ideRolResp := 6;   --jefe procesos
   c_etapaSgte :='04';
  ELSIF (p_etapa = '04')THEN   --pendiente aprobacion perfil
   c_ideRolResp := 3;  --jefe de area
   c_etapaSgte :='05';
  ELSIF (p_etapa = '05')THEN   --pendiente aprobacion perfil2  
   c_ideRolResp := 3;  --encargado seleccion
   c_etapaSgte :='06';
  ELSIF (p_etapa = '06')THEN   --pendiente publicacion 
   c_ideRolResp := 3; --destinatario recursos 
   c_etapaSgte :='07';
  END IF;
  
   SELECT  U.IDUSUARIO,R.IDROL
   INTO c_ideUsuarioResp, c_ideRolResp
   FROM USUARIO_NIVEL U, ROL R , USUAROLSEDE UR
   WHERE U.IDUSUARIO = UR.IDUSUARIO
   AND R.IDROL = UR.IDROL
   AND R.IDROL = c_ideRolResp
   AND U.IDEAREA = p_ideArea
   AND U.IDESEDE = p_ideSede
   AND U.FLGESTADO ='A'; -- ESTACTIVO
  
   --ultimo registro de log de solicitud
   IF( p_etapa <> '01')THEN      
     SELECT LG.IDELOGSOLNUEVOCARGO, LG.ROLRESPONSABLE
     INTO c_ideLogSolicitud, c_ideRolRespSuc
     FROM LOGSOLNUEVO_CARGO LG   
     WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                        FROM  LOGSOLNUEVO_CARGO SN
                        WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
     AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo;  
   END IF;
   BEGIN
     --   AND ESTADO = "PENDIENTE"
     IF( p_etapa <> '01')THEN
         IF (p_ideRol = c_ideRolRespSuc) THEN
            UPDATE LOGSOLNUEVO_CARGO LS
            SET LS.TIPSUCESO = p_suceso
            WHERE LS.IDELOGSOLNUEVOCARGO = c_ideLogSolicitud;
         END IF;
     END IF;

     IF (p_etapa <> '06') THEN --VALIDAR EL SUCESO SOLO SI ES APROBADO SE INSERTA EL DATO --PENDIENTE
        INSERT INTO LOGSOLNUEVO_CARGO 
        (IDELOGSOLNUEVOCARGO, IDESOLNUEVOCARGO,TIPETAPA, TIPSUCESO,OBSERVACION,FECSUCESO,USRSUCESO,ROLSUCESO, USRESPONSABLE,ROLRESPONSABLE)
        VALUES(IDESOLNUEVOCARGO_SQ.NEXTVAL,p_ideSolCargo,c_etapaSgte,'01',p_observacion,sysdate,p_ideUsuario,p_ideRol,c_ideUsuarioResp,c_ideRolResp);
     END IF;
     COMMIT;
     
   EXCEPTION
       WHEN OTHERS THEN
       ROLLBACK;  
       c_ideUsuarioResp:= 0;
   END;
   
END SP_APROBACION_NUEVO;

END PR_REQUERIMIENTOS;
/
