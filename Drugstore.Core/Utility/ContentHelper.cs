using System;
using System.Collections.Generic;
using System.Text;

namespace Drugstore.Core.Utility
{
    public class ContentHelper
    {

        public static string VerificationToString(VerificationState state)
        {
            switch (state)
            {
                case VerificationState.NotVerified:
                    return "Oczekujący";
                case VerificationState.Accepted:
                    return "Zaakceptowany";
                case VerificationState.Rejected:
                    return "Odrzucony";
                default:
                    return "Brak";
            }
        }
    }
}
