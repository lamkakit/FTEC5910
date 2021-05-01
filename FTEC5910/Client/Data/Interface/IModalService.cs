using FTEC5910.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Interface
{
    public interface IModalService
    {
        event Func<string, string, string, string, Task<YesNoModalResult>> YesNoModal;
        event Func<string, string, string, Task> ConfirmModal;
        Task<YesNoModalResult> ShowYesNoModal(string title, string msg);
        Task ShowConfirmModal(string title, string msg);
    }
}
