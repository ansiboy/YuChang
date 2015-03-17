using System;
namespace tenpayApp
{
    internal static class Error
    {
        public static Exception KeyRequired()
        {
            string message = "The key is required.";
            return new Exception(message);
        }
    }
}
