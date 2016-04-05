using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ListBoxIssue.ViewModel
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public Image ImageUrl { get; set; }

        public bool IsRead { get; set; }

        public void SetIsRead()
        {
            IsRead = true;
        }
    }
}
