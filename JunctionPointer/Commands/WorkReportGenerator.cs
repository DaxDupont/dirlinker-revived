using System;

namespace DirLinker.Commands
{
    /// <summary>
    /// An imutable data class for the state of an operation
    /// </summary>
    public class WorkReport
    {
        public Exception UndoException { get; private set; }
        public Exception ExecuteException { get; private set; }
        public WorkStatus Status { get; private set; }
        public Boolean UserCancelled { get; private set; }

        public WorkReport(WorkStatus status)
            : this(status, null, null, false)
        { }

        public WorkReport(WorkStatus status, Exception exeException)
            : this(status, exeException, null, false)
        { }

        public WorkReport(WorkStatus status, Exception exeException, Exception undoException)
            : this(status, exeException, undoException, false)
        { }

        public WorkReport(WorkStatus status, Exception exeException, Exception undoException, Boolean userCancelled)
        {
            Status = status;
            ExecuteException = exeException;
            UndoException = undoException;
            UserCancelled = userCancelled;
        }
    }

    /// <summary>
    /// Used to indentify what work is being reported to the WorkReportGenerator
    /// </summary>
    public enum WorkAction
    {
        NotSet,
        Execute,
        Undo
    }

    /// <summary>
    /// The overall status of the work
    /// </summary>
    public enum WorkStatus
    {
        NotSet,
        Success,
        CommandFailWithException,
        UserCancelled,
        UndoFailWithException
    }

    /// <summary>
    /// Creates the immutable data class WorkReport
    /// </summary>
    public class WorkReportGenerator
    {
        private Exception _exeCaughtException;
        private Exception _undoCaughtException;
        private Boolean _userCancelled;

        /// <summary>
        /// Records the user cancelling the command run.
        /// </summary>
        public void UserCancelledRequested()
        {
            _userCancelled = true;
        }

        /// <summary>
        /// This methods records the exception to be included in the work report
        /// </summary>
        /// <param name="exception">The caught exception</param>
        /// <param name="action">What was happening at the time</param>
        public void ProcessException(Exception exception, WorkAction action)
        {
            if (action == WorkAction.Execute)
            {
                _exeCaughtException = exception;
            }
            else
            {
                _undoCaughtException = exception;
            }
        }

        /// <summary>
        /// Creates the report
        /// </summary>
        /// <returns>An immutable data object that summaries as information</returns>
        public WorkReport GenerateReport()
        {
            WorkStatus finalStatus = WorkStatus.Success;
            
            
            if(_exeCaughtException != null)
            {
                finalStatus = WorkStatus.CommandFailWithException;
            }

            if(_undoCaughtException != null)
            {
                finalStatus = WorkStatus.UndoFailWithException;    
            }

            ResetData();

            return new WorkReport(finalStatus,
                _exeCaughtException,
                _undoCaughtException,
                _userCancelled);
        }

        /// <summary>
        /// Resets the internal state of the object so it can be reused
        /// </summary>
        private void ResetData()
        {
            _exeCaughtException = null;
            _undoCaughtException = null;
            _userCancelled = false;
        }

    }
}
