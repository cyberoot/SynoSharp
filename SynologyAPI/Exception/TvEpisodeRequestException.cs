using SynologyRestDAL;

namespace SynologyAPI.Exception
{
    public class TvEpisodeRequestException : System.Exception
    {
        public TvEpisodeRequestException(string error)
            : base(error)
        {
            
        }
    }
}