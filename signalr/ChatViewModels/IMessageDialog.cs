using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatViewModels;

public interface IMessageDialog
{
    Task ShowMessageAsync(string message);
}
