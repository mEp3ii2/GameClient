using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BusinessLayer;

namespace GameClient
{
    [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ProcessServiceCallBackImpl : ProcessServiceCallBack 
    {
        private MainWindow window;

        public ProcessServiceCallBackImpl(MainWindow window)
        {
            this.window = window;
        }

        public void UpdateUserCount(int userCount)
        {
            window.UpdateUserCount(userCount);
        }
    }
}
