using System;

namespace Kbg.NppPluginNET
{
    public static class Guard
    {
        public static void ThrowExceptionIfNull(object parameter, string parameterName)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
