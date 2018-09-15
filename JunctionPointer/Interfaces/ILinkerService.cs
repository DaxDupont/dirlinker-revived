using System;
using DirLinker.Data;
using System.Windows.Threading;
using DirLinker.Commands;

namespace DirLinker.Interfaces
{

    public interface ILinkerService
    {
        void SetOperationData(LinkOperationData linkData);
        FeedbackData GetStatusData(Dispatcher dispatcher);
        void PerformOperation();
        void CancelOperation();

        Action OperationComplete { set; get; }
    }
}
