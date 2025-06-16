using System;
using Windows.Storage;

namespace FluentNotes.Utils.Providers
{
    public static class ApplicationTypeDetector
    {
        public static bool IsPackagedApp()
        {
            try
            {
                _ = ApplicationData.Current;
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

    }
}
