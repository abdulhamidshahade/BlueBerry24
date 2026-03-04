using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Constants
{
    public enum ResultStatus
    {
        Success,
        ValidationError,
        Conflict,
        VerificationRequired,
        Failure,
        NoContent,
        NotFound,
        Forbidden,
        Unauthorized
    }
}
