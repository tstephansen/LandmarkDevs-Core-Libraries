using System.Threading.Tasks;

namespace LandmarkDevs.Core.Infrastructure
{
    public interface IErrorTracker
    {
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        int LogError(IErrorModel model);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> LogErrorAsync(IErrorModel model);
    }
}