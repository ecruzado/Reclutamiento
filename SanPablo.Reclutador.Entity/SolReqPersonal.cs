

namespace SanPablo.Reclutador.Entity
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    
    public class SolReqPersonal : BaseEntity
    {

        public virtual int IDESOLREQPERSONAL     { get; set; }
	    public virtual string CODSOLREQPERSONAL  { get; set; }
	    public virtual int IDESEDE               { get; set; }
	    public virtual string TIPPUESTO          { get; set; }
	    public virtual string DESCARGO           { get; set; }
	    public virtual int IDEDEPENDENCIA        { get; set; }
	    public virtual int IDEDEPARTAMENTO       { get; set; }
	    public virtual int IDEAREA               { get; set; }
	    public virtual string OBJETIVOCARGO      { get; set; }
	    public virtual int PUNTPOSTUINTE         { get; set; }
	    public virtual int PUNTREFELABORAL       { get; set; }
	    public virtual string INDMASCULINO       { get; set; }
	    public virtual string INDFEMENINO        { get; set; }
	    public virtual int PUNTSEXO              { get; set; }
	    public virtual int EDADINICIO            { get; set; }
	    public virtual int  EDADFIN              { get; set; }
	    public virtual int PUNTEDAD              { get; set; }
	    public virtual double SALARIOINICIAL     { get; set; }
	    public virtual double  SALARIOFIN        { get; set; }
	    public virtual string TIPMONEDA          { get; set; }
	    public virtual double  PUNTSALARIO       { get; set; }
	    public virtual string INDVERSALARIO      { get; set; }
	    public virtual string INDSEXMASCU        { get; set; }
	    public virtual string INDSEXFEMEN        { get; set; }
	    public virtual string OBSERVACION        { get; set; }
	    public virtual int PUNTTOTPOSTUINTE    { get; set; }
	    public virtual int PUNTMINPOSTUINTE    { get; set; }
	    public virtual int PUNTTOTEDAD         { get; set; }
	    public virtual int PUNTMINEDAD         { get; set; }
	    public virtual int PUNTTOTSEXO         { get; set; }
	    public virtual int PUNTMINSEXO         { get; set; }
	    public virtual int PUNTTOTSALARIO      { get; set; }
	    public virtual int PUNTMINSALARIO      { get; set; }
	    public virtual int PUNTTOTNIVELEST     { get; set; }
	    public virtual int PUNTMINNIVELEST     { get; set; }
	    public virtual int PUNTTOTCENTROEST    { get; set; }
	    public virtual int PUNTMINCENTROEST    { get; set; }
	    public virtual int PUNTTOTEXPLABORAL   { get; set; }
	    public virtual int PUNTMINEXPLABORAL   { get; set; }
	    public virtual int PUNTTOTFUNDESE      { get; set; }
	    public virtual int PUNTMINFUNDESE      { get; set; }
	    public virtual int PUNTTOTCONOGEN      { get; set; }
	    public virtual int PUNTMINCONOGEN      { get; set; }
	    public virtual int PUNTTOTCONOIDIOMA   { get; set; }
	    public virtual int PUNTMINCONOIDIOMA   { get; set; }
	    public virtual int PUNTTOTDISCAPA      { get; set; }
	    public virtual int PUNTMINDISCAPA      { get; set; }
	    public virtual int PUNTTOTHORARIO      { get; set; }
	    public virtual int PUNTMINHORARIO      { get; set; }
	    public virtual int PUNTTOTUBIGEO       { get; set; }
	    public virtual int PUNTMINUBIGEO       { get; set; }
	    public virtual int PUNTTOTREFLABORAL   { get; set; }
	    public virtual int PUNTMINREFLABORAL   { get; set; }
	    public virtual int PUNTTOTEXAMEN       { get; set; }
	    public virtual int PUNTMINEXAMEN       { get; set; }
	    public virtual int CANTPRESELEC        { get; set; }
	    public virtual DateTime FECPUBLICACION    { get; set; }
	    public virtual DateTime FECEXPIRACACION   { get; set; }
	    public virtual string TIPVACANTE          { get; set; }
	    public virtual int NUMVACANTES            { get; set; }
	    public virtual int IDECARGO               { get; set; }
	    public virtual string NOMPERSONREEMPLAZO  { get; set; }
	    public virtual DateTime FECINIREEMPLAZO   { get; set; }
	    public virtual DateTime FECFINREEMPLAZO   { get; set; }
	    public virtual string INDCARGO            { get; set; }
	    public virtual string INDVERSUELDO        { get; set; }
	    public virtual string INDVERSEXOMASC      { get; set; }
	    public virtual string INDVERSEXOFEM       { get; set; }
	    public virtual string USRCREACION         { get; set; }
	    public virtual DateTime FECCREACION       { get; set; }
	    public virtual string USRMODIFICA         { get; set; }
        public virtual DateTime FECMODIFICA       { get; set; }

        public virtual string DEPENDENCIA_DES     { get; set; }
        public virtual string DEPARTAMENTO_DES    { get; set; }
        public virtual string AREA_DES            { get; set; }
        public virtual string SEDE_DES            { get; set; }
        



    }
}
