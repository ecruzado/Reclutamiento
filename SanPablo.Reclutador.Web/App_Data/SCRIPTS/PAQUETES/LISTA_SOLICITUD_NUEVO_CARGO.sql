CREATE OR REPLACE VIEW LISTA_SOLICITUD_NUEVO_CARGO AS
SELECT SN.IDESOLNUEVOCARGO, SN.ESTACTIVO, SN.CODCARGO, SN.NOMBRE ,DE.IDEDEPENDENCIA,
         DE.NOMDEPENDENCIA,DP.IDEDEPARTAMENTO ,DP.NOMDEPARTAMENTO, AR.IDEAREA,AR.NOMAREA, 
         SN.NUMPOSICIONES, SN.FECCREACION,(PR_REQUERIMIENTOS.FN_RESPONSABLE_SOL(SN.IDESOLNUEVOCARGO)) RESPONSABLE, (PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SN.IDESOLNUEVOCARGO)) NOMRESPONSABLE
  FROM SOLNUEVO_CARGO SN, AREA AR, DEPARTAMENTO DP, DEPENDENCIA DE
  WHERE SN.IDEAREA = AR.IDEAREA
  AND AR.IDEDEPARTAMENTO = DP.IDEDEPARTAMENTO
  AND DP.IDEDEPENDENCIA = DE.IDEDEPENDENCIA;
  
  

