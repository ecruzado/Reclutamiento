CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is

                         
  PROCEDURE SP_APROBACION_NUEVO(p_ideSede IN SEDE.IDESEDE%TYPE,
                                p_ideArea IN AREA.IDEAREA%TYPE,
                                p_suceso  IN DETALLE_GENERAL.VALOR%TYPE,
                                p_etapa   IN DETALLE_GENERAL.VALOR%TYPE);       
 
END PR_REQUERIMIENTOS;
