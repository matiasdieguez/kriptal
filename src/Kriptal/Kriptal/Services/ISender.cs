using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptal.Services
{
    public interface ISender
    {
        void Send(string text, string to);
        void Send(string text, string to, string fileUri);
    }
}
