CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is


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
                            p_ideUsuarioResp    OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE);
                            
PROCEDURE SP_INSERTAR_LOG(p_ideSolicitudNuevo  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                          p_ideSede            IN SEDE.IDESEDE%TYPE,
                          p_ideArea            IN AREA.IDEAREA%TYPE,
                          p_ideUsuarioSuceso   IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                          p_ideRolSuceso       IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                          p_ideRolResponsable  IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                          p_indArea            IN VARCHAR2,
                          p_etapa              IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE, 
                          p_ideUsuarioResp     OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE);                         
                              
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR);
                                     
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                            RETURN VARCHAR2;

FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                           p_tipoSolicitud   IN VARCHAR2)
                           RETURN VARCHAR2;
                           
FUNCTION FN_VERIFICAR_CODCARGO(p_codCargo IN SOLNUEVO_CARGO.CODCARGO%TYPE)RETURN NUMBER ;


PROCEDURE COPIA_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                             p_ideSolReqPersonal IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_usrCreacion IN SOLREQ_PERSONAL.USRCREACION%TYPE);
                             
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
                                p_cRetVal          OUT NUMBER); 
                                
PROCEDURE SP_RESPONSABLE_PUBLICACION(p_idSolicitudNuevo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_idSede           IN SEDE.IDESEDE%TYPE,
                                     p_idUsuarioResp    OUT USUARIO.IDUSUARIO%TYPE,
                                     p_idRolResp        OUT ROL.IDROL%TYPE);
                                
PROCEDURE SP_DETERMINAR_RESPONSABLE(p_idCargo       IN CARGO.IDECARGO%TYPE,
                                   p_idSede         IN SEDE.IDESEDE%TYPE,
                                   p_idUsuarioResp  OUT USUARIO.IDUSUARIO%TYPE); 
                                   
PROCEDURE SP_OBTENER_ETAPA_SOLREQ(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetVal OUT SYS_REFCURSOR);
                                  
PROCEDURE SP_INSERTAR_APROB_AMP(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_ideRolResponsable   IN ROL.IDROL%TYPE,
                                p_ideSede             IN SEDE.IDESEDE%TYPE,
                                p_ideArea             IN AREA.IDEAREA%TYPE,
                                p_indicArea           IN VARCHAR2,
                                p_tipoEtapa           IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                p_usuarioSuceso       IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso        IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_observacion         IN LOGSOLREQ_PERSONAL.OBSERVACION%TYPE,
                                c_ideUsuarioResp      IN OUT USUARIO.IDUSUARIO%TYPE);
                                
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
                           p_cRetVal         OUT SYS_REFCURSOR);

END PR_REQUERIMIENTOS;
/
