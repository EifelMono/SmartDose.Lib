using Rowa.Lib.Log.Types;

namespace Rowa.Lib.Log.Events
{
    /// <summary>
    /// Event that is thrown when an Error during the Logger Operation occured
    /// </summary>
    /// <param name="args">Information about the occuring error</param>
    public delegate void OnNewError(LogErrorEventArgs args);

    /// <summary>
    /// Is Called when a Log Entry gets processsed
    /// </summary>
    /// <param name="entry">Entry to be processed</param>
    /// <returns>Wheter the processing of the LogAction was successfull</returns>
    internal delegate bool OnLogAction(LogEntryExt entry);
}
