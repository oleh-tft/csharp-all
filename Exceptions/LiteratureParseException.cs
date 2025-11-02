using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when parsing iterature type fails
    /// </summary>
    internal class LiteratureParseException(String message) : Exception(message)
    {

    }
}
