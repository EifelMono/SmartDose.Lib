using SmartDose.WcfLib;

namespace SmartDose.WcfMasterData10000
{
    public partial class ServiceResult : object, IServiceResult
    {
        public object Data { get; set; }
    }

    public class ServiceResult<TData> : ServiceResult, IServiceResult<TData>
    {
        public ServiceResult() : base()
        {
        }
        public new TData Data { get => (TData)base.Data; set => base.Data = value; }
    }
}
